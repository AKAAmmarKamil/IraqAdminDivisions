using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class KurdishDatasetApplier
{
    public KurdishDatasetApplication Apply(IReadOnlyList<Division> divisions, IReadOnlyList<DatasetEvaluation> evaluations)
    {
        var trustedComparisonsByCode = evaluations
            .Where(evaluation => evaluation.Calibration.IsTrusted)
            .OrderByDescending(evaluation => evaluation.Calibration.ExactRate)
            .ThenBy(evaluation => evaluation.Dataset.Repository, StringComparer.Ordinal)
            .SelectMany(evaluation => evaluation.Comparisons.Where(comparison => comparison.IsUsable))
            .ToLookup(comparison => comparison.Match.Code, StringComparer.Ordinal);

        var decisions = divisions
            .Select(division => Decide(division, trustedComparisonsByCode[division.Code].ToList()))
            .ToList();
        var decisionsByCode = decisions.ToDictionary(decision => decision.Original.Code, StringComparer.Ordinal);

        var crosscheck = new KurdishCrosscheck(
            evaluations,
            BuildDisagreements(evaluations, decisionsByCode),
            evaluations.Select(evaluation => CountUse(evaluation.Dataset, decisions)).ToList(),
            DatasetPairAnalyzer.Analyze(evaluations),
            decisions.Count(decision => decision.Original.KurdishNameSource == KurdishNameSources.Transliterated && decision.Chosen is null));

        return new KurdishDatasetApplication(decisions.Select(decision => decision.Result).ToList(), crosscheck);
    }

    private static DatasetUse CountUse(KurdishDataset dataset, List<NameDecision> decisions)
    {
        var chosenHere = decisions.Where(decision => decision.Chosen?.Dataset == dataset).ToList();
        return new DatasetUse(dataset, chosenHere.Count(decision => !decision.IsReplacement), chosenHere.Count(decision => decision.IsReplacement));
    }

    private static NameDecision Decide(Division division, List<DatasetNameComparison> trustedComparisons)
    {
        if (division.KurdishNameSource != KurdishNameSources.Transliterated || trustedComparisons.Count == 0)
            return new NameDecision(division, division, null, false);

        var confirming = trustedComparisons.FirstOrDefault(comparison => comparison.Agreement == KurdishAgreement.Exact);
        if (confirming is not null)
            return new NameDecision(division, WithDataset(division, division.Name.Ku, division.Aliases, confirming), confirming, false);

        var replacing = trustedComparisons[0];
        var aliases = division.Aliases.Append(division.Name.Ku!).Distinct(StringComparer.Ordinal).ToList();
        return new NameDecision(division, WithDataset(division, replacing.TheirName, aliases, replacing), replacing, true);
    }

    private static Division WithDataset(Division division, string? kurdishName, IReadOnlyList<string> aliases, DatasetNameComparison source) =>
        division with
        {
            Name = division.Name with { Ku = kurdishName },
            Aliases = aliases,
            KurdishNameSource = KurdishNameSources.Dataset,
            KurdishNameDataset = source.Dataset.Repository
        };

    private static List<KurdishDisagreement> BuildDisagreements(IReadOnlyList<DatasetEvaluation> evaluations, Dictionary<string, NameDecision> decisionsByCode)
    {
        var disagreements = new List<KurdishDisagreement>();
        foreach (var evaluation in evaluations)
        {
            foreach (var comparison in evaluation.Comparisons.Where(comparison => comparison.Agreement is not null and not KurdishAgreement.Exact))
            {
                var ours = comparison.Ours;
                disagreements.Add(new KurdishDisagreement(
                    evaluation.Dataset.Repository,
                    ours.Code,
                    ours.Level,
                    ours.Name.En,
                    ours.Name.Ku!,
                    ours.KurdishNameSource!,
                    comparison.TheirName!,
                    comparison.Agreement!.Value,
                    comparison.Match.Method,
                    DescribeOutcome(evaluation, comparison, decisionsByCode[ours.Code])));
            }
        }

        return disagreements
            .OrderBy(disagreement => disagreement.Repository, StringComparer.Ordinal)
            .ThenBy(disagreement => disagreement.Code, StringComparer.Ordinal)
            .ToList();
    }

    private static string DescribeOutcome(DatasetEvaluation evaluation, DatasetNameComparison comparison, NameDecision decision)
    {
        var ours = comparison.Ours;
        if (ours.KurdishNameSource != KurdishNameSources.Transliterated)
            return $"kept ours ({ours.KurdishNameSource})";

        if (!evaluation.Calibration.IsTrusted)
            return "not applied (dataset not trusted)";

        if (!comparison.IsPureSorani)
            return "not applied (name is not pure Sorani)";

        if (decision.Chosen?.Dataset == comparison.Dataset)
            return "replaced our transliteration, which is kept as an alias";

        return decision.IsReplacement
            ? $"superseded by {decision.Chosen!.Dataset.Repository}"
            : $"kept ours (confirmed by {decision.Chosen!.Dataset.Repository})";
    }

    private sealed record NameDecision(Division Original, Division Result, DatasetNameComparison? Chosen, bool IsReplacement);
}
