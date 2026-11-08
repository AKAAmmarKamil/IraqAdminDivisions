namespace IraqAdminDivisions.Builder.Models;

public sealed record DivisionName(string Ar, string En, string? Ku);

public sealed record DivisionCenter(double Lat, double Lng);

public sealed record Division(
    string Code,
    AdminLevel Level,
    string? ParentCode,
    DivisionName Name,
    IReadOnlyList<string> Aliases,
    DivisionCenter Center,
    double AreaSqKm,
    string? WikidataId,
    string? IsoCode,
    string? KurdishNameSource,
    string? KurdishNameDataset,
    bool IsDistrictCentre,
    bool IsDeprecated = false,
    string? ReplacedBy = null,
    string? OchaCode = null);
