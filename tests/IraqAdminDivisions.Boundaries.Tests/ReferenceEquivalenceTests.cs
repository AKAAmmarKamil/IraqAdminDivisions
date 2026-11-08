namespace IraqAdminDivisions.Boundaries.Tests;

public class ReferenceEquivalenceTests
{
    private const int RandomPointCount = 200_000;
    private const int BoundaryPointCount = 200_000;
    private const double MinLatitude = 28.9;
    private const double MaxLatitude = 37.4;
    private const double MinLongitude = 38.7;
    private const double MaxLongitude = 48.7;
    private const double BoundaryJitterDegrees = 0.0015;

    [Theory]
    [InlineData(BoundaryIndex.GridCellDegrees)]
    [InlineData(0.02)]
    [InlineData(0.1)]
    public void Locate_MatchesReference_OnRandomPointsAndCentres(double cellDegrees)
    {
        var data = BoundaryLoader.Load();
        var index = new BoundaryIndex(data, cellDegrees);
        var reference = new ReferenceLocator(data);
        var random = new Random(20240607);
        var points = IraqDivisions.All.Select(division => (division.Latitude, division.Longitude)).ToList();
        for (var i = 0; i < RandomPointCount; i++)
        {
            points.Add((MinLatitude + random.NextDouble() * (MaxLatitude - MinLatitude), MinLongitude + random.NextDouble() * (MaxLongitude - MinLongitude)));
        }

        Assert.Equal(0, CountMismatches(index, reference, points));
    }

    [Theory]
    [InlineData(BoundaryIndex.GridCellDegrees)]
    [InlineData(0.02)]
    [InlineData(0.1)]
    public void Locate_MatchesReference_NearPolygonEdges(double cellDegrees)
    {
        var data = BoundaryLoader.Load();
        var index = new BoundaryIndex(data, cellDegrees);
        var reference = new ReferenceLocator(data);
        var random = new Random(777);
        var vertexCount = data.Coordinates.Length / 2;
        var points = new List<(double Latitude, double Longitude)>();
        for (var i = 0; i < BoundaryPointCount; i++)
        {
            var vertex = random.Next(vertexCount);
            var next = vertex + 1 < vertexCount ? vertex + 1 : vertex;
            var blend = random.NextDouble();
            var longitude = data.Coordinates[2 * vertex] * (1 - blend) + data.Coordinates[2 * next] * blend;
            var latitude = data.Coordinates[2 * vertex + 1] * (1 - blend) + data.Coordinates[2 * next + 1] * blend;
            points.Add((latitude + (random.NextDouble() - 0.5) * 2 * BoundaryJitterDegrees, longitude + (random.NextDouble() - 0.5) * 2 * BoundaryJitterDegrees));
        }

        Assert.Equal(0, CountMismatches(index, reference, points));
    }

    private static int CountMismatches(BoundaryIndex index, ReferenceLocator reference, List<(double Latitude, double Longitude)> points)
    {
        var mismatches = 0;
        foreach (var (latitude, longitude) in points)
        {
            var actual = index.Locate(latitude, longitude, out var isActualSnapped);
            var expected = reference.Locate(latitude, longitude, out var isExpectedSnapped);
            if (!ReferenceEquals(actual, expected) || isActualSnapped != isExpectedSnapped)
            {
                mismatches++;
            }
        }

        return mismatches;
    }
}
