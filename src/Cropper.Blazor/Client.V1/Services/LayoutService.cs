using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services.UserPreferences;
using MudBlazor;

namespace Cropper.Blazor.Client.Services;

public class LayoutService
{
    private readonly IUserPreferencesService _userPreferencesService;
    private UserPreferences.UserPreferences _userPreferences = null!;
    private bool _systemDarkMode;
    public event EventHandler MajorUpdateOccured = null!;

    public DarkLightMode CurrentDarkLightMode { get; private set; }

    public bool IsDarkMode { get; private set; }

    public bool ObserveSystemThemeChange { get; private set; }

    public MudTheme CurrentTheme { get; private set; } = null!;

    public LayoutService(IUserPreferencesService userPreferencesService) =>
        _userPreferencesService = userPreferencesService;

    /// <summary>
    /// Updates the dark mode state based on user preference and, optionally, the system's dark mode setting.
    /// </summary>
    /// <param name="systemMode">The current system dark mode setting. If <c>null</c>, the existing known system mode is used.</param>
    public void UpdateDarkModeState(bool? systemMode = null)
    {
        if (systemMode.HasValue)
        {
            _systemDarkMode = systemMode.Value;
        }

        IsDarkMode = CurrentDarkLightMode switch
        {
            DarkLightMode.Dark => true,
            DarkLightMode.Light => false,
            _ => _systemDarkMode,
        };
    }

    public async Task ApplyUserPreferencesAsync()
    {
        _userPreferences = await _userPreferencesService.LoadUserPreferences();

        if (_userPreferences is null)
        {
            _userPreferences = new()
            {
                DarkLightTheme = DarkLightMode.System,
            };
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
        else
        {
            CurrentDarkLightMode = _userPreferences.DarkLightTheme;
            UpdateDarkModeState();
        }
    }

    /// <summary>
    /// Handles changes in the system's dark mode setting.
    /// </summary>
    /// <param name="isSystemDarkMode"><c>true</c> if the system is in dark mode, otherwise <c>false</c>.</param>
    public Task OnSystemModeChangedAsync(bool isSystemDarkMode)
    {
        _systemDarkMode = isSystemDarkMode;
        UpdateDarkModeState();
        OnMajorUpdateOccurred();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Cycles through the available dark/light mode options (System, Light, Dark) and saves the new preference.
    /// </summary>
    public async Task CycleDarkLightModeAsync()
    {
        CurrentDarkLightMode = CurrentDarkLightMode switch
        {
            DarkLightMode.System => DarkLightMode.Light,
            DarkLightMode.Light => DarkLightMode.Dark,
            DarkLightMode.Dark => DarkLightMode.System,
            _ => DarkLightMode.System, // Default case, should not happen.
        };

        ObserveSystemThemeChange = CurrentDarkLightMode == DarkLightMode.System;
        UpdateDarkModeState();

        _userPreferences.DarkLightTheme = CurrentDarkLightMode;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccurred();
    }

    public void SetBaseTheme(MudTheme theme)
    {
        CurrentTheme = theme;
        OnMajorUpdateOccurred();
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

    private void OnMajorUpdateOccurred() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);
}
