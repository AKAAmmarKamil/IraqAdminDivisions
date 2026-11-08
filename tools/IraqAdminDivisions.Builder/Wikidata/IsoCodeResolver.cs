namespace IraqAdminDivisions.Builder.Wikidata;

public sealed record IsoCodeResult(
    IReadOnlyDictionary<string, string> CodesByItemId,
    IReadOnlyList<string> Notes,
    IReadOnlyList<string> Issues);

public static class IsoCodeResolver
{
    public static IsoCodeResult Resolve(IEnumerable<string> itemIds, IReadOnlyList<IsoStatement> statements)
    {
        var codes = new Dictionary<string, string>(StringComparer.Ordinal);
        var notes = new List<string>();
        var issues = new List<string>();
        var statementsByItem = statements.ToLookup(statement => statement.ItemId, StringComparer.Ordinal);

        foreach (var itemId in itemIds.OrderBy(id => id, StringComparer.Ordinal))
        {
            var itemStatements = statementsByItem[itemId].ToList();
            var current = itemStatements.Where(statement => !statement.HasEndTime).ToList();
            if (current.Count > 1 && current.Any(statement => statement.IsPreferred))
                current = current.Where(statement => statement.IsPreferred).ToList();

            if (current.Count != 1)
            {
                issues.Add($"{itemId}: expected one current ISO 3166-2 code (P300), found {current.Count} ({Describe(itemStatements)}); run with --refresh if Wikidata changed");
                continue;
            }

            codes[itemId] = current[0].Code;
            if (itemStatements.Count > 1)
                notes.Add($"{itemId}: {itemStatements.Count} P300 values ({Describe(itemStatements)}); chose {current[0].Code}, the current one");
        }

        return new IsoCodeResult(codes, notes, issues);
    }

    private static string Describe(IEnumerable<IsoStatement> statements) =>
        statements.Any()
            ? string.Join(", ", statements.Select(statement => statement.HasEndTime ? $"{statement.Code} (ended)" : statement.Code))
            : "none";
}
