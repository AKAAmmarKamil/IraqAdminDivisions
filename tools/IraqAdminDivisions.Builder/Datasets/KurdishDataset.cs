namespace IraqAdminDivisions.Builder.Datasets;

public sealed record KurdishDataset(
    string Repository,
    string License,
    string CommitSha,
    string DirectoryName,
    IDatasetReader Reader,
    bool DeclaresKurdishUnreviewed = false)
{
    public string Url => $"https://github.com/{Repository}";
}
