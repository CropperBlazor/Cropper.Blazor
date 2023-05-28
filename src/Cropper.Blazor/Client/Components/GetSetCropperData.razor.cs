using System.ComponentModel.DataAnnotations;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components
{
    public partial class GetSetCropperData
    {
        [Parameter, Required]
        public Action<SetCropBoxDataOptions> SetCropBoxDataOptions { get; set; } = null!;
        [Parameter, Required]
        public Action<SetDataOptions> SetDataOptions { get; set; } = null!;
        [Parameter, Required]
        public Action<SetCanvasDataOptions> SetCanvasDataOptions { get; set; } = null!;
        [Parameter, Required]
        public Func<ValueTask<CropBoxData>> GetCropBoxData { get; set; } = null!;
        [Parameter, Required]
        public Func<bool, ValueTask<CropperData>> GetData { get; set; } = null!;
        [Parameter, Required]
        public Func<ValueTask<ContainerData>> GetContainerData { get; set; } = null!;
        [Parameter, Required]
        public Func<ValueTask<ImageData>> GetImageData { get; set; } = null!;
        [Parameter, Required]
        public Func<ValueTask<CanvasData>> GetCanvasData { get; set; } = null!;

        private CropBoxData CropBoxData = null!;
        private CropperData CropperData = null!;
        private ContainerData ContainerData = null!;
        private ImageData ImageData = null!;
        private CanvasData CanvasData = null!;
        private ZoomRatioSettings ZoomRatioSettings = null!;
        public CroppedDimensionsSettings CroppedDimensionsSettings = null!;

        protected override void OnInitialized()
        {
            CropBoxData = new CropBoxData();
            CropperData = new CropperData();
            ContainerData = new ContainerData();
            ImageData = new ImageData();
            CanvasData = new CanvasData();
        }

        public void OnZoomEvent(ZoomEvent? zoomEvent)
        {
            ZoomRatioSettings!.OnZoomEvent(zoomEvent);
        }

        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            SetCropBoxDataOptions.Invoke(cropBoxDataOptions);
        }

        public void SetData(SetDataOptions setDataOptions)
        {
            SetDataOptions.Invoke(setDataOptions);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            SetCanvasDataOptions.Invoke(setCanvasDataOptions);
        }

        public async void GetCropBoxDataAsync()
        {
            CropBoxData = await GetCropBoxData.Invoke();
            StateHasChanged();
        }

        public async void GetDataAsync(bool rounded)
        {
            CropperData = await GetData.Invoke(rounded);
            StateHasChanged();
        }

        public async void GetContainerDataAsync()
        {
            ContainerData = await GetContainerData.Invoke();
            StateHasChanged();
        }

        public async void GetImageDataAsync()
        {
            ImageData = await GetImageData.Invoke();
            StateHasChanged();
        }

        public async void GetCanvasDataAsync()
        {
            CanvasData = await GetCanvasData.Invoke();
            StateHasChanged();
        }
    }
}
