using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cropper.Blazor.Client.Shared;

public partial class AppbarButtons
{
    [Inject]
    private LayoutService LayoutService { get; set; } = null!;

    /// <summary>
    /// Gets the text for the dark/light mode toggle button, indicating the next mode.
    /// </summary>
    public string DarkLightModeButtonText => LayoutService.CurrentDarkLightMode switch
    {
        DarkLightMode.Dark => "Auto mode",
        DarkLightMode.Light => "Dark mode",
        _ => "Light mode"
    };

    /// <summary>
    /// Gets the icon for the dark/light mode toggle button.
    /// </summary>
    public string DarkLightModeButtonIcon => LayoutService.CurrentDarkLightMode switch
    {
        DarkLightMode.Dark => Icons.Material.Rounded.AutoMode,
        DarkLightMode.Light => Icons.Material.Outlined.DarkMode,
        _ => Icons.Material.Filled.LightMode
    };
}
