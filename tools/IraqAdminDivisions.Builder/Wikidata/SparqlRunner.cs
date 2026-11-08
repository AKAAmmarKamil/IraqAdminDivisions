namespace IraqAdminDivisions.Builder.Wikidata;

public sealed class SparqlRunner(HttpClient httpClient)
{
    private const int MaxAttempts = 6;
    private static readonly TimeSpan DefaultRetryDelay = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan PoliteDelayBetweenQueries = TimeSpan.FromSeconds(5);

    public async Task<string> RunCachedAsync(string query, string cachePath, bool shouldRefresh)
    {
        if (File.Exists(cachePath) && !shouldRefresh)
        {
            Console.WriteLine($"Wikidata: using cached {Path.GetFileName(cachePath)}");
            return await File.ReadAllTextAsync(cachePath);
        }

        var json = await RunAsync(query);
        Directory.CreateDirectory(Path.GetDirectoryName(cachePath)!);
        await File.WriteAllTextAsync(cachePath, json);
        await Task.Delay(PoliteDelayBetweenQueries);
        return json;
    }

    private async Task<string> RunAsync(string query)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            Console.WriteLine($"Wikidata: SPARQL attempt {attempt}");
            using var request = new HttpRequestMessage(HttpMethod.Post, WikidataQueries.SparqlEndpoint)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["query"] = query })
            };
            request.Headers.Accept.ParseAdd("application/sparql-results+json");

            using var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            if (!IsRetryable(response.StatusCode) || attempt == MaxAttempts)
                throw new HttpRequestException($"Wikidata SPARQL failed with {(int)response.StatusCode} {response.StatusCode}.");

            var delay = response.Headers.RetryAfter?.Delta ?? DefaultRetryDelay * attempt;
            Console.WriteLine($"Wikidata: {(int)response.StatusCode}, waiting {delay.TotalSeconds:0}s");
            await Task.Delay(delay);
        }

        throw new InvalidOperationException("Unreachable.");
    }

    private static bool IsRetryable(System.Net.HttpStatusCode statusCode) =>
        statusCode is System.Net.HttpStatusCode.TooManyRequests
            or System.Net.HttpStatusCode.ServiceUnavailable
            or System.Net.HttpStatusCode.GatewayTimeout
            or System.Net.HttpStatusCode.BadGateway;
}
