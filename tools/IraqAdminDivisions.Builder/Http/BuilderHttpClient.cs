using IraqAdminDivisions.Builder.Models;

namespace IraqAdminDivisions.Builder.Http;

public static class BuilderHttpClient
{
    public static HttpClient Create()
    {
        var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true })
        {
            Timeout = TimeSpan.FromMinutes(3)
        };
        client.DefaultRequestHeaders.UserAgent.ParseAdd(BuildSettings.UserAgent);
        return client;
    }
}
