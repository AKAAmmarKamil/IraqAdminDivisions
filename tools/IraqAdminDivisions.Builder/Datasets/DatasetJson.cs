using System.Text.Json;

namespace IraqAdminDivisions.Builder.Datasets;

internal static class DatasetJson
{
    private const string ArabicCentrePrefix = "مركز";

    public static JsonDocument Load(string directory, string fileName) =>
        JsonDocument.Parse(File.ReadAllText(Path.Combine(directory, fileName)));

    public static string? Text(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(value.GetString())
            ? value.GetString()!.Trim()
            : null;

    public static string RequiredText(JsonElement element, string name) =>
        Text(element, name) ?? string.Empty;

    public static double? Number(JsonElement element, string name) =>
        element.TryGetProperty(name, out var value) && value.ValueKind == JsonValueKind.Number ? value.GetDouble() : null;

    public static IReadOnlyList<string> TextList(JsonElement element, string name) =>
        element.TryGetProperty(name, out var list) && list.ValueKind == JsonValueKind.Array
            ? list.EnumerateArray().Select(item => item.GetString()?.Trim()).OfType<string>().Where(item => item.Length > 0).ToList()
            : [];

    public static bool IsCentreName(string arabicName) =>
        arabicName.StartsWith(ArabicCentrePrefix, StringComparison.Ordinal);
}
