using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Overrides;

public sealed class OverridesValidator
{
    private const string CorrectionsFileName = "corrections.tsv";
    private const string TransliterationsFileName = "kurdish-transliterated.tsv";

    public IReadOnlyList<string> Validate(OverridesData overrides, IReadOnlyList<OchaRow> rows)
    {
        var issues = new List<string>(overrides.Issues);
        var knownCodes = rows.Select(row => row.Code).ToHashSet(StringComparer.Ordinal);

        foreach (var correction in overrides.Corrections.Where(correction => !knownCodes.Contains(correction.Code)))
            issues.Add($"{CorrectionsFileName} line {correction.LineNumber}: code {correction.Code} does not exist");

        foreach (var transliteration in overrides.Transliterations.Where(entry => !knownCodes.Contains(entry.Code)))
            issues.Add($"{TransliterationsFileName} line {transliteration.LineNumber}: code {transliteration.Code} does not exist");

        foreach (var group in overrides.Corrections.GroupBy(entry => (entry.Code, entry.Field)).Where(group => group.Count() > 1))
            issues.Add($"{CorrectionsFileName}: {group.Key.Code} has {group.Count()} corrections for field {group.Key.Field}");

        foreach (var group in overrides.Transliterations.GroupBy(entry => entry.Code, StringComparer.Ordinal).Where(group => group.Count() > 1))
            issues.Add($"{TransliterationsFileName}: {group.Key} appears {group.Count()} times");

        return issues;
    }
}
