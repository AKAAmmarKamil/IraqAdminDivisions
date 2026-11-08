namespace IraqAdminDivisions.Boundaries;

internal sealed class BoundaryGridBuilder
{
    private const double Margin = 1e-7;
    private const int InsideSegment = -1;
    private const byte TouchesBoundary = 1;
    private const byte TouchesSnapRange = 2;

    private readonly BoundaryData _data;
    private readonly double _cell;
    private readonly double _snapDegrees;
    private readonly double _snapReach;
    private readonly Func<int, int, bool> _isPreferred;
    private readonly BoundaryGrid _grid = new();

    private readonly List<int> _entryCells = new();
    private readonly List<int> _entryPolygons = new();
    private readonly List<int> _entrySegments = new();
    private readonly List<int> _segmentStarts = new();
    private readonly List<bool> _segmentParities = new();
    private readonly List<int> _segmentEdges = new();
    private readonly List<int> _snapCells = new();
    private readonly List<int> _snapPolygons = new();
    private readonly List<int> _snapEdgeStarts = new();
    private readonly List<int> _snapEdges = new();

    internal BoundaryGridBuilder(BoundaryData data, double cellDegrees, double snapDegrees, Func<int, int, bool> isPreferred)
    {
        _data = data;
        _cell = cellDegrees;
        _snapDegrees = snapDegrees;
        _snapReach = snapDegrees + Margin;
        _isPreferred = isPreferred;
        _grid.CellDegrees = cellDegrees;
        var minLng = double.MaxValue;
        var minLat = double.MaxValue;
        var maxLng = double.MinValue;
        var maxLat = double.MinValue;
        for (var polygon = 0; polygon < data.PolygonDivisions.Length; polygon++)
        {
            minLng = Math.Min(minLng, data.PolygonBounds[4 * polygon]);
            minLat = Math.Min(minLat, data.PolygonBounds[4 * polygon + 1]);
            maxLng = Math.Max(maxLng, data.PolygonBounds[4 * polygon + 2]);
            maxLat = Math.Max(maxLat, data.PolygonBounds[4 * polygon + 3]);
        }

        _grid.MinLongitude = minLng - snapDegrees;
        _grid.MinLatitude = minLat - snapDegrees;
        _grid.Columns = CellIndex(maxLng + snapDegrees, _grid.MinLongitude) + 1;
        _grid.Rows = CellIndex(maxLat + snapDegrees, _grid.MinLatitude) + 1;
    }

    internal BoundaryGrid Build()
    {
        _grid.Edges = BuildEdgeTable();
        for (var polygon = 0; polygon < _data.PolygonDivisions.Length; polygon++)
        {
            AddPolygon(polygon);
        }

        _segmentStarts.Add(_segmentEdges.Count);
        _snapEdgeStarts.Add(_snapEdges.Count);
        _grid.SegmentStarts = _segmentStarts.ToArray();
        _grid.SegmentParities = _segmentParities.ToArray();
        _grid.SegmentEdges = _segmentEdges.ToArray();
        CompactEntries();
        return _grid;
    }

    private double[] BuildEdgeTable()
    {
        var edges = new double[2 * _data.Coordinates.Length];
        for (var ring = 0; ring < _data.RingStarts.Length; ring++)
        {
            var start = _data.RingStarts[ring];
            var length = _data.RingLengths[ring];
            for (var i = 0; i < length; i++)
            {
                var previous = i == 0 ? length - 1 : i - 1;
                var edge = 2 * (start + 2 * i);
                edges[edge] = _data.Coordinates[start + 2 * previous];
                edges[edge + 1] = _data.Coordinates[start + 2 * previous + 1];
                edges[edge + 2] = _data.Coordinates[start + 2 * i];
                edges[edge + 3] = _data.Coordinates[start + 2 * i + 1];
            }
        }

        return edges;
    }

