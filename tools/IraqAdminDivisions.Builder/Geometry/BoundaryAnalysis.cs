using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Geometry;

public sealed record CentreCheck(string Code, AdminLevel Level, bool IsInsideOwnPolygon);

public sealed record BboxCheck(string Code, string GovernorateCode, bool IsInsideGovernorateBox);

public sealed record BoundaryAnalysis(IReadOnlyList<CentreCheck> CentreChecks, IReadOnlyList<BboxCheck> BboxChecks);
