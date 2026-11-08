using System.Text.Json;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class OfficialGovernoratesReader : IDatasetReader
{
    public IReadOnlyList<DatasetRow> Read(string directory)
    {
        using var document = DatasetJson.Load(directory, "governorates.json");
        var rows = new List<DatasetRow>();
        foreach (var governorate in document.RootElement.GetProperty("governorates").EnumerateArray())
        {
            var governorateId = DatasetJson.RequiredText(governorate, "id");
            var coordinates = governorate.GetProperty("coordinates");
            rows.Add(ToRow(governorate, AdminLevel.Governorate, null, DatasetJson.Number(coordinates, "latitude"), DatasetJson.Number(coordinates, "longitude")));
            foreach (var district in governorate.GetProperty("districts").EnumerateArray())
            {
                rows.Add(ToRow(district, AdminLevel.District, governorateId, null, null));
                rows.AddRange(district.GetProperty("sub_districts").EnumerateArray()
                    .Select(subDistrict => ToRow(subDistrict, AdminLevel.SubDistrict, governorateId, null, null)));
            }
        }

        return rows;
    }

    private static DatasetRow ToRow(JsonElement entry, AdminLevel level, string? governorateId, double? lat, double? lng)
    {
        var name = entry.GetProperty("name");
        var nameAr = DatasetJson.RequiredText(name, "ar");
        return new DatasetRow(
            level,
            DatasetJson.RequiredText(entry, "id"),
            governorateId,
            nameAr,
            DatasetJson.RequiredText(name, "en"),
            DatasetJson.Text(name, "ku"),
            null,
            lat,
            lng,
            DatasetJson.IsCentreName(nameAr),
            []);
    }
}
