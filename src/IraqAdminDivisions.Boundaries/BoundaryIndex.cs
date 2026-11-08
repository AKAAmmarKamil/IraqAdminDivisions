namespace IraqAdminDivisions.Boundaries;

internal sealed class BoundaryIndex
{
    internal const double GridCellDegrees = 0.05;
    internal const double SnapDistanceDegrees = 0.0005;
    private const double SnapDistanceSquared = SnapDistanceDegrees * SnapDistanceDegrees;

    private static readonly Lazy<BoundaryIndex> Shared = new(
        () => new BoundaryIndex(BoundaryLoader.Load()),
        LazyThreadSafetyMode.ExecutionAndPublication);

    private readonly int[] _polygonRingCounts;
    private readonly int[] _polygonDivisions;
    private readonly double[] _polygonBounds;
    private readonly double[] _polygonAreas;
    private readonly Division[] _divisions;
    private readonly int[] _codeRanks;
    private readonly BoundaryGrid _grid;
    private readonly double _gridMinLng;
    private readonly double _gridMinLat;
    private readonly double _cellDegrees;
    private readonly int _columns;
    private readonly int _rows;

    internal BoundaryIndex(BoundaryData data)
        : this(data, GridCellDegrees)
    {
    }

    internal BoundaryIndex(BoundaryData data, double cellDegrees)
    {
        _polygonRingCounts = data.PolygonRingCounts;
        _polygonDivisions = data.PolygonDivisions;
        _polygonBounds = data.PolygonBounds;
        _polygonAreas = data.PolygonAreas;
        _divisions = ResolveDivisions(data.Codes);
        _codeRanks = RankCodes(data.Codes);
        _grid = new BoundaryGridBuilder(data, cellDegrees, SnapDistanceDegrees, IsPreferred).Build();
        _gridMinLng = _grid.MinLongitude;
        _gridMinLat = _grid.MinLatitude;
        _cellDegrees = cellDegrees;
        _columns = _grid.Columns;
        _rows = _grid.Rows;
    }

    internal static BoundaryIndex Instance => Shared.Value;

    internal int PolygonCount => _polygonDivisions.Length;

    internal int GridCellCount => _columns * _rows;

    internal int PolygonCountOf(string code)
    {
        var divisionIndex = Array.IndexOf(_divisions, IraqDivisions.Find(code));
        var count = 0;
        foreach (var owner in _polygonDivisions)
        {
            if (owner == divisionIndex)
            {
                count++;
            }
        }

        return count;
    }

    internal IEnumerable<Division> LoadedDivisions => _divisions;

    internal Division? Locate(double latitude, double longitude, out bool isSnapped)
    {
        isSnapped = false;
        var column = CellIndex(longitude, _gridMinLng);
        var row = CellIndex(latitude, _gridMinLat);
        if (column < 0 || column >= _columns || row < 0 || row >= _rows)
        {
            return null;
        }

        var cell = row * _columns + column;
        var direct = _grid.CellDirect[cell];
        if (direct >= 0)
        {
            return _divisions[_polygonDivisions[direct]];
        }

        var best = -1;
        var last = _grid.EntryStarts[cell + 1];
        for (var k = _grid.EntryStarts[cell]; k < last; k++)
        {
            var polygon = _grid.EntryPolygons[k];
            var segment = _grid.EntrySegments[k];
            if (segment >= 0 && !IsInBounds(polygon, longitude, latitude))
            {
                continue;
            }

            if ((segment < 0 || PolygonContains(polygon, segment, longitude, latitude)) && (best < 0 || IsPreferred(polygon, best)))
            {
                best = polygon;
            }
        }

        if (best >= 0)
        {
            return _divisions[_polygonDivisions[best]];
        }

        var nearest = -1;
        var nearestDistance = SnapDistanceSquared;
        var lastSnap = _grid.SnapStarts[cell + 1];
        for (var k = _grid.SnapStarts[cell]; k < lastSnap; k++)
        {
            var polygon = _grid.SnapPolygons[k];
            var distance = EdgesDistanceSquared(k, longitude, latitude);
            if (distance < nearestDistance
                || (distance == nearestDistance && nearest >= 0 && IsPreferred(polygon, nearest)))
            {
                nearest = polygon;
                nearestDistance = distance;
            }
        }

        if (nearest < 0)
        {
            return null;
        }

        isSnapped = true;
        return _divisions[_polygonDivisions[nearest]];
    }

