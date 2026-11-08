using System.Globalization;
using System.Text;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Output;

public enum SqlDialect
{
    Standard,
    SqlServer
}

public sealed class SqlDivisionsWriter
{
    private readonly SqlDialect _dialect;

    public SqlDivisionsWriter(SqlDialect dialect = SqlDialect.Standard) => _dialect = dialect;

    private const string TableName = "iraq_admin_divisions";
    private const string NullLiteral = "NULL";

    private static readonly string CreateTable = BuildCreateTable("VARCHAR", "BOOLEAN");
    private static readonly string SqlServerCreateTable = BuildCreateTable("NVARCHAR", "BIT");

    private static string BuildCreateTable(string textType, string booleanType) => $"""
        CREATE TABLE {TableName} (
            code VARCHAR(16) NOT NULL PRIMARY KEY,
            level VARCHAR(16) NOT NULL,
            parent_code VARCHAR(16) NULL,
            name_ar {textType}(200) NOT NULL,
            name_en {textType}(200) NOT NULL,
            name_ku {textType}(200) NULL,
            lat DECIMAL(9, 6) NOT NULL,
            lng DECIMAL(9, 6) NOT NULL,
            area_sqkm DECIMAL(12, 2) NOT NULL,
            wikidata_id VARCHAR(16) NULL,
            iso_code VARCHAR(8) NULL,
            name_ku_source VARCHAR(16) NULL,
            kurdish_name_dataset VARCHAR(64) NULL,
            is_district_centre {booleanType} NOT NULL,
            ocha_code VARCHAR(16) NULL,
            is_deprecated {booleanType} NOT NULL,
            replaced_by VARCHAR(16) NULL,
            has_boundary {booleanType} NOT NULL,
            carved_from VARCHAR(16) NULL,
            source_urls {textType}(2000) NULL
        );
        """;

    public Task WriteAsync(string path, IReadOnlyList<Division> divisions)
    {
        var builder = new StringBuilder();
        builder.Append("-- Iraqi administrative divisions. Data: OCHA COD-AB Iraq (CC BY-IGO), Wikidata (CC0); Kurdish name datasets and their licences: see DATA_LICENSE.md.\n");
        builder.Append(_dialect == SqlDialect.SqlServer ? SqlServerCreateTable : CreateTable).Append("\n\n");
        foreach (var division in divisions)
            builder.Append(ToInsert(division)).Append('\n');

        return OutputFile.WriteAsync(path, builder.ToString(), shouldWriteBom: false);
    }

    private string ToInsert(Division division) =>
        $"INSERT INTO {TableName} (code, level, parent_code, name_ar, name_en, name_ku, lat, lng, area_sqkm, wikidata_id, iso_code, name_ku_source, kurdish_name_dataset, is_district_centre, ocha_code, is_deprecated, replaced_by, has_boundary, carved_from, source_urls) VALUES ("
        + string.Join(", ",
            Literal(division.Code),
            Literal(division.Level.ToString()),
            Literal(division.ParentCode),
            TextLiteral(division.Name.Ar),
            TextLiteral(division.Name.En),
            TextLiteral(division.Name.Ku),
            division.Center.Lat.ToString("F6", CultureInfo.InvariantCulture),
            division.Center.Lng.ToString("F6", CultureInfo.InvariantCulture),
            division.AreaSqKm.ToString("F2", CultureInfo.InvariantCulture),
            Literal(division.WikidataId),
            Literal(division.IsoCode),
            Literal(division.KurdishNameSource),
            Literal(division.KurdishNameDataset),
            BooleanLiteral(division.IsDistrictCentre),
            Literal(division.OchaCode),
            BooleanLiteral(division.IsDeprecated),
            Literal(division.ReplacedBy),
            BooleanLiteral(division.HasBoundary),
            Literal(division.CarvedFrom),
            TextLiteral(SourceUrls.Join(division)))
        + ");";

    private string TextLiteral(string? value) =>
        _dialect == SqlDialect.SqlServer && value is not null ? "N" + Literal(value) : Literal(value);

    private string BooleanLiteral(bool value) =>
        _dialect == SqlDialect.SqlServer ? (value ? "1" : "0") : (value ? "TRUE" : "FALSE");

    private static string Literal(string? value) =>
        value is null ? NullLiteral : $"'{value.Replace("'", "''")}'";
}
