namespace IraqAdminDivisions.Builder.Datasets;

public static class DatasetMatchMethods
{
    public const string PCode = "pcode";
    public const string Name = "name";
    public const string NameInGovernorate = "name+governorate";
    public const string NameInGovernorateAndDistance = "name+governorate+distance";
    public const string NameAndDistance = "name+distance";
}

public sealed record DatasetMatch(DatasetRow Row, string Code, string Method);

public sealed record UnmatchedDatasetRow(DatasetRow Row, string Reason);

public sealed record DatasetMatchResult(
    IReadOnlyList<DatasetMatch> Matches,
    IReadOnlyList<UnmatchedDatasetRow> Unmatched);
