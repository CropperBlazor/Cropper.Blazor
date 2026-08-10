using Cropper.Blazor.Components;
using Cropper.Blazor.Events.CropEvent;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components
{
    public partial class CropperDemoLiveSelection
    {
        private CropperDataPreview? CropperDataPreview;

        [Parameter]
        public CropperState CropperState { get; set; } = null!;

        [Parameter]
        public int ActiveSelectionVersion { get; set; }

        [Parameter]
        public IReadOnlyList<string> RelatedImageSources { get; set; } = [];

        [Parameter]
        public EventCallback<string> RelatedImageSelected { get; set; }

        public void OnCropEvent(CropEvent cropEvent)
        {
            CropperDataPreview?.OnCropEvent(cropEvent);
        }

        public void ClearCropData()
        {
            CropperDataPreview?.OnCropEvent(new CropEvent
            {
                X = 0,
                Y = 0,
                Width = 0,
                Height = 0,
                Rotate = 0,
                ScaleX = 1,
                ScaleY = 1
            });
        }
    }
}
