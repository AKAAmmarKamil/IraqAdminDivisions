using System.Globalization;
using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class DatasetMatcher
{
    private const double DominantDistanceRatio = 3;
    private const double MaxDistanceFactor = 4;

    private readonly IReadOnlyList<Division> _divisions;
    private readonly Dictionary<string, Division> _divisionsByCode;
    private readonly Dictionary<string, NameKeys> _divisionKeys;

    public DatasetMatcher(IReadOnlyList<Division> divisions)
    {
        _divisions = divisions;
        _divisionsByCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
        _divisionKeys = divisions.ToDictionary(
            division => division.Code,
            division => KeysOf(division.Name.Ar, division.Name.En, division.Aliases),
            StringComparer.Ordinal);
    }

    public DatasetMatchResult Match(IReadOnlyList<DatasetRow> rows)
    {
        var governorateCodes = new Dictionary<string, string>(StringComparer.Ordinal);
        var attempts = new List<(DatasetRow Row, DatasetMatch? Match, string? Failure)>();

        foreach (var row in rows.Where(row => row.Level == AdminLevel.Governorate))
        {
            var attempt = MatchGovernorate(row);
            attempts.Add((row, attempt.Match, attempt.Failure));
            if (attempt.Match is not null)
                governorateCodes[row.Id] = attempt.Match.Code;
        }

        foreach (var row in rows.Where(row => row.Level != AdminLevel.Governorate))
        {
            var scope = row.GovernorateId is null ? null : governorateCodes.GetValueOrDefault(row.GovernorateId);
            var attempt = MatchByPCode(row) is { } pcodeMatch ? (Match: pcodeMatch, Failure: null) : MatchByName(row, scope);
            attempts.Add((row, attempt.Match, attempt.Failure));
        }

        return ResolveClaims(attempts);
    }

    private (DatasetMatch? Match, string? Failure) MatchGovernorate(DatasetRow row)
    {
        var keys = KeysOf(row.Ar, row.En, row.Aliases);
        var candidates = _divisions.Where(division => division.Level == AdminLevel.Governorate && Overlaps(keys, _divisionKeys[division.Code])).ToList();
        return candidates.Count switch
        {
            1 => (new DatasetMatch(row, candidates[0].Code, DatasetMatchMethods.Name), null),
            0 => (null, "no governorate with a matching name"),
            _ => (null, $"several governorates match the name: {string.Join(", ", candidates.Select(candidate => candidate.Code))}")
        };
    }

    private DatasetMatch? MatchByPCode(DatasetRow row)
    {
        if (row.PCode is null || !_divisionsByCode.TryGetValue(row.PCode, out var division) || division.IsDistrictCentre != row.IsCentre)
            return null;

        return Overlaps(KeysOf(row.Ar, row.En, row.Aliases), _divisionKeys[division.Code])
            ? new DatasetMatch(row, division.Code, DatasetMatchMethods.PCode)
            : null;
    }

    private (DatasetMatch? Match, string? Failure) MatchByName(DatasetRow row, string? governorateCode)
    {
        var keys = KeysOf(row.Ar, row.En, row.Aliases);
        var candidates = _divisions
            .Where(division => division.Level != AdminLevel.Governorate
                && division.IsDistrictCentre == row.IsCentre
                && (governorateCode is null || division.Code.StartsWith(governorateCode, StringComparison.Ordinal))
                && Overlaps(keys, _divisionKeys[division.Code]))
            .ToList();
        if (candidates.Count == 0)
            return (null, governorateCode is null ? "no division with a matching name (governorate unresolved)" : "no division with a matching name in the governorate");

        var sameLevel = candidates.Where(division => division.Level == row.Level).ToList();
        if (sameLevel.Count > 0)
            candidates = sameLevel;

        var isDistanceUsed = false;
        if (candidates.Count > 1)
        {
            var nearest = NearestDominant(row, candidates);
            if (nearest is null)
                return (null, $"ambiguous: {string.Join(", ", candidates.Select(candidate => candidate.Code))}");

            candidates = [nearest];
            isDistanceUsed = true;
        }

        var chosen = candidates[0];
        var distanceKm = DistanceKm(row, chosen);
        if (distanceKm is { } km && km > BuildSettings.MatchThresholdKm[chosen.Level] * MaxDistanceFactor)
            return (null, $"name matches {chosen.Code} but its centroid is {km.ToString("0", CultureInfo.InvariantCulture)} km away");

        if (governorateCode is null && distanceKm is null)
            return (null, $"name matches {chosen.Code} but the governorate is unresolved and there are no coordinates");

        var method = governorateCode is null
            ? DatasetMatchMethods.NameAndDistance
            : isDistanceUsed ? DatasetMatchMethods.NameInGovernorateAndDistance : DatasetMatchMethods.NameInGovernorate;
        return (new DatasetMatch(row, chosen.Code, method), null);
    }

    private static Division? NearestDominant(DatasetRow row, List<Division> candidates)
    {
        var ranked = candidates
            .Select(candidate => (Division: candidate, DistanceKm: DistanceKm(row, candidate)))
            .Where(entry => entry.DistanceKm is not null)
            .OrderBy(entry => entry.DistanceKm)
            .ToList();
        if (ranked.Count != candidates.Count)
            return null;

        return ranked[1].DistanceKm >= ranked[0].DistanceKm * DominantDistanceRatio ? ranked[0].Division : null;
    }

    private static double? DistanceKm(DatasetRow row, Division division) =>
        row.Lat is { } lat && row.Lng is { } lng
            ? GeoDistance.Kilometers(lat, lng, division.Center.Lat, division.Center.Lng)
            : null;

    private DatasetMatchResult ResolveClaims(List<(DatasetRow Row, DatasetMatch? Match, string? Failure)> attempts)
    {
        var matches = new List<DatasetMatch>();
        var unmatched = attempts.Where(attempt => attempt.Match is null)
            .Select(attempt => new UnmatchedDatasetRow(attempt.Row, attempt.Failure!))
            .ToList();

        foreach (var group in attempts.Where(attempt => attempt.Match is not null).GroupBy(attempt => attempt.Match!.Code, StringComparer.Ordinal))
        {
            var claims = group.Select(attempt => attempt.Match!).ToList();
            var winner = claims.Count == 1 ? claims[0] : PickWinner(claims, _divisionsByCode[group.Key].Level);
            if (winner is not null)
                matches.Add(winner);

            unmatched.AddRange(claims.Where(claim => claim != winner)
                .Select(claim => new UnmatchedDatasetRow(claim.Row, $"{group.Key} is claimed by several rows of this dataset")));
        }

        return new DatasetMatchResult(
            matches.OrderBy(match => match.Code, StringComparer.Ordinal).ToList(),
            unmatched.OrderBy(entry => entry.Row.Id, StringComparer.Ordinal).ToList());
    }

    private static DatasetMatch? PickWinner(List<DatasetMatch> claims, AdminLevel divisionLevel)
    {
        var byPCode = claims.Where(claim => claim.Method == DatasetMatchMethods.PCode).ToList();
        if (byPCode.Count == 1)
            return byPCode[0];

        var sameLevel = claims.Where(claim => claim.Row.Level == divisionLevel).ToList();
        return sameLevel.Count == 1 ? sameLevel[0] : null;
    }

    private static NameKeys KeysOf(string ar, string en, IReadOnlyList<string> aliases) =>
        new(NameNormalizer.ArabicKeys([ar, .. aliases]), NameNormalizer.EnglishKeys([en, .. aliases]));

    private static bool Overlaps(NameKeys first, NameKeys second) =>
        first.Arabic.Overlaps(second.Arabic) || first.English.Overlaps(second.English);

    private sealed record NameKeys(IReadOnlySet<string> Arabic, IReadOnlySet<string> English);
}
