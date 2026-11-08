namespace IraqAdminDivisions.Tests;

public class LookupTests
{
    private static Division SampleSubDistrict => IraqDivisions.Find("IQG07Q02N02") ?? IraqDivisions.All.First(d => d.Level == AdminLevel.SubDistrict);

    [Fact]
    public void Find_IsCaseInsensitiveAndTrims()
    {
        var expected = IraqDivisions.Find("IQG02");

        Assert.NotNull(expected);
        Assert.Same(expected, IraqDivisions.Find("iqg02"));
        Assert.Same(expected, IraqDivisions.Find("  IqG02 \t"));
    }

    [Theory]
    [InlineData("IQG99")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Find_ReturnsNullForUnknownOrEmptyCode(string? code)
    {
        Assert.Null(IraqDivisions.Find(code));
    }

    [Fact]
    public void IsValid_ChecksExistenceAndLevel()
    {
        var subDistrict = SampleSubDistrict;

        Assert.True(IraqDivisions.IsValid(subDistrict.Code));
        Assert.True(IraqDivisions.IsValid(subDistrict.Code, AdminLevel.SubDistrict));
        Assert.False(IraqDivisions.IsValid(subDistrict.Code, AdminLevel.District));
        Assert.True(IraqDivisions.IsValid(subDistrict.Governorate.Code, AdminLevel.Governorate));
        Assert.False(IraqDivisions.IsValid(subDistrict.Governorate.Code, AdminLevel.SubDistrict));
        Assert.False(IraqDivisions.IsValid("NOPE"));
        Assert.False(IraqDivisions.IsValid(null));
    }

    [Fact]
    public void FormatAddress_RendersSubDistrictDistrictAndGovernorate()
    {
        var subDistrict = SampleSubDistrict;
        var district = subDistrict.District!;
        var governorate = subDistrict.Governorate;

        var arabic = IraqDivisions.FormatAddress(subDistrict.Code, Language.Arabic);
        var english = IraqDivisions.FormatAddress(subDistrict.Code, Language.English);

        Assert.Equal(
            string.Join("، ", subDistrict.Name.Ar, district.Name.Ar, governorate.Name.Ar),
            arabic);
        Assert.Equal(
            string.Join(", ", subDistrict.Name.En, district.Name.En, governorate.Name.En),
            english);
        Assert.Equal(english, subDistrict.FormatAddress(Language.English));
    }

    [Fact]
    public void FormatAddress_ForGovernorateIsJustItsName()
    {
        var governorate = IraqDivisions.Governorates[0];

        Assert.Equal(governorate.Name.En, IraqDivisions.FormatAddress(governorate.Code, Language.English));
    }

    [Fact]
    public void FormatAddress_ThrowsWithTheCodeForUnknownCode()
    {
        var exception = Assert.Throws<KeyNotFoundException>(() => IraqDivisions.FormatAddress("IQG99X", Language.English));

        Assert.Contains("IQG99X", exception.Message);
    }

    [Fact]
    public void ToString_IsEnglishNameAndCode()
    {
        var division = IraqDivisions.Find("IQG02")!;

        Assert.Contains(division.Name.En, division.ToString());
        Assert.Contains(division.Code, division.ToString());
    }
}
