using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Shared;

public partial class DocsLayout : LayoutComponentBase
{
    [Inject] private LayoutService LayoutService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private NavMenu? NavMenuRef;
    private bool _drawerOpen = true;
    private bool _topMenuOpen = false;

    protected override void OnInitialized()
    {
        LayoutService.SetBaseTheme(Theme.Theme.CropperBlazorDocsTheme());
    }

    protected override void OnAfterRender(bool firstRender)
    {
        //refresh nav menu because no parameters change in nav menu but internal data does
        NavMenuRef?.Refresh();
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OpenTopMenu()
    {
        _topMenuOpen = true;
    }

    private void OnDrawerOpenChanged(bool value)
    {
        _topMenuOpen = false;
        _drawerOpen = value;
        StateHasChanged();
    }
}