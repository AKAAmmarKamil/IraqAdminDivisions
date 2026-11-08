using System.Globalization;
using System.Text;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Output;

public sealed class CsvDivisionsWriter
{
    private const string Header = "code,level,parent_code,name_ar,name_en,name_ku,lat,lng,area_sqkm,wikidata_id,iso_code,name_ku_source,kurdish_name_dataset,is_district_centre,ocha_code,is_deprecated,replaced_by";
    private const string LineBreak = "\r\n";

    public Task WriteAsync(string path, IReadOnlyList<Division> divisions)
    {
        var builder = new StringBuilder(Header).Append(LineBreak);
        foreach (var division in divisions)
            builder.Append(ToLine(division)).Append(LineBreak);

        return OutputFile.WriteAsync(path, builder.ToString(), shouldWriteBom: true);
    }

    private static string ToLine(Division division) => string.Join(',',
        Escape(division.Code),
        Escape(division.Level.ToString()),
        Escape(division.ParentCode),
        Escape(division.Name.Ar),
        Escape(division.Name.En),
        Escape(division.Name.Ku),
        division.Center.Lat.ToString("F6", CultureInfo.InvariantCulture),
        division.Center.Lng.ToString("F6", CultureInfo.InvariantCulture),
        division.AreaSqKm.ToString("F2", CultureInfo.InvariantCulture),
        Escape(division.WikidataId),
        Escape(division.IsoCode),
        Escape(division.KurdishNameSource),
        Escape(division.KurdishNameDataset),
        division.IsDistrictCentre ? "true" : "false",
        Escape(division.OchaCode),
        division.IsDeprecated ? "true" : "false",
        Escape(division.ReplacedBy));

    private static string Escape(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.IndexOfAny([',', '"', '\r', '\n']) >= 0
            ? $"\"{value.Replace("\"", "\"\"")}\""
            : value;
    }
}
