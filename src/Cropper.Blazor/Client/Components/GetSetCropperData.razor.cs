using Cropper.Blazor.Components;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Cropper.Blazor.Client.Components
{
    public partial class GetSetCropperData
    {
        [Parameter]
        public CropperComponent? cropperComponent { get; set; } = null!;

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
            cropperComponent?.SetCropBoxData(cropBoxDataOptions);
        }

        public void SetData(SetDataOptions setDataOptions)
        {
            cropperComponent?.SetData(setDataOptions);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            cropperComponent?.SetCanvasData(setCanvasDataOptions);
        }

        public async void GetCropBoxData()
        {
            cropBoxData = await cropperComponent!.GetCropBoxDataAsync();
            StateHasChanged();
        }

        public async void GetData(bool rounded)
        {
            cropperData = await cropperComponent!.GetDataAsync(rounded);
            StateHasChanged();
        }

        public async void GetContainerData()
        {
            containerData = await cropperComponent!.GetContainerDataAsync();
            StateHasChanged();
        }

        public async void GetImageData()
        {
            imageData = await cropperComponent!.GetImageDataAsync();
            StateHasChanged();
        }

        public async void GetCanvasData()
        {
            canvasData = await cropperComponent!.GetCanvasDataAsync();
            StateHasChanged();
        }
    }
}
