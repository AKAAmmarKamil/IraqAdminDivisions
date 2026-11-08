using System.Text.Json;
using IraqAdminDivisions.Builder.Geometry;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Output;

public sealed class GeoJsonWriter
{
    private const string PolygonType = "Polygon";
    private const string MultiPolygonType = "MultiPolygon";

    public Task WriteAsync(string path, IReadOnlyList<Division> divisions, IReadOnlyList<DivisionBoundary> boundaries)
    {
        var divisionsByCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
        var ordered = boundaries
            .Where(boundary => boundary.Polygons.Count > 0)
            .OrderBy(boundary => boundary.Level)
            .ThenBy(boundary => boundary.Code, StringComparer.Ordinal)
            .ToList();

        var json = GeometryJson.Build(writer =>
        {
            writer.WriteStartObject();
            writer.WriteString("type", "FeatureCollection");
            writer.WriteStartArray("features");
            foreach (var boundary in ordered)
                WriteFeature(writer, divisionsByCode[boundary.Code], boundary);
            writer.WriteEndArray();
            writer.WriteEndObject();
        });

        return OutputFile.WriteAsync(path, json, shouldWriteBom: false);
    }

    private static void WriteFeature(Utf8JsonWriter writer, Division division, DivisionBoundary boundary)
    {
        writer.WriteStartObject();
        writer.WriteString("type", "Feature");
        writer.WriteStartObject("properties");
        writer.WriteString("code", division.Code);
        writer.WriteString("level", division.Level.ToString());
        writer.WriteString("parentCode", division.ParentCode);
        writer.WriteString("name_ar", division.Name.Ar);
        writer.WriteString("name_en", division.Name.En);
        writer.WriteString("name_ku", division.Name.Ku);
        writer.WriteString("iso_code", division.IsoCode);
        writer.WriteString("name_ku_source", division.KurdishNameSource);
        writer.WriteString("kurdish_name_dataset", division.KurdishNameDataset);
        writer.WriteString("ocha_code", division.OchaCode);
        writer.WriteEndObject();
        WriteGeometry(writer, boundary);
        writer.WriteEndObject();
    }

    private static void WriteGeometry(Utf8JsonWriter writer, DivisionBoundary boundary)
    {
        writer.WriteStartObject("geometry");
        var isSinglePolygon = boundary.Polygons.Count == 1;
        writer.WriteString("type", isSinglePolygon ? PolygonType : MultiPolygonType);
        writer.WritePropertyName("coordinates");
        if (isSinglePolygon)
            GeometryJson.WritePolygonRings(writer, boundary.Polygons[0]);
        else
            GeometryJson.WritePolygons(writer, boundary.Polygons);
        writer.WriteEndObject();
    }
}
