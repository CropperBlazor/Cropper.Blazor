using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Cropper.Blazor.Client.Shared
{
    public partial class UpdateAvailableDetector
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        [Inject] private ISnackbar? Snackbar { get; set; }
        [Inject] private NavigationManager? Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await RegisterForUpdateAvailableNotification();
        }

        private async Task RegisterForUpdateAvailableNotification()
        {
            await JSRuntime!.InvokeAsync<object>(
                identifier: "registerForUpdateAvailableNotification",
                DotNetObjectReference.Create(this),
                nameof(OnUpdateAvailable));
        }

        [JSInvokable(nameof(OnUpdateAvailable))]
        public void OnUpdateAvailable()
        {
            Snackbar!.Add("A new version of the application is available.", Severity.Warning, config =>
            {
                config.Action = "Reload";
                config.IconColor = Color.Error;
                config.ActionColor = Color.Error;
                config.Icon = Icons.Material.Filled.BrowserUpdated;
                config.RequireInteraction = true;
                config.Onclick = snackbar =>
                {
                    Navigation!.NavigateTo(Navigation.Uri, true);

                    return Task.CompletedTask;
                };
            });
        }
    }
}
