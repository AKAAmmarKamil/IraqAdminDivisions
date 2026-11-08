namespace IraqAdminDivisions.Builder.Geometry;

public sealed class BoundarySimplifier(double toleranceDegrees)
{
    private const int CoordinateDecimals = 5;
    private const int MinimumClosedRingPoints = 4;

    public DivisionBoundary Simplify(DivisionBoundary boundary)
    {
        var polygons = boundary.Polygons
            .Select(SimplifyPolygon)
            .OfType<Polygon>()
            .ToList();

        return new DivisionBoundary(boundary.Code, boundary.Level, polygons);
    }

    private Polygon? SimplifyPolygon(Polygon polygon)
    {
        var rings = polygon.Rings.Select(SimplifyRing).ToList();
        if (rings.Count == 0 || rings[0] is null)
            return null;

        return new Polygon(rings.OfType<IReadOnlyList<LngLat>>().ToList());
    }

    private IReadOnlyList<LngLat>? SimplifyRing(IReadOnlyList<LngLat> ring)
    {
        var rounded = DouglasPeucker.SimplifyRing(ring, toleranceDegrees)
            .Select(point => new LngLat(Math.Round(point.Lng, CoordinateDecimals), Math.Round(point.Lat, CoordinateDecimals)))
            .ToList();

        var distinct = new List<LngLat>(rounded.Count);
        foreach (var point in rounded)
        {
            if (distinct.Count == 0 || distinct[^1] != point)
                distinct.Add(point);
        }

        return distinct.Count < MinimumClosedRingPoints ? null : distinct;
    }
}
