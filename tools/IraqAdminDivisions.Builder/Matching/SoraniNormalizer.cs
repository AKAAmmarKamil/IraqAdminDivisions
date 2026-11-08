using System.Globalization;
using System.Text;

namespace IraqAdminDivisions.Builder.Matching;

public enum KurdishAgreement
{
    Exact,
    Near,
    Different
}

public static class SoraniNormalizer
{
    private const char Space = ' ';

    private static readonly IReadOnlyDictionary<char, char> LetterVariants = new Dictionary<char, char>
    {
        ['ي'] = 'ی', ['ى'] = 'ی',
        ['ك'] = 'ک',
        ['ه'] = 'ە', ['ھ'] = 'ە', ['ہ'] = 'ە'
    };

    private static readonly IReadOnlyDictionary<char, char> SkeletonLetters = new Dictionary<char, char>
    {
        ['ڕ'] = 'ر', ['ڵ'] = 'ل', ['ێ'] = 'ی', ['ۆ'] = 'و'
    };

    private static readonly HashSet<char> SkeletonDroppedLetters = ['ە', 'ئ'];

    public static string Key(string text)
    {
        var builder = new StringBuilder();
        foreach (var character in text.Normalize(NormalizationForm.FormKC))
        {
            if (character is '‌' or '‍' or 'ـ' || CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
                continue;

            var mapped = LetterVariants.GetValueOrDefault(character, character);
            builder.Append(char.IsWhiteSpace(mapped) ? Space : mapped);
        }

        return string.Join(Space, builder.ToString().Split(Space, StringSplitOptions.RemoveEmptyEntries));
    }

    public static string Skeleton(string text)
    {
        var builder = new StringBuilder();
        foreach (var character in Key(text))
        {
            var mapped = SkeletonLetters.GetValueOrDefault(character, character);
            if (mapped == Space || SkeletonDroppedLetters.Contains(mapped) || (builder.Length > 0 && builder[^1] == mapped))
                continue;

            builder.Append(mapped);
        }

        return builder.ToString();
    }

    public static KurdishAgreement Compare(string ours, string theirs)
    {
        var ourKey = Key(ours);
        var theirKey = Key(theirs);
        if (ourKey == theirKey)
            return KurdishAgreement.Exact;

        var isNear = Skeleton(ourKey) == Skeleton(theirKey)
            || IsWithinOneEdit(ourKey.Replace(Space.ToString(), string.Empty), theirKey.Replace(Space.ToString(), string.Empty));
        return isNear ? KurdishAgreement.Near : KurdishAgreement.Different;
    }

    private static bool IsWithinOneEdit(string first, string second)
    {
        if (Math.Abs(first.Length - second.Length) > 1)
            return false;

        var (shorter, longer) = first.Length <= second.Length ? (first, second) : (second, first);
        var prefix = 0;
        while (prefix < shorter.Length && shorter[prefix] == longer[prefix])
            prefix++;

        if (shorter.Length == longer.Length)
            return prefix == shorter.Length || shorter[(prefix + 1)..] == longer[(prefix + 1)..];

        return shorter[prefix..] == longer[(prefix + 1)..];
    }
}