    private void AddPolygon(int polygon)
    {
        var bounds = 4 * polygon;
        var firstColumn = CellIndex(_data.PolygonBounds[bounds] - _snapDegrees, _grid.MinLongitude);
        var firstRow = CellIndex(_data.PolygonBounds[bounds + 1] - _snapDegrees, _grid.MinLatitude);
        var lastColumn = CellIndex(_data.PolygonBounds[bounds + 2] + _snapDegrees, _grid.MinLongitude);
        var lastRow = CellIndex(_data.PolygonBounds[bounds + 3] + _snapDegrees, _grid.MinLatitude);
        var width = lastColumn - firstColumn + 1;
        var height = lastRow - firstRow + 1;
        var touches = new byte[width * height];
        var snapEdges = new List<int>?[width * height];
        var ringFirst = _data.PolygonFirstRings[polygon];
        var buckets = new RowBuckets[_data.PolygonRingCounts[polygon]];
        for (var ring = 0; ring < buckets.Length; ring++)
        {
            MarkTouchedCells(ringFirst + ring, firstColumn, firstRow, width, height, touches, snapEdges);
            buckets[ring] = BuildRowBuckets(ringFirst + ring, firstRow, height);
        }

        for (var localRow = 0; localRow < height; localRow++)
        {
            var hasState = false;
            var isInside = false;
            for (var localColumn = 0; localColumn < width; localColumn++)
            {
                var local = localRow * width + localColumn;
                var column = firstColumn + localColumn;
                var row = firstRow + localRow;
                var cell = row * _grid.Columns + column;
                if ((touches[local] & TouchesBoundary) != 0)
                {
                    hasState = false;
                    AddEntry(cell, polygon, AddSegments(buckets, column, row, localRow));
                }
                else
                {
                    if (!hasState)
                    {
                        isInside = ContainsCellCenter(buckets, column, row, localRow);
                        hasState = true;
                    }

                    if (isInside)
                    {
                        AddEntry(cell, polygon, InsideSegment);
                    }
                }

                var snapList = snapEdges[local];
                if (snapList is not null)
                {
                    _snapCells.Add(cell);
                    _snapPolygons.Add(polygon);
                    _snapEdgeStarts.Add(_snapEdges.Count);
                    _snapEdges.AddRange(snapList);
                }
            }
        }
    }

    private void AddEntry(int cell, int polygon, int segment)
    {
        _entryCells.Add(cell);
        _entryPolygons.Add(polygon);
        _entrySegments.Add(segment);
    }

    private void MarkTouchedCells(int ring, int firstColumn, int firstRow, int width, int height, byte[] touches, List<int>?[] snapEdges)
    {
        var start = _data.RingStarts[ring];
        var length = _data.RingLengths[ring];
        var xj = _data.Coordinates[start + 2 * (length - 1)];
        var yj = _data.Coordinates[start + 2 * (length - 1) + 1];
        for (var i = 0; i < length; i++)
        {
            var xi = _data.Coordinates[start + 2 * i];
            var yi = _data.Coordinates[start + 2 * i + 1];
            var columnFrom = Math.Max(firstColumn, CellIndex(Math.Min(xi, xj) - _snapReach, _grid.MinLongitude));
            var columnTo = Math.Min(firstColumn + width - 1, CellIndex(Math.Max(xi, xj) + _snapReach, _grid.MinLongitude));
            var rowFrom = Math.Max(firstRow, CellIndex(Math.Min(yi, yj) - _snapReach, _grid.MinLatitude));
            var rowTo = Math.Min(firstRow + height - 1, CellIndex(Math.Max(yi, yj) + _snapReach, _grid.MinLatitude));
            for (var row = rowFrom; row <= rowTo; row++)
            {
                var minY = _grid.MinLatitude + row * _cell;
                for (var column = columnFrom; column <= columnTo; column++)
                {
                    var minX = _grid.MinLongitude + column * _cell;
                    if (!SegmentTouchesRect(xj, yj, xi, yi, minX - _snapReach, minY - _snapReach, minX + _cell + _snapReach, minY + _cell + _snapReach))
                    {
                        continue;
                    }

                    var local = (row - firstRow) * width + column - firstColumn;
                    touches[local] |= TouchesSnapRange;
                    var list = snapEdges[local] ??= new List<int>();
                    list.Add(start / 2 + i);
                    if (SegmentTouchesRect(xj, yj, xi, yi, minX - Margin, minY - Margin, minX + _cell + Margin, minY + _cell + Margin))
                    {
                        touches[local] |= TouchesBoundary;
                    }
                }
            }

            xj = xi;
            yj = yi;
        }
    }

    private RowBuckets BuildRowBuckets(int ring, int firstRow, int height)
    {
        var start = _data.RingStarts[ring];
        var length = _data.RingLengths[ring];
        var starts = new int[height + 1];
        ForEachEdgeRow(start, length, firstRow, height, (_, row) => starts[row + 1]++);
        for (var row = 0; row < height; row++)
        {
            starts[row + 1] += starts[row];
        }

        var edges = new int[starts[height]];
        var cursors = (int[])starts.Clone();
        ForEachEdgeRow(start, length, firstRow, height, (edge, row) => edges[cursors[row]++] = edge);
        return new RowBuckets(start, length, starts, edges);
    }

