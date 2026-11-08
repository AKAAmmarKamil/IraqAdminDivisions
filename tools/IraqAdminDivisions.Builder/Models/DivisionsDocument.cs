namespace IraqAdminDivisions.Builder.Models;

public sealed record DataSource(string Name, string? Version, string Url, string License);

public sealed record DivisionsDocument(
    int SchemaVersion,
    string DataVersion,
    string GeneratedAtUtc,
    IReadOnlyList<DataSource> Sources,
    IReadOnlyList<Division> Divisions);
