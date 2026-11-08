using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed record DatasetNameComparison(
    KurdishDataset Dataset,
    DatasetMatch Match,
    Division Ours,
    string? TheirName,
    bool IsPureSorani,
    KurdishAgreement? Agreement)
{
    public bool IsUsable => TheirName is not null && IsPureSorani;
}

public sealed record LevelAgreement(AdminLevel Level, int Compared, int Exact, int Near);

public sealed record DatasetCalibration(
    int Compared,
    int Exact,
    int Near,
    IReadOnlyList<LevelAgreement> Levels,
    int ComparedBeyondGovernorates,
    int ExactBeyondGovernorates,
    int KurdishNameCount,
    int PureSoraniCount,
    int LevelsWithKurdishNames,
    bool IsTrusted,
    string Verdict)
{
    public double ExactRate => Compared == 0 ? 0 : (double)Exact / Compared;
    public double ExactRateBeyondGovernorates => ComparedBeyondGovernorates == 0 ? 0 : (double)ExactBeyondGovernorates / ComparedBeyondGovernorates;
    public double PureSoraniShare => KurdishNameCount == 0 ? 0 : (double)PureSoraniCount / KurdishNameCount;
}

public sealed record DatasetEvaluation(
    KurdishDataset Dataset,
    IReadOnlyList<DatasetRow> Rows,
    DatasetMatchResult Matching,
    IReadOnlyList<DatasetNameComparison> Comparisons,
    DatasetCalibration Calibration);
