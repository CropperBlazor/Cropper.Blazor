using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services.UserPreferences;
using MudBlazor;

namespace Cropper.Blazor.Client.Services;
public class LayoutService
{
    private readonly IUserPreferencesService _userPreferencesService = null!;
    private UserPreferences.UserPreferences UserPreferences = null!;

    public bool IsDarkMode { get; private set; } = false;

    public MudTheme CurrentTheme { get; private set; } = null!;
    public event EventHandler MajorUpdateOccured = null!;

    public LayoutService(IUserPreferencesService userPreferencesService) => _userPreferencesService = userPreferencesService;

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        UserPreferences = await _userPreferencesService.LoadUserPreferences();
        if (UserPreferences != null)
        {
            IsDarkMode = UserPreferences.IsDarkMode;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            UserPreferences = new UserPreferences.UserPreferences { IsDarkMode = IsDarkMode };
            await _userPreferencesService.SaveUserPreferences(UserPreferences);
        }
    }

    private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

    public async Task ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        UserPreferences.IsDarkMode = IsDarkMode;
        await _userPreferencesService.SaveUserPreferences(UserPreferences);
        OnMajorUpdateOccured();
    }

    public void SetBaseTheme(MudTheme theme)
    {
        CurrentTheme = theme;
        OnMajorUpdateOccured();
    }

    public BasePage GetDocsBasePage(string uri)
    {
        if (uri.Contains("/demo"))
            return BasePage.Demo;
        else if (uri.Contains("/api"))
            return BasePage.Api;
        else if (uri.Contains("/about"))
            return BasePage.About;
        else
            return BasePage.None;
    }
}
