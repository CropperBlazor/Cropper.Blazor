using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Services;

public enum DocsVersion
{
    V1,
    V2
}

public sealed class DocsVersionService
{
    public DocsVersion GetVersion(string uri)
    {
        Uri webUri = new(uri);
        return webUri.AbsolutePath.StartsWith("/v2", StringComparison.OrdinalIgnoreCase)
            ? DocsVersion.V2
            : DocsVersion.V1;
    }

    public string GetRoutePrefix(string uri)
    {
        return GetVersion(uri) == DocsVersion.V2 ? "/v2" : "/v1";
    }

    public string GetHomeRoute(string uri)
    {
        return GetVersion(uri) == DocsVersion.V2 ? "/v2/home" : "/v1";
    }

    public string GetDemoRoute(string uri)
    {
        return GetVersion(uri) == DocsVersion.V2 ? "/v2" : "/v1";
    }

    public string GetOtherVersionHomeRoute(string uri)
    {
        return GetVersion(uri) == DocsVersion.V2 ? "/v1" : "/v2/home";
    }

    public string WithVersion(string uri, string route)
    {
        if (route.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            return route;
        }

        string normalizedRoute = route.StartsWith('/') ? route : $"/{route}";

        if (normalizedRoute.StartsWith("/v1", StringComparison.OrdinalIgnoreCase)
            || normalizedRoute.StartsWith("/v2", StringComparison.OrdinalIgnoreCase)
            || normalizedRoute.StartsWith("/releases", StringComparison.OrdinalIgnoreCase))
        {
            return normalizedRoute;
        }

        if (normalizedRoute.Equals("/demo", StringComparison.OrdinalIgnoreCase))
        {
            return GetDemoRoute(uri);
        }

        if (normalizedRoute.Equals("/", StringComparison.OrdinalIgnoreCase))
        {
            return GetHomeRoute(uri);
        }

        return $"{GetRoutePrefix(uri)}{normalizedRoute}";
    }

    public static bool IsV1(NavigationManager navigationManager)
    {
        return new Uri(navigationManager.Uri).AbsolutePath.StartsWith("/v1", StringComparison.OrdinalIgnoreCase);
    }
}
