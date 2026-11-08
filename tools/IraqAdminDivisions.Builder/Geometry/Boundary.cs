using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Geometry;

public readonly record struct LngLat(double Lng, double Lat);

public sealed record Polygon(IReadOnlyList<IReadOnlyList<LngLat>> Rings);

public sealed record DivisionBoundary(string Code, AdminLevel Level, IReadOnlyList<Polygon> Polygons)
{
    public BoundingBox Box { get; } = BoundingBox.Around(Polygons);
}
