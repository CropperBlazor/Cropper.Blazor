using Cropper.Blazor.Base;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent : ICropperComponentBase
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; }

        private ElementReference imageReference;

        [Parameter]
        public Options Options { get; set; }

        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public Action? OnLoadImageEvent { get; set; }

        [Parameter]
        public Action<CropReadyEvent>? OnReadyEvent { get; set; }

        [Parameter]
        public Action<CropEvent>? OnCropEvent { get; set; }

        [Parameter]
        public Action<CropStartEvent>? OnCropStartEvent { get; set; }

        [Parameter]
        public Action<CropEndEvent>? OnCropEndEvent { get; set; }

        [Parameter]
        public Action<CropMoveEvent>? OnCropMoveEvent { get; set; }

        [Parameter]
        public Action<ZoomEvent>? OnZoomEvent { get; set; }

        protected override void OnInitialized()
        {

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CropperJsIntertop.LoadAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            InitCropper();
        }

        public void InitCropper()
        {
            ICropperComponentBase cropperComponentBase = this;
            CropperJsIntertop.InitCropper(imageReference, Options, DotNetObjectReference.Create(cropperComponentBase));
            OnLoadImageEvent?.Invoke();
        }

        [JSInvokable]
        public void CropperIsCroped(CropEvent cropEvent)
        {
            OnCropEvent?.Invoke(cropEvent);
        }

        [JSInvokable]
        public void CropperIsEnded(CropEndEvent cropEndEvent)
        {
            OnCropEndEvent?.Invoke(cropEndEvent);
        }

        [JSInvokable]
        public void CropperIsMoved(CropMoveEvent cropMoveEvent)
        {
            OnCropMoveEvent?.Invoke(cropMoveEvent);
        }

        [JSInvokable]
        public void CropperIsStarted(CropStartEvent cropStartEvent)
        {
            OnCropStartEvent?.Invoke(cropStartEvent);
        }

        [JSInvokable]
        public void CropperIsZoomed(ZoomEvent zoomEvent)
        {
            OnZoomEvent?.Invoke(zoomEvent);
        }

        [JSInvokable]
        public void IsReady(CropReadyEvent cropReadyEvent)
        {
            OnReadyEvent?.Invoke(cropReadyEvent);
        }

        public void SetDragMode(DragMode dragMode)
        {
            CropperJsIntertop?.SetDragMode(dragMode);
        }

        public void Zoom(decimal ratio)
        {
            CropperJsIntertop?.Zoom(ratio);
        }

        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperJsIntertop?.ZoomTo(ratio, pivotX, pivotY);
        }

        public void Move(decimal offsetX, decimal? offsetY)
        {
            CropperJsIntertop?.Move(offsetX, offsetY);
        }

        public void MoveTo(decimal x, decimal? y)
        {
            CropperJsIntertop?.MoveTo(x, y);
        }

        public void Rotate(decimal degree)
        {
            CropperJsIntertop?.Rotate(degree);
        }

        public void ScaleX(decimal scaleX)
        {
            CropperJsIntertop?.ScaleX(scaleX);
        }

        public void ScaleY(decimal scaleY)
        {
            CropperJsIntertop?.ScaleY(scaleY);
        }

        public void Scale(decimal scaleX, decimal scaleY)
        {
            CropperJsIntertop?.Scale(scaleX, scaleY);
        }

        public void Crop()
        {
            CropperJsIntertop?.Crop();
        }

        public void Clear()
        {
            CropperJsIntertop?.Clear();
        }

        public void Enable()
        {
            CropperJsIntertop?.Enable();
        }

        public void Disable()
        {
            CropperJsIntertop?.Disable();
        }

        public void Reset()
        {
            CropperJsIntertop?.Reset();
        }

        public void Destroy()
        {
            CropperJsIntertop?.Destroy();
        }

        public void SetAspectRatio(decimal aspectRatio)
        {
            CropperJsIntertop?.SetAspectRatio(aspectRatio);
        }

        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperJsIntertop?.SetCropBoxData(cropBoxDataOptions);
        }

        public void SetData(SetDataOptions setDataOptions)
        {
            CropperJsIntertop?.SetData(setDataOptions);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperJsIntertop?.SetCanvasData(setCanvasDataOptions);
        }

        public async ValueTask<CropBoxData> GetCropBoxData()
        {
            return await CropperJsIntertop.GetCropBoxData();
        }

        public async ValueTask<CropperData> GetData(bool rounded)
        {
            return await CropperJsIntertop.GetData(rounded);
        }

        public async ValueTask<ContainerData> GetContainerData()
        {
            return await CropperJsIntertop.GetContainerData();
        }

        public async ValueTask<ImageData> GetImageData()
        {
            return await CropperJsIntertop.GetImageData();
        }

        public async ValueTask<CanvasData> GetCanvasData()
        {
            return await CropperJsIntertop.GetCanvasData();
        }

        public async ValueTask<string> GetImageUsingStreaming(IBrowserFile imageFile, long maxAllowedSize = 512000L, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CropperJsIntertop.GetImageUsingStreaming(imageFile, maxAllowedSize, cancellationToken);
        }

        public async ValueTask RevokeObjectUrl(string url)
        {
            await CropperJsIntertop.RevokeObjectUrl(url);
        }

        public async ValueTask<object> GetCroppedCanvas(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop.GetCroppedCanvas(getCroppedCanvasOptions);
        }

        public async ValueTask<string> GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop.GetCroppedCanvasDataURL(getCroppedCanvasOptions);
        }
    }
}
