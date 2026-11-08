using IraqAdminDivisions.Builder.Matching;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed record DatasetPairAgreement(KurdishDataset First, KurdishDataset Second, int Shared, int Exact, int Near);

public static class DatasetPairAnalyzer
{
    public static IReadOnlyList<DatasetPairAgreement> Analyze(IReadOnlyList<DatasetEvaluation> evaluations)
    {
        var pairs = new List<DatasetPairAgreement>();
        for (var first = 0; first < evaluations.Count; first++)
        {
            for (var second = first + 1; second < evaluations.Count; second++)
                pairs.Add(Compare(evaluations[first], evaluations[second]));
        }

        return pairs;
    }

    private static DatasetPairAgreement Compare(DatasetEvaluation first, DatasetEvaluation second)
    {
        var secondNames = second.Comparisons
            .Where(comparison => comparison.TheirName is not null)
            .GroupBy(comparison => comparison.Match.Code, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.First().TheirName!, StringComparer.Ordinal);
        var agreements = first.Comparisons
            .Where(comparison => comparison.TheirName is not null && secondNames.ContainsKey(comparison.Match.Code))
            .Select(comparison => SoraniNormalizer.Compare(comparison.TheirName!, secondNames[comparison.Match.Code]))
            .ToList();

        return new DatasetPairAgreement(
            first.Dataset,
            second.Dataset,
            agreements.Count,
            agreements.Count(agreement => agreement == KurdishAgreement.Exact),
            agreements.Count(agreement => agreement == KurdishAgreement.Near));
    }
}
