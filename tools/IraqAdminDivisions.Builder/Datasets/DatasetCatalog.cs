namespace IraqAdminDivisions.Builder.Datasets;

public static class DatasetCatalog
{
    public const string LicenseFileName = "LICENSE";

    public static readonly IReadOnlyList<KurdishDataset> All =
    [
        new("karam-fikre/iraqi-data", "MIT", "4b80dabf979101e95bef8fe88f71b61301fa46c4", "karam-fikre-iraqi-data", new IraqiDataReader(), DeclaresKurdishUnreviewed: true),
        new("Iraqi-Open-Source/official-iraqi-governorates", "MIT", "bd1f03c841fd4cfd35484f51f4765ee525e46a85", "iraqi-open-source-official-iraqi-governorates", new OfficialGovernoratesReader()),
        new("aryzonedev/iraq-cities", "CC-BY-4.0", "ce066afd246be55353f74a7cb70a2132e4e9f252", "aryzonedev-iraq-cities", new IraqCitiesReader())
    ];
}
