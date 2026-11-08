namespace IraqAdminDivisions.Boundaries.Tests;

internal sealed class ReferenceLocator
{
    private readonly BoundaryData _data;
    private readonly Division[] _divisions;
    private readonly int[] _codeRanks;
    private readonly int[] _ringsOffsets;

    internal ReferenceLocator(BoundaryData data)
    {
        _data = data;
        _divisions = data.Codes.Select(code => IraqDivisions.Find(code)!).ToArray();
        var ordered = data.Codes.Select((code, index) => (code, index)).OrderBy(pair => pair.code, StringComparer.Ordinal).ToArray();
        _codeRanks = new int[data.Codes.Length];
        for (var rank = 0; rank < ordered.Length; rank++)
        {
            _codeRanks[ordered[rank].index] = rank;
        }

        _ringsOffsets = data.PolygonFirstRings;
    }

    internal Division? Locate(double latitude, double longitude, out bool isSnapped)
    {
        isSnapped = false;
        var best = -1;
        for (var polygon = 0; polygon < _data.PolygonDivisions.Length; polygon++)
        {
            var bounds = 4 * polygon;
            if (longitude < _data.PolygonBounds[bounds] || latitude < _data.PolygonBounds[bounds + 1]
                || longitude > _data.PolygonBounds[bounds + 2] || latitude > _data.PolygonBounds[bounds + 3])
            {
                continue;
            }

            if (Contains(polygon, longitude, latitude) && (best < 0 || IsPreferred(polygon, best)))
            {
                best = polygon;
            }
        }

        if (best >= 0)
        {
            return _divisions[_data.PolygonDivisions[best]];
        }

        var snapSquared = BoundaryIndex.SnapDistanceDegrees * BoundaryIndex.SnapDistanceDegrees;
        var nearest = -1;
        var nearestDistance = snapSquared;
        for (var polygon = 0; polygon < _data.PolygonDivisions.Length; polygon++)
        {
            var distance = DistanceSquared(polygon, longitude, latitude);
            if (distance < nearestDistance || (distance == nearestDistance && nearest >= 0 && IsPreferred(polygon, nearest)))
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
        return _divisions[_data.PolygonDivisions[nearest]];
    }

    private bool IsPreferred(int candidate, int current)
    {
        var areaDifference = _data.PolygonAreas[candidate] - _data.PolygonAreas[current];
        if (areaDifference != 0)
        {
            return areaDifference < 0;
        }

        return _codeRanks[_data.PolygonDivisions[candidate]] < _codeRanks[_data.PolygonDivisions[current]];
    }

    private bool Contains(int polygon, double x, double y)
    {
        var first = _ringsOffsets[polygon];
        if (!RingContains(first, x, y))
        {
            return false;
        }

        for (var hole = first + 1; hole < first + _data.PolygonRingCounts[polygon]; hole++)
        {
            if (RingContains(hole, x, y))
            {
                return false;
            }
        }

        return true;
    }

    private bool RingContains(int ring, double x, double y)
    {
        var start = _data.RingStarts[ring];
        var length = _data.RingLengths[ring];
        var isInside = false;
        var xj = _data.Coordinates[start + 2 * (length - 1)];
        var yj = _data.Coordinates[start + 2 * (length - 1) + 1];
        for (var i = 0; i < length; i++)
        {
            var xi = _data.Coordinates[start + 2 * i];
            var yi = _data.Coordinates[start + 2 * i + 1];
            if ((yi > y) != (yj > y) && x < (xj - xi) * (y - yi) / (yj - yi) + xi)
            {
                isInside = !isInside;
            }

            xj = xi;
            yj = yi;
        }

        return isInside;
    }

    private double DistanceSquared(int polygon, double x, double y)
    {
        var bounds = 4 * polygon;
        var snap = BoundaryIndex.SnapDistanceDegrees;
        if (x < _data.PolygonBounds[bounds] - snap || y < _data.PolygonBounds[bounds + 1] - snap
            || x > _data.PolygonBounds[bounds + 2] + snap || y > _data.PolygonBounds[bounds + 3] + snap)
        {
            return double.MaxValue;
        }

        var best = double.MaxValue;
        for (var ring = _ringsOffsets[polygon]; ring < _ringsOffsets[polygon] + _data.PolygonRingCounts[polygon]; ring++)
        {
            var start = _data.RingStarts[ring];
            var length = _data.RingLengths[ring];
            var xj = _data.Coordinates[start + 2 * (length - 1)];
            var yj = _data.Coordinates[start + 2 * (length - 1) + 1];
            for (var i = 0; i < length; i++)
            {
                var xi = _data.Coordinates[start + 2 * i];
                var yi = _data.Coordinates[start + 2 * i + 1];
                best = Math.Min(best, SegmentDistanceSquared(x, y, xj, yj, xi, yi));
                xj = xi;
                yj = yi;
            }
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
}
