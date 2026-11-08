using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed class BoundaryValidator
{
    public IReadOnlyList<string> Validate(IReadOnlyList<Division> divisions, IReadOnlyList<DivisionBoundary> boundaries)
    {
        var issues = new List<string>();
        var divisionsByCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);

        foreach (var boundary in boundaries.Where(boundary => !divisionsByCode.ContainsKey(boundary.Code)))
            issues.Add($"{boundary.Code}: polygon has no matching division");

        foreach (var group in boundaries.GroupBy(boundary => boundary.Code, StringComparer.Ordinal).Where(group => group.Count() > 1))
            issues.Add($"{group.Key}: {group.Count()} polygons share the same code");

        var polygonsByCode = boundaries
            .Where(boundary => boundary.Polygons.Count > 0)
            .GroupBy(boundary => boundary.Code, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First(), StringComparer.Ordinal);

        foreach (var subDistrict in divisions.Where(division => division.Level == AdminLevel.SubDistrict && !division.IsDeprecated && division.HasBoundary && !polygonsByCode.ContainsKey(division.Code)))
            issues.Add($"{subDistrict.Code}: sub-district has no polygon");

        foreach (var division in divisions.Where(division => !division.HasBoundary && polygonsByCode.ContainsKey(division.Code)))
            issues.Add($"{division.Code}: has a polygon but is marked without boundary");

        foreach (var division in divisions.Where(division => division.CarvedFrom is not null))
        {
            if (!polygonsByCode.TryGetValue(division.CarvedFrom!, out var carvedFrom))
                issues.Add($"{division.Code}: carvedFrom {division.CarvedFrom} has no polygon");
            else if (divisionsByCode.TryGetValue(division.CarvedFrom!, out var source) && source.Center == division.Center)
                continue;
            else if (!PointInPolygon.IsInside(carvedFrom, new LngLat(division.Center.Lng, division.Center.Lat)))
                issues.Add($"{division.Code}: its coordinate ({division.Center.Lat:F4}, {division.Center.Lng:F4}) is outside carvedFrom {division.CarvedFrom}");
        }

        return issues;
    }
}
