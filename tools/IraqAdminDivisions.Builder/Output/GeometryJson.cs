using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using IraqAdminDivisions.Builder.Geometry;

namespace IraqAdminDivisions.Builder.Output;

public static class GeometryJson
{
    private static readonly JsonWriterOptions CompactOptions = new()
    {
        Indented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string Build(Action<Utf8JsonWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream, CompactOptions))
            write(writer);

        return Encoding.UTF8.GetString(stream.ToArray()) + "\n";
    }

    public static void WritePolygons(Utf8JsonWriter writer, IReadOnlyList<Polygon> polygons)
    {
        writer.WriteStartArray();
        foreach (var polygon in polygons)
            WritePolygonRings(writer, polygon);
        writer.WriteEndArray();
    }

    public static void WritePolygonRings(Utf8JsonWriter writer, Polygon polygon)
    {
        writer.WriteStartArray();
        foreach (var ring in polygon.Rings)
        {
            writer.WriteStartArray();
            foreach (var point in ring)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(point.Lng);
                writer.WriteNumberValue(point.Lat);
                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
