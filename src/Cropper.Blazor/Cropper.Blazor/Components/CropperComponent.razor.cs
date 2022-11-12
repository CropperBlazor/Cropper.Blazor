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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent : ICropperComponentBase
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; } = null!;

        private ElementReference imageReference;

        [Parameter]
        public Options Options { get; set; } = null!;

        [Parameter]
        public string Src { get; set; } = null!;

        [Parameter]
        public string ErrorLoadImageSrc { get; set; } = null!;

        [Parameter]
        public string ErrorLoadImageClass { get; set; } = null!;

        [Parameter]
        public bool IsErrorLoadImage { get; set; }

        [Parameter]
        public string Class { get; set; } = null!;

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

        [Parameter]
        public Action<ErrorEventArgs>? OnErrorLoadImageEvent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CropperJsIntertop!.LoadModuleAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            InitCropper();
        }

        public void OnErrorLoadImage(ErrorEventArgs errorEventArgs)
        {
            OnErrorLoadImageEvent?.Invoke(errorEventArgs);
        }

        public void InitCropper()
        {
            ICropperComponentBase cropperComponentBase = this;
            CropperJsIntertop!.InitCropperAsync(imageReference, Options!, DotNetObjectReference.Create(cropperComponentBase));
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
            CropperJsIntertop?.SetDragModeAsync(dragMode);
        }

        public void Zoom(decimal ratio)
        {
            CropperJsIntertop?.ZoomAsync(ratio);
        }

        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperJsIntertop?.ZoomToAsync(ratio, pivotX, pivotY);
        }

        public void Move(decimal offsetX, decimal? offsetY)
        {
            CropperJsIntertop?.MoveAsync(offsetX, offsetY);
        }

        public void MoveTo(decimal x, decimal? y)
        {
            CropperJsIntertop?.MoveToAsync(x, y);
        }

        public void Rotate(decimal degree)
        {
            CropperJsIntertop?.RotateAsync(degree);
        }

        public void ScaleX(decimal scaleX)
        {
            CropperJsIntertop?.ScaleXAsync(scaleX);
        }

        public void ScaleY(decimal scaleY)
        {
            CropperJsIntertop?.ScaleYAsync(scaleY);
        }

        public void Scale(decimal scaleX, decimal scaleY)
        {
            CropperJsIntertop?.ScaleAsync(scaleX, scaleY);
        }

        public void Crop()
        {
            CropperJsIntertop?.CropAsync();
        }

        public void Clear()
        {
            CropperJsIntertop?.ClearAsync();
        }

        public void Enable()
        {
            CropperJsIntertop?.EnableAsync();
        }

        public void Disable()
        {
            CropperJsIntertop?.DisableAsync();
        }

        public void Reset()
        {
            CropperJsIntertop?.ResetAsync();
        }

        public void Destroy()
        {
            CropperJsIntertop?.DestroyAsync();
        }

        public void SetAspectRatio(decimal aspectRatio)
        {
            CropperJsIntertop?.SetAspectRatioAsync(aspectRatio);
        }

        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperJsIntertop?.SetCropBoxDataAsync(cropBoxDataOptions);
        }

        public void SetData(SetDataOptions setDataOptions)
        {
            CropperJsIntertop?.SetDataAsync(setDataOptions);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperJsIntertop?.SetCanvasDataAsync(setCanvasDataOptions);
        }

        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await CropperJsIntertop!.GetCropBoxDataAsync();
        }

        public async ValueTask<CropperData> GetDataAsync(bool isRounded)
        {
            return await CropperJsIntertop!.GetDataAsync(isRounded);
        }

        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await CropperJsIntertop!.GetContainerDataAsync();
        }

        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await CropperJsIntertop!.GetImageDataAsync();
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await CropperJsIntertop!.GetCanvasDataAsync();
        }

        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CropperJsIntertop!.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
        }

        public async ValueTask RevokeObjectUrlAsync(string url)
        {
            await CropperJsIntertop!.RevokeObjectUrlAsync(url);
        }

        public async ValueTask<object> GetCroppedCanvasAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
        }

        public async ValueTask<string> GetCroppedCanvasDataURLAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);
        }
    }
}
