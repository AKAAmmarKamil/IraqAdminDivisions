using System.Globalization;
using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;
using IraqAdminDivisions.Builder.Overrides;

namespace IraqAdminDivisions.Builder.Merge;

public sealed class RestructureApplier
{
    /// <summary>Codes 01–50 inside a parent are OCHA's numbering space; project-assigned district and sub-district codes start here (PLAN-1.1 D2).</summary>
    public const int FirstProjectCodeNumber = 51;

    private const int GovernoratePrefixLength = 5;

    private readonly RestructureFile _file;

    public RestructureApplier(RestructureFile file) => _file = file;

    public IReadOnlyList<Restructure> Restructures => _file.Restructures;

    public int AddedActiveCount(AdminLevel level) =>
        _file.Restructures.Sum(restructure => restructure.NewDivisions.Count(newDivision => newDivision.Level == level));

    public IReadOnlyList<string> FindIssues(IReadOnlyList<Division> divisions)
    {
        var byCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
        var newCodes = _file.Restructures
            .SelectMany(restructure => restructure.NewDivisions)
            .Select(newDivision => newDivision.Code)
            .ToHashSet(StringComparer.Ordinal);
        var movedCodes = _file.Restructures
            .SelectMany(restructure => restructure.Moves)
            .Select(move => move.To);
        var knownCodes = byCode.Keys.Concat(newCodes).Concat(movedCodes).ToHashSet(StringComparer.Ordinal);
        var issues = new List<string>();
        foreach (var restructure in _file.Restructures)
        {
            foreach (var move in restructure.Moves.Where(move => !byCode.ContainsKey(move.From)))
                issues.Add($"restructure {restructure.Id}: moved code {move.From} does not exist");

            foreach (var newDivision in restructure.NewDivisions)
                CheckNewDivision(restructure, newDivision, byCode, knownCodes, issues);

            foreach (var reparent in restructure.ReparentOrEmpty)
                CheckReparent(restructure, reparent, byCode, knownCodes, issues);

            // Later restructures see the codes this one created (Halabja's moved rows, for example).
            foreach (var move in restructure.Moves.Where(move => byCode.ContainsKey(move.From)))
            {
                if (byCode.ContainsKey(move.To))
                    issues.Add($"restructure {restructure.Id}: move target {move.To} already exists");
                if (!move.To.StartsWith(move.ParentCode, StringComparison.Ordinal))
                    issues.Add($"restructure {restructure.Id}: move target {move.To} does not start with its parent {move.ParentCode}");
                if (move.Name is { } name && (string.IsNullOrWhiteSpace(name.Ku) || !SoraniAlphabet.IsSorani(name.Ku)))
                    issues.Add($"restructure {restructure.Id}: move {move.From}: Kurdish name is missing or not Sorani");
                byCode[move.To] = byCode[move.From] with { Code = move.To, ParentCode = move.ParentCode };
                if (!move.KeepOldCode)
                    byCode.Remove(move.From);
            }
            foreach (var newDivision in restructure.NewDivisions.Where(newDivision => !byCode.ContainsKey(newDivision.Code)))
                byCode[newDivision.Code] = new Division(
                    newDivision.Code, newDivision.Level, newDivision.ParentCode, newDivision.Name, newDivision.Aliases,
                    newDivision.Center ?? new DivisionCenter(0, 0), 0, newDivision.WikidataId, IsoCode: null,
                    newDivision.KurdishNameSource, KurdishNameDataset: null, newDivision.IsDistrictCentre,
                    HasBoundary: newDivision.GeometryFrom is not null);
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
                byCode.Add(newDivision.Code, CreateDivision(newDivision, byCode));

            foreach (var move in restructure.Moves)
            {
                var original = byCode[move.From];
                var aliases = move.Name is null
                    ? original.Aliases
                    : original.Aliases.Concat([original.Name.Ar, original.Name.En]).Distinct(StringComparer.Ordinal).ToList();
                byCode.Add(move.To, original with
                {
                    Code = move.To,
                    ParentCode = move.ParentCode,
                    Name = move.Name ?? original.Name,
                    Aliases = aliases,
                    IsDistrictCentre = move.IsDistrictCentre ?? original.IsDistrictCentre
                });
                if (move.KeepOldCode)
                {
                    byCode[move.From] = original with { IsDeprecated = true, ReplacedBy = move.To };
                }
                else
                {
                    byCode.Remove(move.From);
                    RenameReferences(byCode, move.From, move.To);
                }
            }

            foreach (var reparent in restructure.ReparentOrEmpty)
            {
                var original = byCode[reparent.Code];
                byCode[reparent.Code] = original with { ParentCode = reparent.ParentCode };
            }
        }

        return byCode.Values.OrderBy(division => division.Code, StringComparer.Ordinal).ToList();
    }

