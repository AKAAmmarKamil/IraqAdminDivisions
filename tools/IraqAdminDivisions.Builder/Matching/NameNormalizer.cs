using System.Globalization;
using System.Text;

namespace IraqAdminDivisions.Builder.Matching;

public static class NameNormalizer
{
    private const string DefiniteArticle = "ال";

    private static readonly HashSet<string> ArabicGenericWords = new(StringComparer.Ordinal)
    {
        "قضاء", "ناحيه", "محافظه", "مركز", "منطقه"
    };

    private static readonly HashSet<string> EnglishGenericWords = new(StringComparer.Ordinal)
    {
        "district", "subdistrict", "sub", "governorate", "province", "nahiyah", "nahia", "nahiya", "nahiyat",
        "qadaa", "qada", "qadha", "markaz", "center", "centre", "city", "of", "al", "el", "ash", "as", "an",
        "ad", "ar", "at", "az", "ad-"
    };

    private static readonly IReadOnlyDictionary<char, char> ArabicLetterMap = new Dictionary<char, char>
    {
        ['أ'] = 'ا', ['إ'] = 'ا', ['آ'] = 'ا', ['ٱ'] = 'ا',
        ['ى'] = 'ي', ['ئ'] = 'ي', ['ی'] = 'ي', ['ێ'] = 'ي',
        ['ؤ'] = 'و', ['ۆ'] = 'و',
        ['ة'] = 'ه', ['ە'] = 'ه',
        ['ک'] = 'ك'
    };

    public static IReadOnlySet<string> ArabicKeys(IEnumerable<string?> names)
    {
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var name in names)
        {
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var words = ArabicWords(name);
            AddKey(keys, words);
            AddKey(keys, words.Where(word => !ArabicGenericWords.Contains(word)).ToList());
        }

        return keys;
    }

    public static IReadOnlySet<string> EnglishKeys(IEnumerable<string?> names)
    {
        var keys = new HashSet<string>(StringComparer.Ordinal);
        foreach (var name in names)
        {
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var words = EnglishWords(name);
            AddKey(keys, words.Where(word => !EnglishGenericWords.Contains(word)).Select(CollapseEnglishWord).ToList());
        }

        return keys;
    }

    public static bool HasArabicLetter(string text) => text.Any(IsArabicLetter);

    public static bool IsArabicLetter(char character) => character is >= 'ء' and <= 'ي'
        or >= 'ٮ' and <= 'ۓ'
        or >= 'ݐ' and <= 'ݿ';

    private static void AddKey(HashSet<string> keys, IReadOnlyList<string> words)
    {
        var key = string.Concat(words);
        if (key.Length > 0)
            keys.Add(key);
    }

    private static List<string> ArabicWords(string name)
    {
        var builder = new StringBuilder();
        foreach (var character in name.Normalize(NormalizationForm.FormKC))
        {
            if (character == 'ـ' || CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
                continue;

            var mapped = ArabicLetterMap.GetValueOrDefault(character, character);
            builder.Append(char.IsLetter(mapped) ? mapped : ' ');
        }

        return builder.ToString()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(StripArticle)
            .Where(word => word.Length > 0)
            .ToList();
    }

    private static string StripArticle(string word) =>
        word.Length > DefiniteArticle.Length + 1 && word.StartsWith(DefiniteArticle, StringComparison.Ordinal)
            ? word[DefiniteArticle.Length..]
            : word;

    private static List<string> EnglishWords(string name)
    {
        var builder = new StringBuilder();
        foreach (var character in name.ToLowerInvariant().Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
                continue;

            builder.Append(character is >= 'a' and <= 'z' ? character : ' ');
        }

        return builder.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    private static string CollapseEnglishWord(string word)
    {
        var builder = new StringBuilder();
        foreach (var character in word.Replace("ou", "u").Replace("ee", "i").Replace("oo", "u").Replace("q", "k"))
        {
            if (builder.Length == 0 || builder[^1] != character)
                builder.Append(character);
        }

        return builder.ToString().TrimEnd('h');
    }
}
