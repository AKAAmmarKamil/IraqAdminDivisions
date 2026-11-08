using System.Globalization;
using System.Text;
using IraqAdminDivisions.Builder.Datasets;
using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Merge;
using IraqAdminDivisions.Builder.Models;
using IraqAdminDivisions.Builder.Wikidata;

namespace IraqAdminDivisions.Builder.Output;

public sealed class BuildReportWriter
{
    public Task WriteAsync(
        string path,
        IReadOnlyList<OchaRow> rows,
        IReadOnlyList<MatchOutcome> outcomes,
        MergeResult? mergeResult,
        IReadOnlyList<string> validationIssues,
        DateTime generatedAtUtc,
        BoundaryReport? boundaryReport = null,
        KurdishCrosscheck? crosscheck = null,
        IsoCodeResult? isoCodes = null)
    {
        var builder = new StringBuilder();
        builder.Append("# Build report\n\n");
        builder.Append($"Generated: {generatedAtUtc:yyyy-MM-dd HH:mm:ss}Z  \n");
        builder.Append($"Status: {(validationIssues.Count == 0 ? "OK" : "FAILED - data files were not written")}\n\n");

        AppendCounts(builder, rows);
        AppendMatchRate(builder, outcomes);
        AppendKurdishCoverage(builder, mergeResult);
        AppendDatasetCalibration(builder, crosscheck);
        AppendIsoCodes(builder, mergeResult, isoCodes);
        AppendRejectedNameOnly(builder, outcomes);
        AppendOutcomeList(builder, "Unmatched rows", outcomes, MatchStatus.Unmatched);
        AppendOutcomeList(builder, "Ambiguous matches", outcomes, MatchStatus.Ambiguous);
        AppendRejectedKurdish(builder, mergeResult);
        AppendCorrections(builder, mergeResult);
        AppendBoundaries(builder, mergeResult, boundaryReport);
        AppendValidation(builder, validationIssues);

        return OutputFile.WriteAsync(path, builder.ToString(), shouldWriteBom: false);
    }

    private static void AppendCounts(StringBuilder builder, IReadOnlyList<OchaRow> rows)
    {
        builder.Append("## Counts per level\n\n| Level | Rows | Expected |\n|---|---|---|\n");
        foreach (var level in Enum.GetValues<AdminLevel>())
            builder.Append($"| {level} | {rows.Count(row => row.Level == level)} | {BuildSettings.ExpectedCounts[level]} |\n");
        builder.Append($"| Total | {rows.Count} | {BuildSettings.ExpectedCounts.Values.Sum()} |\n\n");
    }

    private static void AppendMatchRate(StringBuilder builder, IReadOnlyList<MatchOutcome> outcomes)
    {
        builder.Append("## Wikidata match rate\n\n| Level | Matched | Ambiguous | Unmatched | Rate |\n|---|---|---|---|---|\n");
        foreach (var level in Enum.GetValues<AdminLevel>())
            AppendMatchLine(builder, level.ToString(), outcomes.Where(outcome => outcome.Row.Level == level).ToList());
        AppendMatchLine(builder, "Total", outcomes);
        builder.Append('\n');
    }

    private static void AppendMatchLine(StringBuilder builder, string label, IReadOnlyList<MatchOutcome> outcomes)
    {
        var matched = outcomes.Count(outcome => outcome.Status == MatchStatus.Matched);
        var ambiguous = outcomes.Count(outcome => outcome.Status == MatchStatus.Ambiguous);
        var unmatched = outcomes.Count(outcome => outcome.Status == MatchStatus.Unmatched);
        builder.Append($"| {label} | {matched} | {ambiguous} | {unmatched} | {Percent(matched, outcomes.Count)} |\n");
    }

