namespace IraqAdminDivisions.Boundaries;

internal sealed class BoundaryGrid
{
    public double MinLongitude;
    public double MinLatitude;
    public double CellDegrees;
    public int Columns;
    public int Rows;
    public int[] CellDirect = Array.Empty<int>();
    public int[] EntryStarts = Array.Empty<int>();
    public int[] EntryPolygons = Array.Empty<int>();
    public int[] EntrySegments = Array.Empty<int>();
    public int[] SegmentStarts = Array.Empty<int>();
    public bool[] SegmentParities = Array.Empty<bool>();
    public double[] Edges = Array.Empty<double>();
    public int[] SegmentEdges = Array.Empty<int>();
    public int[] SnapStarts = Array.Empty<int>();
    public int[] SnapPolygons = Array.Empty<int>();
    public int[] SnapEdgeStarts = Array.Empty<int>();
    public int[] SnapEdges = Array.Empty<int>();
}
