using System.Text;

namespace IraqAdminDivisions.Builder.Output;

public static class OutputFile
{
    public static async Task WriteAsync(string path, string content, bool shouldWriteBom)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await File.WriteAllTextAsync(path, content, new UTF8Encoding(shouldWriteBom));
    }
}