    private bool IsInBounds(int polygon, double longitude, double latitude)
    {
        var bounds = 4 * polygon;
        return longitude >= _polygonBounds[bounds] && latitude >= _polygonBounds[bounds + 1]
            && longitude <= _polygonBounds[bounds + 2] && latitude <= _polygonBounds[bounds + 3];
    }

    private bool IsPreferred(int candidate, int current)
    {
        var areaDifference = _polygonAreas[candidate] - _polygonAreas[current];
        if (areaDifference != 0)
        {
            return areaDifference < 0;
        }

        return _codeRanks[_polygonDivisions[candidate]] < _codeRanks[_polygonDivisions[current]];
    }

    private bool PolygonContains(int polygon, int firstSegment, double longitude, double latitude)
    {
        if (!SegmentContains(firstSegment, longitude, latitude))
        {
            return false;
        }

        var end = firstSegment + _polygonRingCounts[polygon];
        for (var hole = firstSegment + 1; hole < end; hole++)
        {
            if (SegmentContains(hole, longitude, latitude))
            {
                return false;
            }
        }

        return true;
    }

    private bool SegmentContains(int segment, double x, double y)
    {
        var edges = _grid.Edges;
        var isInside = _grid.SegmentParities[segment];
        var end = _grid.SegmentStarts[segment + 1];
        for (var k = _grid.SegmentStarts[segment]; k < end; k++)
        {
            var e = 4 * _grid.SegmentEdges[k];
            var yj = edges[e + 1];
            var yi = edges[e + 3];
            if ((yi > y) != (yj > y) && x < (edges[e] - edges[e + 2]) * (y - yi) / (yj - yi) + edges[e + 2])
            {
                isInside = !isInside;
            }
        }

        return isInside;
    }

    private double EdgesDistanceSquared(int snapEntry, double x, double y)
    {
        var edges = _grid.Edges;
        var best = double.MaxValue;
        var end = _grid.SnapEdgeStarts[snapEntry + 1];
        for (var k = _grid.SnapEdgeStarts[snapEntry]; k < end; k++)
        {
            var e = 4 * _grid.SnapEdges[k];
            best = Math.Min(best, SegmentDistanceSquared(x, y, edges[e], edges[e + 1], edges[e + 2], edges[e + 3]));
        }

        return best;
    }

    private static double SegmentDistanceSquared(double px, double py, double ax, double ay, double bx, double by)
    {
        var dx = bx - ax;
        var dy = by - ay;
        var lengthSquared = dx * dx + dy * dy;
        var t = lengthSquared == 0 ? 0 : ((px - ax) * dx + (py - ay) * dy) / lengthSquared;
        t = t < 0 ? 0 : t > 1 ? 1 : t;
        var ex = px - (ax + t * dx);
        var ey = py - (ay + t * dy);
        return ex * ex + ey * ey;
    }

    private int CellIndex(double value, double origin) => (int)Math.Floor((value - origin) / _cellDegrees);

    private static Division[] ResolveDivisions(string[] codes)
    {
        var divisions = new Division[codes.Length];
        for (var i = 0; i < codes.Length; i++)
        {
            divisions[i] = IraqDivisions.Find(codes[i])
                ?? throw new InvalidDataException($"Boundary code '{codes[i]}' is not a known division.");
        }

        return divisions;
    }

    private static int[] RankCodes(string[] codes)
    {
        var keys = (string[])codes.Clone();
        var order = new int[codes.Length];
        for (var i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }

        Array.Sort(keys, order, StringComparer.Ordinal);
        var ranks = new int[codes.Length];
        for (var rank = 0; rank < order.Length; rank++)
        {
            ranks[order[rank]] = rank;
        }

        return ranks;
    }
}
