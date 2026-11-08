using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Datasets;

public sealed class KurdishDatasetEvaluator
{
    private readonly DatasetMatcher _matcher;
    private readonly DatasetCalibrator _calibrator = new();
    private readonly Dictionary<string, Division> _divisionsByCode;

    public KurdishDatasetEvaluator(IReadOnlyList<Division> divisions)
    {
        _matcher = new DatasetMatcher(divisions);
        _divisionsByCode = divisions.ToDictionary(division => division.Code, StringComparer.Ordinal);
    }

    public IReadOnlyList<DatasetEvaluation> Evaluate(string sourcesDirectory) =>
        DatasetCatalog.All.Select(dataset => Evaluate(dataset, sourcesDirectory)).ToList();

    private DatasetEvaluation Evaluate(KurdishDataset dataset, string sourcesDirectory)
    {
        var rows = dataset.Reader.Read(Path.Combine(sourcesDirectory, dataset.DirectoryName));
        var matching = _matcher.Match(rows);
        var comparisons = matching.Matches.Select(match => Compare(dataset, match)).ToList();
        return new DatasetEvaluation(dataset, rows, matching, comparisons, _calibrator.Calibrate(dataset, rows, comparisons));
    }

    private DatasetNameComparison Compare(KurdishDataset dataset, DatasetMatch match)
    {
        var ours = _divisionsByCode[match.Code];
        var theirName = DatasetKurdishName.Clean(match.Row.Ku);
        return new DatasetNameComparison(
            dataset,
            match,
            ours,
            theirName,
            theirName is not null && SoraniAlphabet.IsSorani(theirName),
            theirName is null || ours.Name.Ku is null ? null : SoraniNormalizer.Compare(ours.Name.Ku, theirName));
    }
}
