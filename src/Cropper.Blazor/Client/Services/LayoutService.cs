using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services.UserPreferences;
using MudBlazor;

namespace Cropper.Blazor.Client.Services;
public class LayoutService
{
    private readonly IUserPreferencesService _userPreferencesService;
    private UserPreferences.UserPreferences _userPreferences;

    public bool IsDarkMode { get; private set; } = false;

    public MudTheme CurrentTheme { get; private set; }

    public LayoutService(IUserPreferencesService userPreferencesService)
    {
        _userPreferencesService = userPreferencesService;
    }

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _userPreferences = await _userPreferencesService.LoadUserPreferences();
        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.IsDarkMode;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences.UserPreferences { IsDarkMode = IsDarkMode };
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
    }

    public event EventHandler MajorUpdateOccured;

    private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

    public async Task ToggleDarkMode()
    {
        IsDarkMode = !IsDarkMode;
        _userPreferences.IsDarkMode = IsDarkMode;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
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
