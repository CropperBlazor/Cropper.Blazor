using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services.UserPreferences;
using MudBlazor;

namespace Cropper.Blazor.Client.Services;

public class LayoutService
{
    public ThemeMode DarkModeToggle = ThemeMode.System;
    private readonly IUserPreferencesService _userPreferencesService;
    private bool _systemPreferences;
    private UserPreferences.UserPreferences _userPreferences = null!;
    public event EventHandler MajorUpdateOccured = null!;

    public MudTheme CurrentTheme { get; private set; } = null!;
    public bool IsDarkMode { get; private set; }

    public LayoutService(IUserPreferencesService userPreferencesService) =>
        _userPreferencesService = userPreferencesService;

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        _systemPreferences = isDarkModeDefaultTheme;
        _userPreferences = await _userPreferencesService.LoadUserPreferences();

        if (_userPreferences != null)
        {
            IsDarkMode = _userPreferences.ThemeMode switch
            {
                ThemeMode.Dark => true,
                ThemeMode.Light => false,
                ThemeMode.System => isDarkModeDefaultTheme,
                _ => IsDarkMode
            };
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences.UserPreferences { ThemeMode = ThemeMode.System };
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
    }

    public BasePage GetDocsBasePage(string uri)
    {
        Uri webUri = new(uri);

        if (webUri.AbsolutePath.Contains("/demo"))
        {
            return BasePage.Demo;
        }
        else if (webUri.AbsolutePath.Contains("/examples"))
        {
            return BasePage.Examples;
        }
        else if (webUri.AbsolutePath.Contains("/api"))
        {
            return BasePage.Api;
        }
        else if (webUri.AbsolutePath.Contains("/about"))
        {
            return BasePage.About;
        }
        else if (webUri.AbsolutePath.Contains("/releases"))
        {
            return BasePage.Releases;
        }
        else if (webUri.AbsolutePath == "/")
        {
            return BasePage.Home;
        }
        else
        {
            return BasePage.None;
        }
    }

    public Task OnSystemPreferenceChanged(bool newValue)
    {
        _systemPreferences = newValue;

        if (DarkModeToggle == ThemeMode.System)
        {
            IsDarkMode = newValue;
            OnMajorUpdateOccured();
        }

        return Task.CompletedTask;
    }

    public void SetBaseTheme(MudTheme theme)
    {
        CurrentTheme = theme;
        OnMajorUpdateOccured();
    }

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }
    public async Task ToggleDarkMode()
    {
        switch (DarkModeToggle)
        {
            case ThemeMode.System:
                DarkModeToggle = ThemeMode.Light;
                IsDarkMode = false;
                break;

            case ThemeMode.Light:
                DarkModeToggle = ThemeMode.Dark;
                IsDarkMode = true;
                break;

            case ThemeMode.Dark:
                DarkModeToggle = ThemeMode.System;
                IsDarkMode = _systemPreferences;
                break;
        }

        _userPreferences.ThemeMode = DarkModeToggle;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccured();
    }

    private void OnMajorUpdateOccured()
    {
        MajorUpdateOccured?.Invoke(this, EventArgs.Empty);
    }
}