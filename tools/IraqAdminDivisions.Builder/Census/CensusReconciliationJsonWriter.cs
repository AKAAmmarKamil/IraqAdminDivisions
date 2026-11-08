using System.Text.Encodings.Web;
using System.Text.Json;
using IraqAdminDivisions.Builder.Output;

namespace IraqAdminDivisions.Builder.Census;

/// <summary>Writes the reconciliation as JSON next to the local checklist (gitignored), as input for drafting restructures.json batches.</summary>
public sealed class CensusReconciliationJsonWriter
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public Task WriteAsync(string path, ReconciliationResult result)
    {
        var governorates = result.Tallies.Select(tally => new { census = tally.Governorate, code = tally.Division?.Code });
        var rows = result.Rows.Select(row => new
        {
            bucket = row.Bucket.ToString(),
            level = row.Level.ToString(),
            governorate = row.Governorate,
            censusDistrict = row.Census?.District,
            censusNameEn = row.Census?.NameEn,
            censusNameAr = row.Census?.NameNative,
            censusNote = row.Census?.Note,
            code = row.Division?.Code,
            censusParentCode = row.CensusParent?.Code,
            note = row.Note
        });

        return OutputFile.WriteAsync(path, JsonSerializer.Serialize(new { governorates, rows }, Options) + "\n", shouldWriteBom: false);
    }
}
