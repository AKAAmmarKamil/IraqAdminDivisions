using IraqAdminDivisions.Builder;
using IraqAdminDivisions.Builder.Census;
using IraqAdminDivisions.Builder.Datasets;
using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Http;
using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Merge;
using IraqAdminDivisions.Builder.Models;
using IraqAdminDivisions.Builder.Ocha;
using IraqAdminDivisions.Builder.Output;
using IraqAdminDivisions.Builder.Overrides;
using IraqAdminDivisions.Builder.Validation;
using IraqAdminDivisions.Builder.Wikidata;

var shouldRefresh = args.Contains(BuildSettings.RefreshArgument);
var layout = RepoLayout.Locate();
var generatedAtUtc = DateTime.UtcNow;
using var httpClient = BuilderHttpClient.Create();
var ochaDownloader = new OchaDownloader(httpClient);

var workbookPath = await ochaDownloader.EnsureDownloadedAsync(
    layout.OchaCachePath, shouldRefresh, BuildSettings.OchaDownloadUrl, BuildSettings.OchaWorkbookDescription);
var rows = new OchaReader().Read(workbookPath);
Console.WriteLine($"OCHA: {rows.Count} rows");

var wikidataClient = new WikidataClient(new SparqlRunner(httpClient));
var wikidataItems = await wikidataClient.LoadAsync(layout, shouldRefresh);
var outcomes = new WikidataMatcher(wikidataItems).Match(rows);
var governorateItemIds = outcomes
    .Where(outcome => outcome.Status == MatchStatus.Matched && outcome.Row.Level == AdminLevel.Governorate)
    .Select(outcome => outcome.Item!.Id)
    .ToList();
var isoCodes = await wikidataClient.LoadIsoCodesAsync(layout, shouldRefresh, governorateItemIds);
KurdishCrosscheck? crosscheck = null;
var overrides = new OverridesReader().Read(layout.CorrectionsPath, layout.TransliterationsPath);
var validationIssues = new List<string>();
validationIssues.AddRange(new DivisionValidator().Validate(rows));
validationIssues.AddRange(new OverridesValidator().Validate(overrides, rows));
validationIssues.AddRange(new DatasetSourceValidator().Validate(layout.SourcesDirectory));
validationIssues.AddRange(isoCodes.Issues);
var reportWriter = new BuildReportWriter();

if (validationIssues.Count > 0)
    return await FailAsync(null);

var wikidataMerge = new DivisionMerger().Merge(rows, outcomes, overrides, isoCodes.CodesByItemId);
var evaluations = new KurdishDatasetEvaluator(wikidataMerge.Divisions).Evaluate(layout.SourcesDirectory);
var datasetApplication = new KurdishDatasetApplier().Apply(wikidataMerge.Divisions, evaluations);
var mergeResult = wikidataMerge with { Divisions = datasetApplication.Divisions };
crosscheck = datasetApplication.Crosscheck;
await new KurdishCrosscheckWriter().WriteAsync(layout.CrosscheckReportPath, crosscheck, generatedAtUtc);
validationIssues.AddRange(new MergedDivisionValidator().Validate(mergeResult.Divisions));
validationIssues.AddRange(new IsoCodeValidator().Validate(mergeResult.Divisions));
var restructureApplier = new RestructureApplier(new RestructuresReader().Read(layout.RestructuresPath));
validationIssues.AddRange(restructureApplier.FindIssues(mergeResult.Divisions));
if (validationIssues.Count > 0)
    return await FailAsync(mergeResult);

mergeResult = mergeResult with { Divisions = restructureApplier.ApplyToDivisions(mergeResult.Divisions) };
var expectedActiveCounts = BuildSettings.ExpectedCounts.ToDictionary(
    pair => pair.Key, pair => pair.Value + restructureApplier.AddedActiveCount(pair.Key));
var reparentedCodes = restructureApplier.Restructures
    .SelectMany(restructure => restructure.ReparentOrEmpty)
    .Select(reparent => reparent.Code)
    .ToHashSet(StringComparer.Ordinal);
validationIssues.AddRange(new HierarchyValidator().Validate(mergeResult.Divisions, expectedActiveCounts, reparentedCodes));
if (validationIssues.Count > 0)
    return await FailAsync(mergeResult);

var geoJsonPath = await ochaDownloader.EnsureDownloadedAsync(
    layout.OchaGeoJsonCachePath, shouldRefresh, BuildSettings.OchaGeoJsonUrl, BuildSettings.OchaGeoJsonDescription);
var simplifier = new BoundarySimplifier(BuildSettings.BoundaryToleranceDegrees);
var boundaries = restructureApplier.ApplyToBoundaries(
    new OchaBoundariesReader().Read(geoJsonPath).Select(simplifier.Simplify).ToList());
validationIssues.AddRange(new BoundaryValidator().Validate(mergeResult.Divisions, boundaries));
if (validationIssues.Count > 0)
    return await FailAsync(mergeResult);

await new JsonDivisionsWriter().WriteAsync(layout.DivisionsJsonPath, mergeResult.Divisions, [.. BuildSettings.Sources, .. crosscheck.UsedSources()], generatedAtUtc);
await new CsvDivisionsWriter().WriteAsync(layout.CsvPath, mergeResult.Divisions);
await new SqlDivisionsWriter().WriteAsync(layout.SqlPath, mergeResult.Divisions);
await new SqlDivisionsWriter(SqlDialect.SqlServer).WriteAsync(layout.SqlServerPath, mergeResult.Divisions);
await new BoundariesWriter().WriteAsync(layout.BoundariesJsonPath, boundaries);
await new GeoJsonWriter().WriteAsync(layout.GeoJsonPath, mergeResult.Divisions, boundaries);

var boundaryReport = new BoundaryReport(
    BuildSettings.BoundaryToleranceDegrees,
    new FileInfo(layout.BoundariesJsonPath).Length,
    new FileInfo(layout.GeoJsonPath).Length,
    new BoundaryAnalyzer().Analyze(mergeResult.Divisions, boundaries));
await reportWriter.WriteAsync(layout.ReportPath, rows, outcomes, mergeResult, validationIssues, generatedAtUtc, boundaryReport, crosscheck, isoCodes);

var censusChecklist = new CensusChecklistReader().Read(layout.CensusChecklistPath);
if (censusChecklist is not null)
{
    var reconciliation = new CensusReconciler(mergeResult.Divisions).Reconcile(censusChecklist);
    await new CensusReconciliationWriter().WriteAsync(layout.CensusReportPath, reconciliation, generatedAtUtc);
    await new CensusReconciliationJsonWriter().WriteAsync(layout.CensusReconciliationJsonPath, reconciliation);
    Console.WriteLine($"Census: reconciled {censusChecklist.Count} checklist rows into {layout.CensusReportPath}");
}

Console.WriteLine($"Wrote {mergeResult.Divisions.Count} divisions and {boundaries.Count} polygons");
return 0;

async Task<int> FailAsync(MergeResult? partialMerge)
{
    await reportWriter.WriteAsync(layout.ReportPath, rows, outcomes, partialMerge, validationIssues, generatedAtUtc, crosscheck: crosscheck, isoCodes: isoCodes);
    foreach (var issue in validationIssues)
        Console.Error.WriteLine($"VALIDATION: {issue}");
    return 1;
}
