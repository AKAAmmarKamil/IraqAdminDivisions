using System.Diagnostics;
using Xunit.Abstractions;

namespace IraqAdminDivisions.Boundaries.Tests;

public class LocatePerformanceTests
{
    private const int PointCount = 1_000_000;
    private const double MinLatitude = 28.9;
    private const double MaxLatitude = 37.4;
    private const double MinLongitude = 38.7;
    private const double MaxLongitude = 48.7;

    private readonly ITestOutputHelper _output;

    public LocatePerformanceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    [Trait("Category", "Perf")]
    public void Load_ReportsTime()
    {
        var stopwatch = Stopwatch.StartNew();
        var index = new BoundaryIndex(BoundaryLoader.Load());
        stopwatch.Stop();

        _output.WriteLine($"load+index: {stopwatch.Elapsed.TotalMilliseconds:F1} ms, polygons {index.PolygonCount}, cells {index.GridCellCount}");
        Assert.True(index.PolygonCount > 0);
    }

    [Fact]
    [Trait("Category", "Perf")]
    public void Load_ReportsRetainedMemory()
    {
        var before = GC.GetTotalMemory(true);
        var index = new BoundaryIndex(BoundaryLoader.Load());
        var after = GC.GetTotalMemory(true);
        GC.KeepAlive(index);

        _output.WriteLine($"retained: {(after - before) / 1024.0:F0} KiB");
        Assert.True(after > before);
    }

    [Theory]
    [Trait("Category", "Perf")]
    [InlineData(0.02)]
    [InlineData(0.05)]
    [InlineData(0.1)]
    public void CellSize_ReportsLoadMemoryAndLocateTime(double cellDegrees)
    {
        var data = BoundaryLoader.Load();
        var before = GC.GetTotalMemory(true);
        var stopwatch = Stopwatch.StartNew();
        var index = new BoundaryIndex(data, cellDegrees);
        stopwatch.Stop();
        var loadMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
        var retained = GC.GetTotalMemory(true) - before;

        var random = new Random(12345);
        var latitudes = new double[PointCount];
        var longitudes = new double[PointCount];
        for (var i = 0; i < PointCount; i++)
        {
            latitudes[i] = MinLatitude + random.NextDouble() * (MaxLatitude - MinLatitude);
            longitudes[i] = MinLongitude + random.NextDouble() * (MaxLongitude - MinLongitude);
        }

        for (var i = 0; i < 100_000; i++)
        {
            index.Locate(latitudes[i], longitudes[i], out _);
        }

        var hits = 0;
        stopwatch.Restart();
        for (var i = 0; i < PointCount; i++)
        {
            if (index.Locate(latitudes[i], longitudes[i], out _) is not null)
            {
                hits++;
            }
        }

        stopwatch.Stop();
        _output.WriteLine($"cell {cellDegrees}: load {loadMilliseconds:F1} ms, retained {retained / 1024.0:F0} KiB, {stopwatch.Elapsed.TotalMilliseconds * 1_000_000 / PointCount:F1} ns/op, hits {hits}");
        Assert.True(hits > 0);
    }

    [Fact]
    [Trait("Category", "Perf")]
    public void Locate_MillionRandomPoints_ReportsNanosecondsAndAllocations()
    {
        var random = new Random(12345);
        var latitudes = new double[PointCount];
        var longitudes = new double[PointCount];
        for (var i = 0; i < PointCount; i++)
        {
            latitudes[i] = MinLatitude + random.NextDouble() * (MaxLatitude - MinLatitude);
            longitudes[i] = MinLongitude + random.NextDouble() * (MaxLongitude - MinLongitude);
        }

        for (var i = 0; i < 100_000; i++)
        {
            IraqBoundaries.Locate(latitudes[i], longitudes[i]);
        }

        var hits = 0;
        var allocatedBefore = GC.GetAllocatedBytesForCurrentThread();
        var stopwatch = Stopwatch.StartNew();
        for (var i = 0; i < PointCount; i++)
        {
            if (IraqBoundaries.Locate(latitudes[i], longitudes[i]) is not null)
            {
                hits++;
            }
        }

        stopwatch.Stop();
        var allocated = GC.GetAllocatedBytesForCurrentThread() - allocatedBefore;

        _output.WriteLine($"Locate: {stopwatch.Elapsed.TotalMilliseconds * 1_000_000 / PointCount:F1} ns/op, {allocated / (double)PointCount:F3} bytes/op, hits {hits}/{PointCount}");
        Assert.True(hits > 0);
        Assert.True(allocated < 1024, $"Locate allocated {allocated} bytes over {PointCount} calls.");
    }
}
