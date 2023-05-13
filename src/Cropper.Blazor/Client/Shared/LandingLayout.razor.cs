using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

namespace Cropper.Blazor.Client.Shared;
public partial class LandingLayout : LayoutComponentBase
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;


    private bool _drawerOpen = false;

    protected override void OnInitialized()
    {
        LayoutService.SetBaseTheme(Theme.Theme.LandingPageTheme());
        base.OnInitialized();
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    public BasePage GetDocsBasePage(string uri)
    {
        if (uri.Contains("/demo"))
        {
            return BasePage.Demo;
        }
        else if (uri.Contains("/api"))
        {
            return BasePage.Api;
        }
        else if (uri.Contains("/about"))
        {
            return BasePage.About;
        }
        else
        {
            return BasePage.None;
        }
    }
}
