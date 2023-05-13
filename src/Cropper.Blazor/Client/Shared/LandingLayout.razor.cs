using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;

namespace Cropper.Blazor.Client.Shared;
public partial class LandingLayout : LayoutComponentBase
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    [Inject] IResizeService ResizeService { get; set; } = null!;

    public async ValueTask DisposeAsync() => await ResizeService.UnsubscribeAsync(SubscriptionId);

    private bool _drawerOpen = false;

    private Guid SubscriptionId;

    protected override void OnInitialized()
    {
        LayoutService.SetBaseTheme(Theme.Theme.LandingPageTheme());
        base.OnInitialized();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            SubscriptionId = await ResizeService.SubscribeAsync((size) =>
            {
                if (size.Width > 960)
                {
                    OnDrawerOpenChanged(false);
                }

                InvokeAsync(StateHasChanged);
            }, new ResizeOptions
            {
                ReportRate = 50,
                NotifyOnBreakpointOnly = false,
            });

            var size = await ResizeService.GetBrowserWindowSize();

            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OnDrawerOpenChanged(bool value)
    {
        _drawerOpen = value;
        StateHasChanged();
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
