using System.Text.Json;
using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Output;

public sealed class BoundariesWriter
{
    public Task WriteAsync(string path, IReadOnlyList<DivisionBoundary> boundaries)
    {
        var subDistricts = boundaries
            .Where(boundary => boundary.Level == AdminLevel.SubDistrict)
            .OrderBy(boundary => boundary.Code, StringComparer.Ordinal)
            .ToList();

        var json = GeometryJson.Build(writer =>
        {
            writer.WriteStartObject();
            writer.WriteNumber("schemaVersion", BuildSettings.SchemaVersion);
            writer.WriteString("source", BuildSettings.BoundariesSource);
            writer.WriteNumber("toleranceDegrees", BuildSettings.BoundaryToleranceDegrees);
            writer.WriteStartArray("divisions");
            foreach (var boundary in subDistricts)
                WriteDivision(writer, boundary);
            writer.WriteEndArray();
            writer.WriteEndObject();
        });

        return OutputFile.WriteAsync(path, json, shouldWriteBom: false);
    }

    private static void WriteDivision(Utf8JsonWriter writer, DivisionBoundary boundary)
    {
        writer.WriteStartObject();
        writer.WriteString("code", boundary.Code);
        writer.WriteStartArray("bbox");
        writer.WriteNumberValue(boundary.Box.MinLng);
        writer.WriteNumberValue(boundary.Box.MinLat);
        writer.WriteNumberValue(boundary.Box.MaxLng);
        writer.WriteNumberValue(boundary.Box.MaxLat);
        writer.WriteEndArray();
        writer.WritePropertyName("polygons");
        GeometryJson.WritePolygons(writer, boundary.Polygons);
        writer.WriteEndObject();
    }
}
