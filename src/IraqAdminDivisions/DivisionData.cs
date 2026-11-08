using System.Text.Json.Serialization;

namespace IraqAdminDivisions;

internal sealed class DivisionFile
{
    [JsonPropertyName("divisions")]
    public List<DivisionRecord> Divisions { get; set; } = new List<DivisionRecord>();
}

internal sealed class DivisionRecord
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";

    [JsonPropertyName("level")]
    public string Level { get; set; } = "";

    [JsonPropertyName("parentCode")]
    public string? ParentCode { get; set; }

    [JsonPropertyName("name")]
    public NameRecord Name { get; set; } = new NameRecord();

    [JsonPropertyName("aliases")]
    public List<string> Aliases { get; set; } = new List<string>();

    [JsonPropertyName("center")]
    public CenterRecord Center { get; set; } = new CenterRecord();

    [JsonPropertyName("areaSqKm")]
    public double AreaSqKm { get; set; }

    [JsonPropertyName("wikidataId")]
    public string? WikidataId { get; set; }

    [JsonPropertyName("isoCode")]
    public string? IsoCode { get; set; }

    [JsonPropertyName("kurdishNameSource")]
    public string KurdishNameSource { get; set; } = "";

    [JsonPropertyName("kurdishNameDataset")]
    public string? KurdishNameDataset { get; set; }

    [JsonPropertyName("ochaCode")]
    public string? OchaCode { get; set; }

    [JsonPropertyName("isDistrictCentre")]
    public bool IsDistrictCentre { get; set; }

    [JsonPropertyName("isDeprecated")]
    public bool IsDeprecated { get; set; }

    [JsonPropertyName("replacedBy")]
    public string? ReplacedBy { get; set; }
}

internal sealed class NameRecord
{
    [JsonPropertyName("ar")]
    public string Ar { get; set; } = "";

    [JsonPropertyName("en")]
    public string En { get; set; } = "";

    [JsonPropertyName("ku")]
    public string Ku { get; set; } = "";
}

internal sealed class CenterRecord
{
    [JsonPropertyName("lat")]
    public double Lat { get; set; }

    [JsonPropertyName("lng")]
    public double Lng { get; set; }
}
