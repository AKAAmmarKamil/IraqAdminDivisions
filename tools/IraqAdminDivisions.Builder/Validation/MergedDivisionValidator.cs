using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed class MergedDivisionValidator
{
    public IReadOnlyList<string> Validate(IReadOnlyList<Division> divisions)
    {
        var issues = new List<string>();
        foreach (var division in divisions)
        {
            CheckKurdishName(division, issues);
            CheckNames(division, issues);
        }

        return issues;
    }

    private static void CheckKurdishName(Division division, List<string> issues)
    {
        if (string.IsNullOrWhiteSpace(division.Name.Ku))
        {
            issues.Add($"{division.Code}: no Kurdish name after merge");
            return;
        }

        if (division.KurdishNameSource is null || !KurdishNameSources.All.Contains(division.KurdishNameSource))
            issues.Add($"{division.Code}: unknown Kurdish name source '{division.KurdishNameSource}'");

        CheckKurdishNameDataset(division, issues);

        var foreignCharacters = SoraniAlphabet.ForeignCharacters(division.Name.Ku);
        if (foreignCharacters.Count > 0)
            issues.Add($"{division.Code}: Kurdish name '{division.Name.Ku}' ({division.KurdishNameSource}) contains non-Sorani characters ({string.Join(", ", foreignCharacters)})");
    }

    private static void CheckKurdishNameDataset(Division division, List<string> issues)
    {
        var isDatasetSource = division.KurdishNameSource == KurdishNameSources.Dataset;
        if (isDatasetSource && string.IsNullOrWhiteSpace(division.KurdishNameDataset))
            issues.Add($"{division.Code}: Kurdish name source is dataset but no dataset is recorded");
        else if (!isDatasetSource && division.KurdishNameDataset is not null)
            issues.Add($"{division.Code}: Kurdish name dataset '{division.KurdishNameDataset}' is set while the source is '{division.KurdishNameSource}'");
    }

    private static void CheckNames(Division division, List<string> issues)
    {
        if (string.IsNullOrWhiteSpace(division.Name.En))
            issues.Add($"{division.Code}: empty English name after corrections");

        if (!NameNormalizer.HasArabicLetter(division.Name.Ar))
            issues.Add($"{division.Code}: Arabic name has no Arabic letters after corrections ({division.Name.Ar})");
    }
}
