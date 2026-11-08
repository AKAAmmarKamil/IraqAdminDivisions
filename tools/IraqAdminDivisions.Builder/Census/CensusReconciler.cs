using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Census;

public enum ReconciliationBucket
{
    Matched,
    Renamed,
    Reparented,
    CrossGovernorate,
    Missing,
    Extra
}

/// <summary>One census unit (or one unmatched dataset row for <see cref="ReconciliationBucket.Extra"/>) and what it matched.</summary>
public sealed record ReconciliationRow(
    ReconciliationBucket Bucket,
    AdminLevel Level,
    string Governorate,
    CensusUnit? Census,
    Division? Division,
    Division? CensusParent,
    string? Note);

public sealed record GovernorateTally(
    string Governorate,
    Division? Division,
    int CensusDistricts,
    int CensusSubDistricts,
    int ActiveDistricts,
    int ActiveSubDistricts);

public sealed record ReconciliationResult(IReadOnlyList<GovernorateTally> Tallies, IReadOnlyList<ReconciliationRow> Rows);

/// <summary>Phase 1 of PLAN-1.1: matches every census unit to an existing row by normalised Arabic name (English as fallback) and parent. Changes no data.</summary>
public sealed class CensusReconciler
{
    private const int ArabicScore = 4;
    private const int ArabicOneEditScore = 3;
    private const int EnglishScore = 2;
    private const int ArabicContainsScore = 1;
    private const int MinimumFuzzyKeyLength = 5;
    private const int GovernoratePrefixLength = 5;

    private static readonly IReadOnlyDictionary<char, char> ExtraArabicLetters = new Dictionary<char, char>
    {
        ['ھ'] = 'ه', ['ہ'] = 'ه', ['ۀ'] = 'ه', ['ڕ'] = 'ر', ['ڵ'] = 'ل', ['ڤ'] = 'ف', ['گ'] = 'ك', ['پ'] = 'ب', ['چ'] = 'ج', ['ژ'] = 'ز'
    };

    private readonly IReadOnlyList<Division> _active;
    private readonly Dictionary<string, Division> _byCode;
    private readonly Dictionary<string, Keys> _keys = new(StringComparer.Ordinal);

    public CensusReconciler(IReadOnlyList<Division> divisions)
    {
        _active = divisions.Where(division => !division.IsDeprecated).ToList();
        _byCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
        foreach (var division in _active)
            _keys[division.Code] = KeysOf(division);
    }

    public ReconciliationResult Reconcile(IReadOnlyList<CensusUnit> census)
    {
        var used = new HashSet<string>(StringComparer.Ordinal);
        var rows = new List<ReconciliationRow>();

        var governorates = new Dictionary<string, Division?>(StringComparer.Ordinal);
        foreach (var unit in census.Where(unit => unit.Level == AdminLevel.Governorate))
        {
            var match = BestMatch(unit, _active.Where(division => division.Level == AdminLevel.Governorate), used);
            governorates[unit.Governorate] = match?.Division;
            if (match is not null)
                used.Add(match.Division.Code);
        }

        var districts = new Dictionary<(string, string), Division?>();
        var districtUnits = census.Where(unit => unit.Level == AdminLevel.District).ToList();
        foreach (var (unit, match) in MatchInPasses(districtUnits, AdminLevel.District, governorates, districts, used))
        {
            districts[(unit.Governorate, unit.District)] = match?.Division;
            rows.Add(ToRow(unit, match, governorates.GetValueOrDefault(unit.Governorate), governorates.GetValueOrDefault(unit.Governorate)));
        }

        var subDistrictUnits = census.Where(unit => unit.Level == AdminLevel.SubDistrict).ToList();
        foreach (var (unit, match) in MatchInPasses(subDistrictUnits, AdminLevel.SubDistrict, governorates, districts, used))
            rows.Add(ToRow(unit, match, districts.GetValueOrDefault((unit.Governorate, unit.District)), governorates.GetValueOrDefault(unit.Governorate)));

        foreach (var division in _active.Where(division => division.Level != AdminLevel.Governorate && !used.Contains(division.Code)))
            rows.Add(new ReconciliationRow(ReconciliationBucket.Extra, division.Level, GovernorateName(division, governorates), null, division, null,
                "in the dataset but not in the census list; never removed, at most deprecated with a reason"));

        var tallies = governorates
            .Select(pair => new GovernorateTally(
                pair.Key,
                pair.Value,
                census.Count(unit => unit.Governorate == pair.Key && unit.Level == AdminLevel.District),
                census.Count(unit => unit.Governorate == pair.Key && unit.Level == AdminLevel.SubDistrict && !unit.IsDissolved),
                CountActive(pair.Value, AdminLevel.District),
                CountActive(pair.Value, AdminLevel.SubDistrict)))
            .OrderBy(tally => tally.Division?.Code ?? "~", StringComparer.Ordinal)
            .ToList();

        return new ReconciliationResult(tallies, rows);
    }

