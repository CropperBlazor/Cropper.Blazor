using Cropper.Blazor.Components;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace Cropper.Blazor.Client.Components
{
    public partial class ZoomRatioSettings
    {
        private decimal? minZoomRatio = null;
        private decimal? maxZoomRatio = null;

        private decimal? MinZoomRatio
        {
            get => minZoomRatio;
            set
            {
                minZoomRatio = value;
                ApplyZoomRulesForCropperAsync();
            }
        }
        private decimal? MaxZoomRatio
        {
            get => maxZoomRatio;
            set
            {
                maxZoomRatio = value;
                ApplyZoomRulesForCropperAsync();
            }
        }
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private decimal? OldRatio { get; set; } = null;

        private decimal? Ratio { get; set; } = null;

        [CascadingParameter(Name = "CropperComponent"), Required]
        private CropperComponent CropperComponent { get; set; } = null!;

        public void OnZoomEvent(ZoomEvent? zoomEvent)
        {
            OldRatio = zoomEvent?.OldRatio;
            Ratio = zoomEvent?.Ratio;

            StateHasChanged();
        }

        public void SetRatio(decimal? ratio)
        {
            Ratio = ratio;

            StateHasChanged();
        }

        public async Task ApplyZoomRulesForCropperAsync()
        {
            ImageData currentImageData = await CropperComponent!.GetImageDataAsync();
            decimal currentZoomRatio = currentImageData.Width / currentImageData.NaturalWidth;

            if ((MinZoomRatio is not null) && (MinZoomRatio > currentZoomRatio))
            {
                ContainerData containerData = await CropperComponent.GetContainerDataAsync();
                CropperComponent.ZoomTo((decimal)MinZoomRatio, containerData.Width / 2, containerData.Height / 2);
            }
            else if ((MaxZoomRatio is not null) && (currentZoomRatio > MaxZoomRatio))
            {
                ContainerData containerData = await CropperComponent.GetContainerDataAsync();
                CropperComponent.ZoomTo((decimal)MaxZoomRatio, containerData.Width / 2, containerData.Height / 2);
            }

            await JSRuntime!.InvokeVoidAsync("window.overrideOnZoomCropperEvent", MinZoomRatio, MaxZoomRatio);
        }
    }
}
