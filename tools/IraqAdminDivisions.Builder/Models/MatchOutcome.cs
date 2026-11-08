namespace IraqAdminDivisions.Builder.Models;

public enum MatchStatus
{
    Matched,
    Unmatched,
    Ambiguous
}

public sealed record MatchOutcome(OchaRow Row, MatchStatus Status, WikidataItem? Item, string Detail, IReadOnlyList<string>? RejectedNameOnlyItemIds = null);
