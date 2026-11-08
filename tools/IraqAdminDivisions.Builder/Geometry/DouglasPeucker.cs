namespace IraqAdminDivisions.Builder.Geometry;

public static class DouglasPeucker
{
    private const int MinimumOpenRingPoints = 3;

    public static List<LngLat> SimplifyRing(IReadOnlyList<LngLat> closedRing, double toleranceDegrees)
    {
        var openRing = closedRing.Take(closedRing.Count - 1).ToList();
        if (openRing.Count < MinimumOpenRingPoints)
            return closedRing.ToList();

        var splitIndex = IndexOfFarthestPoint(openRing, openRing[0]);
        var firstHalf = SimplifyLine(openRing.Take(splitIndex + 1).ToList(), toleranceDegrees);
        var secondHalf = SimplifyLine(openRing.Skip(splitIndex).Append(openRing[0]).ToList(), toleranceDegrees);

        var simplified = new List<LngLat>(firstHalf);
        simplified.AddRange(secondHalf.Skip(1));
        return simplified;
    }

    private static int IndexOfFarthestPoint(List<LngLat> points, LngLat origin)
    {
        var farthestIndex = 0;
        var farthestDistance = -1.0;
        for (var i = 0; i < points.Count; i++)
        {
            var distance = DistanceBetween(points[i], origin);
            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestIndex = i;
            }
        }

        return farthestIndex;
    }

    private static List<LngLat> SimplifyLine(List<LngLat> line, double toleranceDegrees)
    {
        var isKept = new bool[line.Count];
        isKept[0] = true;
        isKept[^1] = true;
        var pending = new Stack<(int First, int Last)>();
        pending.Push((0, line.Count - 1));

        while (pending.Count > 0)
        {
            var (first, last) = pending.Pop();
            var farthestIndex = -1;
            var farthestDistance = 0.0;
            for (var i = first + 1; i < last; i++)
            {
                var distance = DistanceToSegment(line[i], line[first], line[last]);
                if (distance > farthestDistance)
                {
                    farthestDistance = distance;
                    farthestIndex = i;
                }
            }

            if (farthestIndex < 0 || farthestDistance <= toleranceDegrees)
                continue;

            isKept[farthestIndex] = true;
            pending.Push((first, farthestIndex));
            pending.Push((farthestIndex, last));
        }

        return line.Where((_, index) => isKept[index]).ToList();
    }

    private static double DistanceBetween(LngLat from, LngLat to) =>
        Math.Sqrt(Math.Pow(from.Lng - to.Lng, 2) + Math.Pow(from.Lat - to.Lat, 2));

    private static double DistanceToSegment(LngLat point, LngLat start, LngLat end)
    {
        var deltaLng = end.Lng - start.Lng;
        var deltaLat = end.Lat - start.Lat;
        var lengthSquared = deltaLng * deltaLng + deltaLat * deltaLat;
        if (lengthSquared == 0)
            return DistanceBetween(point, start);

        var position = ((point.Lng - start.Lng) * deltaLng + (point.Lat - start.Lat) * deltaLat) / lengthSquared;
        var clamped = Math.Clamp(position, 0, 1);
        return DistanceBetween(point, new LngLat(start.Lng + clamped * deltaLng, start.Lat + clamped * deltaLat));
    }
}
