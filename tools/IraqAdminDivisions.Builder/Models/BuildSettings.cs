namespace IraqAdminDivisions.Builder.Models;

public static class BuildSettings
{
    public const int SchemaVersion = 1;
    public const string DataVersion = "1.0.0";
    public const string OchaSourceVersion = "v01";
    public const string UserAgent = "IraqAdminDivisionsBuilder/1.0 (open-source dataset builder; https://github.com/ammarkamil/IraqAdminDivisions)";
    public const string OchaDownloadUrl = "https://data.humdata.org/dataset/488bb3cd-3ce9-49d3-862a-3ce7975c63e1/resource/bde103b3-dc51-4e7b-9fd2-e38fa5600dc6/download/irq_admin_boundaries.xlsx";
    public const string OchaWorkbookDescription = "COD-AB workbook";
    public const string OchaGeoJsonUrl = "https://data.humdata.org/dataset/488bb3cd-3ce9-49d3-862a-3ce7975c63e1/resource/a9ed4d00-1182-4fdc-a0e4-82bbd20786db/download/irq_admin_boundaries.geojson.zip";
    public const string OchaGeoJsonDescription = "COD-AB GeoJSON boundaries";
    public const string BoundariesSource = "OCHA COD-AB Iraq " + OchaSourceVersion;
    public const double BoundaryToleranceDegrees = 0.0001;
    public const string RefreshArgument = "--refresh";

    public static readonly IReadOnlyList<DataSource> Sources =
    [
        new("OCHA COD-AB Iraq", OchaSourceVersion, "https://data.humdata.org/dataset/cod-ab-irq", "CC BY-IGO"),
        new("Wikidata", null, "https://www.wikidata.org", "CC0")
    ];

    public static readonly IReadOnlyDictionary<AdminLevel, int> ExpectedCounts = new Dictionary<AdminLevel, int>
    {
        [AdminLevel.Governorate] = 18,
        [AdminLevel.District] = 101,
        [AdminLevel.SubDistrict] = 294
    };

    public static readonly IReadOnlyDictionary<AdminLevel, int> ExpectedActiveCounts = new Dictionary<AdminLevel, int>
    {
        [AdminLevel.Governorate] = 19,
        [AdminLevel.District] = 101,
        [AdminLevel.SubDistrict] = 294
    };

    public static readonly IReadOnlyDictionary<AdminLevel, double> MatchThresholdKm = new Dictionary<AdminLevel, double>
    {
        [AdminLevel.Governorate] = 60,
        [AdminLevel.District] = 25,
        [AdminLevel.SubDistrict] = 15
    };

    public const double MinLat = 29;
    public const double MaxLat = 37.5;
    public const double MinLng = 38.5;
    public const double MaxLng = 49;
}
