using System.Text.Json;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class IraqCitiesReader : IDatasetReader
{
    private const string EnglishCentreSuffix = " Center";

    public IReadOnlyList<DatasetRow> Read(string directory)
    {
        using var document = DatasetJson.Load(directory, "iraq_cities.json");
        var rows = new List<DatasetRow>();
        foreach (var governorate in document.RootElement.GetProperty("governorates").EnumerateArray())
        {
            var governorateId = DatasetJson.RequiredText(governorate, "id");
            rows.Add(ToRow(governorate, AdminLevel.Governorate, null));
            rows.AddRange(governorate.GetProperty("cities").EnumerateArray().Select(city => ToRow(city, AdminLevel.District, governorateId)));
        }

        return rows;
    }

    private static DatasetRow ToRow(JsonElement entry, AdminLevel level, string? governorateId)
    {
        var nameAr = DatasetJson.RequiredText(entry, "name_ar");
        var nameEn = DatasetJson.RequiredText(entry, "name_en");
        return new DatasetRow(
            level,
            DatasetJson.RequiredText(entry, "id"),
            governorateId,
            nameAr,
            nameEn,
            DatasetJson.Text(entry, "name_ckb"),
            null,
            null,
            null,
            DatasetJson.IsCentreName(nameAr) || nameEn.EndsWith(EnglishCentreSuffix, StringComparison.Ordinal),
            []);
    }
}
