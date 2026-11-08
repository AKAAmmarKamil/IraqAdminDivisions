namespace IraqAdminDivisions;

/// <summary>Entry point for looking up, searching and formatting Iraqi administrative divisions.</summary>
public static class IraqDivisions
{
    private const int ExactScore = 100;
    private const int PrefixScore = 80;
    private const int ContainsScore = 60;
    private const int TypoScore = 40;
    private const int MinimumTypoQueryLength = 5;
    private const double EarthRadiusKm = 6371.0088;

    private static readonly IReadOnlyList<SearchResult> NoResults = Array.AsReadOnly(new SearchResult[0]);

    private static readonly Lazy<DivisionStore> Store = new Lazy<DivisionStore>(DivisionStore.Load, LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>Every current division of every level, ordered by code. Deprecated codes are left out but still resolve through <see cref="Find"/>.</summary>
    public static IReadOnlyList<Division> All => Store.Value.All;

    /// <summary>The governorates, ordered by code.</summary>
    public static IReadOnlyList<Division> Governorates => Store.Value.Governorates;

    /// <summary>Finds a division by code, including a deprecated code (check <see cref="Division.IsDeprecated"/> and <see cref="Division.ReplacedByCode"/>). The code is trimmed and matched case-insensitively.</summary>
    /// <param name="code">The division code.</param>
    /// <returns>The division, or <c>null</c> when the code is unknown.</returns>
    public static Division? Find(string? code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return null;
        }

        var needsTrim = char.IsWhiteSpace(code![0]) || char.IsWhiteSpace(code[code.Length - 1]);
        return Store.Value.Find(needsTrim ? code.Trim() : code);
    }

    /// <summary>Checks whether a code is a current (non-deprecated) division, optionally of a specific level. Use it to validate new input; stored deprecated codes still resolve through <see cref="Find"/>.</summary>
    /// <param name="code">The division code.</param>
    /// <param name="level">The required level, or <c>null</c> to accept any level.</param>
    public static bool IsValid(string? code, AdminLevel? level = null)
    {
        var division = Find(code);
        return division != null && !division.IsDeprecated && (level == null || division.Level == level);
    }

    /// <summary>Formats the address for a code, from the most specific division up to the governorate.</summary>
    /// <param name="code">The division code.</param>
    /// <param name="language">The language to render.</param>
    /// <exception cref="KeyNotFoundException">The code is unknown.</exception>
    public static string FormatAddress(string code, Language language) => GetAddress(code).Get(language);

    /// <summary>Gets the full address for a code in Arabic, English and Kurdish, from the most specific division up to the governorate. Precomputed, so the call allocates nothing.</summary>
    /// <param name="code">The division code.</param>
    /// <exception cref="KeyNotFoundException">The code is unknown.</exception>
    public static LocalizedName GetAddress(string code) =>
        (Find(code) ?? throw new KeyNotFoundException($"Unknown Iraqi division code '{code}'.")).Address;

    /// <summary>Searches names and aliases in Arabic, English and Kurdish, tolerating common spelling variants and single-character typos.</summary>
    /// <param name="query">The text to search for.</param>
    /// <param name="limit">The maximum number of results.</param>
    /// <param name="level">Restricts results to one level, or <c>null</c> for all levels.</param>
    /// <returns>Results ordered by score, then level, then code.</returns>
    public static IReadOnlyList<SearchResult> Search(string? query, int limit = 10, AdminLevel? level = null)
    {
        Span<char> buffer = query != null && query.Length > SearchNormalizer.StackBufferLength
            ? new char[query.Length]
            : stackalloc char[SearchNormalizer.StackBufferLength];
        var normalizedQuery = SearchNormalizer.NormalizeInto(query, buffer);
        if (normalizedQuery.Length == 0 || limit <= 0)
        {
            return NoResults;
        }

        var entries = Store.Value.SearchEntries;
        var results = new List<SearchResult>(Math.Min(limit, entries.Length));
        foreach (var entry in entries)
        {
            if (level != null && entry.Division.Level != level)
            {
                continue;
            }

            var score = ScoreEntry(entry, normalizedQuery);
            if (score > 0)
            {
                InsertRanked(results, entry.Division, score, limit);
            }
        }

        return results;
    }

