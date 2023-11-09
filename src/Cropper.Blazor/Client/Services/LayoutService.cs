﻿using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Client.Services.UserPreferences;
using MudBlazor;

namespace Cropper.Blazor.Client.Services;

public class LayoutService
{
    private readonly IUserPreferencesService _userPreferencesService = null!;
    private UserPreferences.UserPreferences _userPreferences = null!;
    private bool _systemPreferences;

    public bool IsDarkMode { get; private set; } = false;

    public ThemeMode ThemeMode = ThemeMode.System;

    public MudTheme CurrentTheme { get; private set; } = null!;
    public event EventHandler MajorUpdateOccured = null!;

    public LayoutService(IUserPreferencesService userPreferencesService) => _userPreferencesService = userPreferencesService;

    public void SetDarkMode(bool value) => IsDarkMode = value;

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

    public async Task OnSystemPreferenceChanged(bool newValue)
    {
        _systemPreferences = newValue;
        if (ThemeMode == ThemeMode.System)
        {
            IsDarkMode = newValue;
            OnMajorUpdateOccured();
        }
    }

    private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

    public async Task ToggleDarkMode()
    {
        switch (ThemeMode)
        {
            case ThemeMode.System:
                ThemeMode = ThemeMode.Light;
                IsDarkMode = false;
                break;
            case ThemeMode.Light:
                ThemeMode = ThemeMode.Dark;
                IsDarkMode = true;
                break;
            case ThemeMode.Dark:
                ThemeMode = ThemeMode.System;
                IsDarkMode = _systemPreferences;
                break;
        }

        _userPreferences.ThemeMode = ThemeMode;
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
        else if (uri.Contains("/examples"))
            return BasePage.Examples;
        else if (uri.Contains("/api"))
            return BasePage.Api;
        else if (uri.Contains("/about"))
            return BasePage.About;
        else if (uri.Contains("/contract"))
            return BasePage.Contract;
        else
            return BasePage.None;
    }
}
