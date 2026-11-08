using System.Globalization;
using System.Text;
using ExcelDataReader;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Ocha;

public sealed class OchaReader
{
    private static readonly IReadOnlyDictionary<string, AdminLevel> SheetLevels = new Dictionary<string, AdminLevel>(StringComparer.OrdinalIgnoreCase)
    {
        ["irq_admin1"] = AdminLevel.Governorate,
        ["irq_admin2"] = AdminLevel.District,
        ["irq_admin3"] = AdminLevel.SubDistrict
    };

    public IReadOnlyList<OchaRow> Read(string workbookPath)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var stream = File.OpenRead(workbookPath);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var rows = new List<OchaRow>();

        do
        {
            if (SheetLevels.TryGetValue(reader.Name, out var level))
                rows.AddRange(ReadSheet(reader, level));
        }
        while (reader.NextResult());

        return rows;
    }

    private static List<OchaRow> ReadSheet(IExcelDataReader reader, AdminLevel level)
    {
        var columns = ReadHeader(reader);
        var prefix = $"adm{(int)level}";
        var parentPrefix = $"adm{(int)level - 1}";
        var rows = new List<OchaRow>();

        while (reader.Read())
        {
            var code = CellText(reader, columns, $"{prefix}_pcode");
            if (code is null)
                continue;

            rows.Add(new OchaRow(
                level,
                code,
                level == AdminLevel.Governorate ? null : CellText(reader, columns, $"{parentPrefix}_pcode"),
                CellText(reader, columns, $"{prefix}_name") ?? string.Empty,
                CellText(reader, columns, $"{prefix}_name1") ?? string.Empty,
                CellText(reader, columns, $"{prefix}_ref_name"),
                CellNumber(reader, columns, "center_lat"),
                CellNumber(reader, columns, "center_lon"),
                CellNumber(reader, columns, "area_sqkm")));
        }

        return rows;
    }

    private static Dictionary<string, int> ReadHeader(IExcelDataReader reader)
    {
        if (!reader.Read())
            throw new InvalidDataException($"Sheet {reader.Name} is empty.");

        var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < reader.FieldCount; i++)
        {
            var header = reader.GetValue(i)?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(header))
                columns[header] = i;
        }

        return columns;
    }

    private static string? CellText(IExcelDataReader reader, Dictionary<string, int> columns, string column)
    {
        if (!columns.TryGetValue(column, out var index))
            return null;

        var text = Convert.ToString(reader.GetValue(index), CultureInfo.InvariantCulture)?.Trim();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    private static double? CellNumber(IExcelDataReader reader, Dictionary<string, int> columns, string column)
    {
        var text = CellText(reader, columns, column);
        return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : null;
    }
}
