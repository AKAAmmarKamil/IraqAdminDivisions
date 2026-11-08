using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Overrides;

public sealed record RestructureFile(IReadOnlyList<Restructure> Restructures);

public sealed record Restructure(
    string Id,
    string EffectiveDate,
    string Reason,
    IReadOnlyList<string> Sources,
    IReadOnlyList<NewDivision> NewDivisions,
    IReadOnlyList<DivisionMove> Moves);

public sealed record NewDivision(
    string Code,
    AdminLevel Level,
    string? ParentCode,
    DivisionName Name,
    IReadOnlyList<string> Aliases,
    string? WikidataId,
    string GeometryFrom);

public sealed record DivisionMove(string From, string To, string ParentCode);
