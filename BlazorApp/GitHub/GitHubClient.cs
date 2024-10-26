using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorApp.GitHub;

internal class GitHubClientOptions
{
    public string UserAgent { get; set; } = default!;
    public string RepositoryOwner { get; set; } = default!;
    public string RepositoryName { get; set; } = default!;
    public string Branch { get; set; } = default!;
	public string PersonalAccessToken { get; set; } = default!;
}

internal class GitHubClient
{
    private readonly HttpClient _httpClient;
    private readonly GitHubClientOptions _options;

    public GitHubClient(IHttpClientFactory httpClientFactory, IOptions<GitHubClientOptions> options)
    {
        _options = options.Value;

        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://api.github.com/");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", _options.UserAgent);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.PersonalAccessToken);
    }

    public async Task<string> GetLatestCommitIdAsync()
    {
        var response = await _httpClient.GetStringAsync($"repos/{_options.RepositoryOwner}/{_options.RepositoryName}/commits/{_options.Branch}?per_page=1");
        var json = JsonDocument.Parse(response);
		return json.RootElement.GetProperty("sha").GetString() ?? throw new InvalidOperationException("Could not get commit id.");
	}

    public async Task<int> GetCommitsBehindAsync(string commitId)
	{
		var response = await _httpClient.GetStringAsync($"repos/{_options.RepositoryOwner}/{_options.RepositoryName}/compare/{commitId}...{_options.Branch}");
		var json = JsonDocument.Parse(response);
		return json.RootElement.GetProperty("behind_by").GetInt32();
	}
}
