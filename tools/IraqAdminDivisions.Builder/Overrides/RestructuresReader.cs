using System.Text.Json;
using System.Text.Json.Serialization;

namespace IraqAdminDivisions.Builder.Overrides;

public sealed class RestructuresReader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public RestructureFile Read(string path)
    {
        if (!File.Exists(path))
            return new RestructureFile([]);

        using var stream = File.OpenRead(path);
        return JsonSerializer.Deserialize<RestructureFile>(stream, Options)
            ?? throw new InvalidOperationException($"{Path.GetFileName(path)} is empty.");
    }
}
