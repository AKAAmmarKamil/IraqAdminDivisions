namespace IraqAdminDivisions;

/// <summary>A division returned by <see cref="IraqDivisions.Search"/> with its relevance score.</summary>
public sealed class SearchResult
{
    internal SearchResult(Division division, int score)
    {
        Division = division;
        Score = score;
    }

    /// <summary>The matching division.</summary>
    public Division Division { get; }

    /// <summary>The relevance score. Higher is better: exact, then prefix, then contains, then a single typo.</summary>
    public int Score { get; }

    /// <inheritdoc />
    public override string ToString() => $"{Division} [{Score}]";
}
