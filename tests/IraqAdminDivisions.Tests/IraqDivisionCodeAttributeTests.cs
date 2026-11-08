using System.ComponentModel.DataAnnotations;

namespace IraqAdminDivisions.Tests;

public class IraqDivisionCodeAttributeTests
{
    private static string SubDistrictCode => IraqDivisions.All.First(d => d.Level == AdminLevel.SubDistrict).Code;

    [Fact]
    public void AcceptsValidCodeNullAndEmpty()
    {
        var attribute = new IraqDivisionCodeAttribute(AdminLevel.SubDistrict);

        Assert.True(attribute.IsValid(SubDistrictCode));
        Assert.True(attribute.IsValid(null));
        Assert.True(attribute.IsValid(""));
    }

    [Fact]
    public void RejectsUnknownCodeAndWrongLevel()
    {
        var attribute = new IraqDivisionCodeAttribute(AdminLevel.SubDistrict);

        Assert.False(attribute.IsValid("IQG99"));
        Assert.False(attribute.IsValid(IraqDivisions.Governorates[0].Code));
        Assert.False(attribute.IsValid(42));
    }

    [Fact]
    public void WithoutLevel_AcceptsAnyKnownCode()
    {
        var attribute = new IraqDivisionCodeAttribute();

        Assert.True(attribute.IsValid(IraqDivisions.Governorates[0].Code));
        Assert.True(attribute.IsValid(SubDistrictCode));
        Assert.False(attribute.IsValid("nope"));
    }

    [Fact]
    public void WorksWithTheValidator_AndMessageNamesTheMember()
    {
        var model = new AddressModel { CityCode = IraqDivisions.Governorates[0].Code };
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);

        Assert.False(isValid);
        var result = Assert.Single(results);
        Assert.Contains(nameof(AddressModel.CityCode), result.ErrorMessage);
    }

    private sealed class AddressModel
    {
        [IraqDivisionCode(AdminLevel.SubDistrict)]
        public string? CityCode { get; set; }
    }
}
