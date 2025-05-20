using Cropper.Blazor.Components;
using Cropper.Blazor.Events;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Exceptions;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.WebView.Net6
{
    public partial class Component : IDisposable
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private CropperComponent? cropperComponent = null!;
        private Options options = null!;
        private decimal? scaleX;
        private decimal? scaleY;
        private decimal aspectRatio = 1.7777777777777777m;

        private string Src = "https://fengyuanchen.github.io/cropperjs/images/picture.jpg";
        private bool IsErrorLoadImage { get; set; } = false;
        private readonly string _errorLoadImageSrc = "not-found-image.jpg";
        public Dictionary<string, object> InputAttributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "loading", "lazy" },
                { "test-Attribute", "123-test" }
            };
        protected override void OnInitialized()
        {
            options = new Options()
            {
                Preview = ".img-preview",
                AspectRatio = (decimal)16 / 9,
                ViewMode = ViewMode.Vm0
            };
        }

        public void OnCropEvent(JSEventData<CropEvent> cropEvent)
        {
            scaleX = cropEvent.Detail?.ScaleX;
            scaleY = cropEvent.Detail?.ScaleY;
        }

        public async void OnCropEndEvent(JSEventData<CropEndEvent> cropEndEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndEvent, {cropEndEvent.Detail?.ActionEvent}");
        }

        public async void OnCropStartEvent(JSEventData<CropStartEvent> cropStartEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartEvent, {cropStartEvent.Detail?.ActionEvent}");
        }

        public async void OnZoomEvent(JSEventData<ZoomEvent> zoomEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"ZoomEvent, OldRatio: {zoomEvent.Detail?.OldRatio}, Ratio: {zoomEvent.Detail?.Ratio}");
        }

        public async void OnCropMoveEvent(JSEventData<CropMoveEvent> cropMoveEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropMoveEvent, {cropMoveEvent.Detail?.ActionEvent}");
        }

        public async void OnCropReadyEvent(JSEventData<CropReadyEvent> cropReadyEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Cropper Is Ready");
        }

        public async void OnLoadImageEvent()
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Image Is loaded");
        }

        public void OnErrorLoadImageEvent(ErrorEventArgs errorEventArgs)
        {
            IsErrorLoadImage = true;
            Destroy();
            StateHasChanged();
        }

        public async void GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            ImageReceiver imageReceiver = await cropperComponent!.GetCroppedCanvasDataInBackgroundAsync(
                            getCroppedCanvasOptions);

            InvokeAsync(async () =>
            {
                try
                {
                    using MemoryStream croppedCanvasDataStream = await imageReceiver.GetImageChunkStreamAsync();
                    byte[] croppedCanvasData = croppedCanvasDataStream.ToArray();

                    string croppedCanvasDataURL = "data:image/png;base64," + Convert.ToBase64String(croppedCanvasData);

                    DialogParameters parameters = new()
                    {
                        { "Src", croppedCanvasDataURL }
                    };
                    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
                    _dialogService!.Show<Shared.CroppedCanvasDialog>("CroppedCanvasDialog", parameters, options);
                }
                catch (ImageProcessingException ex)
                {
                    JSRuntime.InvokeVoidAsync("console.log", ex.ToString());
                }
            });
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
                cropperComponent?.RevokeObjectUrlAsync(oldSrc).AsTask();
            }
        }

        private void Destroy()
        {
            cropperComponent?.Destroy();
            cropperComponent?.RevokeObjectUrlAsync(Src).AsTask();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Destroy();
                cropperComponent?.DisposeAsync();
                JSRuntime!.InvokeVoidAsync("console.log", "Cropper Demo component is destroyed");
            }
        }
    }
}
