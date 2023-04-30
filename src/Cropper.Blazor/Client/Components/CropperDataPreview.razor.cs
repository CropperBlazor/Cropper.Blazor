using Cropper.Blazor.Components;
using Cropper.Blazor.Events.CropEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Components
{
    public partial class CropperDataPreview
    {
        [Parameter]
        public CropperComponent? cropperComponent { get; set; } = null!;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private decimal? X;
        private decimal? Y;
        private decimal? Height;
        private decimal? Width;
        private decimal? Rotate;
        private decimal? ScaleX;
        private decimal? ScaleY;

        public async Task OnCropEvent(CropEvent cropEvent)
        {
            X = cropEvent.X;
            Y = cropEvent.Y;
            Width = cropEvent.Width;
            Height = cropEvent.Height;
            Rotate = cropEvent.Rotate;
            ScaleX = cropEvent.ScaleX;
            ScaleY = cropEvent.ScaleY;

            StateHasChanged();
        }
    }
}