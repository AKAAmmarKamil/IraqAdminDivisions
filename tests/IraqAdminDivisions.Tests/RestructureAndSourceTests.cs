using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IraqAdminDivisions.Tests;

public class RestructureAndSourceTests
{
    private const string HalabjaGovernorateCode = "IQG19";
    private const string HalabjaDistrictCode = "IQG19Q01";
    private const string KhurmalCode = "IQG19Q01N02";
    private const string OldHalabjaDistrictCode = "IQG06Q05";
    private const string OldKhurmalCode = "IQG06Q05N02";
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
        Assert.Equal(HalabjaDistrictCode, Assert.Single(halabja.Children).Code);
    }

    [Fact]
    public void MovedSubDistrict_HasTheNewParentChainAndKeepsItsOchaCode()
    {
        var khurmal = IraqDivisions.Find(KhurmalCode)!;

        Assert.False(khurmal.IsDeprecated);
        Assert.Equal(HalabjaDistrictCode, khurmal.District!.Code);
        Assert.Equal(HalabjaGovernorateCode, khurmal.Governorate.Code);
        Assert.Equal(OldKhurmalCode, khurmal.OchaCode);
        Assert.EndsWith(khurmal.Governorate.Name.En, khurmal.Address.En);
        Assert.True(IraqDivisions.IsValid(KhurmalCode, AdminLevel.SubDistrict));
    }

    [Fact]
    public void OldCode_StillResolvesAsDeprecatedAndPointsToItsReplacement()
    {
        var oldKhurmal = IraqDivisions.Find(OldKhurmalCode)!;

        Assert.True(oldKhurmal.IsDeprecated);
        Assert.Equal(KhurmalCode, oldKhurmal.ReplacedByCode);
        Assert.False(IraqDivisions.Find(oldKhurmal.ReplacedByCode!)!.IsDeprecated);
        Assert.Equal(SulaymaniyahCode, oldKhurmal.Governorate.Code);
        Assert.Equal(oldKhurmal.Name.Ar, IraqDivisions.Find(KhurmalCode)!.Name.Ar);
        Assert.False(string.IsNullOrWhiteSpace(IraqDivisions.FormatAddress(OldKhurmalCode, Language.Arabic)));
    }

    [Fact]
    public void DeprecatedCodes_AreLeftOutOfEveryListButStillFound()
    {
        var sulaymaniyah = IraqDivisions.Find(SulaymaniyahCode)!;

        Assert.DoesNotContain(IraqDivisions.All, division => division.IsDeprecated);
        Assert.DoesNotContain(sulaymaniyah.Children, division => division.Code == OldHalabjaDistrictCode);
        Assert.DoesNotContain(sulaymaniyah.Localities, division => division.Code.StartsWith(OldHalabjaDistrictCode, StringComparison.Ordinal));
        Assert.DoesNotContain(IraqDivisions.Search("خورمال"), result => result.Division.IsDeprecated);
        Assert.Equal(KhurmalCode, IraqDivisions.Search("خورمال")[0].Division.Code);
        Assert.NotNull(IraqDivisions.Find(OldHalabjaDistrictCode));
    }

    [Fact]
    public void DeprecatedDistrict_ListsOnlyItsDeprecatedChildren()
    {
        var oldDistrict = IraqDivisions.Find(OldHalabjaDistrictCode)!;

        Assert.NotEmpty(oldDistrict.Children);
        Assert.All(oldDistrict.Children, child => Assert.True(child.IsDeprecated));
    }

    [Fact]
    public void IsValid_AndTheAttribute_RejectDeprecatedCodes()
    {
        var attribute = new IraqDivisionCodeAttribute(AdminLevel.SubDistrict);
        var context = new ValidationContext(new object()) { MemberName = "AreaCode" };

        Assert.False(IraqDivisions.IsValid(OldKhurmalCode));
        Assert.NotEqual(ValidationResult.Success, attribute.GetValidationResult(OldKhurmalCode, context));
        Assert.Equal(ValidationResult.Success, attribute.GetValidationResult(KhurmalCode, context));
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
        var unrestructured = IraqDivisions.All.Where(division => !division.Code.StartsWith(HalabjaGovernorateCode, StringComparison.Ordinal));

        Assert.All(unrestructured, division => Assert.Equal(division.Code, division.OchaCode));
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
