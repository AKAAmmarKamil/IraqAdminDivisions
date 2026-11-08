using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace IraqAdminDivisions.AspNetCore;

/// <summary>Maps the Iraqi administrative division endpoints.</summary>
public static class IraqDivisionsEndpoints
{
    private const string DefaultPrefix = "/api/iraq-divisions";
    private const string TagName = "IraqDivisions";
    private const string InvalidLevelMessage = "Level must be Governorate, District or SubDistrict.";
    private const string NotGovernorateMessage = "Code must be a governorate code.";
    private const int DefaultSearchLimit = 10;
    private const int MaxSearchLimit = 50;

    /// <summary>
    /// Maps <c>/governorates</c>, <c>/{code}</c>, <c>/{code}/children</c>, <c>/{code}/localities</c>, <c>/search</c> and <c>/nearest</c> under <paramref name="prefix"/>.
    /// </summary>
    /// <param name="endpoints">The route builder.</param>
    /// <param name="prefix">Route prefix for the group.</param>
    /// <returns>The route group, so callers can chain <c>RequireAuthorization()</c> and similar.</returns>
    public static RouteGroupBuilder MapIraqDivisions(this IEndpointRouteBuilder endpoints, string prefix = DefaultPrefix)
    {
        var group = endpoints.MapGroup(prefix).WithTags(TagName);

        group.MapGet("/governorates", (HttpRequest request) =>
            {
                var language = LanguageResolver.Resolve(request);
                return TypedResults.Ok(IraqDivisions.Governorates.Select(division => division.ToDto(language)).ToList());
            })
            .WithName("GetIraqGovernorates")
            .WithSummary("Lists the 18 governorates.");

        group.MapGet("/search", Results<Ok<List<DivisionSearchResultDto>>, ValidationProblem> (HttpRequest request, string? q, string? level, int? limit) =>
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return Invalid("q", "Query 'q' is required.");
                }

                if (!TryParseLevel(level, out var adminLevel))
                {
                    return Invalid("level", InvalidLevelMessage);
                }

                var language = LanguageResolver.Resolve(request);
                var clampedLimit = Math.Clamp(limit ?? DefaultSearchLimit, 1, MaxSearchLimit);
                var results = IraqDivisions.Search(q, clampedLimit, adminLevel).Select(result => result.ToDto(language)).ToList();
                return TypedResults.Ok(results);
            })
            .WithName("SearchIraqDivisions")
            .WithSummary("Searches division names in Arabic, English and Kurdish.");

        group.MapGet("/nearest", Results<Ok<DivisionDto>, ValidationProblem> (HttpRequest request, double? lat, double? lng, string? level) =>
            {
                if (lat is null or double.NaN or < -90 or > 90)
                {
                    return Invalid("lat", "Latitude must be between -90 and 90.");
                }

                if (lng is null or double.NaN or < -180 or > 180)
                {
                    return Invalid("lng", "Longitude must be between -180 and 180.");
                }

                if (!TryParseLevel(level, out var adminLevel))
                {
                    return Invalid("level", InvalidLevelMessage);
                }

                var nearest = IraqDivisions.Nearest(lat.Value, lng.Value, adminLevel ?? AdminLevel.SubDistrict);
                return TypedResults.Ok(nearest.ToDto(LanguageResolver.Resolve(request)));
            })
            .WithName("GetNearestIraqDivision")
            .WithSummary("Finds the division whose centroid is closest to a coordinate.");

        group.MapGet("/{code}", Results<Ok<DivisionDto>, NotFound> (HttpRequest request, string code) =>
                IraqDivisions.Find(code) is { } division
                    ? TypedResults.Ok(division.ToDto(LanguageResolver.Resolve(request)))
                    : TypedResults.NotFound())
            .WithName("GetIraqDivision")
            .WithSummary("Gets one division by code.");

        group.MapGet("/{code}/children", Results<Ok<List<DivisionDto>>, NotFound> (HttpRequest request, string code) =>
            {
                if (IraqDivisions.Find(code) is not { } division)
                {
                    return TypedResults.NotFound();
                }

                var language = LanguageResolver.Resolve(request);
                return TypedResults.Ok(division.Children.Select(child => child.ToDto(language)).ToList());
            })
            .WithName("GetIraqDivisionChildren")
            .WithSummary("Lists the direct children of a division.");

        group.MapGet("/{code}/localities", Results<Ok<List<DivisionDto>>, NotFound, ValidationProblem> (HttpRequest request, string code) =>
            {
                if (IraqDivisions.Find(code) is not { } division)
                {
                    return TypedResults.NotFound();
                }

                if (division.Level != AdminLevel.Governorate)
                {
                    return Invalid("code", NotGovernorateMessage);
                }

                var language = LanguageResolver.Resolve(request);
                return TypedResults.Ok(division.Localities.Select(locality => locality.ToDto(language)).ToList());
            })
            .WithName("GetIraqGovernorateLocalities")
            .WithSummary("Lists the flat areas of a governorate: its districts plus its sub-districts that are not district centres.");

        return group;
    }

    private static bool TryParseLevel(string? value, out AdminLevel? level)
    {
        level = null;
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        if (Enum.TryParse<AdminLevel>(value, true, out var parsed) && Enum.IsDefined(parsed))
        {
            level = parsed;
            return true;
        }

        return false;
    }

    private static ValidationProblem Invalid(string field, string message) =>
        TypedResults.ValidationProblem(new Dictionary<string, string[]> { [field] = new[] { message } });
}
