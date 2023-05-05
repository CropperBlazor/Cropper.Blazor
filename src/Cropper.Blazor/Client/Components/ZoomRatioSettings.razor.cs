using Cropper.Blazor.Events.ZoomEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Components
{
    public partial class ZoomRatioSettings
    {
        private decimal? minZoomRatio = null;
        private decimal? maxZoomRatio = null;

        private decimal? MinZoomRatio { get => minZoomRatio; set { minZoomRatio=value; ApplyZoomRulesForCropperAsync(); } }
        private decimal? MaxZoomRatio { get => maxZoomRatio; set { maxZoomRatio=value; ApplyZoomRulesForCropperAsync(); } }
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private decimal? OldRatio { get; set; } = null;

        private decimal? Ratio { get; set; } = null;

        public void OnZoomEvent(ZoomEvent? zoomEvent)
        {
            OldRatio = zoomEvent?.OldRatio;
            Ratio = zoomEvent?.Ratio;

            StateHasChanged();
        }

        public async Task ApplyZoomRulesForCropperAsync()
        {
            await JSRuntime!.InvokeVoidAsync("window.overrideOnZoomCropperEvent", MinZoomRatio, MaxZoomRatio);
        }
    }
}
