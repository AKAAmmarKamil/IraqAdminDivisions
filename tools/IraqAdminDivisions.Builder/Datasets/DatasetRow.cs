using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed record DatasetRow(
    AdminLevel Level,
    string Id,
    string? GovernorateId,
    string Ar,
    string En,
    string? Ku,
    string? PCode,
    double? Lat,
    double? Lng,
    bool IsCentre,
    IReadOnlyList<string> Aliases);
