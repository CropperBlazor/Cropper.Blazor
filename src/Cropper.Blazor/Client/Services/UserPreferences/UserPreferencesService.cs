using Blazored.LocalStorage;

namespace Cropper.Blazor.Client.Services.UserPreferences;
public interface IUserPreferencesService
{
    Task SaveUserPreferences(UserPreferences userPreferences);

    Task<UserPreferences> LoadUserPreferences();
}

public class UserPreferencesService : IUserPreferencesService
{
    private readonly ILocalStorageService _localStorageService;
    private const string Key = "userPreferences";

    public UserPreferencesService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorageService.SetItemAsync(Key, userPreferences);
    }

    public async Task<UserPreferences> LoadUserPreferences()
    {
        return await _localStorageService.GetItemAsync<UserPreferences>(Key);
    }
}