    private void ForEachEdgeRow(int start, int length, int firstRow, int height, Action<int, int> visit)
    {
        var yj = _data.Coordinates[start + 2 * (length - 1) + 1];
        for (var i = 0; i < length; i++)
        {
            var yi = _data.Coordinates[start + 2 * i + 1];
            var rowFrom = Math.Max(0, CellIndex(Math.Min(yi, yj) - _snapReach, _grid.MinLatitude) - firstRow);
            var rowTo = Math.Min(height - 1, CellIndex(Math.Max(yi, yj) + _snapReach, _grid.MinLatitude) - firstRow);
            for (var row = rowFrom; row <= rowTo; row++)
            {
                visit(i, row);
            }

            yj = yi;
        }
    }

    private int AddSegments(RowBuckets[] buckets, int column, int row, int localRow)
    {
        var minX = _grid.MinLongitude + column * _cell - Margin;
        var maxX = minX + _cell + 2 * Margin;
        var minY = _grid.MinLatitude + row * _cell - Margin;
        var maxY = minY + _cell + 2 * Margin;
        var firstSegment = _segmentParities.Count;
        foreach (var bucket in buckets)
        {
            _segmentStarts.Add(_segmentEdges.Count);
            var parity = false;
            for (var k = bucket.Starts[localRow]; k < bucket.Starts[localRow + 1]; k++)
            {
                var edge = bucket.Start / 2 + bucket.Edges[k];
                var xj = _grid.Edges[4 * edge];
                var yj = _grid.Edges[4 * edge + 1];
                var xi = _grid.Edges[4 * edge + 2];
                var yi = _grid.Edges[4 * edge + 3];
                if (Math.Max(xi, xj) < minX || Math.Min(yi, yj) > maxY || Math.Max(yi, yj) < minY)
                {
                    continue;
                }

                if (Math.Min(xi, xj) > maxX && Math.Min(yi, yj) < minY && Math.Max(yi, yj) > maxY)
                {
                    parity = !parity;
                    continue;
                }

                _segmentEdges.Add(edge);
            }

            _segmentParities.Add(parity);
        }

        return firstSegment;
    }

    private bool ContainsCellCenter(RowBuckets[] buckets, int column, int row, int localRow)
    {
        var x = _grid.MinLongitude + (column + 0.5) * _cell;
        var y = _grid.MinLatitude + (row + 0.5) * _cell;
        if (!BucketContains(buckets[0], localRow, x, y))
        {
            return false;
        }

        for (var hole = 1; hole < buckets.Length; hole++)
        {
            if (BucketContains(buckets[hole], localRow, x, y))
            {
                return false;
            }
        }

        return true;
    }

    private bool BucketContains(RowBuckets bucket, int localRow, double x, double y)
    {
        var isInside = false;
        for (var k = bucket.Starts[localRow]; k < bucket.Starts[localRow + 1]; k++)
        {
            var i = bucket.Edges[k];
            var j = i == 0 ? bucket.Length - 1 : i - 1;
            var xj = _data.Coordinates[bucket.Start + 2 * j];
            var yj = _data.Coordinates[bucket.Start + 2 * j + 1];
            var xi = _data.Coordinates[bucket.Start + 2 * i];
            var yi = _data.Coordinates[bucket.Start + 2 * i + 1];
            if ((yi > y) != (yj > y) && x < (xj - xi) * (y - yi) / (yj - yi) + xi)
            {
                isInside = !isInside;
            }
        }

        return isInside;
    }

    private void CompactEntries()
    {
        var cellCount = _grid.Columns * _grid.Rows;
        var entryOrder = OrderByCell(_entryCells, cellCount, out var entryRanges);
        _grid.CellDirect = new int[cellCount];
        var entryStarts = new int[cellCount + 1];
        var entryPolygons = new List<int>();
        var entrySegments = new List<int>();
        for (var cell = 0; cell < cellCount; cell++)
        {
            _grid.CellDirect[cell] = -1;
            var hasBoundary = false;
            for (var k = entryRanges[cell]; k < entryRanges[cell + 1]; k++)
            {
                hasBoundary |= _entrySegments[entryOrder[k]] != InsideSegment;
            }

            if (!hasBoundary && entryRanges[cell + 1] > entryRanges[cell])
            {
                _grid.CellDirect[cell] = PreferredPolygon(entryOrder, entryRanges[cell], entryRanges[cell + 1]);
            }
            else
            {
                for (var k = entryRanges[cell]; k < entryRanges[cell + 1]; k++)
                {
                    entryPolygons.Add(_entryPolygons[entryOrder[k]]);
                    entrySegments.Add(_entrySegments[entryOrder[k]]);
                }
            }

            entryStarts[cell + 1] = entryPolygons.Count;
        }

        _grid.EntryStarts = entryStarts;
        _grid.EntryPolygons = entryPolygons.ToArray();
        _grid.EntrySegments = entrySegments.ToArray();
        CompactSnapEntries(cellCount);
    }

