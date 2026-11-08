using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Wikidata;

public static partial class WikidataResultParser
{
    private const string KindLabel = "label";
    private const string LangArabic = "ar";
    private const string LangEnglish = "en";
    private const string LangKurdishCentral = "ckb";

    [GeneratedRegex(@"^Point\(\s*(?<lng>-?[\d.eE+-]+)\s+(?<lat>-?[\d.eE+-]+)\s*\)$")]
    private static partial Regex PointPattern();

    public static Dictionary<string, WikidataItem> Parse(string itemsJson, string labelsJson)
    {
        var items = new Dictionary<string, WikidataItem>(StringComparer.Ordinal);
        ParseItems(itemsJson, items);
        ParseLabels(labelsJson, items);
        return items;
    }

    private static void ParseItems(string json, Dictionary<string, WikidataItem> items)
    {
        foreach (var binding in Bindings(json))
        {
            var id = EntityId(binding, "item");
            if (id is null)
                continue;

            if (!items.TryGetValue(id, out var item))
                items[id] = item = new WikidataItem { Id = id };

            var classId = EntityId(binding, "cls");
            if (classId is not null && WikidataQueries.ClassLevels.TryGetValue(classId, out var level))
                item.Levels.Add(level);

            var parentId = EntityId(binding, "parent");
            if (parentId is not null)
                item.ParentIds.Add(parentId);

            if (item.Lat is null && TryParsePoint(Value(binding, "coord"), out var lat, out var lng))
            {
                item.Lat = lat;
                item.Lng = lng;
            }
        }
    }

    private static void ParseLabels(string json, Dictionary<string, WikidataItem> items)
    {
        foreach (var binding in Bindings(json))
        {
            var id = EntityId(binding, "item");
            var text = Value(binding, "text")?.Trim();
            if (id is null || string.IsNullOrEmpty(text) || !items.TryGetValue(id, out var item))
                continue;

            var language = Value(binding, "lang");
            if (Value(binding, "kind") == KindLabel)
                SetLabel(item, language, text);
            else
                AddAlias(item, language, text);
        }
    }

    private static void SetLabel(WikidataItem item, string? language, string text)
    {
        switch (language)
        {
            case LangArabic: item.LabelAr = text; break;
            case LangEnglish: item.LabelEn = text; break;
            case LangKurdishCentral: item.LabelCkb = text; break;
        }
    }

    private static void AddAlias(WikidataItem item, string? language, string text)
    {
        switch (language)
        {
            case LangArabic: item.AliasesAr.Add(text); break;
            case LangEnglish: item.AliasesEn.Add(text); break;
        }
    }

    private static IEnumerable<JsonElement> Bindings(string json)
    {
        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("results").GetProperty("bindings").EnumerateArray().Select(b => b.Clone()).ToList();
    }

    private static string? Value(JsonElement binding, string name) =>
        binding.TryGetProperty(name, out var cell) ? cell.GetProperty("value").GetString() : null;

    private static string? EntityId(JsonElement binding, string name)
    {
        var value = Value(binding, name);
        return value is not null && value.StartsWith(WikidataQueries.EntityUriPrefix, StringComparison.Ordinal)
            ? value[WikidataQueries.EntityUriPrefix.Length..]
            : null;
    }

    private static bool TryParsePoint(string? wkt, out double lat, out double lng)
    {
        lat = lng = 0;
        if (wkt is null)
            return false;

        var match = PointPattern().Match(wkt);
        return match.Success
            && double.TryParse(match.Groups["lat"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lat)
            && double.TryParse(match.Groups["lng"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out lng);
    }
}
