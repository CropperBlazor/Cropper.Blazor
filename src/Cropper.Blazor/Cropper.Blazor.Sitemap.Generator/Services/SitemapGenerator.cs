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
                IEnumerable<SitemapUrlAttribute> sitemapAttributes = componentType.GetCustomAttributes<SitemapUrlAttribute>();

                if (sitemapAttributes.Any())
                {
                    foreach (SitemapUrlAttribute sitemapAttribute in sitemapAttributes)
                    {
                        if (!string.IsNullOrWhiteSpace(sitemapAttribute?.Url))
                        {
                            // Create a sitemap entry for the component using the sitemap URL routes
                            AddSitemapEntry(
                                sitemapEntries,
                                baseUrl,
                                sitemapAttribute!.Url,
                                sitemapAttribute?.ChangeFreq,
                                sitemapAttribute?.Priority);
                        }
                        else
                        {
                            AddRouteSitemapEntry(
                                sitemapEntries,
                                componentType,
                                baseUrl,
                                sitemapAttribute?.ChangeFreq,
                                sitemapAttribute?.Priority);
                        }
                    }
                }
                else
                {
                    AddRouteSitemapEntry(
                        sitemapEntries,
                        componentType,
                        baseUrl,
                        null,
                        null);
                }
            }

            return sitemapEntries;
        }

        private void AddRouteSitemapEntry(
            List<SitemapEntry> sitemapEntries,
            Type componentType,
            string baseUrl,
            ChangeFreq? changeFreq,
            double? priority)
        {
            IEnumerable<RouteAttribute> routeAttributes = componentType.GetCustomAttributes<RouteAttribute>();

            foreach (RouteAttribute routeAttribute in routeAttributes)
            {
                string url = routeAttribute!.Template;
                if (!string.IsNullOrWhiteSpace(url))
                {
                    // Create a sitemap entry for the component using the current route
                    AddSitemapEntry(
                        sitemapEntries,
                        baseUrl,
                        url,
                        changeFreq,
                        priority);
                }
            }
        }

        private void AddSitemapEntry(
            List<SitemapEntry> sitemapEntries,
            string baseUrl,
            string url,
            ChangeFreq? changeFreq,
            double? priority)
        {
            SitemapEntry entry = new SitemapEntry
            {
                Url = $"{baseUrl}{url}",
                LastModified = DateTime.UtcNow,
                ChangeFrequency = changeFreq ?? ChangeFreq.Daily,
                Priority = priority ?? 0.5
            };

            sitemapEntries.Add(entry);
        }
    }
}
