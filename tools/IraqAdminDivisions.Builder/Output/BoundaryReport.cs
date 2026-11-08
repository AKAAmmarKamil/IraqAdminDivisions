using IraqAdminDivisions.Builder.Geometry;

namespace IraqAdminDivisions.Builder.Output;

public sealed record BoundaryReport(double ToleranceDegrees, long BoundariesJsonBytes, long GeoJsonBytes, BoundaryAnalysis Analysis);
