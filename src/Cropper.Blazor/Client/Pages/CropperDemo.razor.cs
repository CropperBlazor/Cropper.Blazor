using Cropper.Blazor.Client.Components;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Extensions;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.Client.Pages
{
    public partial class CropperDemo : IDisposable
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private CropperComponent? cropperComponent = null!;
        private CropperDataPreview? cropperDataPreview = null!;
        private Options options = null!;
        private decimal? scaleX;
        private decimal? scaleY;

        private string Src = "https://fengyuanchen.github.io/cropperjs/images/picture.jpg";
        private string ErrorLoadImageSrc = "not-found-image.jpg";
        private bool IsErrorLoadImage { get; set; } = false;

        protected override void OnInitialized()
        {
            options = new Options()
            {
                Preview = ".img-preview",
                AspectRatio = (decimal)16 / 9,
                //DragMode =  DragMode.Crop.ToString()
            };
        }

        public void OnCropEvent(CropEvent cropEvent)
        {
            scaleX = cropEvent.ScaleX;
            scaleY = cropEvent.ScaleY;
            InvokeAsync(() =>
            {
                cropperDataPreview?.OnCropEvent(cropEvent);
            });
        }

        public async void OnCropEndEvent(CropEndEvent cropEndEvent)
        {
             await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndEvent, {cropEndEvent.ActionEvent}");
            //Console.WriteLine($"CropEndEvent, {cropEndEvent.ActionEvent}");
        }

        public async void OnCropStartEvent(CropStartEvent cropStartEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartEvent, {cropStartEvent.ActionEvent}");
            //Console.WriteLine($"CropStartEvent, {cropStartEvent.ActionEvent}");
        }

        public async void OnZoomEvent(ZoomEvent zoomEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"ZoomEvent, OldRatio: {zoomEvent.OldRatio}, Ratio: {zoomEvent.Ratio}");
            //Console.WriteLine($"ZoomEvent, OldRatio: {zoomEvent.OldRatio}, Ratio: {zoomEvent.Ratio}");
        }

        public async void OnCropMoveEvent(CropMoveEvent cropMoveEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropMoveEvent, {cropMoveEvent.ActionEvent}");
            //Console.WriteLine($"CropMoveEvent, {cropMoveEvent.ActionEvent}");
        }

        public async void OnCropReadyEvent(CropReadyEvent cropReadyEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Cropper Is Ready");
            //Console.WriteLine("Cropper Is Ready");
        }

        public async void OnLoadImageEvent()
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Image Is loaded");
            //Console.WriteLine("Image Is loaded");
        }

        public void OnErrorLoadImageEvent(ErrorEventArgs errorEventArgs)
        {
            IsErrorLoadImage = true;
            Destroy();
            StateHasChanged();
        }


        private void SetMoveDragMode()
        {
            cropperComponent?.SetDragMode(DragMode.Move);
        }

        private void SetCropDragMode()
        {
            cropperComponent?.SetDragMode(DragMode.Crop);
        }

        private void Zoom(decimal ratio)
        {
            cropperComponent?.Zoom(ratio);
        }

        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            cropperComponent?.ZoomTo(ratio, pivotX, pivotY);
        }

        private void Move(decimal offsetX, decimal? offsetY)
        {
            cropperComponent?.Move(offsetX, offsetY);
        }

        public void MoveTo(decimal x, decimal? y)
        {
            cropperComponent?.MoveTo(x, y);
        }

        private void Rotate(decimal degree)
        {
            cropperComponent?.Rotate(degree);
        }

        private void ScaleX(decimal? scaleX)
        {
            cropperComponent?.ScaleX(scaleX ?? 0);
        }

        private void ScaleY(decimal? scaleY)
        {
            cropperComponent?.ScaleY(scaleY ?? 0);
        }

        public void Scale(decimal? scaleX, decimal? scaleY)
        {
            cropperComponent?.Scale(scaleX ?? 0, scaleY ?? 0);
        }

        private void Crop()
        {
            cropperComponent?.Crop();
        }

        private void Clear()
        {
            cropperComponent?.Clear();
        }

        private void Enable()
        {
            cropperComponent?.Enable();
        }

        private void Disable()
        {
            cropperComponent?.Disable();
        }

        private void Destroy()
        {
            cropperComponent?.Destroy();
            cropperComponent?.RevokeObjectUrlAsync(Src);
        }

        public void SetAspectRatio(decimal aspectRatio)
        {
            cropperComponent?.SetAspectRatio(aspectRatio);
        }

        public void SetViewMode(ViewMode viewMode)
        {
            options.ViewMode = viewMode;
            cropperComponent?.Destroy();
            cropperComponent?.InitCropper();
        }

        private void Reset()
        {
            cropperComponent?.Reset();
        }

        public async void GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            string croppedCanvasDataURL = await cropperComponent!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);
            var parameters = new DialogParameters();
            parameters.Add("Src", croppedCanvasDataURL);
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.CroppedCanvasDialog>("CroppedCanvasDialog", parameters, options);
        }

        public async Task InputFileChange(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;
            if (imageFile != null)
            {
                var oldSrc = Src;
                Src = await cropperComponent!.GetImageUsingStreamingAsync(imageFile, imageFile.Size);
                IsErrorLoadImage = false;
                cropperComponent?.Destroy();
                cropperComponent?.RevokeObjectUrlAsync(oldSrc);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
