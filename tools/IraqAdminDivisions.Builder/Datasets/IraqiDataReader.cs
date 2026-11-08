using System.Text.Json;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class IraqiDataReader : IDatasetReader
{
    public IReadOnlyList<DatasetRow> Read(string directory) =>
    [
        .. ReadFile(directory, "governorates.json", AdminLevel.Governorate),
        .. ReadFile(directory, "districts.json", AdminLevel.District),
        .. ReadFile(directory, "subdistricts.json", AdminLevel.SubDistrict)
    ];

    private static List<DatasetRow> ReadFile(string directory, string fileName, AdminLevel level)
    {
        using var document = DatasetJson.Load(directory, fileName);
        return document.RootElement.EnumerateArray().Select(entry => ToRow(entry, level)).ToList();
    }

    private static DatasetRow ToRow(JsonElement entry, AdminLevel level)
    {
        var point = Point(entry);
        var nameAr = DatasetJson.RequiredText(entry, "ar");
        var isMarkedCentre = entry.TryGetProperty("isCenter", out var isCenter) && isCenter.ValueKind == JsonValueKind.True;

        return new DatasetRow(
            level,
            DatasetJson.RequiredText(entry, "id"),
            level == AdminLevel.Governorate ? null : DatasetJson.Text(entry, "governorateId"),
            nameAr,
            DatasetJson.RequiredText(entry, "en"),
            DatasetJson.Text(entry, "ku"),
            DatasetJson.Text(entry, "pcode"),
            point is { } found ? DatasetJson.Number(found, "lat") : null,
            point is { } located ? DatasetJson.Number(located, "lng") : null,
            isMarkedCentre || DatasetJson.IsCentreName(nameAr),
            DatasetJson.TextList(entry, "aliases"));
    }

    private static JsonElement? Point(JsonElement entry)
    {
        foreach (var propertyName in new[] { "center", "capital" })
        {
            if (entry.TryGetProperty(propertyName, out var point) && point.ValueKind == JsonValueKind.Object)
                return point;
        }

        return null;
    }
}
