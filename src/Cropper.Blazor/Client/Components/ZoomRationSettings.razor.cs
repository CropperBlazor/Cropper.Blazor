using Cropper.Blazor.Events.ZoomEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Components
{
    public partial class ZoomRationSettings
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private decimal? oldRatio { get; set; } = null;
        private decimal? ratio { get; set; } = null;
        private decimal? minZoomRatio = null;
        private decimal? maxZoomRatio = null;

        public async Task OnZoomEventAsync(ZoomEvent? zoomEvent)
        {
            oldRatio = zoomEvent?.OldRatio;
            ratio = zoomEvent?.Ratio;

            await InvokeAsync(StateHasChanged);
        }

        public async Task ApplyZoomRulesForCropperAsync()
        {
            await JSRuntime!.InvokeVoidAsync("window.overrideCropperJsInteropModule", minZoomRatio, maxZoomRatio);
        }
    }
}