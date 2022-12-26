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

        /// <summary>
        /// The options for cropping. Check out the available <see cref="Models.Options"/>.
        /// </summary>
        [Parameter]
        public Options Options { get; set; } = null!;

        /// <summary>
        /// Specifies the path to the image.
        /// </summary>
        [Parameter]
        public string Src { get; set; } = null!;

        /// <summary>
        /// Specifies the path to the image when loading from <see cref="Src"/> fails.
        /// </summary>
        [Parameter]
        public string ErrorLoadImageSrc { get; set; } = null!;

        /// <summary>
        /// User class names for error image, separated by space.
        /// </summary>
        [Parameter]
        public string ErrorLoadImageClass { get; set; } = null!;

        /// <summary>
        /// Returns the state of image loading.
        /// </summary>
        [Parameter]
        public bool IsErrorLoadImage { get; set; }

        /// <summary>
        /// User class names, separated by space.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = null!;

        /// <summary>
        /// A shortcut to the load image event.
        /// </summary>
        [Parameter]
        public Action? OnLoadImageEvent { get; set; }

        /// <summary>
        /// A shortcut to the ready event.
        /// </summary>
        [Parameter]
        public Action<CropReadyEvent>? OnReadyEvent { get; set; }

        /// <summary>
        /// A shortcut to the crop event.
        /// </summary>
        [Parameter]
        public Action<CropEvent>? OnCropEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropstart event.
        /// </summary>
        [Parameter]
        public Action<CropStartEvent>? OnCropStartEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropend event.
        /// </summary>
        [Parameter]
        public Action<CropEndEvent>? OnCropEndEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropmove event.
        /// </summary>
        [Parameter]
        public Action<CropMoveEvent>? OnCropMoveEvent { get; set; }

        /// <summary>
        /// A shortcut to the zoom event.
        /// </summary>
        [Parameter]
        public Action<ZoomEvent>? OnZoomEvent { get; set; }

        /// <summary>
        /// A shortcut to the image loading error event.
        /// </summary>
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

        /// <summary>
        ///  This event is fired when the image is loaded.
        /// </summary>
        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            InitCropper();
        }

        /// <summary>
        /// This event occurs if an error occurred while loading the image.
        /// </summary>
        public void OnErrorLoadImage(ErrorEventArgs errorEventArgs)
        {
            OnErrorLoadImageEvent?.Invoke(errorEventArgs);
        }

        /// <summary>
        /// Initializes cropper. 
        /// </summary>
        public void InitCropper()
        {
            ICropperComponentBase cropperComponentBase = this;
            CropperJsIntertop!.InitCropperAsync(imageReference, Options!, DotNetObjectReference.Create(cropperComponentBase)).AsTask();
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

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="dragMode"></param>
        public void SetDragMode(DragMode dragMode)
        {
            CropperJsIntertop?.SetDragModeAsync(dragMode).AsTask();
        }

        /// <summary>
        /// Zoom the canvas (image wrapper) with a relative ratio.
        /// </summary>
        /// <param name="ratio">
        /// Zoom in: requires a positive number (ratio &gt; 0).
        /// <br/>
        /// Zoom out: requires a negative number (ratio &lt; 0).
        /// </param>
        public void Zoom(decimal ratio)
        {
            CropperJsIntertop?.ZoomAsync(ratio).AsTask();
        }

        /// <summary>
        /// Zoom the canvas (image wrapper) to an absolute ratio.
        /// </summary>
        /// <param name="ratio">Requires a positive number (ratio > 0)</param>
        /// <param name="pivotX">The X coordinate of the center point for zooming, base on the top left corner of the cropper container.</param>
        /// <param name="pivotY">The Y coordinate of the center point for zooming, base on the top left corner of the cropper container.</param>
        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperJsIntertop?.ZoomToAsync(ratio, pivotX, pivotY).AsTask();
        }

        /// <summary>
        /// Move the canvas (image wrapper) with relative offsets.
        /// </summary>
        /// <param name="offsetX">Moving size (px) in the horizontal direction.</param>
        /// <param name="offsetY">Moving size (px) in the vertical direction. If not present, its default value is offsetX.</param>
        public void Move(decimal offsetX, decimal? offsetY)
        {
            CropperJsIntertop?.MoveAsync(offsetX, offsetY).AsTask();
        }

        /// <summary>
        /// Move the canvas (image wrapper) to an absolute point.
        /// </summary>
        /// <param name="x">The left value of the canvas</param>
        /// <param name="y">The top value of the canvas. If not present, its default value is x.</param>
        public void MoveTo(decimal x, decimal? y)
        {
            CropperJsIntertop?.MoveToAsync(x, y).AsTask();
        }

        /// <summary>
        /// Rotate the image to a relative degree.
        /// </summary>
        /// <param name="degree"> 
        /// Rotate right: requires a positive number (degree &gt; 0).
        /// <br/>
        /// Rotate left: requires a negative number (degree &lt; 0).
        /// </param>
        public void Rotate(decimal degree)
        {
            CropperJsIntertop?.RotateAsync(degree).AsTask();
        }

        /// <summary>
        /// Scale the abscissa of the image.
        /// </summary>
        /// <param name="scaleX"> 
        /// The scaling factor applies to the abscissa of the image.
        /// <br/>
        /// When equal to 1 (default value) it does nothing.
        /// </param>
        public void ScaleX(decimal scaleX)
        {
            CropperJsIntertop?.ScaleXAsync(scaleX).AsTask();
        }

        /// <summary>
        /// Scale the ordinate of the image.
        /// </summary>
        /// <param name="scaleY">
        /// The scaling factor to apply on the ordinate of the image.
        /// <br/>
        /// When equal to 1 (default value) it does nothing.
        /// </param>
        public void ScaleY(decimal scaleY)
        {
            CropperJsIntertop?.ScaleYAsync(scaleY).AsTask();
        }

        /// <summary>
        /// Scale the image.
        /// </summary>
        /// <param name="scaleX"> 
        /// The scaling factor applies to the abscissa of the image.
        /// <br/>
        /// When equal to 1 (default value) it does nothing.
        /// </param>
        /// <param name="scaleY">
        /// The scaling factor to apply on the ordinate of the image.
        /// <br/>
        /// If not present, its default value is <paramref name="scaleX"/>.
        /// </param>
        public void Scale(decimal scaleX, decimal scaleY)
        {
            CropperJsIntertop?.ScaleAsync(scaleX, scaleY).AsTask();
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        public void Crop()
        {
            CropperJsIntertop?.CropAsync().AsTask();
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        public void Clear()
        {
            CropperJsIntertop?.ClearAsync().AsTask();
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        public void Enable()
        {
            CropperJsIntertop?.EnableAsync().AsTask();
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        public void Disable()
        {
            CropperJsIntertop?.DisableAsync().AsTask();
        }

        /// <summary>
        /// Reset the image and crop box to its initial states.
        /// </summary>
        public void Reset()
        {
            CropperJsIntertop?.ResetAsync().AsTask();
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        public void Destroy()
        {
            CropperJsIntertop?.DestroyAsync().AsTask();
        }

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="aspectRatio">Requires a positive number.</param>
        public void SetAspectRatio(decimal aspectRatio)
        {
            CropperJsIntertop?.SetAspectRatioAsync(aspectRatio).AsTask();
        }

        /// <summary>
        /// Change the crop box position and size with new data.
        /// </summary>
        /// <param name="cropBoxDataOptions"></param>
        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperJsIntertop?.SetCropBoxDataAsync(cropBoxDataOptions).AsTask();
        }

        /// <summary>
        /// Change the cropped area position and size with new data (based on the original image).
        /// </summary>
        /// <param name="setDataOptions"></param>
        public void SetData(SetDataOptions setDataOptions)
        {
            CropperJsIntertop?.SetDataAsync(setDataOptions).AsTask();
        }

        /// <summary>
        /// Change the canvas (image wrapper) position and size with new data.
        /// </summary>
        /// <param name="setCanvasDataOptions"></param>
        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperJsIntertop?.SetCanvasDataAsync(setCanvasDataOptions).AsTask();
        }

        /// <summary>
        /// Output the crop box position and size data.
        /// </summary>
        /// <returns>Cropper box options.</returns>
        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await CropperJsIntertop!.GetCropBoxDataAsync().AsTask();
        }

        /// <summary>
        /// Output the final cropped area position and size data (based on the natural size of the original image).
        /// </summary>
        /// <returns>Cropper options.</returns>
        public async ValueTask<CropperData> GetDataAsync(bool isRounded)
        {
            return await CropperJsIntertop!.GetDataAsync(isRounded);
        }

        /// <summary>
        /// Output the final cropped area position and size data (based on the natural size of the original image).
        /// </summary>
        /// <returns>Cropper options.</returns>
        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await CropperJsIntertop!.GetContainerDataAsync();
        }

        /// <summary>
        /// Output the image position, size, and other related data.
        /// </summary>
        /// <returns>Image options.</returns>
        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await CropperJsIntertop!.GetImageDataAsync();
        }

        /// <summary>
        /// Output the canvas (image wrapper) position and size data.
        /// </summary>
        /// <returns>Canvas options.</returns>
        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await CropperJsIntertop!.GetCanvasDataAsync();
        }

        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
        }

        public async ValueTask RevokeObjectUrlAsync(string url)
        {
            await CropperJsIntertop!.RevokeObjectUrlAsync(url);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">Options for getting cropped canvas.</param>
        /// <returns>A canvas drawn the cropped image.</returns>
        public async ValueTask<object> GetCroppedCanvasAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">Options for getting cropped canvas.</param>
        /// <returns>A canvas drawn the cropped image in URL format.</returns>
        public async ValueTask<string> GetCroppedCanvasDataURLAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);
        }
    }
}
