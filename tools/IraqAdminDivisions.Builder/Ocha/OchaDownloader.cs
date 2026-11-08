namespace IraqAdminDivisions.Builder.Ocha;

public sealed class OchaDownloader(HttpClient httpClient)
{
    public async Task<string> EnsureDownloadedAsync(string cachePath, bool shouldRefresh, string url, string description)
    {
        if (File.Exists(cachePath) && !shouldRefresh)
        {
            Console.WriteLine($"OCHA: using cached {description}");
            return cachePath;
        }

        Console.WriteLine($"OCHA: downloading {description}");
        Directory.CreateDirectory(Path.GetDirectoryName(cachePath)!);
        var bytes = await httpClient.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(cachePath, bytes);
        return cachePath;
    }
}
