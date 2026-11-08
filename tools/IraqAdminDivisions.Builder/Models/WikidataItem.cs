namespace IraqAdminDivisions.Builder.Models;

public sealed class WikidataItem
{
    public required string Id { get; init; }
    public HashSet<AdminLevel> Levels { get; } = [];
    public HashSet<string> ParentIds { get; } = [];
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? LabelAr { get; set; }
    public string? LabelEn { get; set; }
    public string? LabelCkb { get; set; }
    public SortedSet<string> AliasesAr { get; } = new(StringComparer.Ordinal);
    public SortedSet<string> AliasesEn { get; } = new(StringComparer.Ordinal);
}
