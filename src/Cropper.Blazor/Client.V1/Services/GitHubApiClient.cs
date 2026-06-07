using System.Net.Http.Json;
using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Services
{
    public class GitHubApiClient : IDisposable
    {
        private readonly HttpClient _http;

        public GitHubApiClient()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com:443/")
            };
            _http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.106 Mobile Safari/537.36");
        }

        public async Task<GitHubReleases[]> GetReleasesAsync(CancellationToken cancellationToken)
        {
            GitHubReleases[]? result = await _http.GetFromJsonAsync<GitHubReleases[]>("repos/CropperBlazor/Cropper.Blazor/releases?per_page=100", cancellationToken);

            return result ?? Array.Empty<GitHubReleases>();
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
