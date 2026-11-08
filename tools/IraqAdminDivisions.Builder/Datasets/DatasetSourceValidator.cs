namespace IraqAdminDivisions.Builder.Datasets;

public sealed class DatasetSourceValidator
{
    public IReadOnlyList<string> Validate(string sourcesDirectory)
    {
        var issues = new List<string>();
        foreach (var dataset in DatasetCatalog.All)
        {
            var directory = Path.Combine(sourcesDirectory, dataset.DirectoryName);
            if (!File.Exists(Path.Combine(directory, DatasetCatalog.LicenseFileName)))
                issues.Add($"{dataset.Repository}: {DatasetCatalog.LicenseFileName} is missing from {dataset.DirectoryName}");
        }

        return issues;
    }
}
