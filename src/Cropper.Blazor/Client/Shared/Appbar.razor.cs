using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Cropper.Blazor.Client.Shared;

public partial class Appbar
{
    [Parameter] public EventCallback<MouseEventArgs> DrawerToggleCallback { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private LayoutService LayoutService { get; set; } = null!;
    [Inject] private DocsVersionService DocsVersionService { get; set; } = null!;

    private string RoutePrefix => DocsVersionService.GetRoutePrefix(NavigationManager.Uri);

    private string HomeRoute => DocsVersionService.GetHomeRoute(NavigationManager.Uri).TrimStart('/');

    private string DemoRoute => DocsVersionService.GetDemoRoute(NavigationManager.Uri).TrimStart('/');

    private string OtherVersionHome => DocsVersionService.GetOtherVersionHomeRoute(NavigationManager.Uri).TrimStart('/');

    private string OtherVersionLabel => DocsVersionService.GetVersion(NavigationManager.Uri) == DocsVersion.V2 ? "v1" : "v2";

    private string GetActiveClass(BasePage page)
    {
        return page == LayoutService.GetDocsBasePage(NavigationManager.Uri) ? "mud-chip-text mud-chip-color-primary mx-1 px-3" : "mx-1 px-3";
    }
}
