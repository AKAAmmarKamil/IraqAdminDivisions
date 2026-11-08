using System.Text.RegularExpressions;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed partial class IsoCodeValidator
{
    [GeneratedRegex("^IQ-[A-Z]{2}$")]
    private static partial Regex IsoCodePattern();

    public IReadOnlyList<string> Validate(IReadOnlyList<Division> divisions)
    {
        var issues = new List<string>();
        var governorates = divisions.Where(division => division.Level == AdminLevel.Governorate).ToList();

        foreach (var governorate in governorates)
        {
            if (governorate.IsoCode is null)
                issues.Add($"{governorate.Code}: governorate has no ISO 3166-2 code");
            else if (!IsoCodePattern().IsMatch(governorate.IsoCode))
                issues.Add($"{governorate.Code}: ISO 3166-2 code '{governorate.IsoCode}' does not match ^IQ-[A-Z]{{2}}$");
        }

        foreach (var group in governorates.Where(governorate => governorate.IsoCode is not null).GroupBy(governorate => governorate.IsoCode).Where(group => group.Count() > 1))
            issues.Add($"ISO 3166-2 code {group.Key} is shared by {string.Join(", ", group.Select(governorate => governorate.Code))}");

        foreach (var division in divisions.Where(division => division.Level != AdminLevel.Governorate && division.IsoCode is not null))
            issues.Add($"{division.Code}: only governorates carry an ISO 3166-2 code");

        return issues;
    }
}
