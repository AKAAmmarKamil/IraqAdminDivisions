namespace IraqAdminDivisions;

internal static class SearchNormalizer
{
    public const int StackBufferLength = 256;

    private const string DefiniteArticle = "ال";
    private const int MinimumLengthAfterArticle = 2;

    public static string Normalize(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "";
        }

        Span<char> buffer = text!.Length <= StackBufferLength ? stackalloc char[StackBufferLength] : new char[text.Length];
        return NormalizeInto(text, buffer).ToString();
    }

    public static ReadOnlySpan<char> NormalizeInto(string? text, Span<char> destination)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return ReadOnlySpan<char>.Empty;
        }

        return ContainsArabicLetters(text!) ? NormalizeArabic(text!, destination) : NormalizeEnglish(text!, destination);
    }

    private static bool ContainsArabicLetters(string text)
    {
        foreach (var character in text)
        {
            if (character >= '؀' && character <= 'ۿ')
            {
                return true;
            }
        }

        return false;
    }

    private static ReadOnlySpan<char> NormalizeArabic(string text, Span<char> destination)
    {
        var length = 0;
        var isPreviousSpace = true;
        foreach (var character in text)
        {
            if (IsDiacriticOrTatweel(character))
            {
                continue;
            }

            var mapped = MapArabicLetter(character);
            if (char.IsWhiteSpace(mapped))
            {
                if (!isPreviousSpace)
                {
                    destination[length++] = ' ';
                }

                isPreviousSpace = true;
                continue;
            }

            destination[length++] = mapped;
            isPreviousSpace = false;
        }

        if (length > 0 && destination[length - 1] == ' ')
        {
            length--;
        }

        ReadOnlySpan<char> normalized = destination.Slice(0, length);
        return normalized.StartsWith(DefiniteArticle.AsSpan(), StringComparison.Ordinal) && length > DefiniteArticle.Length + MinimumLengthAfterArticle - 1
            ? normalized.Slice(DefiniteArticle.Length)
            : normalized;
    }

    private static bool IsDiacriticOrTatweel(char character) =>
        character == 'ـ' || (character >= 'ً' && character <= 'ٟ') || character == 'ٰ';

    private static char MapArabicLetter(char character) => character switch
    {
        'أ' or 'إ' or 'آ' => 'ا',
        'ة' or 'ە' => 'ه',
        'ى' or 'ئ' or 'ی' => 'ي',
        'ؤ' => 'و',
        'ک' => 'ك',
        _ => character
    };

    private static ReadOnlySpan<char> NormalizeEnglish(string text, Span<char> destination)
    {
        var start = 0;
        var end = text.Length;
        while (char.IsWhiteSpace(text[start]))
        {
            start++;
        }

        while (char.IsWhiteSpace(text[end - 1]))
        {
            end--;
        }

        if (HasEnglishArticle(text, start, end))
        {
            start += 3;
        }

        var length = 0;
        for (var i = start; i < end; i++)
        {
            var character = text[i];
            if (IsIgnoredInEnglish(character))
            {
                continue;
            }

            destination[length++] = char.ToLowerInvariant(character);
        }

        return destination.Slice(0, length);
    }

    private static bool HasEnglishArticle(string text, int start, int end)
    {
        if (end - start < 3)
        {
            return false;
        }

        var first = char.ToLowerInvariant(text[start]);
        var second = char.ToLowerInvariant(text[start + 1]);
        var third = text[start + 2];
        return (first == 'a' && second == 'l' && (third == '-' || third == ' '))
            || (first == 'e' && second == 'l' && third == '-');
    }

    private static bool IsIgnoredInEnglish(char character) =>
        character == '-' || character == '\'' || character == '’' || character == '‘' || character == '`' || char.IsWhiteSpace(character);
}
