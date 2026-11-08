namespace IraqAdminDivisions.Builder.Geometry;

public static class PointInPolygon
{
    public static bool IsInside(DivisionBoundary boundary, LngLat point) =>
        boundary.Polygons.Any(polygon => IsInside(polygon, point));

    private static bool IsInside(Polygon polygon, LngLat point) =>
        IsInsideRing(polygon.Rings[0], point) && !polygon.Rings.Skip(1).Any(hole => IsInsideRing(hole, point));

    private static bool IsInsideRing(IReadOnlyList<LngLat> ring, LngLat point)
    {
        var isInside = false;
        for (int current = 0, previous = ring.Count - 1; current < ring.Count; previous = current++)
        {
            var from = ring[previous];
            var to = ring[current];
            var isCrossing = (to.Lat > point.Lat) != (from.Lat > point.Lat)
                && point.Lng < (from.Lng - to.Lng) * (point.Lat - to.Lat) / (from.Lat - to.Lat) + to.Lng;
            if (isCrossing)
                isInside = !isInside;
        }

        return isInside;
    }
}