    private static void InsertRanked(List<SearchResult> results, Division division, int score, int limit)
    {
        var position = results.Count;
        while (position > 0 && IsRankedBefore(division, score, results[position - 1]))
        {
            position--;
        }

        if (position >= limit)
        {
            return;
        }

        results.Insert(position, new SearchResult(division, score));
        if (results.Count > limit)
        {
            results.RemoveAt(results.Count - 1);
        }
    }

    private static bool IsRankedBefore(Division division, int score, SearchResult existing) =>
        score > existing.Score || (score == existing.Score && division.Level < existing.Division.Level);

    /// <summary>Finds the division whose centroid is closest to a point. The result is centroid-based and approximate: it is not a point-in-polygon test, so a point near a boundary can resolve to a neighbouring division.</summary>
    /// <param name="latitude">The latitude in degrees.</param>
    /// <param name="longitude">The longitude in degrees.</param>
    /// <param name="level">The level to search.</param>
    public static Division Nearest(double latitude, double longitude, AdminLevel level = AdminLevel.SubDistrict)
    {
        if (double.IsNaN(latitude) || latitude < -90 || latitude > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), latitude, "Latitude must be between -90 and 90.");
        }

        if (double.IsNaN(longitude) || longitude < -180 || longitude > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), longitude, "Longitude must be between -180 and 180.");
        }

        return Store.Value.All
            .Where(division => division.Level == level)
            .OrderBy(division => HaversineKm(latitude, longitude, division.Latitude, division.Longitude))
            .ThenBy(division => division.Code, StringComparer.Ordinal)
            .First();
    }

    private static int ScoreEntry(SearchEntry entry, ReadOnlySpan<char> normalizedQuery)
    {
        var best = 0;
        foreach (var key in entry.Keys)
        {
            best = Math.Max(best, ScoreKey(key, normalizedQuery));
            if (best == ExactScore)
            {
                break;
            }
        }

        return best;
    }

    private static int ScoreKey(string key, ReadOnlySpan<char> normalizedQuery)
    {
        var keySpan = key.AsSpan();
        if (keySpan.SequenceEqual(normalizedQuery))
        {
            return ExactScore;
        }

        if (keySpan.StartsWith(normalizedQuery))
        {
            return PrefixScore;
        }

        if (keySpan.IndexOf(normalizedQuery) >= 0)
        {
            return ContainsScore;
        }

        return normalizedQuery.Length >= MinimumTypoQueryLength && IsWithinOneEdit(keySpan, normalizedQuery) ? TypoScore : 0;
    }

    private static bool IsWithinOneEdit(ReadOnlySpan<char> first, ReadOnlySpan<char> second)
    {
        if (Math.Abs(first.Length - second.Length) > 1)
        {
            return false;
        }

        var shorter = first.Length <= second.Length ? first : second;
        var longer = first.Length <= second.Length ? second : first;
        var index = 0;
        while (index < shorter.Length && shorter[index] == longer[index])
        {
            index++;
        }

        if (index == shorter.Length)
        {
            return true;
        }

        var shorterRemainder = shorter.Slice(shorter.Length == longer.Length ? index + 1 : index);
        return shorterRemainder.SequenceEqual(longer.Slice(index + 1));
    }

    private static double HaversineKm(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
    {
        var deltaLatitude = ToRadians(latitudeB - latitudeA);
        var deltaLongitude = ToRadians(longitudeB - longitudeA);
        var haversine = Math.Pow(Math.Sin(deltaLatitude / 2), 2)
            + Math.Cos(ToRadians(latitudeA)) * Math.Cos(ToRadians(latitudeB)) * Math.Pow(Math.Sin(deltaLongitude / 2), 2);
        return 2 * EarthRadiusKm * Math.Asin(Math.Min(1, Math.Sqrt(haversine)));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}
