using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Output;

public sealed class JsonDivisionsWriter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        NewLine = "\n",
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() }
    };

    public Task WriteAsync(string path, IReadOnlyList<Division> divisions, IReadOnlyList<DataSource> sources, DateTime generatedAtUtc)
    {
        var document = new DivisionsDocument(
            BuildSettings.SchemaVersion,
            BuildSettings.DataVersion,
            generatedAtUtc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"),
            sources,
            divisions);

        return OutputFile.WriteAsync(path, JsonSerializer.Serialize(document, Options) + "\n", shouldWriteBom: false);
    }
}
