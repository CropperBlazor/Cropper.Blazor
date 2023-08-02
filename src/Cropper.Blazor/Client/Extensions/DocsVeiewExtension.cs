using Blazored.LocalStorage;
using Cropper.Blazor.Client.Services;
using Cropper.Blazor.Client.Services.UserPreferences;
using Cropper.Blazor.Extensions;
using MudBlazor;
using MudBlazor.Services;

namespace Cropper.Blazor.Client.Extensions;
public static class DocsViewExtension
{
    public static void TryAddDocsViewServices(this IServiceCollection services)
    {
        services.AddCropper();

        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 200;
            config.SnackbarConfiguration.ShowTransitionDuration = 100;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        services.AddBlazoredLocalStorage();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();
        services.AddScoped<LayoutService>();
    }
}
