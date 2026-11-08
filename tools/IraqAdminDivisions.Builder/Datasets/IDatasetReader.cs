namespace IraqAdminDivisions.Builder.Datasets;

public interface IDatasetReader
{
    IReadOnlyList<DatasetRow> Read(string directory);
}
