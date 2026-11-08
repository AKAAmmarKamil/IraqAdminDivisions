namespace IraqAdminDivisions.Builder.Models;

public static class KurdishNameSources
{
    public const string Wikidata = "wikidata";
    public const string Dataset = "dataset";
    public const string Transliterated = "transliterated";
    public const string Manual = "manual";

    public static readonly IReadOnlyList<string> All = [Wikidata, Dataset, Transliterated, Manual];
}
