using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Merge;

namespace IraqAdminDivisions.Builder.Datasets;

public static class DatasetKurdishName
{
    private static readonly HashSet<string> AdministrativeWords = new[] { "قەزای", "قەزا", "ناحیەی", "ناحیە", "پارێزگای", "پارێزگا" }
        .Select(SoraniNormalizer.Key)
        .ToHashSet(StringComparer.Ordinal);

    private static readonly IReadOnlyDictionary<char, char> SoraniLetterForms = new Dictionary<char, char>
    {
        ['ي'] = 'ی', ['ى'] = 'ی',
        ['ك'] = 'ک',
        ['ه'] = 'ھ'
    };

    public static string? Clean(string? rawName)
    {
        if (string.IsNullOrWhiteSpace(rawName))
            return null;

        var words = KurdishNamePrefix.Strip(ToSoraniLetterForms(rawName))
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !AdministrativeWords.Contains(SoraniNormalizer.Key(word)));
        var cleaned = string.Join(' ', words);
        return cleaned.Length == 0 ? null : cleaned;
    }

    private static string ToSoraniLetterForms(string text) =>
        string.Concat(text.Select(letter => SoraniLetterForms.GetValueOrDefault(letter, letter)));
}
