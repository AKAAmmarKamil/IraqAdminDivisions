namespace IraqAdminDivisions.Builder.Overrides;

public sealed record OverridesData(
    IReadOnlyList<Correction> Corrections,
    IReadOnlyList<Transliteration> Transliterations,
    IReadOnlyList<string> Issues)
{
    public IReadOnlyDictionary<(string Code, string Field), Correction> CorrectionsByKey { get; } =
        BuildCorrectionIndex(Corrections);

    public IReadOnlyDictionary<string, Transliteration> TransliterationsByCode { get; } =
        Transliterations
            .GroupBy(entry => entry.Code, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);

    private static Dictionary<(string Code, string Field), Correction> BuildCorrectionIndex(IReadOnlyList<Correction> corrections) =>
        corrections
            .GroupBy(entry => (entry.Code, entry.Field))
            .ToDictionary(group => group.Key, group => group.First());
}
