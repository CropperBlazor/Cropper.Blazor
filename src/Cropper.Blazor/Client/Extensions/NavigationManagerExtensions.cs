using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Extensions
{
    internal static class NavigationManagerExtensions
    {
        /// <summary>
        /// Gets the section part of the documentation page
        /// Ex: /components/button;  "components" is the section
        /// </summary>
        public static string GetSection(this NavigationManager navMan)
        {
            // get the absolute path with out the base path
            string currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            string[] routeParts = currentUri
                .Split("/", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string? firstElement = routeParts.FirstOrDefault();

            if (firstElement is "v1" or "v2")
            {
                return routeParts.ElementAtOrDefault(1) ?? "demo";
            }

            return firstElement;
        }

        /// <summary>
        /// Gets the link of the component on the documentation page
        /// Ex: api/button; "button" is the component link, and "api" is the section
        /// </summary>
        public static string GetComponentLink(this NavigationManager navMan)
        {
            // get the absolute path with out the base path
            string currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            string[] routeParts = currentUri
                .Split("/", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string? secondElement = routeParts.FirstOrDefault() is "v1" or "v2"
                ? routeParts.ElementAtOrDefault(2)
                : routeParts.ElementAtOrDefault(1);
            return secondElement;
        }

        /// <summary>
        /// Determines if the current page is the base page
        /// </summary>
        public static bool IsHomePage(this NavigationManager navMan)
        {
            return navMan.Uri == navMan.BaseUri;
        }
    }
}
