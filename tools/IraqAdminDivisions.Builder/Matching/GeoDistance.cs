namespace IraqAdminDivisions.Builder.Matching;

public static class GeoDistance
{
    private const double EarthRadiusKm = 6371.0088;

    public static double Kilometers(double latA, double lngA, double latB, double lngB)
    {
        var deltaLat = ToRadians(latB - latA);
        var deltaLng = ToRadians(lngB - lngA);
        var haversine = Math.Pow(Math.Sin(deltaLat / 2), 2)
            + Math.Cos(ToRadians(latA)) * Math.Cos(ToRadians(latB)) * Math.Pow(Math.Sin(deltaLng / 2), 2);
        return 2 * EarthRadiusKm * Math.Asin(Math.Min(1, Math.Sqrt(haversine)));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}
