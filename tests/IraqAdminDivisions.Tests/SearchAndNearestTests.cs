namespace IraqAdminDivisions.Tests;

public class SearchAndNearestTests
{
    [Theory]
    [InlineData("البصره")]
    [InlineData("البصرة")]
    [InlineData("basra")]
    [InlineData("Al-Basrah")]
    public void Search_FindsBasrahGovernorateFirst(string query)
    {
        var results = IraqDivisions.Search(query);

        Assert.NotEmpty(results);
        Assert.Equal("IQG02", results[0].Division.Code);
    }

    [Fact]
    public void Search_ToleratesTaMarbutaWrittenAsHa()
    {
        var district = IraqDivisions.All.First(d => d.Level == AdminLevel.District && d.Name.Ar.EndsWith("ة", StringComparison.Ordinal));
        var query = district.Name.Ar.Substring(0, district.Name.Ar.Length - 1) + "ه";

        var results = IraqDivisions.Search(query, 20, AdminLevel.District);

        Assert.Contains(results, result => result.Division.Code == district.Code);
    }

    [Fact]
    public void Search_RespectsLimitAndLevelFilter()
    {
        var results = IraqDivisions.Search("a", 3, AdminLevel.SubDistrict);

        Assert.True(results.Count <= 3);
        Assert.All(results, result => Assert.Equal(AdminLevel.SubDistrict, result.Division.Level));
    }

    [Fact]
    public void Search_ReturnsEmptyForBlankQuery()
    {
        Assert.Empty(IraqDivisions.Search("   "));
        Assert.Empty(IraqDivisions.Search(null));
    }

    [Fact]
    public void Search_IsDeterministicAndOrderedByScore()
    {
        var first = IraqDivisions.Search("al", 25);
        var second = IraqDivisions.Search("al", 25);

        Assert.Equal(first.Select(r => r.Division.Code), second.Select(r => r.Division.Code));
        Assert.Equal(first.Select(r => r.Score).OrderByDescending(s => s), first.Select(r => r.Score));
    }

    [Fact]
    public void Search_AcceptsASingleTypoForLongQueries()
    {
        var results = IraqDivisions.Search("Basrahh");

        Assert.Contains(results, result => result.Division.Code == "IQG02");
    }

    [Theory]
    [InlineData("البصرَة", "بصره")]
    [InlineData("Al-Basrah", "basrah")]
    [InlineData("El-Sa'ad", "saad")]
    public void Normalizer_FoldsVariants(string input, string expected)
    {
        Assert.Equal(expected, SearchNormalizer.Normalize(input));
    }

    [Fact]
    public void Nearest_FromSouthEastIraqAtGovernorateLevelIsBasrah()
    {
        var nearest = IraqDivisions.Nearest(30.5, 47.8, AdminLevel.Governorate);

        Assert.Equal("IQG02", nearest.Code);
    }

    [Fact]
    public void Nearest_ReturnsADivisionOfTheRequestedLevel()
    {
        Assert.Equal(AdminLevel.SubDistrict, IraqDivisions.Nearest(30.5, 47.8).Level);
        Assert.Equal(AdminLevel.District, IraqDivisions.Nearest(33.3, 44.4, AdminLevel.District).Level);
    }

    [Fact]
    public void Nearest_RejectsOutOfRangeCoordinates()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqDivisions.Nearest(95, 47.8));
        Assert.Throws<ArgumentOutOfRangeException>(() => IraqDivisions.Nearest(30.5, 200));
    }
}
