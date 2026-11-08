using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Wikidata;

public static class WikidataQueries
{
    public const string SparqlEndpoint = "https://query.wikidata.org/sparql";
    public const string EntityUriPrefix = "http://www.wikidata.org/entity/";

    public static readonly IReadOnlyDictionary<string, AdminLevel> ClassLevels = new Dictionary<string, AdminLevel>
    {
        ["Q841753"] = AdminLevel.Governorate,
        ["Q4117798"] = AdminLevel.District,
        ["Q12249052"] = AdminLevel.SubDistrict,
        ["Q1087624"] = AdminLevel.SubDistrict
    };

    private static readonly string ClassValues = string.Join(' ', ClassLevels.Keys.Select(id => $"wd:{id}"));

    public static readonly string Items = $$"""
        SELECT ?item ?cls ?parent ?coord WHERE {
          VALUES ?cls { {{ClassValues}} }
          ?item wdt:P31 ?cls .
          OPTIONAL { ?item wdt:P131 ?parent }
          OPTIONAL { ?item wdt:P625 ?coord }
        }
        """;

    public static readonly string Labels = $$"""
        SELECT ?item ?kind ?lang ?text WHERE {
          VALUES ?cls { {{ClassValues}} }
          ?item wdt:P31 ?cls .
          { ?item rdfs:label ?text BIND("label" AS ?kind) } UNION { ?item skos:altLabel ?text BIND("alias" AS ?kind) }
          BIND(LANG(?text) AS ?lang)
          FILTER(?lang IN ("ar", "en", "ckb"))
        }
        """;

    public static string IsoCodes(IEnumerable<string> itemIds) => $$"""
        SELECT ?item ?code ?rank ?end WHERE {
          VALUES ?item { {{string.Join(' ', itemIds.OrderBy(id => id, StringComparer.Ordinal).Select(id => $"wd:{id}"))}} }
          ?item p:P300 ?statement .
          ?statement ps:P300 ?code ; wikibase:rank ?rank .
          OPTIONAL { ?statement pq:P582 ?end }
        }
        """;
}
