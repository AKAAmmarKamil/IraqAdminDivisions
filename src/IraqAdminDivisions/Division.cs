namespace IraqAdminDivisions;

/// <summary>An Iraqi administrative division: a governorate, district or sub-district.</summary>
public sealed class Division
{
    private static readonly IReadOnlyList<Division> NoDivisions = Array.AsReadOnly(new Division[0]);

    internal Division(
        string code,
        AdminLevel level,
        LocalizedName name,
        IReadOnlyList<string> aliases,
        double latitude,
        double longitude,
        double areaSqKm,
        string? wikidataId,
        string? isoCode,
        KurdishNameSource kurdishNameSource,
        string? kurdishNameDataset,
        bool isDistrictCentre,
        bool isDeprecated,
        string? replacedByCode,
        string? ochaCode,
        bool hasBoundary,
        string? carvedFromCode)
    {
        HasBoundary = hasBoundary;
        CarvedFromCode = carvedFromCode;
        CarvedOut = NoDivisions;
        IsoCode = isoCode;
        KurdishNameDataset = kurdishNameDataset;
        OchaCode = ochaCode;
        Code = code;
        Level = level;
        Name = name;
        Aliases = aliases;
        Latitude = latitude;
        Longitude = longitude;
        AreaSqKm = areaSqKm;
        WikidataId = wikidataId;
        KurdishNameSource = kurdishNameSource;
        IsDistrictCentre = isDistrictCentre;
        IsDeprecated = isDeprecated;
        ReplacedByCode = replacedByCode;
        Children = NoDivisions;
        Localities = NoDivisions;
        Address = name;
        Governorate = this;
    }

    /// <summary>The stable hierarchical code (an OCHA P-code such as <c>IQG07Q02N02</c>).</summary>
    public string Code { get; }

    /// <summary>The administrative level.</summary>
    public AdminLevel Level { get; }

    /// <summary>The localized name.</summary>
    public LocalizedName Name { get; }

    /// <summary>Alternative spellings and names.</summary>
    public IReadOnlyList<string> Aliases { get; }

    /// <summary>The centroid latitude in degrees.</summary>
    public double Latitude { get; }

    /// <summary>The centroid longitude in degrees.</summary>
    public double Longitude { get; }

    /// <summary>The area in square kilometres.</summary>
    public double AreaSqKm { get; }

    /// <summary>The Wikidata item id, or <c>null</c>.</summary>
    public string? WikidataId { get; }

    /// <summary>The ISO 3166-2 code of a governorate (for example <c>IQ-BG</c>), or <c>null</c> for other levels and for governorates without a confirmed ISO code.</summary>
    public string? IsoCode { get; }

    /// <summary>The original OCHA COD-AB P-code, for joining with UN and NGO datasets. Equals <see cref="Code"/> unless the division was restructured; <c>null</c> for a division OCHA does not list yet.</summary>
    public string? OchaCode { get; }

    /// <summary>Where the Kurdish name in <see cref="Name"/> came from.</summary>
    public KurdishNameSource KurdishNameSource { get; }

    /// <summary>The open dataset (<c>owner/repository</c>) that supplied the Kurdish name when <see cref="KurdishNameSource"/> is <see cref="IraqAdminDivisions.KurdishNameSource.Dataset"/>; otherwise <c>null</c>.</summary>
    public string? KurdishNameDataset { get; }

    /// <summary>Whether this sub-district is the centre of its district (an OCHA "Markaz X" row that repeats the district).</summary>
    public bool IsDistrictCentre { get; }

    /// <summary>Whether the division is deprecated. A deprecated division still resolves through <see cref="IraqDivisions.Find"/> (with its historical parent and address) but is left out of <see cref="IraqDivisions.All"/>, <see cref="Children"/>, <see cref="Localities"/>, search and <see cref="IraqDivisions.IsValid"/>.</summary>
    public bool IsDeprecated { get; }

    /// <summary>The code of the division that replaces this one, or <c>null</c>.</summary>
    public string? ReplacedByCode { get; }

    /// <summary>Whether the division has its own polygon in the boundaries package. <c>false</c> for a unit created after OCHA COD-AB v01 (for example from the 2024 census): no open source has its boundary, so its centroid comes from its source or from <see cref="CarvedFrom"/>, and its <see cref="AreaSqKm"/> is 0.</summary>
    public bool HasBoundary { get; }

    /// <summary>The code of the OCHA sub-district whose territory this unit was taken from, or <c>null</c>. Set only for units without their own boundary.</summary>
    public string? CarvedFromCode { get; }

    /// <summary>The OCHA sub-district this unit was taken from, or <c>null</c>. See <see cref="CarvedFromCode"/>.</summary>
    public Division? CarvedFrom { get; internal set; }

    /// <summary>The current sub-districts without a boundary of their own that were taken from this sub-district's territory, ordered by code. A point inside this sub-district's polygon may belong to one of them instead; the boundaries package cannot tell which. Empty for most divisions.</summary>
    public IReadOnlyList<Division> CarvedOut { get; internal set; }

    /// <summary>The parent division, or <c>null</c> for a governorate.</summary>
    public Division? Parent { get; internal set; }

    /// <summary>The governorate this division belongs to (the division itself for a governorate).</summary>
    public Division Governorate { get; internal set; }

    /// <summary>The district this division belongs to (the division itself for a district, <c>null</c> for a governorate).</summary>
    public Division? District { get; internal set; }

    /// <summary>The direct children, ordered by code. Empty for a sub-district.</summary>
    public IReadOnlyList<Division> Children { get; internal set; }

    /// <summary>The flat list of areas a person can pick inside a governorate: its districts plus its sub-districts that are not <see cref="IsDistrictCentre"/>, ordered by code. Empty for a district or sub-district.</summary>
    public IReadOnlyList<Division> Localities { get; internal set; }

    /// <summary>The full address in Arabic, English and Kurdish, from the most specific division up to the governorate (separated by "، " in Arabic and Kurdish, ", " in English). Precomputed at load.</summary>
    public LocalizedName Address { get; internal set; }

    /// <summary>Returns the precomputed address in one language, for example "Sub-district, District, Governorate".</summary>
    /// <param name="language">The language to render.</param>
    public string FormatAddress(Language language) => Address.Get(language);

    /// <inheritdoc />
    public override string ToString() => $"{Name.En} ({Code})";
}