    private int CountActive(Division? governorate, AdminLevel level) =>
        governorate is null ? 0 : _active.Count(division => division.Level == level && GovernorateOf(division) == governorate.Code);

    /// <summary>Matches in three passes so that the most specific match claims a row first: same parent, then same governorate, then anywhere.</summary>
    private List<(CensusUnit Unit, Match? Match)> MatchInPasses(
        IReadOnlyList<CensusUnit> units,
        AdminLevel level,
        IReadOnlyDictionary<string, Division?> governorates,
        IReadOnlyDictionary<(string, string), Division?> districts,
        HashSet<string> used)
    {
        var results = new Match?[units.Count];
        var candidates = _active.Where(division => division.Level == level).ToList();
        Func<CensusUnit, Division?> censusParent = level == AdminLevel.District
            ? unit => governorates.GetValueOrDefault(unit.Governorate)
            : unit => districts.GetValueOrDefault((unit.Governorate, unit.District));

        Func<CensusUnit, Division, bool>[] passes =
        [
            (unit, division) => censusParent(unit) is { } parent && division.ParentCode == parent.Code,
            (unit, division) => governorates.GetValueOrDefault(unit.Governorate) is { } governorate && GovernorateOf(division) == governorate.Code,
            (_, _) => true
        ];

        for (var passIndex = 0; passIndex < passes.Length; passIndex++)
        {
            var pass = passes[passIndex];
            var minimumScore = passIndex == passes.Length - 1 ? ArabicOneEditScore : ArabicContainsScore;
            for (var i = 0; i < units.Count; i++)
            {
                if (results[i] is not null)
                    continue;

                var unit = units[i];
                var match = BestMatch(unit, candidates.Where(division => pass(unit, division)), used);
                if (match is null || match.Score < minimumScore)
                    continue;

                results[i] = match;
                used.Add(match.Division.Code);
            }
        }

        return units.Select((unit, index) => (unit, results[index])).ToList();
    }

    private Match? BestMatch(CensusUnit unit, IEnumerable<Division> candidates, HashSet<string> used)
    {
        var unitKeys = KeysOf(unit);
        Match? best = null;
        foreach (var division in candidates)
        {
            if (used.Contains(division.Code))
                continue;

            var keys = _keys[division.Code];
            var score = Score(keys, unitKeys);
            if (score == 0)
                continue;

            var isCentreMatch = division.IsDistrictCentre && unit.NameEn == unit.District;
            if (best is null || score > best.Score || (score == best.Score && isCentreMatch && !best.IsCentreMatch))
                best = new Match(division, score, isCentreMatch);
        }

        return best;
    }

    private static int Score(Keys datasetKeys, Keys censusKeys)
    {
        if (datasetKeys.Arabic.Overlaps(censusKeys.Arabic))
            return ArabicScore;
        if (AnyPair(datasetKeys.Arabic, censusKeys.Arabic, IsOneEditApart))
            return ArabicOneEditScore;
        if (datasetKeys.English.Overlaps(censusKeys.English))
            return EnglishScore;
        if (AnyPair(datasetKeys.Arabic, censusKeys.Arabic, (first, second) => first.Contains(second, StringComparison.Ordinal) || second.Contains(first, StringComparison.Ordinal)))
            return ArabicContainsScore;
        return 0;
    }

    private static bool AnyPair(IReadOnlySet<string> first, IReadOnlySet<string> second, Func<string, string, bool> predicate) =>
        first.Any(left => left.Length >= MinimumFuzzyKeyLength && second.Any(right => right.Length >= MinimumFuzzyKeyLength && predicate(left, right)));

    private static bool IsOneEditApart(string first, string second)
    {
        if (Math.Abs(first.Length - second.Length) > 1)
            return false;

        var shorter = first.Length <= second.Length ? first : second;
        var longer = first.Length <= second.Length ? second : first;
        var index = 0;
        while (index < shorter.Length && shorter[index] == longer[index])
            index++;

        return shorter.AsSpan(shorter.Length == longer.Length ? Math.Min(index + 1, shorter.Length) : index)
            .SequenceEqual(longer.AsSpan(Math.Min(index + 1, longer.Length)));
    }

