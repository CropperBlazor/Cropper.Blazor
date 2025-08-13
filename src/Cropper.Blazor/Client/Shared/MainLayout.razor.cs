using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cropper.Blazor.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        private MudThemeProvider _mudThemeProvider = null!;

        [Inject] 
        private LayoutService LayoutService { get; set; } = null!;


        protected override void OnInitialized()
        {
            LayoutService.MajorUpdateOccured += OnMajorUpdateOccured;
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var dark = await _mudThemeProvider.GetSystemDarkModeAsync();

                LayoutService.UpdateDarkModeState(dark);

                await LayoutService.ApplyUserPreferencesAsync();

                await _mudThemeProvider.WatchSystemDarkModeAsync(LayoutService.OnSystemModeChangedAsync);

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public void Dispose()
        {
            LayoutService.MajorUpdateOccured -= OnMajorUpdateOccured;
        }

        private void OnMajorUpdateOccured(object? sender, EventArgs e) => StateHasChanged();
    }
}