    private static void AppendKurdishCoverage(StringBuilder builder, MergeResult? mergeResult)
    {
        builder.Append("## Kurdish name coverage\n\n");
        if (mergeResult is null)
        {
            builder.Append("Not computed (build failed validation).\n\n");
            return;
        }

        builder.Append($"Kurdish names with an administrative-type prefix stripped: {mergeResult.StrippedKurdishNameCount}\n\n");
        builder.Append("| Level | Wikidata | Dataset | Transliterated | Manual | With Kurdish name | Total | Coverage |\n|---|---|---|---|---|---|---|---|\n");
        foreach (var level in Enum.GetValues<AdminLevel>())
            AppendKurdishLine(builder, level.ToString(), mergeResult.Divisions.Where(division => division.Level == level).ToList());
        AppendKurdishLine(builder, "Total", mergeResult.Divisions);
        builder.Append('\n');
        builder.Append($"Transliteration entries ignored because Wikidata or a correction already supplies the name: {mergeResult.UnusedTransliterationCodes.Count}\n\n");
    }

    private static void AppendKurdishLine(StringBuilder builder, string label, IReadOnlyList<Division> divisions)
    {
        var withKurdish = divisions.Count(division => !string.IsNullOrEmpty(division.Name.Ku));
        var sourceCounts = KurdishNameSources.All
            .Select(source => divisions.Count(division => division.KurdishNameSource == source));
        builder.Append($"| {label} | {string.Join(" | ", sourceCounts)} | {withKurdish} | {divisions.Count} | {Percent(withKurdish, divisions.Count)} |\n");
    }

    private static void AppendDatasetCalibration(StringBuilder builder, KurdishCrosscheck? crosscheck)
    {
        builder.Append("## Kurdish dataset calibration\n\n");
        if (crosscheck is null)
        {
            builder.Append("Not computed (build failed validation).\n\n");
            return;
        }

        builder.Append($"A dataset is trusted only with at least {Percent(DatasetTrustPolicy.MinExactAgreementBeyondGovernorates)} exact agreement with our Wikidata names beyond governorate level (over at least {DatasetTrustPolicy.MinComparedBeyondGovernorates} rows) and at least {Percent(DatasetTrustPolicy.MinPureSoraniShare)} pure Sorani names. Details: reports/kurdish-crosscheck.md\n\n");
        builder.Append("| Dataset | Licence | Commit | Rows | Matched | Compared with Wikidata | Exact | Near | Exact beyond governorates | Pure Sorani | Trusted |\n|---|---|---|---|---|---|---|---|---|---|---|\n");
        foreach (var evaluation in crosscheck.Evaluations)
        {
            var calibration = evaluation.Calibration;
            builder.Append($"| {evaluation.Dataset.Repository} | {evaluation.Dataset.License} | {evaluation.Dataset.CommitSha[..7]} | {evaluation.Rows.Count} | {evaluation.Matching.Matches.Count} | {calibration.Compared} | {calibration.Exact} | {calibration.Near} | {calibration.ExactBeyondGovernorates} of {calibration.ComparedBeyondGovernorates} ({Percent(calibration.ExactRateBeyondGovernorates)}) | {Percent(calibration.PureSoraniShare)} | {(calibration.IsTrusted ? "yes" : "no")} |\n");
        }

        builder.Append("\nAgreement with Wikidata per level (exact / near / compared):\n\n| Dataset | Governorate | District | SubDistrict |\n|---|---|---|---|\n");
        foreach (var evaluation in crosscheck.Evaluations)
            builder.Append($"| {evaluation.Dataset.Repository} | {string.Join(" | ", evaluation.Calibration.Levels.Select(level => $"{level.Exact} / {level.Near} / {level.Compared}"))} |\n");

        builder.Append("\nTransliterated rows resolved by datasets:\n\n| Dataset | Confirmed (kept our text) | Replaced (ours kept as alias) |\n|---|---|---|\n");
        foreach (var use in crosscheck.Uses)
            builder.Append($"| {use.Dataset.Repository} | {use.Confirmed} | {use.Replaced} |\n");
        builder.Append($"| Total | {crosscheck.ConfirmedCount} | {crosscheck.ReplacedCount} |\n\n");
        builder.Append($"Transliterated rows with no trusted dataset name: {crosscheck.TransliteratedWithoutDatasetNameCount}\n\n");
    }

