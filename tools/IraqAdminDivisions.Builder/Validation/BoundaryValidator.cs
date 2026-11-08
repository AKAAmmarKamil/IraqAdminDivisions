using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Validation;

public sealed class BoundaryValidator
{
    public IReadOnlyList<string> Validate(IReadOnlyList<Division> divisions, IReadOnlyList<DivisionBoundary> boundaries)
    {
        var issues = new List<string>();
        var knownCodes = divisions.Select(division => division.Code).ToHashSet(StringComparer.Ordinal);

        foreach (var boundary in boundaries.Where(boundary => !knownCodes.Contains(boundary.Code)))
            issues.Add($"{boundary.Code}: polygon has no matching division");

        foreach (var group in boundaries.GroupBy(boundary => boundary.Code, StringComparer.Ordinal).Where(group => group.Count() > 1))
            issues.Add($"{group.Key}: {group.Count()} polygons share the same code");

        var polygonCodes = boundaries.Where(boundary => boundary.Polygons.Count > 0).Select(boundary => boundary.Code).ToHashSet(StringComparer.Ordinal);
        foreach (var subDistrict in divisions.Where(division => division.Level == AdminLevel.SubDistrict && !division.IsDeprecated && !polygonCodes.Contains(division.Code)))
            issues.Add($"{subDistrict.Code}: sub-district has no polygon");

        return issues;
    }
}
