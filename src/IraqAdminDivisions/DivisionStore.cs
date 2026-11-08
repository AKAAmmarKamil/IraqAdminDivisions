using System.Text.Json;

namespace IraqAdminDivisions;

internal sealed class DivisionStore
{
    private const string ResourceName = "IraqAdminDivisions.divisions.json";
    private const string ArabicAndKurdishSeparator = "، ";
    private const string EnglishSeparator = ", ";

    private readonly Dictionary<string, Division> _byCode;

    private DivisionStore(
        IReadOnlyList<Division> all,
        IReadOnlyList<Division> governorates,
        Dictionary<string, Division> byCode,
        SearchEntry[] searchEntries)
    {
        All = all;
        Governorates = governorates;
        _byCode = byCode;
        SearchEntries = searchEntries;
    }

    public IReadOnlyList<Division> All { get; }

    public IReadOnlyList<Division> Governorates { get; }

    public SearchEntry[] SearchEntries { get; }

    public Division? Find(string code) => _byCode.TryGetValue(code, out var division) ? division : null;

    public static DivisionStore Load() => Load(ReadFile());

    internal static DivisionStore Load(DivisionFile file)
    {
        var records = file.Divisions;
        records.Sort((first, second) => string.CompareOrdinal(first.Code, second.Code));

        var divisions = new Division[records.Count];
        var byCode = new Dictionary<string, Division>(records.Count, StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < records.Count; i++)
        {
            divisions[i] = ToDivision(records[i]);
            byCode.Add(divisions[i].Code, divisions[i]);
        }

        var childrenByParent = new Dictionary<string, List<Division>>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < divisions.Length; i++)
        {
            var parentCode = records[i].ParentCode;
            if (parentCode == null)
            {
                continue;
            }

            var parent = byCode[parentCode];
            divisions[i].Parent = parent;
            if (divisions[i].IsDeprecated != parent.IsDeprecated)
            {
                continue;
            }

            if (!childrenByParent.TryGetValue(parent.Code, out var siblings))
            {
                siblings = new List<Division>();
                childrenByParent[parent.Code] = siblings;
            }

            siblings.Add(divisions[i]);
        }

        // A sub-district moved to a new district keeps its code, so a parent can sort after its child: build addresses level by level.
        for (var level = AdminLevel.Governorate; level <= AdminLevel.SubDistrict; level++)
        {
            for (var i = 0; i < divisions.Length; i++)
            {
                if (divisions[i].Level == level)
                {
                    LinkAncestors(divisions[i]);
                    divisions[i].Address = BuildAddress(divisions[i]);
                }
            }
        }

        var active = new List<Division>(divisions.Length);
        var governorates = new List<Division>();
        var searchEntries = new List<SearchEntry>(divisions.Length);
        for (var i = 0; i < divisions.Length; i++)
        {
            var division = divisions[i];
            if (childrenByParent.TryGetValue(division.Code, out var children))
            {
                division.Children = children.AsReadOnly();
            }

            if (division.IsDeprecated)
            {
                continue;
            }

            active.Add(division);
            searchEntries.Add(new SearchEntry(division, BuildSearchKeys(division)));
            if (division.Level == AdminLevel.Governorate)
            {
                governorates.Add(division);
            }
        }

        foreach (var governorate in governorates)
        {
            governorate.Localities = BuildLocalities(governorate);
        }

        LinkCarvedUnits(divisions, byCode);

