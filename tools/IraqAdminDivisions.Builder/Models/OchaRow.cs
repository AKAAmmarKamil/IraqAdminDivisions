namespace IraqAdminDivisions.Builder.Models;

public sealed record OchaRow(
    AdminLevel Level,
    string Code,
    string? ParentCode,
    string NameEn,
    string NameAr,
    string? RefName,
    double? Lat,
    double? Lng,
    double? AreaSqKm);
