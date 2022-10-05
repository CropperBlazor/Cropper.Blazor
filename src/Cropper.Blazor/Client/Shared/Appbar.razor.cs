using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Cropper.Blazor.Client.Services;
using Cropper.Blazor.Client.Enums;

namespace Cropper.Blazor.Client.Shared;

public partial class Appbar
{
    [Parameter] public EventCallback<MouseEventArgs> DrawerToggleCallback { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private LayoutService LayoutService { get; set; }

    private string GetActiveClass(BasePage page)
    {
        return page == LayoutService.GetDocsBasePage(NavigationManager.Uri) ? "mud-chip-text mud-chip-color-primary mx-1 px-3" : "mx-1 px-3";
    }
}