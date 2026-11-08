namespace IraqAdminDivisions.Tests;

public class DataIntegrityTests
{
    [Fact]
    public void Counts_MatchOchaPlusTheHalabjaGovernorate()
    {
        Assert.Equal(19, IraqDivisions.All.Count(d => d.Level == AdminLevel.Governorate));
        Assert.Equal(101, IraqDivisions.All.Count(d => d.Level == AdminLevel.District));
        Assert.Equal(294, IraqDivisions.All.Count(d => d.Level == AdminLevel.SubDistrict));
        Assert.Equal(19, IraqDivisions.Governorates.Count);
    }

    [Fact]
    public void Codes_AreUnique()
    {
        var codes = IraqDivisions.All.Select(d => d.Code).ToList();

        Assert.Equal(codes.Count, codes.Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Fact]
    public void EveryParent_ExistsAndIsExactlyOneLevelUp()
    {
        foreach (var division in IraqDivisions.All.Where(d => d.Level != AdminLevel.Governorate))
        {
            Assert.NotNull(division.Parent);
            Assert.Equal((int)division.Level - 1, (int)division.Parent!.Level);
            Assert.Same(IraqDivisions.Find(division.Parent.Code), division.Parent);
        }

        Assert.All(IraqDivisions.Governorates, governorate => Assert.Null(governorate.Parent));
    }

    [Fact]
    public void EveryChildCode_StartsWithItsParentCode()
    {
        foreach (var division in IraqDivisions.All)
        {
            Assert.All(division.Children, child =>
            {
                Assert.StartsWith(division.Code, child.Code, StringComparison.Ordinal);
                Assert.Same(division, child.Parent);
            });
        }
    }

    [Fact]
    public void EveryGovernorate_HasAtLeastOneDistrict()
    {
        Assert.All(IraqDivisions.Governorates, governorate =>
        {
            Assert.NotEmpty(governorate.Children);
            Assert.All(governorate.Children, child => Assert.Equal(AdminLevel.District, child.Level));
        });
    }

    [Fact]
    public void Navigation_ResolvesGovernorateAndDistrict()
    {
        var subDistrict = IraqDivisions.All.First(d => d.Level == AdminLevel.SubDistrict);
        var district = subDistrict.Parent!;
        var governorate = district.Parent!;

        Assert.Same(district, subDistrict.District);
        Assert.Same(governorate, subDistrict.Governorate);
        Assert.Same(district, district.District);
        Assert.Same(governorate, district.Governorate);
        Assert.Same(governorate, governorate.Governorate);
        Assert.Null(governorate.District);
    }

    [Fact]
    public void ReleasedCodes_StillResolve()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Snapshots", "released-codes-1.0.0.txt");
        var releasedCodes = File.ReadAllLines(path).Where(line => line.Length > 0).ToList();

        Assert.NotEmpty(releasedCodes);
        Assert.All(releasedCodes, code => Assert.NotNull(IraqDivisions.Find(code)));
    }
}
