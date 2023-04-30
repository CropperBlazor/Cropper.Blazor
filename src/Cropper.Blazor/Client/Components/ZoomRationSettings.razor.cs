using Cropper.Blazor.Events.ZoomEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Components
{
    public partial class ZoomRationSettings
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private decimal? OldRatio { get; set; } = null;
        private decimal? Ratio { get; set; } = null;
        private decimal? MinZoomRatio = null;
        private decimal? MaxZoomRatio = null;

        public async Task OnZoomEventAsync(ZoomEvent? zoomEvent)
        {
            OldRatio = zoomEvent?.OldRatio;
            Ratio = zoomEvent?.Ratio;

            await InvokeAsync(StateHasChanged);
        }

        public async Task ApplyZoomRulesForCropperAsync()
        {
            await JSRuntime!.InvokeVoidAsync("window.overrideCropperJsInteropModule", MinZoomRatio, MaxZoomRatio);
        }
    }
}