using System.Globalization;
using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class DatasetCalibrator
{
    public DatasetCalibration Calibrate(KurdishDataset dataset, IReadOnlyList<DatasetRow> rows, IReadOnlyList<DatasetNameComparison> comparisons)
    {
        var wikidataComparisons = comparisons
            .Where(comparison => comparison.Ours.KurdishNameSource == KurdishNameSources.Wikidata && comparison.Agreement is not null)
            .ToList();
        var beyondGovernorates = wikidataComparisons.Where(comparison => comparison.Ours.Level != AdminLevel.Governorate).ToList();
        var levels = Enum.GetValues<AdminLevel>().Select(level => ToLevelAgreement(level, wikidataComparisons)).ToList();

        var cleanedNames = rows.Select(row => DatasetKurdishName.Clean(row.Ku)).OfType<string>().ToList();
        var pureSoraniCount = cleanedNames.Count(SoraniAlphabet.IsSorani);
        var levelsWithKurdishNames = rows.Where(row => DatasetKurdishName.Clean(row.Ku) is not null).Select(row => row.Level).Distinct().Count();

        var calibration = new DatasetCalibration(
            wikidataComparisons.Count,
            wikidataComparisons.Count(comparison => comparison.Agreement == KurdishAgreement.Exact),
            wikidataComparisons.Count(comparison => comparison.Agreement == KurdishAgreement.Near),
            levels,
            beyondGovernorates.Count,
            beyondGovernorates.Count(comparison => comparison.Agreement == KurdishAgreement.Exact),
            cleanedNames.Count,
            pureSoraniCount,
            levelsWithKurdishNames,
            IsTrusted: false,
            Verdict: string.Empty);

        var failures = Failures(dataset, calibration);
        return calibration with
        {
            IsTrusted = failures.Count == 0,
            Verdict = failures.Count == 0 ? "trusted" : "not trusted: " + string.Join("; ", failures)
        };
    }

    private static LevelAgreement ToLevelAgreement(AdminLevel level, List<DatasetNameComparison> comparisons)
    {
        var atLevel = comparisons.Where(comparison => comparison.Ours.Level == level).ToList();
        return new LevelAgreement(
            level,
            atLevel.Count,
            atLevel.Count(comparison => comparison.Agreement == KurdishAgreement.Exact),
            atLevel.Count(comparison => comparison.Agreement == KurdishAgreement.Near));
    }

    private static List<string> Failures(KurdishDataset dataset, DatasetCalibration calibration)
    {
        var failures = new List<string>();
        if (dataset.DeclaresKurdishUnreviewed)
            failures.Add("its own documentation states that the Kurdish names are unreviewed drafts");

        if (calibration.ComparedBeyondGovernorates < DatasetTrustPolicy.MinComparedBeyondGovernorates)
            failures.Add($"only {calibration.ComparedBeyondGovernorates} district or sub-district names comparable with Wikidata (need {DatasetTrustPolicy.MinComparedBeyondGovernorates})");

        if (calibration.ExactRateBeyondGovernorates < DatasetTrustPolicy.MinExactAgreementBeyondGovernorates)
            failures.Add($"exact agreement with Wikidata beyond governorates {Percent(calibration.ExactRateBeyondGovernorates)} is below {Percent(DatasetTrustPolicy.MinExactAgreementBeyondGovernorates)}");

        if (calibration.PureSoraniShare < DatasetTrustPolicy.MinPureSoraniShare)
            failures.Add($"pure Sorani share {Percent(calibration.PureSoraniShare)} is below {Percent(DatasetTrustPolicy.MinPureSoraniShare)}");

        return failures;
    }

    private static string Percent(double share) =>
        (share * 100).ToString("0.0", CultureInfo.InvariantCulture) + "%";
}
