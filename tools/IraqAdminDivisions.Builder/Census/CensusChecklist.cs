using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Census;

/// <summary>One row of the local census checklist (data/local, gitignored). Only names and hierarchy are read; the checklist is never shipped (PLAN-1.1 D1).</summary>
public sealed record CensusUnit(AdminLevel Level, string Governorate, string District, string NameEn, string NameNative, string Note)
{
    public bool IsDissolved => Note.Contains("dissolved", StringComparison.OrdinalIgnoreCase);
}

public sealed class CensusChecklistReader
{
    private const int ColumnCount = 6;

    public IReadOnlyList<CensusUnit>? Read(string path)
    {
        if (!File.Exists(path))
            return null;

        var units = new List<CensusUnit>();
        foreach (var line in File.ReadLines(path).Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var columns = line.Split('\t');
            if (columns.Length < ColumnCount)
                throw new InvalidOperationException($"{Path.GetFileName(path)}: expected {ColumnCount} columns in '{line}'");

            units.Add(new CensusUnit(ParseLevel(columns[0]), columns[1].Trim(), columns[2].Trim(), columns[3].Trim(), columns[4].Trim(), columns[5].Trim()));
        }

        return units;
    }

    private static AdminLevel ParseLevel(string level) => level.Trim() switch
    {
        "governorate" => AdminLevel.Governorate,
        "district" => AdminLevel.District,
        "subdistrict" => AdminLevel.SubDistrict,
        _ => throw new InvalidOperationException($"Unknown census level '{level}'")
    };
}
