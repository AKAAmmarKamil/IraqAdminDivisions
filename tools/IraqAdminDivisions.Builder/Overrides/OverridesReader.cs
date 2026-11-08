namespace IraqAdminDivisions.Builder.Overrides;

public sealed class OverridesReader
{
    private const char Separator = '\t';
    private const int CorrectionColumnCount = 4;
    private const int TransliterationColumnCount = 2;

    public OverridesData Read(string correctionsPath, string transliterationsPath)
    {
        var issues = new List<string>();
        var corrections = ReadCorrections(correctionsPath, issues);
        var transliterations = ReadTransliterations(transliterationsPath, issues);
        return new OverridesData(corrections, transliterations, issues);
    }

    private static List<Correction> ReadCorrections(string path, List<string> issues)
    {
        var corrections = new List<Correction>();
        foreach (var (lineNumber, columns) in ReadRows(path, CorrectionColumnCount, issues))
        {
            var field = columns[1].Trim();
            if (!OverrideFields.All.Contains(field, StringComparer.Ordinal))
            {
                issues.Add($"{Path.GetFileName(path)} line {lineNumber}: unknown field '{field}'");
                continue;
            }

            if (string.IsNullOrWhiteSpace(columns[2]))
            {
                issues.Add($"{Path.GetFileName(path)} line {lineNumber}: empty value");
                continue;
            }

            corrections.Add(new Correction(columns[0].Trim(), field, columns[2].Trim(), columns[3].Trim(), lineNumber));
        }

        return corrections;
    }

    private static List<Transliteration> ReadTransliterations(string path, List<string> issues) =>
        ReadRows(path, TransliterationColumnCount, issues)
            .Select(row => new Transliteration(row.Columns[0].Trim(), row.Columns[1].Trim(), row.LineNumber))
            .ToList();

    private static IEnumerable<(int LineNumber, string[] Columns)> ReadRows(string path, int columnCount, List<string> issues)
    {
        if (!File.Exists(path))
        {
            issues.Add($"{Path.GetFileName(path)}: file not found");
            yield break;
        }

        var lines = File.ReadAllLines(path);
        for (var i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            var columns = lines[i].Split(Separator);
            if (columns.Length < columnCount || string.IsNullOrWhiteSpace(columns[0]))
            {
                issues.Add($"{Path.GetFileName(path)} line {i + 1}: expected {columnCount} tab-separated columns");
                continue;
            }

            yield return (i + 1, columns);
        }
    }
}
