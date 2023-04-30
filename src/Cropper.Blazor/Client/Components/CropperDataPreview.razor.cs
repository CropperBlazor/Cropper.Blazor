using Cropper.Blazor.Events.CropEvent;

namespace Cropper.Blazor.Client.Components
{
    public partial class CropperDataPreview
    {
        private decimal? X;
        private decimal? Y;
        private decimal? Height;
        private decimal? Width;
        private decimal? Rotate;
        private decimal? ScaleX;
        private decimal? ScaleY;

        public void OnCropEvent(CropEvent cropEvent)
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