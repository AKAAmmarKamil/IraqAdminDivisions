namespace IraqAdminDivisions.Builder.Merge;

public static class KurdishNamePrefix
{
    private static readonly string[] Prefixes =
    [
        "\u067E\u0627\u0631\u06CE\u0632\u06AF\u0627\u06CC ",
        "\u067E\u0627\u0631\u06CE\u0632\u06AF\u0627 ",
        "\u0642\u06D5\u0632\u0627\u06CC ",
        "\u0642\u06D5\u0632\u0627 ",
        "\u0646\u0627\u062D\u06CC\u06D5\u06CC ",
        "\u0646\u0627\u062D\u06CC\u06D5 ",
        "\u0646\u0627\u062D\u06CC\u06D5\u06CC"
    ];

    public static string Strip(string name)
    {
        var trimmed = name.Trim();
        foreach (var prefix in Prefixes)
        {
            if (trimmed.StartsWith(prefix, StringComparison.Ordinal) && trimmed.Length > prefix.Length)
                return trimmed[prefix.Length..].TrimStart();
        }

        return trimmed;
    }
}
