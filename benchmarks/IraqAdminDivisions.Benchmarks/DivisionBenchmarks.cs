using BenchmarkDotNet.Attributes;

namespace IraqAdminDivisions.Benchmarks;

[MemoryDiagnoser]
public class DivisionBenchmarks
{
    private const string SubDistrictCode = "IQG07Q02N02";
    private const string LowerCaseSubDistrictCode = "iqg07q02n02";
    private const string GovernorateCode = "IQG07";
    private const string ArabicQuery = "البصرة";
    private const string EnglishQuery = "Basrah";
    private const string TypoQuery = "Basrahh";

    [GlobalSetup]
    public void WarmUp() => IraqDivisions.Find(SubDistrictCode);

    [Benchmark]
    public int ColdLoad() => DivisionStore.Load().All.Count;

    [Benchmark]
    public Division? Find() => IraqDivisions.Find(SubDistrictCode);

    [Benchmark]
    public Division? FindLowerCase() => IraqDivisions.Find(LowerCaseSubDistrictCode);

    [Benchmark]
    public string FormatAddress() => IraqDivisions.FormatAddress(SubDistrictCode, Language.Arabic);

    [Benchmark]
    public LocalizedName GetAddress() => IraqDivisions.GetAddress(SubDistrictCode);

    [Benchmark]
    public IReadOnlyList<Division> Localities() => IraqDivisions.Find(GovernorateCode)!.Localities;

    [Benchmark]
    public IReadOnlyList<SearchResult> SearchArabicExact() => IraqDivisions.Search(ArabicQuery);

    [Benchmark]
    public IReadOnlyList<SearchResult> SearchEnglish() => IraqDivisions.Search(EnglishQuery);

    [Benchmark]
    public IReadOnlyList<SearchResult> SearchOneTypo() => IraqDivisions.Search(TypoQuery);
}