    private static void AppendIsoCodes(StringBuilder builder, MergeResult? mergeResult, IsoCodeResult? isoCodes)
    {
        builder.Append("## ISO 3166-2 codes\n\n");
        if (mergeResult is null)
        {
            builder.Append("Not computed (build failed validation).\n\n");
            return;
        }

        builder.Append("| Code | English | ISO 3166-2 | Wikidata |\n|---|---|---|---|\n");
        foreach (var governorate in mergeResult.Divisions.Where(division => division.Level == AdminLevel.Governorate))
            builder.Append($"| {governorate.Code} | {Cell(governorate.Name.En)} | {governorate.IsoCode} | {governorate.WikidataId} |\n");
        builder.Append('\n');

        foreach (var note in isoCodes?.Notes ?? [])
            builder.Append($"- {note}\n");
        builder.Append('\n');
    }

    private static void AppendCorrections(StringBuilder builder, MergeResult? mergeResult)
    {
        var corrections = mergeResult?.AppliedCorrections ?? [];
        builder.Append($"## Applied corrections ({corrections.Count})\n\n");
        if (corrections.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        builder.Append("| Code | Field | Replaced | New | Reason |\n|---|---|---|---|---|\n");
        foreach (var correction in corrections)
            builder.Append($"| {correction.Code} | {correction.Field} | {Cell(correction.OldValue ?? string.Empty)} | {Cell(correction.NewValue)} | {Cell(correction.Reason)} |\n");
        builder.Append('\n');
    }

    private static void AppendBoundaries(StringBuilder builder, MergeResult? mergeResult, BoundaryReport? boundaryReport)
    {
        builder.Append("## Polygons\n\n");
        if (mergeResult is null || boundaryReport is null)
        {
            builder.Append("Not computed (build failed validation).\n\n");
            return;
        }

        builder.Append($"Douglas-Peucker tolerance: {boundaryReport.ToleranceDegrees.ToString(CultureInfo.InvariantCulture)} degrees, coordinates rounded to 5 decimals\n\n");
        builder.Append("| File | Size |\n|---|---|\n");
        builder.Append($"| data/boundaries.json | {Megabytes(boundaryReport.BoundariesJsonBytes)} |\n");
        builder.Append($"| data/exports/divisions.geojson | {Megabytes(boundaryReport.GeoJsonBytes)} |\n\n");

        AppendCentreChecks(builder, mergeResult, boundaryReport.Analysis);
        AppendBboxChecks(builder, mergeResult, boundaryReport.Analysis);
    }

    private static void AppendCentreChecks(StringBuilder builder, MergeResult mergeResult, BoundaryAnalysis analysis)
    {
        builder.Append("### OCHA centre point inside its own simplified polygon\n\n");
        builder.Append("| Level | Inside | Checked | Rate |\n|---|---|---|---|\n");
        foreach (var level in Enum.GetValues<AdminLevel>())
            AppendCentreLine(builder, level.ToString(), analysis.CentreChecks.Where(check => check.Level == level).ToList());
        AppendCentreLine(builder, "Total", analysis.CentreChecks);
        builder.Append('\n');

        var misses = analysis.CentreChecks.Where(check => !check.IsInsideOwnPolygon).ToList();
        builder.Append($"Misses ({misses.Count}):\n\n");
        if (misses.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        var divisionsByCode = mergeResult.Divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
        builder.Append("| Code | Level | English |\n|---|---|---|\n");
        foreach (var miss in misses)
            builder.Append($"| {miss.Code} | {miss.Level} | {Cell(divisionsByCode[miss.Code].Name.En)} |\n");
        builder.Append('\n');
    }

    private static void AppendCentreLine(StringBuilder builder, string label, IReadOnlyList<CentreCheck> checks)
    {
        var inside = checks.Count(check => check.IsInsideOwnPolygon);
        builder.Append($"| {label} | {inside} | {checks.Count} | {Percent(inside, checks.Count)} |\n");
    }

    private static void AppendBboxChecks(StringBuilder builder, MergeResult mergeResult, BoundaryAnalysis analysis)
    {
        var outliers = analysis.BboxChecks.Where(check => !check.IsInsideGovernorateBox).ToList();
        builder.Append($"### Sub-district polygon box outside its governorate box ({outliers.Count} of {analysis.BboxChecks.Count})\n\n");
        if (outliers.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        var divisionsByCode = mergeResult.Divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
        builder.Append("| Code | English | Governorate |\n|---|---|---|\n");
        foreach (var outlier in outliers)
            builder.Append($"| {outlier.Code} | {Cell(divisionsByCode[outlier.Code].Name.En)} | {outlier.GovernorateCode} |\n");
        builder.Append('\n');
    }

    private static void AppendOutcomeList(StringBuilder builder, string title, IReadOnlyList<MatchOutcome> outcomes, MatchStatus status)
    {
        var selected = outcomes.Where(outcome => outcome.Status == status).ToList();
        builder.Append($"## {title} ({selected.Count})\n\n");
        if (selected.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        builder.Append("| Code | Level | Arabic | English | Detail |\n|---|---|---|---|---|\n");
        foreach (var outcome in selected)
            builder.Append($"| {outcome.Row.Code} | {outcome.Row.Level} | {Cell(outcome.Row.NameAr)} | {Cell(outcome.Row.NameEn)} | {Cell(outcome.Detail)} |\n");
        builder.Append('\n');
    }

    private static void AppendRejectedNameOnly(StringBuilder builder, IReadOnlyList<MatchOutcome> outcomes)
    {
        var rejections = outcomes
            .SelectMany(outcome => (outcome.RejectedNameOnlyItemIds ?? []).Select(itemId => (outcome.Row, ItemId: itemId)))
            .ToList();
        builder.Append($"## Rejected name-only candidates ({rejections.Count})\n\n");
        if (rejections.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        builder.Append("| OCHA code | English | Candidate |\n|---|---|---|\n");
        foreach (var (row, itemId) in rejections)
            builder.Append($"| {row.Code} | {Cell(row.NameEn)} | {itemId} |\n");
        builder.Append('\n');
    }

    private static void AppendRejectedKurdish(StringBuilder builder, MergeResult? mergeResult)
    {
        var rejected = mergeResult?.RejectedKurdishNames ?? [];
        builder.Append($"## Rejected Wikidata Kurdish labels ({rejected.Count})\n\n");
        if (rejected.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        builder.Append("| Code | Label | Reason |\n|---|---|---|\n");
        foreach (var name in rejected)
            builder.Append($"| {name.Code} | {Cell(name.Name)} | {name.Reason} |\n");
        builder.Append('\n');
    }

    private static void AppendValidation(StringBuilder builder, IReadOnlyList<string> validationIssues)
    {
        builder.Append($"## Validation failures ({validationIssues.Count})\n\n");
        if (validationIssues.Count == 0)
        {
            builder.Append("None.\n");
            return;
        }

        foreach (var issue in validationIssues)
            builder.Append($"- {issue}\n");
    }

    private static string Percent(int part, int total) =>
        total == 0 ? "n/a" : (100.0 * part / total).ToString("0.0", CultureInfo.InvariantCulture) + "%";

    private static string Percent(double share) =>
        (share * 100).ToString("0.0", CultureInfo.InvariantCulture) + "%";

    private static string Megabytes(long bytes) =>
        $"{(bytes / 1024.0 / 1024.0).ToString("0.00", CultureInfo.InvariantCulture)} MB ({bytes:N0} bytes)";

    private static string Cell(string text) => text.Replace("|", "\\|");
}
