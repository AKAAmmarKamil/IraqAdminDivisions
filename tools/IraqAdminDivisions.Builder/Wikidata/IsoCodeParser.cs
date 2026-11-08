using System.Text.Json;

namespace IraqAdminDivisions.Builder.Wikidata;

public static class IsoCodeParser
{
    private const string PreferredRank = "PreferredRank";
    private const string DeprecatedRank = "DeprecatedRank";

    public static IReadOnlyList<IsoStatement> Parse(string json)
    {
        using var document = JsonDocument.Parse(json);
        var statements = new List<IsoStatement>();
        foreach (var binding in document.RootElement.GetProperty("results").GetProperty("bindings").EnumerateArray())
        {
            var itemId = Value(binding, "item");
            var code = Value(binding, "code");
            var rank = Value(binding, "rank");
            if (itemId is null || code is null || rank is null || rank.EndsWith(DeprecatedRank, StringComparison.Ordinal))
                continue;

            statements.Add(new IsoStatement(
                itemId[WikidataQueries.EntityUriPrefix.Length..],
                code.Trim(),
                Value(binding, "end") is not null,
                rank.EndsWith(PreferredRank, StringComparison.Ordinal)));
        }

        return statements;
    }

    private static string? Value(JsonElement binding, string name) =>
        binding.TryGetProperty(name, out var cell) ? cell.GetProperty("value").GetString() : null;
}
