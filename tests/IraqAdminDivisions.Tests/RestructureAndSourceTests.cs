using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IraqAdminDivisions.Tests;

public class RestructureAndSourceTests
{
    private const string HalabjaGovernorateCode = "IQG19";
    private const string HalabjaDistrictCode = "IQG19Q01";
    private const string KhurmalDistrictCode = "IQG19Q53";
    private const string KhurmalCode = "IQG19Q53N51";
    private const string OchaHalabjaDistrictCode = "IQG06Q05";
    private const string OchaKhurmalCode = "IQG06Q05N02";
    private const string SulaymaniyahCode = "IQG06";
    private const string BaghdadCode = "IQG08";
    private static readonly Regex IsoCodePattern = new Regex("^IQ-[A-Z]{2}$");

    [Fact]
    public void HalabjaGovernorate_ExistsWithItsNamesAndNoIsoOrOchaCode()
    {
        var halabja = IraqDivisions.Find(HalabjaGovernorateCode)!;

        Assert.Equal(AdminLevel.Governorate, halabja.Level);
        Assert.Contains(halabja, IraqDivisions.Governorates);
        Assert.Equal("Halabja", halabja.Name.En);
        Assert.False(string.IsNullOrWhiteSpace(halabja.Name.Ar));
        Assert.False(string.IsNullOrWhiteSpace(halabja.Name.Ku));
        Assert.Null(halabja.IsoCode);
        Assert.Null(halabja.OchaCode);
        Assert.Equal(KurdishNameSource.Manual, halabja.KurdishNameSource);
        Assert.Contains(halabja.Children, child => child.Code == HalabjaDistrictCode);
        Assert.All(halabja.Children, child => Assert.StartsWith(HalabjaGovernorateCode, child.Code, StringComparison.Ordinal));
    }

    [Fact]
    public void MovedSubDistrict_HasACodeUnderItsNewDistrictAndKeepsItsOchaCode()
    {
        var khurmal = IraqDivisions.Find(KhurmalCode)!;

        Assert.False(khurmal.IsDeprecated);
        Assert.Equal(KhurmalDistrictCode, khurmal.District!.Code);
        Assert.StartsWith(KhurmalDistrictCode, khurmal.Code, StringComparison.Ordinal);
        Assert.Equal(HalabjaGovernorateCode, khurmal.Governorate.Code);
        Assert.Equal(OchaKhurmalCode, khurmal.OchaCode);
        Assert.True(khurmal.IsDistrictCentre);
        Assert.Contains("خورمال", khurmal.Aliases);
        Assert.EndsWith(khurmal.Governorate.Name.En, khurmal.Address.En);
        Assert.True(IraqDivisions.IsValid(KhurmalCode, AdminLevel.SubDistrict));
    }

    [Fact]
    public void OchaCodesOfMovedRows_AreNotCodesOfTheirOwn()
    {
        Assert.Null(IraqDivisions.Find(OchaKhurmalCode));
        Assert.Null(IraqDivisions.Find(OchaHalabjaDistrictCode));
        Assert.Contains(IraqDivisions.All, division => division.OchaCode == OchaHalabjaDistrictCode && division.Code == HalabjaDistrictCode);
        Assert.DoesNotContain(IraqDivisions.All, division => division.IsDeprecated);
    }

    [Fact]
    public void TheAttribute_AcceptsAMovedCodeAndRejectsItsOchaCode()
    {
        var attribute = new IraqDivisionCodeAttribute(AdminLevel.SubDistrict);
        var context = new ValidationContext(new object()) { MemberName = "AreaCode" };

        Assert.Equal(ValidationResult.Success, attribute.GetValidationResult(KhurmalCode, context));
        Assert.NotEqual(ValidationResult.Success, attribute.GetValidationResult(OchaKhurmalCode, context));
    }

    [Fact]
    public void BaghdadCustomers_FilterByPrefixDoesNotLeakAcrossTheRestructure()
    {
        var sulaymaniyahCodes = IraqDivisions.All.Where(division => division.Code.StartsWith(SulaymaniyahCode, StringComparison.Ordinal)).ToList();

        Assert.All(sulaymaniyahCodes, division => Assert.Equal(SulaymaniyahCode, division.Governorate.Code));
        Assert.All(IraqDivisions.All.Where(division => division.Code.StartsWith(BaghdadCode, StringComparison.Ordinal)),
            division => Assert.Equal(BaghdadCode, division.Governorate.Code));
    }

    [Fact]
    public void OchaCode_EqualsCodeForEveryUnrestructuredDivision()
    {
        static bool HasProjectNumber(Division division) =>
            division.Level != AdminLevel.Governorate
            && int.Parse(division.Code.Substring(division.Code.Length - 2), System.Globalization.CultureInfo.InvariantCulture) >= 51;
        var unrestructured = IraqDivisions.All.Where(division =>
            !division.Code.StartsWith(HalabjaGovernorateCode, StringComparison.Ordinal) && division.HasBoundary && !HasProjectNumber(division));

        Assert.All(unrestructured, division => Assert.Equal(division.Code, division.OchaCode));
        Assert.All(IraqDivisions.All.Where(division => division.HasBoundary && division.Level != AdminLevel.Governorate),
            division => Assert.NotNull(division.OchaCode));
    }

    [Fact]
    public void IsoCodes_AreUniqueAndOnlyOnTheEighteenOchaGovernorates()
    {
        var isoCodes = IraqDivisions.Governorates.Where(g => g.Code != HalabjaGovernorateCode).Select(g => g.IsoCode).ToList();

        Assert.Equal(18, isoCodes.Count);
        Assert.All(isoCodes, code => Assert.Matches(IsoCodePattern, code));
        Assert.Equal(isoCodes.Count, isoCodes.Distinct().Count());
        Assert.Equal("IQ-BG", IraqDivisions.Find(BaghdadCode)!.IsoCode);
        Assert.All(IraqDivisions.All.Where(d => d.Level != AdminLevel.Governorate), d => Assert.Null(d.IsoCode));
    }

    [Fact]
    public void KurdishNameDataset_IsSetExactlyWhenTheSourceIsDataset()
    {
        Assert.Contains(IraqDivisions.All, d => d.KurdishNameSource == KurdishNameSource.Dataset);
        Assert.All(IraqDivisions.All, d =>
        {
            if (d.KurdishNameSource == KurdishNameSource.Dataset)
                Assert.False(string.IsNullOrWhiteSpace(d.KurdishNameDataset));
            else
                Assert.Null(d.KurdishNameDataset);
        });
    }

    [Fact]
    public void ParseKurdishNameSource_ReadsDataset()
    {
        Assert.Equal(KurdishNameSource.Dataset, DivisionStore.ParseKurdishNameSource("dataset"));
    }
}
