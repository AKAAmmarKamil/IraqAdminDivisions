using System.Globalization;
using System.Text;
using IraqAdminDivisions.Builder.Datasets;

namespace IraqAdminDivisions.Builder.Output;

public sealed class KurdishCrosscheckWriter
{
    public Task WriteAsync(string path, KurdishCrosscheck crosscheck, DateTime generatedAtUtc)
    {
        var builder = new StringBuilder();
        builder.Append("# Kurdish name cross-check\n\n");
        builder.Append($"Generated: {generatedAtUtc:yyyy-MM-dd HH:mm:ss}Z\n\n");
        AppendPolicy(builder);
        AppendSummary(builder, crosscheck);
        foreach (var evaluation in crosscheck.Evaluations)
            AppendDataset(builder, evaluation, crosscheck.Disagreements.Where(disagreement => disagreement.Repository == evaluation.Dataset.Repository).ToList());

        return OutputFile.WriteAsync(path, builder.ToString(), shouldWriteBom: false);
    }

    private static void AppendPolicy(StringBuilder builder)
    {
        builder.Append("## Policy\n\n");
        builder.Append($"- A dataset is trusted only if its exact agreement with our Wikidata Kurdish names beyond governorate level is at least {Percent(DatasetTrustPolicy.MinExactAgreementBeyondGovernorates)} over at least {DatasetTrustPolicy.MinComparedBeyondGovernorates} comparable rows, and at least {Percent(DatasetTrustPolicy.MinPureSoraniShare)} of its Kurdish names use only Sorani letters. A dataset whose own documentation calls its Kurdish names unreviewed drafts is never trusted.\n");
        builder.Append("- Exact means equal after Sorani normalisation (ي/ى/ی, ك/ک, ه/ھ/ە unified, tashkeel and joiners removed, administrative words such as قەزای removed). Near means the same consonant skeleton or one character apart.\n");
        builder.Append("- For a transliterated row a trusted dataset name that is exact keeps our text and the source becomes dataset; any other trusted name replaces ours, and our transliteration stays as an alias.\n");
        builder.Append("- Wikidata and manual names are never overridden. Every non-exact pair is listed below, with what happened to it.\n\n");
    }

    private static void AppendSummary(StringBuilder builder, KurdishCrosscheck crosscheck)
    {
        builder.Append("## Summary\n\n| Dataset | Licence | Trusted | Exact vs Wikidata | Confirmed | Replaced | Disagreements listed |\n|---|---|---|---|---|---|---|\n");
        foreach (var evaluation in crosscheck.Evaluations)
        {
            var use = crosscheck.Uses.Single(candidate => candidate.Dataset == evaluation.Dataset);
            var calibration = evaluation.Calibration;
            builder.Append($"| {evaluation.Dataset.Repository} | {evaluation.Dataset.License} | {(calibration.IsTrusted ? "yes" : "no")} | {calibration.Exact} of {calibration.Compared} ({Percent(calibration.ExactRate)}) | {use.Confirmed} | {use.Replaced} | {crosscheck.Disagreements.Count(disagreement => disagreement.Repository == evaluation.Dataset.Repository)} |\n");
        }

        builder.Append("\nAgreement between the datasets themselves (same code, both have a Kurdish name):\n\n| Dataset | Dataset | Shared rows | Exact | Near |\n|---|---|---|---|---|\n");
        foreach (var pair in crosscheck.Pairs)
            builder.Append($"| {pair.First.Repository} | {pair.Second.Repository} | {pair.Shared} | {pair.Exact} | {pair.Near} |\n");

        builder.Append($"\nTransliterated rows confirmed by a dataset: {crosscheck.ConfirmedCount}. Replaced by a dataset name: {crosscheck.ReplacedCount}. Still transliterated: {crosscheck.TransliteratedWithoutDatasetNameCount}.\n\n");
    }

    private static void AppendDataset(StringBuilder builder, DatasetEvaluation evaluation, IReadOnlyList<KurdishDisagreement> disagreements)
    {
        var dataset = evaluation.Dataset;
        builder.Append($"## {dataset.Repository}\n\n");
        builder.Append($"{dataset.Url} at commit `{dataset.CommitSha}`, licence {dataset.License}. Verdict: {evaluation.Calibration.Verdict}.\n\n");
        builder.Append($"Rows read: {evaluation.Rows.Count}. Matched to our codes: {evaluation.Matching.Matches.Count}. Not matched: {evaluation.Matching.Unmatched.Count}.\n\n");
        AppendMethods(builder, evaluation);

        builder.Append($"### Disagreements ({disagreements.Count})\n\n");
        if (disagreements.Count == 0)
        {
            builder.Append("None.\n\n");
            return;
        }

        builder.Append("| Code | Level | English | Ours | Our source | Theirs | Agreement | Match | Outcome |\n|---|---|---|---|---|---|---|---|---|\n");
        foreach (var disagreement in disagreements)
            builder.Append($"| {disagreement.Code} | {disagreement.Level} | {Cell(disagreement.English)} | {Cell(disagreement.OurName)} | {disagreement.OurSource} | {Cell(disagreement.TheirName)} | {disagreement.Agreement.ToString().ToLowerInvariant()} | {disagreement.Method} | {disagreement.Outcome} |\n");
        builder.Append('\n');
    }

    private static void AppendMethods(StringBuilder builder, DatasetEvaluation evaluation)
    {
        builder.Append("| Match method | Rows |\n|---|---|\n");
        foreach (var group in evaluation.Matching.Matches.GroupBy(match => match.Method).OrderBy(group => group.Key, StringComparer.Ordinal))
            builder.Append($"| {group.Key} | {group.Count()} |\n");
        builder.Append('\n');
    }

    private static string Percent(double share) =>
        (share * 100).ToString("0.0", CultureInfo.InvariantCulture) + "%";

    private static string Cell(string text) => text.Replace("|", "\\|");
}
