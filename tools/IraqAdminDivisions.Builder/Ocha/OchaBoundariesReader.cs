using System.IO.Compression;
using System.Text.Json;
using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Ocha;

public sealed class OchaBoundariesReader
{
    private const string PolygonType = "Polygon";
    private const string MultiPolygonType = "MultiPolygon";

    public IReadOnlyList<DivisionBoundary> Read(string zipPath)
    {
        using var archive = ZipFile.OpenRead(zipPath);
        var boundaries = new List<DivisionBoundary>();

        foreach (var level in Enum.GetValues<AdminLevel>())
        {
            var entryName = $"irq_admin{(int)level}.geojson";
            var entry = archive.Entries.FirstOrDefault(candidate => string.Equals(candidate.Name, entryName, StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidDataException($"{entryName} not found in {Path.GetFileName(zipPath)}.");

            using var stream = entry.Open();
            using var document = JsonDocument.Parse(stream);
            boundaries.AddRange(ReadFeatures(document.RootElement, level));
        }

        return boundaries;
    }

    private static IEnumerable<DivisionBoundary> ReadFeatures(JsonElement collection, AdminLevel level)
    {
        var codeProperty = $"adm{(int)level}_pcode";
        foreach (var feature in collection.GetProperty("features").EnumerateArray())
        {
            var code = feature.GetProperty("properties").GetProperty(codeProperty).GetString();
            if (string.IsNullOrWhiteSpace(code))
                continue;

            yield return new DivisionBoundary(code.Trim(), level, ReadPolygons(feature.GetProperty("geometry")));
        }
    }

    private static List<Polygon> ReadPolygons(JsonElement geometry)
    {
        var type = geometry.GetProperty("type").GetString();
        var coordinates = geometry.GetProperty("coordinates");

        return type switch
        {
            PolygonType => [ReadPolygon(coordinates)],
            MultiPolygonType => coordinates.EnumerateArray().Select(ReadPolygon).ToList(),
            _ => throw new InvalidDataException($"Unsupported geometry type {type}.")
        };
    }

    private static Polygon ReadPolygon(JsonElement rings) =>
        new(rings.EnumerateArray().Select(ReadRing).ToList());

    private static IReadOnlyList<LngLat> ReadRing(JsonElement ring) =>
        ring.EnumerateArray()
            .Select(point => new LngLat(point[0].GetDouble(), point[1].GetDouble()))
            .ToList();
}
