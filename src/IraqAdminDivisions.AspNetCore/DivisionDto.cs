namespace IraqAdminDivisions.AspNetCore;

/// <summary>The three localized names of a division.</summary>
public sealed record DivisionNamesDto(string Ar, string En, string Ku);

/// <summary>The full address of a division in the three languages.</summary>
public sealed record DivisionAddressesDto(string Ar, string En, string Ku);

/// <summary>A division as returned by the endpoints, with <c>Name</c> and <c>Address</c> in the requested language.</summary>
public record DivisionDto(
    string Code,
    string Level,
    string Name,
    DivisionNamesDto Names,
    string? ParentCode,
    bool HasChildren,
    string Address,
    DivisionAddressesDto Addresses,
    string KurdishNameSource,
    string? KurdishNameDataset,
    string? IsoCode,
    string? OchaCode,
    bool IsDistrictCentre,
    double Latitude,
    double Longitude,
    bool IsDeprecated,
    string? ReplacedBy);

/// <summary>A search hit: a division plus its relevance score.</summary>
public sealed record DivisionSearchResultDto(
    string Code,
    string Level,
    string Name,
    DivisionNamesDto Names,
    string? ParentCode,
    bool HasChildren,
    string Address,
    DivisionAddressesDto Addresses,
    string KurdishNameSource,
    string? KurdishNameDataset,
    string? IsoCode,
    string? OchaCode,
    bool IsDistrictCentre,
    double Latitude,
    double Longitude,
    bool IsDeprecated,
    string? ReplacedBy,
    int Score)
    : DivisionDto(Code, Level, Name, Names, ParentCode, HasChildren, Address, Addresses, KurdishNameSource, KurdishNameDataset, IsoCode, OchaCode, IsDistrictCentre, Latitude, Longitude, IsDeprecated, ReplacedBy);

internal static class DivisionMapping
{
    public static DivisionDto ToDto(this Division division, Language language) => new(
        division.Code,
        division.Level.ToString(),
        division.Name.Get(language),
        new DivisionNamesDto(division.Name.Ar, division.Name.En, division.Name.Ku),
        division.Parent?.Code,
        division.Children.Count > 0,
        division.FormatAddress(language),
        new DivisionAddressesDto(division.Address.Ar, division.Address.En, division.Address.Ku),
        division.KurdishNameSource.ToString(),
        division.KurdishNameDataset,
        division.IsoCode,
        division.OchaCode,
        division.IsDistrictCentre,
        division.Latitude,
        division.Longitude,
        division.IsDeprecated,
        division.ReplacedByCode);

    public static DivisionSearchResultDto ToDto(this SearchResult result, Language language)
    {
        var dto = result.Division.ToDto(language);
        return new DivisionSearchResultDto(
            dto.Code, dto.Level, dto.Name, dto.Names, dto.ParentCode, dto.HasChildren, dto.Address, dto.Addresses, dto.KurdishNameSource,
            dto.KurdishNameDataset, dto.IsoCode, dto.OchaCode, dto.IsDistrictCentre,
            dto.Latitude, dto.Longitude, dto.IsDeprecated, dto.ReplacedBy, result.Score);
    }
}
