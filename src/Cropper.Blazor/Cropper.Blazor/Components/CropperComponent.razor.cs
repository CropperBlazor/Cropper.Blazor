using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Events;
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
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// The cropper component.
    /// </summary>
    public partial class CropperComponent : ICropperComponentBase, IAsyncDisposable
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; } = null!;
        [Inject] IJSRuntime JSRuntime { get; set; } = null!;

        private ElementReference ImageReference;

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
        public Action<CropReadyJSEvent>? OnReadyEvent { get; set; }

        /// <summary>
        /// A shortcut to the crop event.
        /// </summary>
        [Parameter]
        public Action<CropJSEvent>? OnCropEvent { get; set; }

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

        /// <summary>
        /// Additional attributes can be captured in a dictionary and then splatted onto an element when the component is rendered using the @attributes Razor directive attribute.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = null!;

        /// <summary>
        /// Method invoked after each time the component has been rendered. Note that the component does
        /// not automatically re-render after the completion of any returned <see cref="Task"/>, because
        /// that would cause an infinite render loop.
        /// </summary>
        /// <param name="firstRender">
        /// Set to <c>true</c> if this is the first time <see cref="OnAfterRenderAsync(bool)"/> has been invoked
        /// on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <remarks>
        /// The <see cref="OnAfterRenderAsync(bool)"/> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender"/> parameter to ensure that initialization work is only performed
        /// once.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CropperJsIntertop!.LoadModuleAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// This event is fired when the image is loaded.
        /// </summary>
        /// <param name="progressEventArgs">
        ///     <para>If successful, outputs a <see cref="ProgressEventArgs"/> which is </para>
        ///     <para>generated from the data.</para>
        /// </param>
        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            InitCropper();
        }

        /// <summary>
        /// This event occurs if an error occurred while loading the image.
        /// </summary>
        /// <param name="errorEventArgs">The error event to send if it's not in a good state.</param>
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
            CropperJsIntertop!.InitCropperAsync(ImageReference, Options!, DotNetObjectReference.Create(cropperComponentBase));
            OnLoadImageEvent?.Invoke();
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box changes.
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        [JSInvokable("CropperIsCroped")]
        public async Task CropperIsCropedAsync(IJSObjectReference jSObjectReference)
        {
            if (OnCropEvent is not null)
            {
                CropJSEvent cropJSEvent = new CropJSEvent(JSRuntime, jSObjectReference);
                JSEventData<CropEvent> cropJSEventData = await cropJSEvent.GetCropJSEventDataAsync();
                cropJSEvent.EventData = cropJSEventData;

                OnCropEvent?.Invoke(cropJSEvent);
            }
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box stops changing.
        /// </summary>
        /// <param name="cropEndEvent">The <see cref="CropEndEvent"/>.</param>
        [JSInvokable]
        public void CropperIsEnded(IJSObjectReference cropEndEvent)
        {
            //OnCropEndEvent?.Invoke(null);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box is changing.
        /// </summary>
        /// <param name="cropMoveEvent">The <see cref="CropMoveEvent"/>.</param>
        [JSInvokable]
        public void CropperIsMoved(IJSObjectReference cropMoveEvent)
        {
            //OnCropMoveEvent?.Invoke(null);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box starts to change.
        /// </summary>
        /// <param name="cropStartEvent">The <see cref="CropStartEvent"/>.</param>
        [JSInvokable]
        public void CropperIsStarted(IJSObjectReference cropStartEvent)
        {
            //OnCropStartEvent?.Invoke(null);
        }

        /// <summary>
        /// This event fires when a cropper instance starts to zoom in or zoom out its canvas (image wrapper).
        /// </summary>
        /// <param name="zoomEvent">The <see cref="ZoomEvent"/>.</param>
        [JSInvokable]
        public void CropperIsZoomed(IJSObjectReference zoomEvent)
        {
            //OnZoomEvent?.Invoke(null);
        }

        /// <summary>
        /// This event fires when the target image has been loaded and the cropper instance is ready for operating.
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        [JSInvokable]
        public void IsReady(IJSObjectReference jSObjectReference)
        {
            if (OnReadyEvent is not null)
            {
                CropReadyJSEvent cropReadyJSEvent = new CropReadyJSEvent(JSRuntime, jSObjectReference);
                OnReadyEvent?.Invoke(cropReadyJSEvent);
            }
        }

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="dragMode">The new drag mode.</param>
        public void SetDragMode(DragMode dragMode)
        {
            CropperJsIntertop?.SetDragModeAsync(dragMode);
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
            CropperJsIntertop?.ZoomAsync(ratio);
        }

        /// <summary>
        /// Zoom the canvas (image wrapper) to an absolute ratio.
        /// </summary>
        /// <param name="ratio">Requires a positive number (ratio > 0)</param>
        /// <param name="pivotX">The X coordinate of the center point for zooming, base on the top left corner of the cropper container.</param>
        /// <param name="pivotY">The Y coordinate of the center point for zooming, base on the top left corner of the cropper container.</param>
        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperJsIntertop?.ZoomToAsync(ratio, pivotX, pivotY);
        }

        /// <summary>
        /// Move the canvas (image wrapper) with relative offsets.
        /// </summary>
        /// <param name="offsetX">Moving size (px) in the horizontal direction.</param>
        /// <param name="offsetY">Moving size (px) in the vertical direction. If not present, its default value is offsetX.</param>
        public void Move(decimal offsetX, decimal? offsetY)
        {
            CropperJsIntertop?.MoveAsync(offsetX, offsetY);
        }

        /// <summary>
        /// Move the canvas (image wrapper) to an absolute point.
        /// </summary>
        /// <param name="x">The left value of the canvas</param>
        /// <param name="y">The top value of the canvas. If not present, its default value is x.</param>
        public void MoveTo(decimal x, decimal? y)
        {
            CropperJsIntertop?.MoveToAsync(x, y);
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
            CropperJsIntertop?.RotateAsync(degree);
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
            CropperJsIntertop?.ScaleXAsync(scaleX);
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
            CropperJsIntertop?.ScaleYAsync(scaleY);
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
            CropperJsIntertop?.ScaleAsync(scaleX, scaleY);
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        public void Crop()
        {
            CropperJsIntertop?.CropAsync();
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        public void Clear()
        {
            CropperJsIntertop?.ClearAsync();
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        public void Enable()
        {
            CropperJsIntertop?.EnableAsync();
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        public void Disable()
        {
            CropperJsIntertop?.DisableAsync();
        }

        /// <summary>
        /// Reset the image and crop box to its initial states.
        /// </summary>
        public void Reset()
        {
            CropperJsIntertop?.ResetAsync();
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        public void Destroy()
        {
            CropperJsIntertop?.DestroyAsync();
        }

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="aspectRatio">Requires a positive number.</param>
        public void SetAspectRatio(decimal aspectRatio)
        {
            CropperJsIntertop?.SetAspectRatioAsync(aspectRatio);
        }

        /// <summary>
        /// Change the crop box position and size with new data.
        /// </summary>
        /// <param name="cropBoxDataOptions">The new crop box data.</param>
        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperJsIntertop?.SetCropBoxDataAsync(cropBoxDataOptions);
        }

        /// <summary>
        /// Change the cropped area position and size with new data (based on the original image).
        /// </summary>
        /// <param name="setDataOptions">The new data.</param>
        public void SetData(SetDataOptions setDataOptions)
        {
            CropperJsIntertop?.SetDataAsync(setDataOptions);
        }

        /// <summary>
        /// Change the canvas (image wrapper) position and size with new data.
        /// </summary>
        /// <param name="setCanvasDataOptions">The new canvas data.</param>
        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperJsIntertop?.SetCanvasDataAsync(setCanvasDataOptions);
        }

        /// <summary>
        /// Output the crop box position and size data.
        /// </summary>
        /// <returns>A <see cref="ValueTask{CropBoxData}"/> representing cropper box options asynchronous operation.</returns>
        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await CropperJsIntertop!.GetCropBoxDataAsync();
        }

        /// <summary>
        /// Output the final cropped area position and size data (based on the natural size of the original image).
        /// </summary>
        /// <param name="rounded">Indicate if round the data values or not.</param>
        /// <returns>A <see cref="ValueTask{CropperData}"/> representing cropper options asynchronous operation.</returns>
        public async ValueTask<CropperData> GetDataAsync(bool rounded)
        {
            return await CropperJsIntertop!.GetDataAsync(rounded);
        }

        /// <summary>
        /// Output the final cropped area position and size data (based on the natural size of the original image).
        /// </summary>
        /// <returns>A <see cref="ValueTask{ContainerData}"/> representing container options asynchronous operation.</returns>
        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await CropperJsIntertop!.GetContainerDataAsync();
        }

        /// <summary>
        /// Output the image position, size and other related data.
        /// </summary>
        /// <returns>A <see cref="ValueTask{ImageData}"/> representing image options asynchronous operation.</returns>
        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await CropperJsIntertop!.GetImageDataAsync();
        }

        /// <summary>
        /// Output the canvas (image wrapper) position and size data.
        /// </summary>
        /// <returns>A <see cref="ValueTask{CanvasData}"/> representing canvas options asynchronous operation.</returns>
        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await CropperJsIntertop!.GetCanvasDataAsync();
        }

        /// <summary>
        /// Used to get an image from a stream.
        /// <br/>
        /// Converts JavaScript stream to .NET stream uses <see cref="DotNetStreamReference"/> and then creates a URL blob reference.
        /// </summary>
        /// <param name="imageFile">The <see cref="IBrowserFile"/> to convert to a new image file.</param>
        /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream. Defaults to 500 KB.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing Blob URL asynchronous operation.</returns>
        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
        }

        /// <summary>
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RevokeObjectUrlAsync(string url)
        {
            await CropperJsIntertop!.RevokeObjectUrlAsync(url);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">Options for getting cropped canvas.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing canvas drawn the cropped image asynchronous operation.</returns>
        public async ValueTask<CroppedCanvas> GetCroppedCanvasAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">Options for getting cropped canvas.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing canvas drawn the cropped image in URL format asynchronous operation.</returns>
        public async ValueTask<string> GetCroppedCanvasDataURLAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            return await CropperJsIntertop!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);
        }

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await CropperJsIntertop!.DisposeAsync();
        }
    }
}
