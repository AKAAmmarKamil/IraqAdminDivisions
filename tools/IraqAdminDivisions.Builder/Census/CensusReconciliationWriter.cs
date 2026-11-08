using System.Text;
using IraqAdminDivisions.Builder.Models;
using IraqAdminDivisions.Builder.Output;

namespace IraqAdminDivisions.Builder.Census;

public sealed class CensusReconciliationWriter
{
    private static readonly (ReconciliationBucket Bucket, string Title, string Meaning)[] Buckets =
    [
        (ReconciliationBucket.Missing, "Missing", "needs a new row (Phase 2: a source other than the checklist, then a 51+ code)"),
        (ReconciliationBucket.Reparented, "Re-parented", "same unit, census lists it under another district; `reparent[]` keeps the code (D3)"),
        (ReconciliationBucket.CrossGovernorate, "Cross-governorate", "census lists it in another governorate; only a `moves[]` entry (deprecate + replacedBy) can follow it, so this needs an owner decision"),
        (ReconciliationBucket.Renamed, "Renamed", "same unit, matched by English only; the census Arabic spelling becomes an alias"),
        (ReconciliationBucket.Extra, "Extra", "in the dataset, not in the census list; never removed"),
        (ReconciliationBucket.Matched, "Matched", "same unit, same parent")
    ];

    public Task WriteAsync(string path, ReconciliationResult result, DateTime generatedAtUtc)
    {
        var builder = new StringBuilder();
        builder.Append("# Census 2024 reconciliation\n\n");
        builder.Append($"Generated {generatedAtUtc:yyyy-MM-dd} by the Builder (`CensusReconciler`, PLAN-1.1 Phase 1). No data was changed.\n\n");
        builder.Append("Target list: the 2024 census hierarchy (COSIT) as tallied from a local, gitignored checklist (`data/local/`). ");
        builder.Append("Only names and hierarchy are compared; no population figure or third-party ID is used or shipped. ");
        builder.Append("Matching uses the normalised Arabic name first, then English, and prefers the same parent, then the same governorate, then anywhere (anywhere needs an Arabic match, exact or one letter apart).\n\n");

        AppendTotals(builder, result);
        AppendTallies(builder, result);
        foreach (var (bucket, title, meaning) in Buckets)
            AppendBucket(builder, result.Rows.Where(row => row.Bucket == bucket).ToList(), title, meaning);

        return OutputFile.WriteAsync(path, builder.ToString(), shouldWriteBom: false);
    }

    private static void AppendTotals(StringBuilder builder, ReconciliationResult result)
    {
        builder.Append("## Buckets\n\n| Bucket | Districts | Sub-districts |\n|---|---:|---:|\n");
        foreach (var (bucket, title, _) in Buckets)
        {
            var rows = result.Rows.Where(row => row.Bucket == bucket).ToList();
            builder.Append($"| {title} | {rows.Count(row => row.Level == AdminLevel.District)} | {rows.Count(row => row.Level == AdminLevel.SubDistrict)} |\n");
        }

        builder.Append('\n');
    }

    private static void AppendTallies(StringBuilder builder, ReconciliationResult result)
    {
        builder.Append("## Per governorate\n\n");
        builder.Append("| Governorate | Code | Districts, census | Districts, dataset | Sub-districts, census | Sub-districts, dataset |\n");
        builder.Append("|---|---|---:|---:|---:|---:|\n");
        foreach (var tally in result.Tallies)
            builder.Append($"| {tally.Governorate} | {tally.Division?.Code ?? "unmatched"} | {tally.CensusDistricts} | {tally.ActiveDistricts} | {tally.CensusSubDistricts} | {tally.ActiveSubDistricts} |\n");

        builder.Append($"| **Total** | | {result.Tallies.Sum(tally => tally.CensusDistricts)} | {result.Tallies.Sum(tally => tally.ActiveDistricts)} | {result.Tallies.Sum(tally => tally.CensusSubDistricts)} | {result.Tallies.Sum(tally => tally.ActiveSubDistricts)} |\n\n");
        builder.Append("Census sub-district counts leave out units the list marks as dissolved.\n\n");
    }

    private static void AppendBucket(StringBuilder builder, IReadOnlyList<ReconciliationRow> rows, string title, string meaning)
    {
        builder.Append($"## {title} ({rows.Count})\n\n{meaning}.\n\n");
        if (rows.Count == 0)
            return;

        builder.Append("| Governorate | Level | Census name | Census Arabic | Census district | Dataset code | Dataset name | Note |\n");
        builder.Append("|---|---|---|---|---|---|---|---|\n");
        foreach (var row in rows.OrderBy(row => row.Governorate, StringComparer.Ordinal).ThenBy(row => row.Level).ThenBy(row => row.Census?.District ?? row.Division?.ParentCode, StringComparer.Ordinal))
        {
            var census = row.Census;
            var divisionName = row.Division is null ? "" : $"{row.Division.Name.En} / {row.Division.Name.Ar}";
            builder.Append($"| {Cell(row.Governorate)} | {row.Level} | {Cell(census?.NameEn)} | {Cell(census?.NameNative)} | {Cell(census?.Level == AdminLevel.SubDistrict ? census.District : null)} | {row.Division?.Code} | {Cell(divisionName)} | {Cell(row.Note)} |\n");
        }

        builder.Append('\n');
    }

    private static string Cell(string? value) => (value ?? "").Replace("|", "\\|");
}
