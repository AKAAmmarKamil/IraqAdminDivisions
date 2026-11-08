namespace IraqAdminDivisions.Tests;

/// <summary>The 2024 census additions (1.1.0): moved OCHA rows, promoted districts, units without boundary, project codes and the hierarchy helpers.</summary>
public class CensusRestructureTests
{
    [Fact]
    public void MovedOchaRows_StartWithTheirParentCodeAndKeepTheirOchaCode()
    {
        var moved = IraqDivisions.All.Where(division => division.Level != AdminLevel.Governorate && division.HasBoundary && division.OchaCode != division.Code).ToList();

        Assert.NotEmpty(moved);
        Assert.All(moved, division =>
        {
            Assert.NotNull(division.OchaCode);
            Assert.StartsWith(division.Parent!.Code, division.Code, StringComparison.Ordinal);
            Assert.Null(IraqDivisions.Find(division.OchaCode));
        });
    }

    [Fact]
    public void DistrictsPromotedFromASubDistrict_HaveItAsTheirMarkazCentre()
    {
        var hilal = IraqDivisions.Find("IQG03Q51N51")!;

        Assert.True(hilal.IsDistrictCentre);
        Assert.Equal("IQG03Q02N01", hilal.OchaCode);
        Assert.Equal("مركز الهلال", hilal.Name.Ar);
        Assert.Contains("الهلال", hilal.Aliases);
        Assert.Equal("IQG03Q51", hilal.District!.Code);
        Assert.DoesNotContain(hilal, hilal.Governorate.Localities);
    }

    [Fact]
    public void EveryDistrict_HasAtLeastOneSubDistrict()
    {
        Assert.All(IraqDivisions.All.Where(division => division.Level == AdminLevel.District), district => Assert.NotEmpty(district.Children));
    }

    [Fact]
    public void IsWithin_AgreesWithParentForEveryDivision()
    {
        foreach (var division in IraqDivisions.All)
        {
            Assert.True(IraqDivisions.IsWithin(division.Code, division.Code), division.Code);
            Assert.True(IraqDivisions.IsWithin(division.Code, division.Governorate.Code), division.Code);
            if (division.Parent is { } parent)
            {
                Assert.True(IraqDivisions.IsWithin(division.Code, parent.Code), division.Code);
                Assert.False(IraqDivisions.IsWithin(parent.Code, division.Code), division.Code);
            }
        }
    }

    [Fact]
    public void IsWithin_IsCaseInsensitiveAndFalseForUnknownCodes()
    {
        Assert.True(IraqDivisions.IsWithin("iqg07q02n02", "IQG07"));
        Assert.False(IraqDivisions.IsWithin("IQG07Q02N02", "IQG08"));
        Assert.False(IraqDivisions.IsWithin("IQG99", "IQG07"));
        Assert.False(IraqDivisions.IsWithin("IQG07", "IQG99"));
        Assert.False(IraqDivisions.IsWithin(null, null));
    }

    [Fact]
    public void Descendants_MatchesIsWithinForEveryGovernorateAndDistrict()
    {
        foreach (var division in IraqDivisions.All.Where(division => division.Level != AdminLevel.SubDistrict))
        {
            var expected = IraqDivisions.All
                .Where(candidate => candidate != division && IraqDivisions.IsWithin(candidate.Code, division.Code))
                .Select(candidate => candidate.Code);

            Assert.Equal(expected, IraqDivisions.Descendants(division.Code).Select(descendant => descendant.Code));
        }
    }

    [Fact]
    public void Descendants_IsEmptyForASubDistrictAndThrowsForAnUnknownCode()
    {
        Assert.Empty(IraqDivisions.Descendants("IQG07Q02N02"));
        Assert.Throws<KeyNotFoundException>(() => IraqDivisions.Descendants("IQG99"));
    }

    [Fact]
    public void UnitsWithoutBoundary_NameTheOchaSubDistrictTheyWereCarvedFrom()
    {
        foreach (var division in IraqDivisions.All.Where(division => !division.HasBoundary))
        {
            Assert.Null(division.OchaCode);
            Assert.NotEqual(AdminLevel.Governorate, division.Level);
            if (division.Level == AdminLevel.SubDistrict)
            {
                var source = Assert.IsType<Division>(division.CarvedFrom);
                Assert.Equal(division.CarvedFromCode, source.Code);
                Assert.True(source.HasBoundary, division.Code);
                Assert.Equal(AdminLevel.SubDistrict, source.Level);
                Assert.Equal(division.Governorate.Code, source.Governorate.Code);
                Assert.Contains(division, source.CarvedOut);
            }
        }
    }

    [Fact]
    public void ProjectCodes_UseNumbersFrom51()
    {
        foreach (var division in IraqDivisions.All.Where(division => division.OchaCode is null && division.Level != AdminLevel.Governorate))
        {
            var number = int.Parse(division.Code.Substring(division.Code.Length - 2), System.Globalization.CultureInfo.InvariantCulture);
            Assert.True(number >= 51, division.Code);
        }
    }

    [Fact]
    public void CarvedOut_ListsOnlyCurrentUnitsWithoutBoundary()
    {
        foreach (var division in IraqDivisions.All)
        {
            Assert.All(division.CarvedOut, unit =>
            {
                Assert.Equal(AdminLevel.SubDistrict, unit.Level);
                Assert.False(unit.HasBoundary);
                Assert.False(unit.IsDeprecated);
                Assert.Same(division, unit.CarvedFrom);
            });
        }
    }
}