    private int PreferredPolygon(int[] entryOrder, int first, int last)
    {
        var best = -1;
        for (var k = first; k < last; k++)
        {
            var polygon = _entryPolygons[entryOrder[k]];
            if (best < 0 || _isPreferred(polygon, best))
            {
                best = polygon;
            }
        }

        return best;
    }

    private void CompactSnapEntries(int cellCount)
    {
        var snapOrder = OrderByCell(_snapCells, cellCount, out var snapRanges);
        var snapStarts = new int[cellCount + 1];
        var snapPolygons = new List<int>();
        var snapEdgeStarts = new List<int>();
        var snapEdges = new List<int>();
        for (var cell = 0; cell < cellCount; cell++)
        {
            if (_grid.CellDirect[cell] < 0)
            {
                for (var k = snapRanges[cell]; k < snapRanges[cell + 1]; k++)
                {
                    var source = snapOrder[k];
                    snapPolygons.Add(_snapPolygons[source]);
                    snapEdgeStarts.Add(snapEdges.Count);
                    for (var e = _snapEdgeStarts[source]; e < _snapEdgeStarts[source + 1]; e++)
                    {
                        snapEdges.Add(_snapEdges[e]);
                    }
                }
            }

            snapStarts[cell + 1] = snapPolygons.Count;
        }

        snapEdgeStarts.Add(snapEdges.Count);
        _grid.SnapStarts = snapStarts;
        _grid.SnapPolygons = snapPolygons.ToArray();
        _grid.SnapEdgeStarts = snapEdgeStarts.ToArray();
        _grid.SnapEdges = snapEdges.ToArray();
    }

    private static int[] OrderByCell(List<int> cells, int cellCount, out int[] ranges)
    {
        ranges = new int[cellCount + 1];
        foreach (var cell in cells)
        {
            ranges[cell + 1]++;
        }

        for (var cell = 0; cell < cellCount; cell++)
        {
            ranges[cell + 1] += ranges[cell];
        }

        var cursors = (int[])ranges.Clone();
        var order = new int[cells.Count];
        for (var k = 0; k < cells.Count; k++)
        {
            order[cursors[cells[k]]++] = k;
        }

        return order;
    }

    private static bool SegmentTouchesRect(double ax, double ay, double bx, double by, double minX, double minY, double maxX, double maxY)
    {
        if (Math.Max(ax, bx) < minX || Math.Min(ax, bx) > maxX || Math.Max(ay, by) < minY || Math.Min(ay, by) > maxY)
        {
            return false;
        }

        var dx = bx - ax;
        var dy = by - ay;
        var bottomLeft = Side(dx, dy, ax, ay, minX, minY);
        var bottomRight = Side(dx, dy, ax, ay, maxX, minY);
        var topLeft = Side(dx, dy, ax, ay, minX, maxY);
        var topRight = Side(dx, dy, ax, ay, maxX, maxY);
        var isAllPositive = bottomLeft > 0 && bottomRight > 0 && topLeft > 0 && topRight > 0;
        var isAllNegative = bottomLeft < 0 && bottomRight < 0 && topLeft < 0 && topRight < 0;
        return !isAllPositive && !isAllNegative;
    }

    private static double Side(double dx, double dy, double ax, double ay, double cx, double cy) =>
        dx * (cy - ay) - dy * (cx - ax);

    private int CellIndex(double value, double origin) => (int)Math.Floor((value - origin) / _cell);

    private readonly struct RowBuckets
    {
        internal RowBuckets(int start, int length, int[] starts, int[] edges)
        {
            Start = start;
            Length = length;
            Starts = starts;
            Edges = edges;
        }

        internal int Start { get; }

        internal int Length { get; }

        internal int[] Starts { get; }

        internal int[] Edges { get; }
    }
}
