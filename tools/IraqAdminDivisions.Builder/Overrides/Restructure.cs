using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Overrides;

public sealed record RestructureFile(IReadOnlyList<Restructure> Restructures);

public sealed record Restructure(
    string Id,
    string EffectiveDate,
    string Reason,
    IReadOnlyList<string> Sources,
    IReadOnlyList<NewDivision> NewDivisions,
    IReadOnlyList<DivisionMove> Moves,
    IReadOnlyList<DivisionReparent>? Reparent = null)
{
    public IReadOnlyList<DivisionReparent> ReparentOrEmpty => Reparent ?? [];
}

/// <summary>A division OCHA does not list. <see cref="GeometryFrom"/> copies an existing polygon (a unit that only changed level or parent); a unit without it has no boundary and must name the OCHA sub-district it was <see cref="CarvedFrom"/>.</summary>
public sealed record NewDivision(
    string Code,
    AdminLevel Level,
    string? ParentCode,
    DivisionName Name,
    IReadOnlyList<string> Aliases,
    string? WikidataId,
    string? GeometryFrom,
    string? CarvedFrom = null,
    DivisionCenter? Center = null,
    bool IsDistrictCentre = false,
    string? KurdishNameSource = null,
    IReadOnlyList<SourceReference>? Sources = null,
    string? KurdishNameDataset = null);

/// <summary>A source that backs a new division: the URL and what it proves (existence, parent, name).</summary>
public sealed record SourceReference(string Url, string Proves);

/// <summary>A move to a new code under <see cref="ParentCode"/>. With <see cref="KeepOldCode"/> (the default, for when the
/// previous release has real users) the old code stays as a deprecated row whose <c>replacedBy</c> is <see cref="To"/>;
/// without it the old code is dropped. <see cref="Name"/> and <see cref="IsDistrictCentre"/> optionally restate the row,
/// for a sub-district that becomes the centre of a new district; its old names stay searchable as aliases.</summary>
public sealed record DivisionMove(
    string From,
    string To,
    string ParentCode,
    bool KeepOldCode = true,
    bool? IsDistrictCentre = null,
    DivisionName? Name = null);

/// <summary>A move inside one governorate: the code stays and only the parent changes. Nothing else about a published row changes (projects built on the previous release keep working), so <c>IsDistrictCentre</c> and names stay as they were.</summary>
public sealed record DivisionReparent(string Code, string ParentCode);
