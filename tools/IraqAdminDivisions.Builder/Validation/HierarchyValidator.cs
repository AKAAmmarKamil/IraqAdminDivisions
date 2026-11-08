using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed class HierarchyValidator
{
    public IReadOnlyList<string> Validate(IReadOnlyList<Division> divisions)
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
                CheckActiveParent(division, byCode, issues);
        }

        foreach (var (level, expected) in BuildSettings.ExpectedActiveCounts)
        {
            var actual = divisions.Count(division => !division.IsDeprecated && division.Level == level);
            if (actual != expected)
                issues.Add($"expected {expected} active {level} rows after restructures, found {actual}");
        }

        return issues;
    }

    private static void CheckReplacement(Division division, Dictionary<string, Division> byCode, List<string> issues)
    {
        if (division.ReplacedBy is null || !byCode.TryGetValue(division.ReplacedBy, out var replacement))
            issues.Add($"{division.Code}: deprecated without an existing replacement");
        else if (replacement.IsDeprecated)
            issues.Add($"{division.Code}: replacement {replacement.Code} is itself deprecated");
        else if (replacement.Level != division.Level)
            issues.Add($"{division.Code}: replacement {replacement.Code} is a different level");
    }

    private static void CheckActiveParent(Division division, Dictionary<string, Division> byCode, List<string> issues)
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
        if (!division.Code.StartsWith(parent.Code, StringComparison.Ordinal))
            issues.Add($"{division.Code}: code does not start with parent code {parent.Code}");
    }
}