    private static string MatchNote(int score) => score switch
    {
        ArabicOneEditScore => "Arabic spellings differ by one letter",
        EnglishScore => "matched by English only",
        ArabicContainsScore => "one Arabic name contains the other",
        _ => ""
    };

    private ReconciliationRow ToRow(CensusUnit unit, Match? match, Division? censusParent, Division? censusGovernorateDivision)
    {
        if (match is null)
        {
            var note = unit.IsDissolved ? "dissolved in the census list; nothing to add" : censusParent is null ? "needs a new row; its census parent is new too" : "needs a new row";
            return new ReconciliationRow(ReconciliationBucket.Missing, unit.Level, unit.Governorate, unit, null, censusParent, note);
        }

        var division = match.Division;
        var censusGovernorate = censusGovernorateDivision?.Code;
        if (censusGovernorate is not null && GovernorateOf(division) != censusGovernorate)
            return new ReconciliationRow(ReconciliationBucket.CrossGovernorate, unit.Level, unit.Governorate, unit, division, censusParent,
                $"listed in {censusGovernorateDivision!.Name.En}{(censusParent is null ? $" under '{unit.District}' (new there)" : $" under {censusParent.Name.En}")} but the dataset has it in {GovernorateName(division)}; a cross-governorate move changes the code (deprecate + replacedBy){FuzzySuffix(match)}");

        if (censusParent is null && unit.Level == AdminLevel.SubDistrict)
            return new ReconciliationRow(ReconciliationBucket.Reparented, unit.Level, unit.Governorate, unit, division, null,
                $"census parent '{unit.District}' is a new district; reparent from {division.ParentCode}{FuzzySuffix(match)}");

        if (censusParent is not null && division.ParentCode != censusParent.Code)
            return new ReconciliationRow(ReconciliationBucket.Reparented, unit.Level, unit.Governorate, unit, division, censusParent,
                $"reparent from {division.ParentCode} to {censusParent.Code}{FuzzySuffix(match)}");

        if (match.Score < ArabicScore)
            return new ReconciliationRow(ReconciliationBucket.Renamed, unit.Level, unit.Governorate, unit, division, censusParent,
                $"{MatchNote(match.Score)}; check it is the same unit, then the census spelling becomes an alias");

        return new ReconciliationRow(ReconciliationBucket.Matched, unit.Level, unit.Governorate, unit, division, censusParent, null);
    }

    private static string FuzzySuffix(Match match) => match.Score < ArabicScore ? $" ({MatchNote(match.Score)})" : "";

    private string GovernorateOf(Division division) => division.Code[..GovernoratePrefixLength];

    private string GovernorateName(Division division) =>
        _byCode.TryGetValue(GovernorateOf(division), out var governorate) ? governorate.Name.En : GovernorateOf(division);

    private string GovernorateName(Division division, IReadOnlyDictionary<string, Division?> censusGovernorates) =>
        censusGovernorates.FirstOrDefault(pair => pair.Value?.Code == GovernorateOf(division)).Key ?? GovernorateName(division);

    private static Keys KeysOf(Division division)
    {
        var arabicNames = new List<string?> { division.Name.Ar, division.Name.Ku };
        arabicNames.AddRange(division.Aliases.Where(NameNormalizer.HasArabicLetter));
        var englishNames = new List<string?> { division.Name.En };
        englishNames.AddRange(division.Aliases.Where(alias => !NameNormalizer.HasArabicLetter(alias)));
        return new Keys(ArabicKeys(arabicNames), NameNormalizer.EnglishKeys(englishNames));
    }

    private static Keys KeysOf(CensusUnit unit)
    {
        var englishNames = unit.NameEn
            .Split(['[', ']', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(name => (string?)name);
        return new Keys(ArabicKeys([unit.NameNative]), NameNormalizer.EnglishKeys(englishNames));
    }

    private static IReadOnlySet<string> ArabicKeys(IEnumerable<string?> names) =>
        NameNormalizer.ArabicKeys(names.Select(name => name is null ? null : new string(name.Select(character => ExtraArabicLetters.GetValueOrDefault(character, character)).ToArray())));

    private sealed record Keys(IReadOnlySet<string> Arabic, IReadOnlySet<string> English);

    private sealed record Match(Division Division, int Score, bool IsCentreMatch);
}
