namespace IraqAdminDivisions.Boundaries;

internal sealed class BoundaryData
{
    public string[] Codes = Array.Empty<string>();
    public double[] Coordinates = Array.Empty<double>();
    public int[] RingStarts = Array.Empty<int>();
    public int[] RingLengths = Array.Empty<int>();
    public int[] PolygonFirstRings = Array.Empty<int>();
    public int[] PolygonRingCounts = Array.Empty<int>();
    public int[] PolygonDivisions = Array.Empty<int>();
    public double[] PolygonBounds = Array.Empty<double>();
    public double[] PolygonAreas = Array.Empty<double>();
}
