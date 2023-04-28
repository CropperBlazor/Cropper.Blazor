using System.ComponentModel.DataAnnotations;
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

        private CropBoxData cropBoxData = null!;
        private CropperData cropperData = null!;
        private ContainerData containerData = null!;
        private ImageData imageData = null!;
        private CanvasData canvasData = null!;

        protected override void OnInitialized()
        {
            cropBoxData = new CropBoxData();
            cropperData = new CropperData();
            containerData = new ContainerData();
            imageData = new ImageData();
            canvasData = new CanvasData();
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
            cropBoxData = await GetCropBoxData.Invoke();
            StateHasChanged();
        }

        public async void GetDataAsync(bool rounded)
        {
            cropperData = await GetData.Invoke(rounded);
            StateHasChanged();
        }

        public async void GetContainerDataAsync()
        {
            containerData = await GetContainerData.Invoke();
            StateHasChanged();
        }

        public async void GetImageDataAsync()
        {
            imageData = await GetImageData.Invoke();
            StateHasChanged();
        }

        public async void GetCanvasDataAsync()
        {
            canvasData = await GetCanvasData.Invoke();
            StateHasChanged();
        }
    }
}
