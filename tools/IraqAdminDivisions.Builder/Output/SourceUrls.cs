using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Output;

internal static class SourceUrls
{
    private const string Separator = " ";

    /// <summary>The source URLs of a project-added division, space-separated; <c>null</c> for an OCHA row.</summary>
    public static string? Join(Division division) =>
        division.Sources is { Count: > 0 } sources ? string.Join(Separator, sources.Select(source => source.Url)) : null;
}
