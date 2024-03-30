﻿using System.Text.RegularExpressions;
using Cropper.Blazor.Client.Models;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cropper.Blazor.Client.Pages
{
    public partial class Releases : IDisposable
    {
        [Inject]
        public GitHubApiClient GitHubApiClient { get; set; } = null!;

        [Inject]
        public ISnackbar Snackbar { get; set; } = null!;

        private GitHubReleases[] GitHubReleases = [];
        private CancellationToken CancellationToken;
        private bool? HasGitHubReleasesRequestError = null;

        protected override async Task OnInitializedAsync()
        {
            await GetReleasesAsync();
        }

        private async Task GetReleasesAsync()
        {
            try
            {
                CancellationToken = new();
                GitHubReleases = await GitHubApiClient.GetReleasesAsync(CancellationToken);
                HasGitHubReleasesRequestError = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                HasGitHubReleasesRequestError = true;
                Snackbar.Add(ex.Message, Severity.Error);
            }
        }

        public void Dispose()
        {
            CancellationToken.ThrowIfCancellationRequested();
        }

        private string GetBody(string value)
        {
            value = Regex.Replace(value, @"((###\s)(?<subtitle>.+))(?<items>.*(?:\r?\n[*] .*)*)", "<h6 class=\"mud-typography mud-typography-h6\">${subtitle}</h6><ul class=\"mt-3 mb-6 px-6\">${items}</ul>");
            value = Regex.Replace(value, @"((##\s)(?<title>.+))(?<items>.*(?:\r?\n[*] .*)*)", "<h5 class=\"mud-typography mud-typography-h5\">${title}</h5><ul class=\"mt-3 mb-6 px-6\">${items}</ul>");
            value = Regex.Replace(value, @"-\s*(.*)", "<li>$1</li>", RegexOptions.Multiline);
            value = Regex.Replace(value, @"(@)(\S+),?", (m) => $"<a target=\"_blank\" href=\"https://github.com/{m.Groups[2].Value.TrimEnd(',')}\" class=\"mud-link mud-default-text mud-link-underline-hover\"><b>@{m.Groups[2].Value}</b></a>");
            value = Regex.Replace(value, @"([*][*]Full Changelog[*][*]: )(https://github.com/CropperBlazor/Cropper.Blazor/compare/)(.+)", "<p class=\"mud-typography mud-typography-body1\">Full Changelog: <a rel=\"noopener\" style=\"color: var(--mud-palette-primary); target=\"_blank\" href=\"$2$3\" class=\"docs-code docs-code-primary\">$3</a></p>");
            value = Regex.Replace(value, @"\[([^)]+)\]\(([^)]+)\)", $"<a class=\"text-with-dots\" target=\"_blank\" rel=\"noopener\" style=\"color: var(--mud-palette-primary);\" href=\"$2\">$1</a>");
            value = Regex.Replace(value, @"\(([^)]+)\)", ReplaceLinkUrl, RegexOptions.IgnoreCase);

            return value;
        }

        private string ReplaceLinkUrl(Match match)
        {
            // Get the URL from the match
            string url = match.Groups[1].Value;

            // Check if the URL is valid
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? result))
            {
                string newUrl = url;
                newUrl = Regex.Replace(newUrl, @"https:\/\/github\.com\/([^\/]+)\/([^\/]+)\/pull\/(\d+)", $"<a target=\"_blank\" rel=\"noopener\" style=\"color: var(--mud-palette-primary);\" href=\"{url}\">PR#$3</a>");
                newUrl = Regex.Replace(newUrl, @"https:\/\/github\.com\/([^\/]+)\/([^\/]+)\/issues\/(\d+)", $"<a target=\"_blank\" rel=\"noopener\" style=\"color: var(--mud-palette-primary);\" href=\"{url}\">Issue#$3</a>");

                if (newUrl == url)
                {
                    return $"(<a target=\"_blank\" class=\"text-with-dots\" rel=\"noopener\" style=\"color: var(--mud-palette-primary);\" href=\"{result}\">{result}</a>)";
                }

                return newUrl;
            }
            else
            {
                // If invalid, return the original string
                return match.Value;
            }
        }
    }
}
