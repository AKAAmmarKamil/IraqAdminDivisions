namespace IraqAdminDivisions.Builder.Matching;

public static class SoraniAlphabet
{
    private const char Space = ' ';

    private static readonly HashSet<char> Letters =
    [
        '\u0626', '\u0627', '\u0628', '\u067E', '\u062A', '\u062C', '\u0686', '\u062D', '\u062E', '\u062F',
        '\u0631', '\u0695', '\u0632', '\u0698', '\u0633', '\u0634', '\u0639', '\u063A', '\u0641', '\u06A4',
        '\u0642', '\u06A9', '\u06AF', '\u0644', '\u06B5', '\u0645', '\u0646', '\u06BE', '\u0648', '\u06C6',
        '\u06D5', '\u06CC', '\u06CE'
    ];

    public static IReadOnlyList<char> ForeignCharacters(string text) =>
        text.Where(letter => letter != Space && !Letters.Contains(letter)).Distinct().ToList();

    public static bool IsSorani(string text) => ForeignCharacters(text).Count == 0;
}
