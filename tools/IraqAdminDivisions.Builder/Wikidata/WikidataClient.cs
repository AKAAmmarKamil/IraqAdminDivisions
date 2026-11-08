using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Wikidata;

public sealed class WikidataClient(SparqlRunner sparqlRunner)
{
    public async Task<IReadOnlyDictionary<string, WikidataItem>> LoadAsync(RepoLayout layout, bool shouldRefresh)
    {
        var itemsJson = await sparqlRunner.RunCachedAsync(WikidataQueries.Items, layout.WikidataItemsCachePath, shouldRefresh);
        var labelsJson = await sparqlRunner.RunCachedAsync(WikidataQueries.Labels, layout.WikidataLabelsCachePath, shouldRefresh);
        var items = WikidataResultParser.Parse(itemsJson, labelsJson);
        Console.WriteLine($"Wikidata: {items.Count} candidate items");
        return items;
    }

    public async Task<IsoCodeResult> LoadIsoCodesAsync(RepoLayout layout, bool shouldRefresh, IReadOnlyCollection<string> governorateItemIds)
    {
        var json = await sparqlRunner.RunCachedAsync(WikidataQueries.IsoCodes(governorateItemIds), layout.WikidataIsoCachePath, shouldRefresh);
        var result = IsoCodeResolver.Resolve(governorateItemIds, IsoCodeParser.Parse(json));
        Console.WriteLine($"Wikidata: {result.CodesByItemId.Count} ISO 3166-2 codes");
        return result;
    }
}
