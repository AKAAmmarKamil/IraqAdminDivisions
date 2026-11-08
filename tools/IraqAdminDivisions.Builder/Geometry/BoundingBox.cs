namespace IraqAdminDivisions.Builder.Geometry;

public sealed record BoundingBox(double MinLng, double MinLat, double MaxLng, double MaxLat)
{
    public static BoundingBox Around(IReadOnlyList<Polygon> polygons)
    {
        var points = polygons.SelectMany(polygon => polygon.Rings[0]).ToList();
        if (points.Count == 0)
            return new BoundingBox(0, 0, 0, 0);

        return new BoundingBox(
            points.Min(point => point.Lng),
            points.Min(point => point.Lat),
            points.Max(point => point.Lng),
            points.Max(point => point.Lat));
    }

    public bool Contains(BoundingBox other) =>
        other.MinLng >= MinLng && other.MinLat >= MinLat && other.MaxLng <= MaxLng && other.MaxLat <= MaxLat;
}
