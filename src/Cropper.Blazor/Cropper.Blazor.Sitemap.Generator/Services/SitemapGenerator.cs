using System.Reflection;
using Cropper.Blazor.Client.Pages;
using Cropper.Blazor.Shared.Attributes;
using Cropper.Blazor.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Sitemap.Generator.Services
{
    internal class SitemapGenerator
    {
        public List<SitemapEntry> GenerateSitemapEntries(string baseUrl = "https://cropperblazor.github.io")
        {
            List<SitemapEntry> sitemapEntries = new List<SitemapEntry>();

            IEnumerable<Type> componentTypes = typeof(CropperDemo).Assembly!
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ComponentBase)) && t.Namespace!.StartsWith("Cropper.Blazor.Client"));

            foreach (Type componentType in componentTypes)
            {
                SitemapUrlAttribute? sitemapAttribute = componentType.GetCustomAttribute<SitemapUrlAttribute>();
                RouteAttribute routeAttribute = componentType.GetCustomAttributes<RouteAttribute>().FirstOrDefault();

                string url = routeAttribute?.Template;
                IEnumerable<string> sitemapUrls = sitemapAttribute?.Urls?.Where(url => !string.IsNullOrWhiteSpace(url)) ?? Enumerable.Empty<string>();
                if (sitemapUrls.Any())
                {
                    // Create a sitemap entry for the component using the sitemap URL routes
                    foreach (string sitemapUrl in sitemapUrls)
                    {
                        SitemapEntry entry = new SitemapEntry
                        {
                            Url = $"{baseUrl}{sitemapUrl}",
                            LastModified = DateTime.UtcNow,
                            ChangeFrequency = sitemapAttribute?.ChangeFreq ?? ChangeFreq.Daily,
                            Priority = sitemapAttribute?.Priority ?? 0.5
                        };

                        sitemapEntries.Add(entry);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        // Create a sitemap entry for the component using the current route
                        SitemapEntry entry = new SitemapEntry
                        {
                            Url = $"{baseUrl}{url}",
                            LastModified = DateTime.UtcNow,
                            ChangeFrequency = sitemapAttribute?.ChangeFreq ?? ChangeFreq.Daily,
                            Priority = sitemapAttribute?.Priority ?? 0.5
                        };

                        sitemapEntries.Add(entry);
                    }
                }
            }

            return sitemapEntries;
        }
    }
}
