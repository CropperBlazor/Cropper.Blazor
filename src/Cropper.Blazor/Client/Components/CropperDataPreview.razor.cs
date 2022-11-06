using Cropper.Blazor.Components;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Components
{
    public partial class CropperDataPreview
    {
        [Parameter]
        public CropperComponent? cropperComponent { get; set; } = null!;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private decimal? x;
        private decimal? y;
        private decimal? height;
        private decimal? width;
        private decimal? rotate;
        private decimal? scaleX;
        private decimal? scaleY;

        public void OnCropEvent(CropEvent cropEvent)
        {
            x = cropEvent.X;
            y = cropEvent.Y;
            width = cropEvent.Width;
            height = cropEvent.Height;
            rotate = cropEvent.Rotate;
            scaleX = cropEvent.ScaleX;
            scaleY = cropEvent.ScaleY;

            JSRuntime!.InvokeVoidAsync("console.log", $"CropEvent");
            StateHasChanged();
        }
    }
}