using System.Reflection;
using System.Text.Json;
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
using MudBlazor.Services;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.Client.Pages
{
    public partial class CropperDemo : IDisposable
    {
        [Inject] IBreakpointService BreakpointListener { get; set; } = null!;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        private CropperComponent? cropperComponent = null!;
        private CropperDataPreview? cropperDataPreview = null!;
        private Options options = null!;
        private decimal? scaleX;
        private decimal? scaleY;
        private decimal aspectRatio = 1.7777777777777777m;

        private string Src = "https://fengyuanchen.github.io/cropperjs/v2/picture.jpg";
        private bool IsErrorLoadImage { get; set; } = false;
        private readonly string _errorLoadImageSrc = "not-found-image.jpg";
        private Breakpoint _start;
        private Guid _subscriptionId;

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

        public async void OnCropEvent(CropJSEvent cropJSEvent)
        {
            if (cropJSEvent.EventData.Detail is not null)
            {
                scaleX = cropJSEvent.EventData.Detail.ScaleX;
                scaleY = cropJSEvent.EventData.Detail.ScaleY;

                await InvokeAsync(() =>
                {
                    JSRuntime!.InvokeVoidAsync("console.log", $"CropJSEvent {JsonSerializer.Serialize(cropJSEvent)}");
                    cropperDataPreview?.OnCropEvent(cropJSEvent.EventData.Detail);
                });
            }
        }

        public async void OnCropEndEvent(CropEndEvent cropEndEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndEvent, {cropEndEvent.ActionEvent}");
        }

        public async void OnCropStartEvent(CropStartJSEvent cropStartJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartEvent, {JsonSerializer.Serialize(cropStartJSEvent)}");
        }

        public async void OnZoomEvent(ZoomJSEvent zoomJSEvent)
        {
            if (zoomJSEvent.EventData.Detail is not null)
            {
                await JSRuntime!.InvokeVoidAsync("console.log", $"ZoomEvent {JsonSerializer.Serialize(zoomJSEvent)}");

                //if (zoomJSEvent.EventData.Detail.OriginalEvent is not null)
                //{
                //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
                //        "jsObject.getInstanceProperty",
                //        zoomJSEvent.EventData.Detail.OriginalEvent, "clientX");

                //    await JSRuntime!.InvokeVoidAsync("console.log", $"OriginalEvent clientX: {clientX}");
                //}

                //await zoomJSEvent.PreventDefaultAsync();
                //Console.WriteLine($"PreventDefaultAsync");
            }
        }

        public async void OnCropMoveEvent(CropMoveEvent cropMoveEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropMoveEvent, {cropMoveEvent.ActionEvent}");
        }

        public async void OnCropReadyEvent(CropReadyJSEvent cropReadyJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropReadyJSEvent {JsonSerializer.Serialize(cropReadyJSEvent)}");
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
            this.aspectRatio = aspectRatio;
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
            //CroppedCanvas croppedCanvas = await cropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            //string croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL");

            string croppedCanvasDataURL = await cropperComponent!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);
            DialogParameters parameters = new()
            {
                { "Src", croppedCanvasDataURL }
            };
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            _dialogService.Show<Shared.CroppedCanvasDialog>("CroppedCanvasDialog", parameters, options);
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
                var subscriptionResult = await BreakpointListener.Subscribe((breakpoint) =>
                {
                    InvokeAsync(StateHasChanged);
                });

                _start = subscriptionResult.Breakpoint;
                _subscriptionId = subscriptionResult.SubscriptionId;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
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

        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await cropperComponent!.GetCropBoxDataAsync();
        }

        public async ValueTask<CropperData> GetDataAsync(bool rounded)
        {
            return await cropperComponent!.GetDataAsync(rounded);
        }

        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await cropperComponent!.GetContainerDataAsync();
        }

        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await cropperComponent!.GetImageDataAsync();
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await cropperComponent!.GetCanvasDataAsync();
        }

        public void OptionsChecked(string property, bool? newValue)
        {
            Type type = options.GetType();
            PropertyInfo? propertyInfo = type!.GetProperty(property);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(options, newValue, null);
                cropperComponent?.Destroy();
                cropperComponent?.InitCropper();
            }
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