    /// <summary>A dropped code may still be named by rows created earlier (carvedFrom, children of a moved district).</summary>
    private static void RenameReferences(Dictionary<string, Division> byCode, string from, string to)
    {
        foreach (var division in byCode.Values.ToList())
        {
            if (division.CarvedFrom == from || division.ParentCode == from)
            {
                byCode[division.Code] = division with
                {
                    CarvedFrom = division.CarvedFrom == from ? to : division.CarvedFrom,
                    ParentCode = division.ParentCode == from ? to : division.ParentCode
                };
            }
        }
    }

    public IReadOnlyList<DivisionBoundary> ApplyToBoundaries(IReadOnlyList<DivisionBoundary> boundaries)
    {
        var byCode = boundaries.ToDictionary(boundary => boundary.Code, StringComparer.Ordinal);
        foreach (var restructure in _file.Restructures)
        {
            foreach (var newDivision in restructure.NewDivisions)
            {
                if (newDivision.GeometryFrom is not null && byCode.TryGetValue(newDivision.GeometryFrom, out var source))
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

    /// <summary>Whether a district or sub-district code ends in a project number (51 or higher). Governorate codes are not checked.</summary>
    public static bool HasProjectNumber(string code, AdminLevel level)
    {
        var expectedLength = level switch
        {
            AdminLevel.District => 8,
            AdminLevel.SubDistrict => 11,
            _ => 0
        };
        if (expectedLength == 0)
            return true;

        return code.Length == expectedLength
            && int.TryParse(code.AsSpan(code.Length - 2), NumberStyles.None, CultureInfo.InvariantCulture, out var number)
            && number >= FirstProjectCodeNumber;
    }

    private static void CheckNewDivision(
        Restructure restructure, NewDivision newDivision, Dictionary<string, Division> byCode, HashSet<string> knownCodes, List<string> issues)
    {
        var prefix = $"restructure {restructure.Id}: new {newDivision.Code}";
        if (byCode.ContainsKey(newDivision.Code))
            issues.Add($"{prefix} already exists");
        if (newDivision.Level != AdminLevel.Governorate && !HasProjectNumber(newDivision.Code, newDivision.Level))
            issues.Add($"{prefix} must end in a project number {FirstProjectCodeNumber}-99 (codes 01-50 are OCHA's)");
        if (newDivision.ParentCode is not null && !newDivision.Code.StartsWith(newDivision.ParentCode, StringComparison.Ordinal))
            issues.Add($"{prefix} does not start with its parent code {newDivision.ParentCode}");
        if (newDivision.ParentCode is not null && !knownCodes.Contains(newDivision.ParentCode))
            issues.Add($"{prefix}: parent {newDivision.ParentCode} does not exist");
        if (newDivision.GeometryFrom is not null && !byCode.ContainsKey(newDivision.GeometryFrom))
            issues.Add($"{prefix}: geometry source {newDivision.GeometryFrom} does not exist");

        if (newDivision.GeometryFrom is null)
        {
            if (newDivision.Sources is null || newDivision.Sources.Count == 0)
                issues.Add($"{prefix} has no sources");
            if (newDivision.Level == AdminLevel.SubDistrict && newDivision.CarvedFrom is null)
                issues.Add($"{prefix} has neither geometryFrom nor carvedFrom");
            if (newDivision.Level == AdminLevel.District && newDivision.CarvedFrom is null && newDivision.Center is null)
                issues.Add($"{prefix} needs a center or carvedFrom to place it");
        }

        if (newDivision.KurdishNameSource is not null && !KurdishNameSources.All.Contains(newDivision.KurdishNameSource))
            issues.Add($"{prefix}: unknown kurdishNameSource {newDivision.KurdishNameSource}");
        if ((newDivision.KurdishNameSource == KurdishNameSources.Dataset) != (newDivision.KurdishNameDataset is not null))
            issues.Add($"{prefix}: kurdishNameDataset must be set exactly when kurdishNameSource is dataset");
        if (string.IsNullOrWhiteSpace(newDivision.Name.Ku))
            issues.Add($"{prefix} has no Kurdish name");
        else if (!SoraniAlphabet.IsSorani(newDivision.Name.Ku))
            issues.Add($"{prefix}: Kurdish name {newDivision.Name.Ku} has non-Sorani characters {string.Concat(SoraniAlphabet.ForeignCharacters(newDivision.Name.Ku))}");
        if (string.IsNullOrWhiteSpace(newDivision.Name.Ar) || !NameNormalizer.HasArabicLetter(newDivision.Name.Ar))
            issues.Add($"{prefix} has no Arabic name");
        if (string.IsNullOrWhiteSpace(newDivision.Name.En))
            issues.Add($"{prefix} has no English name");

        foreach (var source in newDivision.Sources ?? [])
        {
            if (source.Url.Contains("citypopulation.de", StringComparison.OrdinalIgnoreCase))
                issues.Add($"{prefix}: citypopulation.de is a checklist only and cannot be a source ({source.Url})");
            if (string.IsNullOrWhiteSpace(source.Proves))
                issues.Add($"{prefix}: source {source.Url} does not say what it proves");
        }

        if (newDivision.CarvedFrom is not null)
        {
            if (!byCode.TryGetValue(newDivision.CarvedFrom, out var carvedFrom))
                issues.Add($"{prefix}: carvedFrom {newDivision.CarvedFrom} does not exist");
            else if (carvedFrom.Level != AdminLevel.SubDistrict)
                issues.Add($"{prefix}: carvedFrom {newDivision.CarvedFrom} is not an OCHA sub-district");
            else if (!SameGovernorate(newDivision.Code, carvedFrom.Code))
                issues.Add($"{prefix}: carvedFrom {newDivision.CarvedFrom} is in another governorate");
        }
    }

    private static void CheckReparent(
        Restructure restructure, DivisionReparent reparent, Dictionary<string, Division> byCode, HashSet<string> knownCodes, List<string> issues)
    {
        var prefix = $"restructure {restructure.Id}: reparent {reparent.Code}";
        if (!byCode.TryGetValue(reparent.Code, out var division))
        {
            issues.Add($"{prefix} does not exist");
            return;
        }

        if (!knownCodes.Contains(reparent.ParentCode))
            issues.Add($"{prefix}: parent {reparent.ParentCode} does not exist");
        if (!SameGovernorate(reparent.Code, reparent.ParentCode))
            issues.Add($"{prefix}: parent {reparent.ParentCode} is in another governorate; use moves[] (deprecate + replacedBy) instead");
        if (division.Level == AdminLevel.Governorate)
            issues.Add($"{prefix}: a governorate has no parent to change");
    }

    private static bool SameGovernorate(string code, string otherCode) =>
        code.Length >= GovernoratePrefixLength
        && otherCode.Length >= GovernoratePrefixLength
        && code.AsSpan(0, GovernoratePrefixLength).SequenceEqual(otherCode.AsSpan(0, GovernoratePrefixLength));

    private static Division CreateDivision(NewDivision newDivision, Dictionary<string, Division> byCode)
    {
        var geometrySource = newDivision.GeometryFrom is null ? null : byCode[newDivision.GeometryFrom];
        var carvedFrom = newDivision.CarvedFrom is null ? null : byCode[newDivision.CarvedFrom];
        var center = newDivision.Center ?? geometrySource?.Center ?? carvedFrom?.Center
            ?? throw new InvalidOperationException($"{newDivision.Code}: no center, geometry source or carvedFrom");

        return new Division(
            newDivision.Code,
            newDivision.Level,
            newDivision.ParentCode,
            newDivision.Name,
            newDivision.Aliases,
            center,
            geometrySource?.AreaSqKm ?? 0,
            newDivision.WikidataId,
            IsoCode: null,
            newDivision.KurdishNameSource ?? KurdishNameSources.Manual,
            newDivision.KurdishNameDataset,
            newDivision.IsDistrictCentre,
            HasBoundary: geometrySource is not null,
            CarvedFrom: newDivision.CarvedFrom,
            Sources: newDivision.Sources);
    }
}
