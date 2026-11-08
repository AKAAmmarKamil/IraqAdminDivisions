using IraqAdminDivisions.Builder.Matching;
using IraqAdminDivisions.Builder.Models;
using IraqAdminDivisions.Builder.Overrides;

namespace IraqAdminDivisions.Builder.Merge;

public sealed record RejectedKurdishName(string Code, string Name, string Reason);

public sealed record MergeResult(
    IReadOnlyList<Division> Divisions,
    IReadOnlyList<RejectedKurdishName> RejectedKurdishNames,
    int StrippedKurdishNameCount,
    IReadOnlyList<AppliedCorrection> AppliedCorrections,
    IReadOnlyList<string> UnusedTransliterationCodes);

public sealed class DivisionMerger
{
    private const int CoordinateDecimals = 6;
    private const int AreaDecimals = 2;
    private const string EnglishCentrePrefix = "Markaz ";
    private const string ArabicCentrePrefix = "مركز ";

    public MergeResult Merge(
        IReadOnlyList<OchaRow> rows,
        IReadOnlyList<MatchOutcome> outcomes,
        OverridesData overrides,
        IReadOnlyDictionary<string, string> isoCodesByWikidataId)
    {
        var matchedItems = outcomes
            .Where(outcome => outcome.Status == MatchStatus.Matched)
            .ToDictionary(outcome => outcome.Row.Code, outcome => outcome.Item!, StringComparer.Ordinal);
        var rejected = new List<RejectedKurdishName>();
        var applied = new List<AppliedCorrection>();

        var divisions = rows
            .OrderBy(row => row.Code, StringComparer.Ordinal)
            .Select(row => BuildDivision(row, matchedItems.GetValueOrDefault(row.Code), overrides, isoCodesByWikidataId, rejected, applied))
            .ToList();

        var strippedCount = divisions.Count(division =>
            division.KurdishNameSource == KurdishNameSources.Wikidata && division.Name.Ku != matchedItems[division.Code].LabelCkb);

        var unusedTransliterations = overrides.Transliterations
            .Select(entry => entry.Code)
            .Where(code => divisions.Any(division => division.Code == code && division.KurdishNameSource != KurdishNameSources.Transliterated))
            .OrderBy(code => code, StringComparer.Ordinal)
            .ToList();

        return new MergeResult(divisions, rejected, strippedCount, applied, unusedTransliterations);
    }

    private static Division BuildDivision(
        OchaRow row,
        WikidataItem? item,
        OverridesData overrides,
        IReadOnlyDictionary<string, string> isoCodesByWikidataId,
        List<RejectedKurdishName> rejected,
        List<AppliedCorrection> applied)
    {
        var nameAr = ApplyCorrection(row.Code, OverrideFields.Arabic, row.NameAr, overrides, applied);
        var nameEn = ApplyCorrection(row.Code, OverrideFields.English, row.NameEn, overrides, applied);
        var (kurdishName, kurdishSource) = SelectKurdishName(row, item, overrides, rejected, applied);

        return new Division(
            row.Code,
            row.Level,
            row.ParentCode,
            new DivisionName(nameAr, nameEn, kurdishName),
            BuildAliases(row, item, nameAr, nameEn),
            new DivisionCenter(Math.Round(row.Lat!.Value, CoordinateDecimals), Math.Round(row.Lng!.Value, CoordinateDecimals)),
            Math.Round(row.AreaSqKm!.Value, AreaDecimals),
            item?.Id,
            SelectIsoCode(row.Level, item, isoCodesByWikidataId),
            kurdishSource,
            KurdishNameDataset: null,
            IsDistrictCentre(row.Level, nameAr, nameEn),
            IsDeprecated: false,
            ReplacedBy: null);
    }

    private static string? SelectIsoCode(AdminLevel level, WikidataItem? item, IReadOnlyDictionary<string, string> isoCodesByWikidataId) =>
        level == AdminLevel.Governorate && item is not null ? isoCodesByWikidataId.GetValueOrDefault(item.Id) : null;

    private static bool IsDistrictCentre(AdminLevel level, string nameAr, string nameEn) =>
        level == AdminLevel.SubDistrict
        && (nameEn.StartsWith(EnglishCentrePrefix, StringComparison.Ordinal) || nameAr.StartsWith(ArabicCentrePrefix, StringComparison.Ordinal));

    private static string ApplyCorrection(string code, string field, string sourceValue, OverridesData overrides, List<AppliedCorrection> applied)
    {
        if (!overrides.CorrectionsByKey.TryGetValue((code, field), out var correction))
            return sourceValue;

        applied.Add(new AppliedCorrection(code, field, sourceValue, correction.Value, correction.Reason));
        return correction.Value;
    }

    private static (string? Name, string? Source) SelectKurdishName(
        OchaRow row,
        WikidataItem? item,
        OverridesData overrides,
        List<RejectedKurdishName> rejected,
        List<AppliedCorrection> applied)
    {
        var wikidataName = item is null ? null : SelectWikidataKurdishName(row, item, rejected);

        if (overrides.CorrectionsByKey.TryGetValue((row.Code, OverrideFields.Kurdish), out var correction))
        {
            applied.Add(new AppliedCorrection(row.Code, OverrideFields.Kurdish, wikidataName, correction.Value, correction.Reason));
            return (correction.Value, KurdishNameSources.Manual);
        }

        if (wikidataName is not null)
            return (wikidataName, KurdishNameSources.Wikidata);

        return overrides.TransliterationsByCode.TryGetValue(row.Code, out var transliteration) && transliteration.Ku.Length > 0
            ? (transliteration.Ku, KurdishNameSources.Transliterated)
            : (null, null);
    }

    private static string? SelectWikidataKurdishName(OchaRow row, WikidataItem item, List<RejectedKurdishName> rejected)
    {
        if (string.IsNullOrWhiteSpace(item.LabelCkb))
            return null;

        var candidate = KurdishNamePrefix.Strip(item.LabelCkb);

        if (!NameNormalizer.HasArabicLetter(candidate))
        {
            rejected.Add(new RejectedKurdishName(row.Code, candidate, "no Arabic-script letters"));
            return null;
        }

        var foreignCharacters = SoraniAlphabet.ForeignCharacters(candidate);
        if (foreignCharacters.Count > 0)
        {
            rejected.Add(new RejectedKurdishName(row.Code, candidate, $"contains non-Sorani characters ({string.Join(", ", foreignCharacters)})"));
            return null;
        }

        return candidate;
    }

    private static IReadOnlyList<string> BuildAliases(OchaRow row, WikidataItem? item, string nameAr, string nameEn)
    {
        var aliases = new List<string>();
        if (row.RefName is not null && !string.Equals(row.RefName, row.NameEn, StringComparison.Ordinal))
            aliases.Add(row.RefName);

        if (item is not null)
        {
            aliases.AddRange(item.AliasesAr);
            aliases.AddRange(item.AliasesEn);
        }

        aliases.Add(row.NameAr);
        aliases.Add(row.NameEn);

        return aliases
            .Select(alias => alias.Trim(' ', '|'))
            .Where(alias => alias.Length > 0)
            .Where(alias => alias != nameAr && alias != nameEn)
            .Distinct(StringComparer.Ordinal)
            .ToList();
    }
}
