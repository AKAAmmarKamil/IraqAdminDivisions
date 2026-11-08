using System.ComponentModel.DataAnnotations;
using IraqAdminDivisions;
using IraqAdminDivisions.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapIraqDivisions();

app.MapPost("/orders", (CreateOrderRequest request, Language language = Language.Arabic) =>
{
    var validationResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, validateAllProperties: true))
    {
        var errors = validationResults
            .SelectMany(result => result.MemberNames.DefaultIfEmpty(string.Empty), (result, member) => (member, result.ErrorMessage))
            .GroupBy(entry => entry.member, entry => entry.ErrorMessage ?? "Invalid.")
            .ToDictionary(group => group.Key, group => group.ToArray());
        return Results.ValidationProblem(errors);
    }

    var address = IraqDivisions.FormatAddress(request.SubDistrictCode, language);
    return Results.Ok(new { request.Customer, request.SubDistrictCode, Address = address });
});

app.Run();

internal sealed record CreateOrderRequest(
    [property: Required] string Customer,
    [property: Required, IraqDivisionCode(AdminLevel.SubDistrict)] string SubDistrictCode);
