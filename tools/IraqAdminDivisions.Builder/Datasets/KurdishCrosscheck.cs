using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed record KurdishDisagreement(
    string Repository,
    string Code,
    AdminLevel Level,
    string English,
    string OurName,
    string OurSource,
    string TheirName,
    KurdishAgreement Agreement,
    string Method,
    string Outcome);

public sealed record DatasetUse(KurdishDataset Dataset, int Confirmed, int Replaced);

public sealed record KurdishCrosscheck(
    IReadOnlyList<DatasetEvaluation> Evaluations,
    IReadOnlyList<KurdishDisagreement> Disagreements,
    IReadOnlyList<DatasetUse> Uses,
    IReadOnlyList<DatasetPairAgreement> Pairs,
    int TransliteratedWithoutDatasetNameCount)
{
    public int ConfirmedCount => Uses.Sum(use => use.Confirmed);
    public int ReplacedCount => Uses.Sum(use => use.Replaced);

    public IReadOnlyList<DataSource> UsedSources() => Uses
        .Where(use => use.Confirmed + use.Replaced > 0)
        .Select(use => new DataSource(use.Dataset.Repository, use.Dataset.CommitSha, use.Dataset.Url, use.Dataset.License))
        .ToList();
}

public sealed record KurdishDatasetApplication(IReadOnlyList<Division> Divisions, KurdishCrosscheck Crosscheck);
