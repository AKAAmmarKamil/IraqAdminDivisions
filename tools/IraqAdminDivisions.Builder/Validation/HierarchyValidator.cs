using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Merge;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed class HierarchyValidator
{
    private const int GovernoratePrefixLength = 5;

    /// <summary>Validates the hierarchy after restructures.</summary>
    /// <param name="divisions">Every division, deprecated included.</param>
    /// <param name="expectedActiveCounts">The active count per level (OCHA plus Halabja, plus every new division in restructures.json).</param>
    /// <param name="reparentedCodes">Codes that keep their OCHA code under a new parent in the same governorate (PLAN-1.1 D3); only their governorate prefix must match the parent.</param>
    public IReadOnlyList<string> Validate(
        IReadOnlyList<Division> divisions,
        IReadOnlyDictionary<AdminLevel, int> expectedActiveCounts,
        IReadOnlySet<string> reparentedCodes)
    {
        var issues = new List<string>();
        var byCode = new Dictionary<string, Division>(StringComparer.Ordinal);
        foreach (var division in divisions)
        {
            if (!byCode.TryAdd(division.Code, division))
                issues.Add($"{division.Code}: duplicate code after restructures");
        }

        foreach (var division in divisions)
        {
            if (division.IsDeprecated)
                CheckReplacement(division, byCode, issues);
            else
                CheckActiveParent(division, byCode, reparentedCodes, issues);
        }

        foreach (var (level, expected) in expectedActiveCounts)
        {
            var actual = divisions.Count(division => !division.IsDeprecated && division.Level == level);
            if (actual != expected)
                issues.Add($"expected {expected} active {level} rows after restructures, found {actual}");
        }

        foreach (var division in divisions.Where(division => division.OchaCode is null && division.Level != AdminLevel.Governorate))
        {
            if (!RestructureApplier.HasProjectNumber(division.Code, division.Level))
                issues.Add($"{division.Code}: project-assigned code must end in {RestructureApplier.FirstProjectCodeNumber} or higher");
        }

        CheckDuplicateArabicNames(divisions, issues);

        // A district without sub-districts would leave an empty cascading dropdown in projects built on the previous release.
        var parentsOfSubDistricts = divisions
            .Where(division => !division.IsDeprecated && division.Level == AdminLevel.SubDistrict)
            .Select(division => division.ParentCode)
            .ToHashSet(StringComparer.Ordinal);
        foreach (var district in divisions.Where(division => !division.IsDeprecated && division.Level == AdminLevel.District && !parentsOfSubDistricts.Contains(division.Code)))
            issues.Add($"{district.Code}: active district has no sub-districts");

        return issues;
    }

    private static void CheckDuplicateArabicNames(IReadOnlyList<Division> divisions, List<string> issues)
    {
        var groups = divisions
            .Where(division => !division.IsDeprecated && division.ParentCode is not null)
            .GroupBy(division => (division.ParentCode, Key: NormalizedArabic(division.Name.Ar)))
            .Where(group => group.Count() > 1);

        foreach (var group in groups)
            issues.Add($"{group.Key.ParentCode}: duplicate Arabic name {group.First().Name.Ar} in {string.Join(", ", group.Select(division => division.Code))}");
    }

    private static string NormalizedArabic(string name) =>
        NameNormalizer.ArabicKeys([name]).OrderBy(key => key.Length).ThenBy(key => key, StringComparer.Ordinal).LastOrDefault() ?? name;

    private static void CheckReplacement(Division division, Dictionary<string, Division> byCode, List<string> issues)
    {
        if (division.ReplacedBy is null || !byCode.TryGetValue(division.ReplacedBy, out var replacement))
            issues.Add($"{division.Code}: deprecated without an existing replacement");
        else if (replacement.IsDeprecated)
            issues.Add($"{division.Code}: replacement {replacement.Code} is itself deprecated");
        else if (replacement.Level != division.Level)
            issues.Add($"{division.Code}: replacement {replacement.Code} is a different level");
    }

    private static void CheckActiveParent(
        Division division, Dictionary<string, Division> byCode, IReadOnlySet<string> reparentedCodes, List<string> issues)
    {
        if (division.ParentCode is null)
        {
            if (division.Level != AdminLevel.Governorate)
                issues.Add($"{division.Code}: active {division.Level} has no parent");
            return;
        }

        if (!byCode.TryGetValue(division.ParentCode, out var parent))
        {
            issues.Add($"{division.Code}: parent {division.ParentCode} does not exist");
            return;
        }

        if (parent.IsDeprecated)
            issues.Add($"{division.Code}: active row has deprecated parent {parent.Code}");
        if ((int)parent.Level != (int)division.Level - 1)
            issues.Add($"{division.Code}: parent {parent.Code} is not one level up");

        var requiredPrefix = reparentedCodes.Contains(division.Code) ? parent.Code[..GovernoratePrefixLength] : parent.Code;
        if (!division.Code.StartsWith(requiredPrefix, StringComparison.Ordinal))
            issues.Add($"{division.Code}: code does not start with {requiredPrefix}");
    }
}
