using System.Globalization;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Matching;

public sealed class WikidataMatcher
{
    private const double DominantDistanceRatio = 3;
    private const double ConsistentParentDistanceFactor = 2;
    private const double ParentOverrideDistanceFactor = 0.5;

    private readonly IReadOnlyDictionary<string, WikidataItem> _items;
    private readonly Dictionary<string, (IReadOnlySet<string> Arabic, IReadOnlySet<string> English)> _itemKeys = new();

    public WikidataMatcher(IReadOnlyDictionary<string, WikidataItem> items)
    {
        _items = items;
        foreach (var item in items.Values)
        {
            _itemKeys[item.Id] = (
                NameNormalizer.ArabicKeys([item.LabelAr, .. item.AliasesAr]),
                NameNormalizer.EnglishKeys([item.LabelEn, .. item.AliasesEn]));
        }
    }

    public IReadOnlyList<MatchOutcome> Match(IReadOnlyList<OchaRow> rows)
    {
        var outcomes = new Dictionary<string, MatchOutcome>(StringComparer.Ordinal);
        var rowsByCode = rows.ToDictionary(row => row.Code, StringComparer.Ordinal);

        foreach (var level in Enum.GetValues<AdminLevel>())
        {
            var levelOutcomes = rows
                .Where(row => row.Level == level)
                .OrderBy(row => row.Code, StringComparer.Ordinal)
                .Select(row => MatchRow(row, rowsByCode, outcomes))
                .ToList();

            foreach (var outcome in RejectSharedItems(levelOutcomes))
                outcomes[outcome.Row.Code] = outcome;
        }

        return outcomes.Values.OrderBy(outcome => outcome.Row.Code, StringComparer.Ordinal).ToList();
    }

    private MatchOutcome MatchRow(OchaRow row, Dictionary<string, OchaRow> rowsByCode, Dictionary<string, MatchOutcome> outcomes)
    {
        var expectedAncestorIds = MatchedAncestorIds(row, rowsByCode, outcomes);
        var arabicKeys = NameNormalizer.ArabicKeys([row.NameAr]);
        var englishKeys = NameNormalizer.EnglishKeys([row.NameEn, row.RefName]);

        var nameCandidates = _items.Values
            .Where(item => item.Levels.Contains(row.Level) && NameMatches(item, arabicKeys, englishKeys))
            .ToList();
        if (nameCandidates.Count == 0)
            return new MatchOutcome(row, MatchStatus.Unmatched, null, "no Wikidata item with a matching name at this level");

        var accepted = nameCandidates
            .Select(item => Evaluate(row, item, expectedAncestorIds))
            .Where(candidate => candidate.IsAccepted)
            .ToList();
        if (accepted.Count == 0)
        {
            var evaluated = nameCandidates.Select(item => Evaluate(row, item, expectedAncestorIds)).ToList();
            var nameOnlyItemIds = evaluated.Where(IsNameOnly).Select(candidate => candidate.Item.Id).ToList();
            return new MatchOutcome(row, MatchStatus.Unmatched, null, $"name matched but rejected: {DescribeRejected(evaluated)}", nameOnlyItemIds);
        }

        return Pick(row, accepted);
    }

    private static MatchOutcome Pick(OchaRow row, List<Candidate> accepted)
    {
        var bestTier = accepted.Any(candidate => candidate.ParentState == ParentCheck.Consistent)
            ? accepted.Where(candidate => candidate.ParentState == ParentCheck.Consistent).ToList()
            : accepted;
        var ranked = bestTier.OrderBy(candidate => candidate.DistanceKm ?? double.MaxValue).ToList();
        var best = ranked[0];

        if (ranked.Count == 1 || IsDominant(best, ranked[1], BuildSettings.MatchThresholdKm[row.Level]))
            return new MatchOutcome(row, MatchStatus.Matched, best.Item, DescribeMatch(best));

        return new MatchOutcome(row, MatchStatus.Ambiguous, null, $"{ranked.Count} candidates: {DescribeRejected(ranked)}");
    }

    private static bool IsNameOnly(Candidate candidate) =>
        IsNameOnly(candidate.DistanceKm, candidate.ParentState);

    private static bool IsNameOnly(double? distanceKm, ParentCheck parentState) =>
        distanceKm is null && parentState == ParentCheck.Unknown;

    private static bool IsDominant(Candidate best, Candidate runnerUp, double thresholdKm) =>
        best.DistanceKm is { } bestDistance
        && (runnerUp.DistanceKm is null
            ? bestDistance <= thresholdKm
            : runnerUp.DistanceKm >= bestDistance * DominantDistanceRatio);

