namespace OneProject.Desktop.Infrastructures;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class VersionChecker
{
    // api docs: https://docs.github.com/en/rest/releases/releases?apiVersion=2022-11-28#get-the-latest-release
    private const string uri = "https://api.github.com/repos/maple512/oneproject/releases/latest";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public VersionChecker(IHttpClientFactory httpClientFactory, ILogger<VersionChecker> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<GithubReleaseModel?> GetLatestVersionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));

            var response = await client.GetStreamAsync(uri, cancellationToken);

            var model = await JsonSerializer.DeserializeAsync(response, GithubReleaseModelContext.Default.GithubReleaseModel, cancellationToken);

            return model;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, null);

            return null;
        }
    }
}

public class GithubReleaseModel
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("prerelease")]
    public bool PreRelease { get; set; }

    [JsonPropertyName("assets")]
    public List<GithubReleaseAssetModel> Assets { get; set; } = [];

    public class GithubReleaseAssetModel
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string? DownloadUrl { get; set; }
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(GithubReleaseModel))]
internal partial class GithubReleaseModelContext : JsonSerializerContext
{
}
