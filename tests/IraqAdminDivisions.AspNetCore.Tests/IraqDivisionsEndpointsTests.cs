using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IraqAdminDivisions.AspNetCore.Tests;

public sealed class IraqDivisionsEndpointsTests : IAsyncLifetime
{
    private const string BasrahCode = "IQG02";
    private const string BabilCode = "IQG07";
    private const string DefaultPrefix = "/api/iraq-divisions";

    private WebApplication _app = null!;
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        _app = await StartAppAsync(app => app.MapIraqDivisions());
        _client = _app.GetTestClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _app.DisposeAsync();
    }

    private static async Task<WebApplication> StartAppAsync(Action<WebApplication> map)
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();
        var app = builder.Build();
        map(app);
        await app.StartAsync();
        return app;
    }

    private async Task<JsonElement> GetJsonAsync(string url, string? acceptLanguage = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (acceptLanguage != null)
        {
            request.Headers.TryAddWithoutValidation("Accept-Language", acceptLanguage);
        }

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    [Fact]
    public async Task Division_ReturnsIsoOchaAndKurdishSourceFields()
    {
        var baghdad = await GetJsonAsync($"{DefaultPrefix}/IQG08");
        var halabja = await GetJsonAsync($"{DefaultPrefix}/IQG19");

        Assert.Equal("IQ-BG", baghdad.GetProperty("isoCode").GetString());
        Assert.Equal("IQG08", baghdad.GetProperty("ochaCode").GetString());
        Assert.Equal(JsonValueKind.Null, baghdad.GetProperty("kurdishNameDataset").ValueKind);
        Assert.Equal(JsonValueKind.Null, halabja.GetProperty("isoCode").ValueKind);
        Assert.Equal(JsonValueKind.Null, halabja.GetProperty("ochaCode").ValueKind);
    }

    [Fact]
    public async Task DeprecatedCode_ResolvesWithItsReplacement()
    {
        var oldKhurmal = await GetJsonAsync($"{DefaultPrefix}/IQG06Q05N02");

        Assert.True(oldKhurmal.GetProperty("isDeprecated").GetBoolean());
        Assert.Equal("IQG19Q01N02", oldKhurmal.GetProperty("replacedBy").GetString());
    }

    [Fact]
    public async Task Governorates_ReturnsAll19()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/governorates");

        Assert.Equal(19, json.GetArrayLength());
        Assert.All(json.EnumerateArray(), item => Assert.Equal("Governorate", item.GetProperty("level").GetString()));
    }

    [Fact]
    public async Task GetByCode_ReturnsDivisionWithAllFields()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BasrahCode}?lang=en");

        Assert.Equal(BasrahCode, json.GetProperty("code").GetString());
        Assert.Equal(IraqDivisions.Find(BasrahCode)!.Name.En, json.GetProperty("name").GetString());
        Assert.Equal(IraqDivisions.Find(BasrahCode)!.Name.Ar, json.GetProperty("names").GetProperty("ar").GetString());
        Assert.Equal(JsonValueKind.Null, json.GetProperty("parentCode").ValueKind);
        Assert.True(json.GetProperty("hasChildren").GetBoolean());
        Assert.Equal(IraqDivisions.FormatAddress(BasrahCode, Language.English), json.GetProperty("address").GetString());
        Assert.False(json.GetProperty("isDeprecated").GetBoolean());
    }

    [Fact]
    public async Task GetByCode_SubDistrict_HasParentAndNoChildren()
    {
        var subDistrict = IraqDivisions.All.First(division => division.Level == AdminLevel.SubDistrict);

        var json = await GetJsonAsync($"{DefaultPrefix}/{subDistrict.Code}");

        Assert.Equal(subDistrict.Parent!.Code, json.GetProperty("parentCode").GetString());
        Assert.False(json.GetProperty("hasChildren").GetBoolean());
    }

    [Fact]
    public async Task GetByCode_UnknownCode_Returns404()
    {
        var response = await _client.GetAsync($"{DefaultPrefix}/NOPE");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Children_ReturnsDirectChildrenOnly()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BabilCode}/children");

        var expected = IraqDivisions.Find(BabilCode)!.Children.Select(child => child.Code).ToList();
        Assert.Equal(expected, json.EnumerateArray().Select(item => item.GetProperty("code").GetString()!).ToList());
        Assert.All(json.EnumerateArray(), item => Assert.Equal(BabilCode, item.GetProperty("parentCode").GetString()));
    }

    [Fact]
    public async Task Children_UnknownCode_Returns404()
    {
        var response = await _client.GetAsync($"{DefaultPrefix}/NOPE/children");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByCode_ReturnsAddressesKurdishSourceAndDistrictCentreFlag()
    {
        var centre = IraqDivisions.All.First(division => division.IsDistrictCentre);

        var json = await GetJsonAsync($"{DefaultPrefix}/{centre.Code}?lang=en");

        var addresses = json.GetProperty("addresses");
        Assert.Equal(centre.Address.Ar, addresses.GetProperty("ar").GetString());
        Assert.Equal(centre.Address.En, addresses.GetProperty("en").GetString());
        Assert.Equal(centre.Address.Ku, addresses.GetProperty("ku").GetString());
        Assert.Equal(centre.KurdishNameSource.ToString(), json.GetProperty("kurdishNameSource").GetString());
        Assert.True(json.GetProperty("isDistrictCentre").GetBoolean());
        Assert.Equal(centre.Name.Ku, json.GetProperty("names").GetProperty("ku").GetString());
    }

    [Fact]
    public async Task Search_ResultsCarryTheNewFields()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/search?q={Uri.EscapeDataString("البصره")}");

        Assert.Equal(IraqDivisions.Find(BasrahCode)!.Address.Ku, json[0].GetProperty("addresses").GetProperty("ku").GetString());
        Assert.False(json[0].GetProperty("isDistrictCentre").GetBoolean());
        Assert.Equal(JsonValueKind.String, json[0].GetProperty("kurdishNameSource").ValueKind);
    }

    [Fact]
    public async Task Localities_ReturnsTheGovernoratesFlatAreas()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BabilCode}/localities");

        var expected = IraqDivisions.Find(BabilCode)!.Localities.Select(locality => locality.Code).ToList();
        Assert.Equal(expected, json.EnumerateArray().Select(item => item.GetProperty("code").GetString()!).ToList());
        Assert.All(json.EnumerateArray(), item => Assert.False(item.GetProperty("isDistrictCentre").GetBoolean()));
    }

    [Fact]
    public async Task Localities_UnknownCode_Returns404()
    {
        var response = await _client.GetAsync($"{DefaultPrefix}/NOPE/localities");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Localities_NonGovernorateCode_Returns400()
    {
        var district = IraqDivisions.Find(BabilCode)!.Children[0];

        var response = await _client.GetAsync($"{DefaultPrefix}/{district.Code}/localities");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_FindsBasrahAndIncludesScore()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/search?q={Uri.EscapeDataString("البصره")}");

        Assert.Equal(BasrahCode, json[0].GetProperty("code").GetString());
        Assert.True(json[0].GetProperty("score").GetInt32() > 0);
    }

    [Fact]
    public async Task Search_FiltersByLevel()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/search?q=a&level=district&limit=50");

        Assert.NotEqual(0, json.GetArrayLength());
        Assert.All(json.EnumerateArray(), item => Assert.Equal("District", item.GetProperty("level").GetString()));
    }

    [Fact]
    public async Task Search_ClampsLimit()
    {
        var capped = await GetJsonAsync($"{DefaultPrefix}/search?q=a&limit=1000");
        var floored = await GetJsonAsync($"{DefaultPrefix}/search?q=a&limit=-5");

        Assert.True(capped.GetArrayLength() <= 50);
        Assert.Equal(1, floored.GetArrayLength());
    }

    [Theory]
    [InlineData("")]
    [InlineData("?q=")]
    [InlineData("?q=%20%20")]
    [InlineData("?q=a&level=bogus")]
    public async Task Search_InvalidInput_Returns400(string query)
    {
        var response = await _client.GetAsync($"{DefaultPrefix}/search{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Nearest_ReturnsClosestDivisionAtRequestedLevel()
    {
        var basrah = IraqDivisions.Find(BasrahCode)!;

        var json = await GetJsonAsync($"{DefaultPrefix}/nearest?lat={basrah.Latitude}&lng={basrah.Longitude}&level=Governorate");

        Assert.Equal(BasrahCode, json.GetProperty("code").GetString());
    }

    [Fact]
    public async Task Nearest_DefaultsToSubDistrict()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/nearest?lat=30.5&lng=47.8");

        Assert.Equal("SubDistrict", json.GetProperty("level").GetString());
    }

    [Theory]
    [InlineData("lat=91&lng=45")]
    [InlineData("lat=-91&lng=45")]
    [InlineData("lat=30&lng=181")]
    [InlineData("lat=30&lng=-181")]
    [InlineData("lat=30")]
    [InlineData("lng=45")]
    [InlineData("lat=30&lng=45&level=bogus")]
    public async Task Nearest_InvalidInput_Returns400(string query)
    {
        var response = await _client.GetAsync($"{DefaultPrefix}/nearest?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("ar", Language.Arabic)]
    [InlineData("en", Language.English)]
    [InlineData("ku", Language.Kurdish)]
    public async Task LangQuery_SelectsNameAndAddressLanguage(string lang, Language expected)
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BasrahCode}?lang={lang}");

        var division = IraqDivisions.Find(BasrahCode)!;
        Assert.Equal(division.Name.Get(expected), json.GetProperty("name").GetString());
        Assert.Equal(division.FormatAddress(expected), json.GetProperty("address").GetString());
    }

    [Fact]
    public async Task LangQuery_OverridesAcceptLanguage()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BasrahCode}?lang=ar", "en-US");

        Assert.Equal(IraqDivisions.Find(BasrahCode)!.Name.Ar, json.GetProperty("name").GetString());
    }

    [Theory]
    [InlineData("en-US,en;q=0.9", Language.English)]
    [InlineData("fr-FR, ku;q=0.8, en;q=0.5", Language.Kurdish)]
    [InlineData("ckb", Language.Kurdish)]
    [InlineData("fr, de;q=0.9, en;q=0.1", Language.English)]
    [InlineData("ar-IQ", Language.Arabic)]
    [InlineData("fr-FR, de", Language.Arabic)]
    [InlineData("en;q=0.2, ku;q=0.9", Language.Kurdish)]
    public async Task AcceptLanguage_PicksFirstSupportedTag(string header, Language expected)
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BasrahCode}", header);

        Assert.Equal(IraqDivisions.Find(BasrahCode)!.Name.Get(expected), json.GetProperty("name").GetString());
    }

    [Fact]
    public async Task NoLanguageHints_DefaultsToArabic()
    {
        var json = await GetJsonAsync($"{DefaultPrefix}/{BasrahCode}");

        Assert.Equal(IraqDivisions.Find(BasrahCode)!.Name.Ar, json.GetProperty("name").GetString());
    }

    [Fact]
    public async Task Prefix_IsConfigurable()
    {
        await using var app = await StartAppAsync(host => host.MapIraqDivisions("/geo"));
        using var client = app.GetTestClient();

        var custom = await client.GetAsync("/geo/governorates");
        var original = await client.GetAsync($"{DefaultPrefix}/governorates");

        Assert.Equal(HttpStatusCode.OK, custom.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, original.StatusCode);
    }

    [Fact]
    public async Task ReturnedGroup_AcceptsChainedConventions()
    {
        await using var app = await StartAppAsync(host => host.MapIraqDivisions("/secure").AddEndpointFilter((context, next) =>
            ValueTask.FromResult<object?>(Results.StatusCode(418))));
        using var client = app.GetTestClient();

        var response = await client.GetAsync("/secure/governorates");

        Assert.Equal((HttpStatusCode)418, response.StatusCode);
    }
}
