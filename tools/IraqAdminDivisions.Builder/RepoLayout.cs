namespace IraqAdminDivisions.Builder;

public sealed class RepoLayout
{
    private const string SolutionFileName = "IraqAdminDivisions.slnx";

    private RepoLayout(string root) => Root = root;

    public string Root { get; }
    public string CacheDirectory => Path.Combine(Root, "tools", ".cache");
    public string OchaCachePath => Path.Combine(CacheDirectory, "irq_admin_boundaries.xlsx");
    public string OchaGeoJsonCachePath => Path.Combine(CacheDirectory, "irq_admin_boundaries.geojson.zip");
    public string WikidataItemsCachePath => Path.Combine(CacheDirectory, "wikidata-items.json");
    public string WikidataLabelsCachePath => Path.Combine(CacheDirectory, "wikidata-labels.json");
    public string WikidataIsoCachePath => Path.Combine(CacheDirectory, "wikidata-iso.json");
    public string DivisionsJsonPath => Path.Combine(Root, "data", "divisions.json");
    public string CorrectionsPath => Path.Combine(Root, "data", "overrides", "corrections.tsv");
    public string TransliterationsPath => Path.Combine(Root, "data", "overrides", "kurdish-transliterated.tsv");
    public string RestructuresPath => Path.Combine(Root, "data", "overrides", "restructures.json");
    public string SourcesDirectory => Path.Combine(Root, "data", "sources");
    public string BoundariesJsonPath => Path.Combine(Root, "data", "boundaries.json");
    public string GeoJsonPath => Path.Combine(Root, "data", "exports", "divisions.geojson");
    public string CsvPath => Path.Combine(Root, "data", "exports", "divisions.csv");
    public string SqlPath => Path.Combine(Root, "data", "exports", "divisions.sql");
    public string SqlServerPath => Path.Combine(Root, "data", "exports", "divisions.sqlserver.sql");
    public string ReportPath => Path.Combine(Root, "reports", "build-report.md");
    public string CrosscheckReportPath => Path.Combine(Root, "reports", "kurdish-crosscheck.md");
    public string CensusReportPath => Path.Combine(Root, "reports", "census-reconciliation.md");
    public string CensusChecklistPath => Path.Combine(Root, "data", "local", "census-2024-citypopulation.tsv");
    public string CensusReconciliationJsonPath => Path.Combine(Root, "data", "local", "census-reconciliation.json");

    public static RepoLayout Locate()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, SolutionFileName)))
                return new RepoLayout(directory.FullName);
            directory = directory.Parent;
        }

        throw new InvalidOperationException($"Could not find {SolutionFileName} above {AppContext.BaseDirectory}.");
    }
}
