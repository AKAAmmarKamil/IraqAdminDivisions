using BenchmarkDotNet.Attributes;
using IraqAdminDivisions.Boundaries;

namespace IraqAdminDivisions.Benchmarks;

[MemoryDiagnoser]
public class LocateBenchmarks
{
    private const int RandomPointCount = 4096;
    private const int RandomSeed = 20261108;
    private const double MinLatitude = 29.0;
    private const double MaxLatitude = 37.5;
    private const double MinLongitude = 38.7;
    private const double MaxLongitude = 48.7;

    private const double BaghdadLatitude = 33.3340;
    private const double BaghdadLongitude = 44.4170;
    private const double RiyadhLatitude = 24.71;
    private const double RiyadhLongitude = 46.67;

    private readonly double[] _randomLatitudes = new double[RandomPointCount];
    private readonly double[] _randomLongitudes = new double[RandomPointCount];
    private int _nextRandomPoint;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(RandomSeed);
        for (var i = 0; i < RandomPointCount; i++)
        {
            _randomLatitudes[i] = MinLatitude + random.NextDouble() * (MaxLatitude - MinLatitude);
            _randomLongitudes[i] = MinLongitude + random.NextDouble() * (MaxLongitude - MinLongitude);
        }

        IraqBoundaries.Locate(BaghdadLatitude, BaghdadLongitude);
    }

    [Benchmark]
    public Division? LocateBaghdad() => IraqBoundaries.Locate(BaghdadLatitude, BaghdadLongitude);

    [Benchmark]
    public Division? LocateOutsideIraq() => IraqBoundaries.Locate(RiyadhLatitude, RiyadhLongitude);

    [Benchmark]
    public Division? LocateRandomPointInIraqBoundingBox()
    {
        var index = _nextRandomPoint++ & (RandomPointCount - 1);
        return IraqBoundaries.Locate(_randomLatitudes[index], _randomLongitudes[index]);
    }

    [Benchmark]
    public Division? LocateGovernorate() =>
        IraqBoundaries.Locate(BaghdadLatitude, BaghdadLongitude, AdminLevel.Governorate);
}