        return new DivisionStore(active.AsReadOnly(), governorates.AsReadOnly(), byCode, searchEntries.ToArray());
    }

    private static void LinkCarvedUnits(Division[] divisions, Dictionary<string, Division> byCode)
    {
        var carvedOut = new Dictionary<string, List<Division>>(StringComparer.OrdinalIgnoreCase);
        foreach (var division in divisions)
        {
            if (division.CarvedFromCode == null)
            {
                continue;
            }

            var source = byCode[division.CarvedFromCode];
            division.CarvedFrom = source;
            if (division.IsDeprecated || division.Level != AdminLevel.SubDistrict)
            {
                continue;
            }

            if (!carvedOut.TryGetValue(source.Code, out var units))
            {
                units = new List<Division>();
                carvedOut[source.Code] = units;
            }

            units.Add(division);
        }

        foreach (var pair in carvedOut)
        {
            byCode[pair.Key].CarvedOut = pair.Value.AsReadOnly();
        }
    }

    private static DivisionFile ReadFile()
    {
        using var stream = typeof(DivisionStore).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' was not found.");
        return JsonSerializer.Deserialize(stream, DivisionJsonContext.Default.DivisionFile)
            ?? throw new InvalidOperationException("The embedded divisions file is empty.");
    }

    private static Division ToDivision(DivisionRecord record) => new Division(
        record.Code,
        ParseLevel(record.Level),
        new LocalizedName(record.Name.Ar, record.Name.En, record.Name.Ku),
        Array.AsReadOnly(record.Aliases.ToArray()),
        record.Center.Lat,
        record.Center.Lng,
        record.AreaSqKm,
        record.WikidataId,
        record.IsoCode,
        ParseKurdishNameSource(record.KurdishNameSource),
        record.KurdishNameDataset,
        record.IsDistrictCentre,
        record.IsDeprecated,
        record.ReplacedBy,
        record.OchaCode,
        record.HasBoundary,
        record.CarvedFrom);

    internal static AdminLevel ParseLevel(string level) =>
        Enum.TryParse<AdminLevel>(level, out var parsed)
            ? parsed
            : throw new InvalidOperationException($"Unknown division level '{level}'.");

    internal static KurdishNameSource ParseKurdishNameSource(string source) => source switch
    {
        "wikidata" => KurdishNameSource.Wikidata,
        "dataset" => KurdishNameSource.Dataset,
        "transliterated" => KurdishNameSource.Transliterated,
        "manual" => KurdishNameSource.Manual,
        _ => throw new InvalidOperationException($"Unknown Kurdish name source '{source}'.")
    };

    private static void LinkAncestors(Division division)
    {
        switch (division.Level)
        {
            case AdminLevel.District:
                division.District = division;
                division.Governorate = division.Parent ?? division;
                break;
            case AdminLevel.SubDistrict:
                division.District = division.Parent;
                division.Governorate = division.Parent?.Parent ?? division;
                break;
        }
    }

    private static LocalizedName BuildAddress(Division division)
    {
        var parent = division.Parent;
        if (parent == null)
        {
            return division.Name;
        }

        return new LocalizedName(
            division.Name.Ar + ArabicAndKurdishSeparator + parent.Address.Ar,
            division.Name.En + EnglishSeparator + parent.Address.En,
            division.Name.Ku + ArabicAndKurdishSeparator + parent.Address.Ku);
    }

    private static IReadOnlyList<Division> BuildLocalities(Division governorate)
    {
        var localities = new List<Division>();
        foreach (var district in governorate.Children)
        {
            localities.Add(district);
            foreach (var subDistrict in district.Children)
            {
                if (!subDistrict.IsDistrictCentre)
                {
                    localities.Add(subDistrict);
                }
            }
        }

        localities.Sort((first, second) => string.CompareOrdinal(first.Code, second.Code));
        return localities.AsReadOnly();
    }

    private static string[] BuildSearchKeys(Division division)
    {
        var keys = new List<string>(division.Aliases.Count + 3);
        AddKey(keys, division.Name.Ar);
        AddKey(keys, division.Name.En);
        AddKey(keys, division.Name.Ku);
        foreach (var alias in division.Aliases)
        {
            AddKey(keys, alias);
        }

        return keys.ToArray();
    }

    private static void AddKey(List<string> keys, string rawKey)
    {
        var key = SearchNormalizer.Normalize(rawKey);
        if (key.Length > 0 && !keys.Contains(key))
        {
            keys.Add(key);
        }
    }
}

internal sealed class SearchEntry
{
    public SearchEntry(Division division, string[] keys)
    {
        Division = division;
        Keys = keys;
    }

    public Division Division { get; }

    public string[] Keys { get; }
}
