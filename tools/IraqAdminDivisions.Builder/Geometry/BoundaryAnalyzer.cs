using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Geometry;

public sealed class BoundaryAnalyzer
{
    public BoundaryAnalysis Analyze(IReadOnlyList<Division> divisions, IReadOnlyList<DivisionBoundary> boundaries)
    {
        var boundariesByCode = boundaries.ToDictionary(boundary => boundary.Code, StringComparer.Ordinal);
        var divisionsByCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);

        var centreChecks = divisions
            .Where(division => boundariesByCode.ContainsKey(division.Code))
            .Select(division => new CentreCheck(
                division.Code,
                division.Level,
                PointInPolygon.IsInside(boundariesByCode[division.Code], new LngLat(division.Center.Lng, division.Center.Lat))))
            .ToList();

        var bboxChecks = divisions
            .Where(division => division.Level == AdminLevel.SubDistrict && boundariesByCode.ContainsKey(division.Code))
            .Select(division => CheckBox(division, divisionsByCode, boundariesByCode))
            .OfType<BboxCheck>()
            .ToList();

        return new BoundaryAnalysis(centreChecks, bboxChecks);
    }

    private static BboxCheck? CheckBox(
        Division subDistrict,
        Dictionary<string, Division> divisionsByCode,
        Dictionary<string, DivisionBoundary> boundariesByCode)
    {
        var governorateCode = FindGovernorateCode(subDistrict, divisionsByCode);
        if (governorateCode is null || !boundariesByCode.TryGetValue(governorateCode, out var governorate))
            return null;

        return new BboxCheck(subDistrict.Code, governorateCode, governorate.Box.Contains(boundariesByCode[subDistrict.Code].Box));
    }

    private static string? FindGovernorateCode(Division division, Dictionary<string, Division> divisionsByCode)
    {
        var current = division;
        while (current.Level != AdminLevel.Governorate)
        {
            if (current.ParentCode is null || !divisionsByCode.TryGetValue(current.ParentCode, out var parent))
                return null;
            current = parent;
        }

        return current.Code;
    }
}
