using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

namespace Cropper.Blazor.Client.Shared;
public partial class MainLayout : LayoutComponentBase
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] IResizeService ResizeService { get; set; } = null!;

    protected override void OnInitialized()
    {
        LayoutService.MajorUpdateOccured += LayoutServiceOnMajorUpdateOccured;
        LayoutService.SetBaseTheme(Theme.CropperBlazorDocsTheme());
        base.OnInitialized();
    }

    private Guid _subscriptionId;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _subscriptionId = await ResizeService.Subscribe((size) =>
            {
                if (size.Width > 960)
                {
                    OnDrawerOpenChanged(false);
                }

                InvokeAsync(StateHasChanged);
            }, new MudBlazor.Services.ResizeOptions
            {
                ReportRate = 50,
                NotifyOnBreakpointOnly = false,
            });

            var size = await ResizeService.GetBrowserWindowSize();

            await ApplyUserPreferences();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public async ValueTask DisposeAsync() => await ResizeService.Unsubscribe(_subscriptionId);

    private async Task ApplyUserPreferences()
    {
        //var defaultDarkMode = await _mudThemeProvider.GetSystemPreference();
        await LayoutService.ApplyUserPreferences(true);
    }

    public void Dispose()
    {
        LayoutService.MajorUpdateOccured -= LayoutServiceOnMajorUpdateOccured;
    }

    private void LayoutServiceOnMajorUpdateOccured(object? sender, EventArgs e) => StateHasChanged();

    //private NavMenu _navMenuRef;
    private bool _drawerOpen = false;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OnDrawerOpenChanged(bool value)
    {
        _drawerOpen = value;
        StateHasChanged();
    }

    private string GetActiveClass(BasePage page)
    {
        return page == GetDocsBasePage(NavigationManager.Uri) ? "mud-chip-text mud-chip-color-primary ml-3" : "ml-3";
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