    private Candidate Evaluate(OchaRow row, WikidataItem item, IReadOnlySet<string> expectedAncestorIds)
    {
        double? distanceKm = row.Lat is { } rowLat && row.Lng is { } rowLng && item.Lat is { } itemLat && item.Lng is { } itemLng
            ? GeoDistance.Kilometers(rowLat, rowLng, itemLat, itemLng)
            : null;

        var parentState = ParentState(item, expectedAncestorIds);
        var thresholdKm = BuildSettings.MatchThresholdKm[row.Level];
        var isAccepted = !IsNameOnly(distanceKm, parentState) && parentState switch
        {
            ParentCheck.Consistent => distanceKm is null || distanceKm <= thresholdKm * ConsistentParentDistanceFactor,
            ParentCheck.Unknown => distanceKm is null || distanceKm <= thresholdKm,
            _ => distanceKm <= thresholdKm * ParentOverrideDistanceFactor
        };

        return new Candidate(item, distanceKm, parentState, isAccepted);
    }

    private ParentCheck ParentState(WikidataItem item, IReadOnlySet<string> expectedAncestorIds)
    {
        var itemAncestorIds = AncestorIds(item);
        if (expectedAncestorIds.Count == 0 || itemAncestorIds.Count == 0)
            return ParentCheck.Unknown;

        return itemAncestorIds.Overlaps(expectedAncestorIds) ? ParentCheck.Consistent : ParentCheck.Inconsistent;
    }

    private HashSet<string> AncestorIds(WikidataItem item)
    {
        var ancestorIds = new HashSet<string>(StringComparer.Ordinal);
        var pending = new Stack<string>(item.ParentIds);
        while (pending.Count > 0)
        {
            var parentId = pending.Pop();
            if (!ancestorIds.Add(parentId) || !_items.TryGetValue(parentId, out var parent))
                continue;

            foreach (var grandParentId in parent.ParentIds)
                pending.Push(grandParentId);
        }

        return ancestorIds;
    }

    private bool NameMatches(WikidataItem item, IReadOnlySet<string> arabicKeys, IReadOnlySet<string> englishKeys)
    {
        var (itemArabic, itemEnglish) = _itemKeys[item.Id];
        return itemArabic.Overlaps(arabicKeys) || itemEnglish.Overlaps(englishKeys);
    }

    private static HashSet<string> MatchedAncestorIds(OchaRow row, Dictionary<string, OchaRow> rowsByCode, Dictionary<string, MatchOutcome> outcomes)
    {
        var ids = new HashSet<string>(StringComparer.Ordinal);
        var parentCode = row.ParentCode;
        while (parentCode is not null && rowsByCode.TryGetValue(parentCode, out var parent))
        {
            if (outcomes.TryGetValue(parent.Code, out var outcome) && outcome.Item is not null)
                ids.Add(outcome.Item.Id);
            parentCode = parent.ParentCode;
        }

        return ids;
    }

    private static IEnumerable<MatchOutcome> RejectSharedItems(List<MatchOutcome> levelOutcomes)
    {
        var sharedItemIds = levelOutcomes
            .Where(outcome => outcome.Item is not null)
            .GroupBy(outcome => outcome.Item!.Id)
            .Where(group => group.Count() > 1)
            .ToDictionary(group => group.Key, group => string.Join(", ", group.Select(outcome => outcome.Row.Code)));

        foreach (var outcome in levelOutcomes)
        {
            if (outcome.Item is not null && sharedItemIds.TryGetValue(outcome.Item.Id, out var codes))
                yield return new MatchOutcome(outcome.Row, MatchStatus.Ambiguous, null, $"{outcome.Item.Id} claimed by several rows: {codes}");
            else
                yield return outcome;
        }
    }

    private static string DescribeMatch(Candidate candidate) =>
        $"{candidate.Item.Id}, {DescribeEvidence(candidate)}";

    private static string DescribeEvidence(Candidate candidate)
    {
        var distance = candidate.DistanceKm is { } km ? $"{km.ToString("0.0", CultureInfo.InvariantCulture)} km" : "no coordinates";
        return $"{distance}, parent {candidate.ParentState.ToString().ToLowerInvariant()}";
    }

    private static string DescribeRejected(IEnumerable<Candidate> candidates) =>
        string.Join("; ", candidates.Select(candidate => $"{candidate.Item.Id} ({candidate.Item.LabelEn ?? candidate.Item.LabelAr}: {DescribeEvidence(candidate)})"));

    private enum ParentCheck
    {
        Unknown,
        Consistent,
        Inconsistent
    }

    private sealed record Candidate(WikidataItem Item, double? DistanceKm, ParentCheck ParentState, bool IsAccepted);
}
