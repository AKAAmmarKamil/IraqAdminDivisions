using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed class DivisionValidator
{
    public IReadOnlyList<string> Validate(IReadOnlyList<OchaRow> rows)
    {
        var issues = new List<string>();
        CheckCounts(rows, issues);
        CheckDuplicateCodes(rows, issues);

        var rowsByCode = rows.GroupBy(row => row.Code, StringComparer.Ordinal).ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);
        foreach (var row in rows.OrderBy(row => row.Code, StringComparer.Ordinal))
        {
            CheckParent(row, rowsByCode, issues);
            CheckNames(row, issues);
            CheckCoordinates(row, issues);
        }

        return issues;
    }

    private static void CheckCounts(IReadOnlyList<OchaRow> rows, List<string> issues)
    {
        foreach (var (level, expected) in BuildSettings.ExpectedCounts)
        {
            var actual = rows.Count(row => row.Level == level);
            if (actual != expected)
                issues.Add($"{level}: expected {expected} rows, found {actual}");
        }
    }

    private static void CheckDuplicateCodes(IReadOnlyList<OchaRow> rows, List<string> issues)
    {
        foreach (var group in rows.GroupBy(row => row.Code, StringComparer.Ordinal).Where(group => group.Count() > 1))
            issues.Add($"{group.Key}: duplicate code ({group.Count()} rows)");
    }

    private static void CheckParent(OchaRow row, Dictionary<string, OchaRow> rowsByCode, List<string> issues)
    {
        if (row.Level == AdminLevel.Governorate)
        {
            if (row.ParentCode is not null)
                issues.Add($"{row.Code}: governorate must not have a parent ({row.ParentCode})");
            return;
        }

        if (row.ParentCode is null)
            issues.Add($"{row.Code}: {row.Level} has no parent code");
        else if (!rowsByCode.TryGetValue(row.ParentCode, out var parent))
            issues.Add($"{row.Code}: parent {row.ParentCode} does not exist");
        else if ((int)parent.Level != (int)row.Level - 1)
            issues.Add($"{row.Code}: parent {row.ParentCode} is a {parent.Level}, expected one level above {row.Level}");
        else if (!row.Code.StartsWith(row.ParentCode, StringComparison.Ordinal))
            issues.Add($"{row.Code}: code does not start with parent code {row.ParentCode}");
    }

    private static void CheckNames(OchaRow row, List<string> issues)
    {
        if (string.IsNullOrWhiteSpace(row.NameEn))
            issues.Add($"{row.Code}: empty English name");

        if (string.IsNullOrWhiteSpace(row.NameAr))
            issues.Add($"{row.Code}: empty Arabic name");
        else if (!NameNormalizer.HasArabicLetter(row.NameAr))
            issues.Add($"{row.Code}: Arabic name has no Arabic letters ({row.NameAr})");
    }

    private static void CheckCoordinates(OchaRow row, List<string> issues)
    {
        if (row.Lat is not { } lat || row.Lng is not { } lng)
            issues.Add($"{row.Code}: missing centroid coordinates");
        else if (lat is < BuildSettings.MinLat or > BuildSettings.MaxLat || lng is < BuildSettings.MinLng or > BuildSettings.MaxLng)
            issues.Add($"{row.Code}: centroid ({lat}, {lng}) is outside the Iraq bounding box");

        if (row.AreaSqKm is not { } area || area < 0)
            issues.Add($"{row.Code}: missing or negative area");
    }
}
