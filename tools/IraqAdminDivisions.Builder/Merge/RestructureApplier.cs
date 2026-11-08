using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Models;
using IraqAdminDivisions.Builder.Overrides;

namespace IraqAdminDivisions.Builder.Merge;

public sealed class RestructureApplier
{
    private readonly RestructureFile _file;

    public RestructureApplier(RestructureFile file) => _file = file;

    public IReadOnlyList<string> FindUnknownCodes(IReadOnlyList<Division> divisions)
    {
        var knownCodes = divisions.Select(division => division.Code).ToHashSet(StringComparer.Ordinal);
        var issues = new List<string>();
        foreach (var restructure in _file.Restructures)
        {
            foreach (var move in restructure.Moves.Where(move => !knownCodes.Contains(move.From)))
                issues.Add($"restructure {restructure.Id}: moved code {move.From} does not exist");

            foreach (var newDivision in restructure.NewDivisions.Where(newDivision => !knownCodes.Contains(newDivision.GeometryFrom)))
                issues.Add($"restructure {restructure.Id}: geometry source {newDivision.GeometryFrom} does not exist");
        }

        return issues;
    }

    public IReadOnlyList<Division> ApplyToDivisions(IReadOnlyList<Division> divisions)
    {
        var byCode = divisions
            .Select(division => division with { OchaCode = division.Code })
            .ToDictionary(division => division.Code, StringComparer.Ordinal);

        foreach (var restructure in _file.Restructures)
        {
            foreach (var newDivision in restructure.NewDivisions)
                byCode.Add(newDivision.Code, CreateDivision(newDivision, byCode[newDivision.GeometryFrom]));

            foreach (var move in restructure.Moves)
            {
                var original = byCode[move.From];
                byCode.Add(move.To, original with { Code = move.To, ParentCode = move.ParentCode });
                byCode[move.From] = original with { IsDeprecated = true, ReplacedBy = move.To };
            }
        }

        return byCode.Values.OrderBy(division => division.Code, StringComparer.Ordinal).ToList();
    }

    public IReadOnlyList<DivisionBoundary> ApplyToBoundaries(IReadOnlyList<DivisionBoundary> boundaries)
    {
        var byCode = boundaries.ToDictionary(boundary => boundary.Code, StringComparer.Ordinal);
        foreach (var restructure in _file.Restructures)
        {
            foreach (var newDivision in restructure.NewDivisions)
            {
                if (byCode.TryGetValue(newDivision.GeometryFrom, out var source))
                    byCode.Add(newDivision.Code, new DivisionBoundary(newDivision.Code, newDivision.Level, source.Polygons));
            }

            foreach (var move in restructure.Moves)
            {
                if (!byCode.Remove(move.From, out var moved))
                    continue;

                byCode.Add(move.To, new DivisionBoundary(move.To, moved.Level, moved.Polygons));
            }
        }

        return byCode.Values.OrderBy(boundary => boundary.Code, StringComparer.Ordinal).ToList();
    }

    private static Division CreateDivision(NewDivision newDivision, Division geometrySource) => new(
        newDivision.Code,
        newDivision.Level,
        newDivision.ParentCode,
        newDivision.Name,
        newDivision.Aliases,
        geometrySource.Center,
        geometrySource.AreaSqKm,
        newDivision.WikidataId,
        IsoCode: null,
        KurdishNameSources.Manual,
        KurdishNameDataset: null,
        IsDistrictCentre: false);
}
