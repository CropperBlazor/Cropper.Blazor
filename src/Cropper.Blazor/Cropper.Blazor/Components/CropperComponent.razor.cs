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
    public partial class CropperComponent : ICropperComponentBase, IAsyncDisposable, IDisposable
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; } = null!;

        /// <summary>
        /// Gets a reference to the img HTML element rendered by the component.
        /// </summary>
        private ElementReference? ImageReference;

        /// <summary>
        /// Gets a reference to the canvas HTML element rendered by the component.
        /// </summary>
        private ElementReference? CanvasReference;

        /// <summary>
        /// The unique identifier of the cropper component.
        /// </summary>
        private Guid CropperComponentId;

        /// <summary>
        /// The options for cropping. Check out the available <see cref="Models.Options"/>.
        /// </summary>
        [Parameter]
        public Options Options { get; set; } = new Options();

        /// <summary>
        /// Specifies the path to the image.
        /// </summary>
        [Parameter]
        public string Src { get; set; } = null!;

        /// <summary>
        /// Specifies the target element for cropping, the default value is <see cref="CropperComponentType.Image"/>.
        /// In addition, for <see cref="CropperComponentType.Canvas"/> type requires manual uploading of images into canvas HTMl element, including error handling.
        /// </summary>
        [Parameter]
        public CropperComponentType CropperComponentType { get; set; } = CropperComponentType.Image;

        /// <summary>
        /// Specifies the path to the image when loading from src fails.
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
        /// Content is shown instead of the default error image.
        /// </summary>
        [Parameter]
        public RenderFragment? ErrorLoadImageContent { get; set; }

        /// <summary>
        /// Responsible for allowing the initialization of the cropper after a successful image download, the default is always allowed (true).
        /// In addition, it should be used to disable re-initialization (replace image) of cropper after successful image load when set to false.
        /// </summary>
        [Parameter]
        public bool IsAvailableInitCropper { get; set; } = true;

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
        public Action<JSEventData<CropReadyEvent>>? OnReadyEvent { get; set; }

        /// <summary>
        /// A shortcut to the crop event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropEvent>>? OnCropEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropstart event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropStartEvent>>? OnCropStartEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropend event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropEndEvent>>? OnCropEndEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropmove event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropMoveEvent>>? OnCropMoveEvent { get; set; }

        /// <summary>
        /// A shortcut to the zoom event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<ZoomEvent>>? OnZoomEvent { get; set; }

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
        /// Called when initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            CropperComponentId = Guid.NewGuid();
        }

        /// <summary>
        /// Returns the reference to the cropper element, which can be either a canvas or an image, depending on the <see cref="CropperComponentType"/>.
        /// If an error occurs while loading the image (when <see cref="IsErrorLoadImage"/> equal to true), null is returned.
        /// </summary>
        /// <returns>A <see cref="Nullable{ElementReference}"/> representing reference to the cropper element.</returns>
        public ElementReference? GetCropperElementReference()
        {
            if (IsErrorLoadImage)
            {
                return null;
            }

            if (CropperComponentType == CropperComponentType.Canvas)
            {
                return CanvasReference;
            }
            else
            {
                return ImageReference;
            }
        }

        /// <summary>
        /// This event is fired when an image is loaded or called manually.
        /// </summary>
        /// <param name="progressEventArgs">
        ///     <para>If successful, outputs a <see cref="ProgressEventArgs"/> which is </para>
        ///     <para>generated from the data.</para>
        /// </param>
        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            if (IsAvailableInitCropper)
            {
                InitCropper();
            }
            else
            {
                OnLoadImageEvent?.Invoke();
            }
        }

        /// <summary>
        /// This event occurs if an error occurred while loading the image.
        /// </summary>
        /// <param name="errorEventArgs">The <see cref="ErrorEventArgs"/> used to send if it's not in a good state.</param>
        public void OnErrorLoadImage(ErrorEventArgs errorEventArgs)
        {
            OnErrorLoadImageEvent?.Invoke(errorEventArgs);
        }

        /// <summary>
        /// Initializes cropper. 
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void InitCropper(CancellationToken cancellationToken = default)
        {
            ICropperComponentBase cropperComponentBase = this;
            ElementReference? cropperElementReference = GetCropperElementReference();

            if (cropperElementReference.HasValue)
            {
                CropperJsIntertop!.InitCropperAsync(
                    CropperComponentId,
                    cropperElementReference.Value,
                    Options!,
                    DotNetObjectReference.Create(cropperComponentBase),
                    cancellationToken);

                OnLoadImageEvent?.Invoke();
            }
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box changes.
        /// </summary>
        /// <param name="jSEventData">The <see cref="JSEventData{CropEvent}"/>.</param>
        [JSInvokable("CropperIsCroped")]
        public void CropperIsCroped(JSEventData<CropEvent> jSEventData)
        {
            OnCropEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box stops changing.
        /// </summary>
        /// <param name="jSEventData">The <see cref="JSEventData{CropEndEvent}"/>.</param>
        [JSInvokable("CropperIsEnded")]
        public void CropperIsEnded(JSEventData<CropEndEvent> jSEventData)
        {
            OnCropEndEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box is changing.
        /// </summary>
        /// <param name="jSEventData">The <see cref="JSEventData{CropMoveEvent}"/>.</param>
        [JSInvokable("CropperIsMoved")]
        public void CropperIsMoved(JSEventData<CropMoveEvent> jSEventData)
        {
            OnCropMoveEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box starts to change.
        /// </summary>
        /// <param name="jSEventData">The <see cref="JSEventData{CropStartEvent}"/>.</param>
        [JSInvokable("CropperIsStarted")]
        public void CropperIsStarted(JSEventData<CropStartEvent> jSEventData)
        {
            OnCropStartEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when a cropper instance starts to zoom in or zoom out its canvas (image wrapper).
        /// </summary>
        /// <param name="jSEventData">The <see cref="JSEventData{ZoomEvent}"/>.</param>
        [JSInvokable("CropperIsZoomed")]
        public void CropperIsZoomed(JSEventData<ZoomEvent> jSEventData)
        {
            OnZoomEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the target image has been loaded and the cropper instance is ready for operating.
        /// </summary>
        /// <param name="jSEventData">The <see cref="JSEventData{CropReadyEvent}"/>.</param>
        [JSInvokable]
        public void IsReady(JSEventData<CropReadyEvent> jSEventData)
        {
            OnReadyEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="dragMode">The <see cref="DragMode"/> used to set new drag mode.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetDragMode(DragMode dragMode = DragMode.None, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetDragModeAsync(CropperComponentId, dragMode, cancellationToken);
        }

        /// <summary>
        /// Zoom the canvas (image wrapper) with a relative ratio.
        /// </summary>
        /// <param name="ratio">
        /// Zoom in: requires a positive number (ratio &gt; 0).
        /// <br/>
        /// Zoom out: requires a negative number (ratio &lt; 0).
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Zoom(decimal ratio, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ZoomAsync(CropperComponentId, ratio, cancellationToken);
        }

        /// <summary>
        /// Zoom the canvas (image wrapper) to an absolute ratio.
        /// </summary>
        /// <param name="ratio">Requires a positive number (ratio > 0)</param>
        /// <param name="pivotX">The X coordinate of the center point for zooming, base on the top left corner of the cropper container.</param>
        /// <param name="pivotY">The Y coordinate of the center point for zooming, base on the top left corner of the cropper container.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ZoomToAsync(CropperComponentId, ratio, pivotX, pivotY, cancellationToken);
        }

        /// <summary>
        /// Move the canvas (image wrapper) with relative offsets.
        /// </summary>
        /// <param name="offsetX">Moving size (px) in the horizontal direction.</param>
        /// <param name="offsetY">Moving size (px) in the vertical direction. If not present, its default value is offsetX.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Move(decimal offsetX, decimal? offsetY, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.MoveAsync(CropperComponentId, offsetX, offsetY, cancellationToken);
        }

        /// <summary>
        /// Move the canvas (image wrapper) to an absolute point.
        /// </summary>
        /// <param name="x">The left value of the canvas</param>
        /// <param name="y">The top value of the canvas. If not present, its default value is x.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void MoveTo(decimal x, decimal? y, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.MoveToAsync(CropperComponentId, x, y, cancellationToken);
        }

        /// <summary>
        /// Rotate the image to a relative degree.
        /// </summary>
        /// <param name="degree"> 
        /// Rotate right: requires a positive number (degree &gt; 0).
        /// <br/>
        /// Rotate left: requires a negative number (degree &lt; 0).
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Rotate(decimal degree, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.RotateAsync(CropperComponentId, degree, cancellationToken);
        }

        /// <summary>
        /// Scale the abscissa of the image.
        /// </summary>
        /// <param name="scaleX"> 
        /// The scaling factor applies to the abscissa of the image.
        /// <br/>
        /// When equal to 1 (default value) it does nothing.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ScaleX(decimal scaleX, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ScaleXAsync(CropperComponentId, scaleX, cancellationToken);
        }

        /// <summary>
        /// Scale the ordinate of the image.
        /// </summary>
        /// <param name="scaleY">
        /// The scaling factor to apply on the ordinate of the image.
        /// <br/>
        /// When equal to 1 (default value) it does nothing.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ScaleY(decimal scaleY, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ScaleYAsync(CropperComponentId, scaleY, cancellationToken);
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
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Scale(decimal scaleX, decimal scaleY, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ScaleAsync(CropperComponentId, scaleX, scaleY, cancellationToken);
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Crop(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.CropAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Clear(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ClearAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Enable(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.EnableAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Disable(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.DisableAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Reset the image and crop box to its initial states.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Reset(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ResetAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Destroy(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.DestroyAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="aspectRatio">Requires a positive number.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetAspectRatio(decimal aspectRatio, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetAspectRatioAsync(CropperComponentId, aspectRatio, cancellationToken);
        }

        /// <summary>
        /// Change the crop box position and size with new data.
        /// </summary>
        /// <param name="cropBoxDataOptions">The <see cref="SetCropBoxDataOptions"/> used to set new crop box data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetCropBoxDataAsync(CropperComponentId, cropBoxDataOptions, cancellationToken);
        }

        /// <summary>
        /// Change the cropped area position and size with new data (based on the original image).
        /// </summary>
        /// <param name="setDataOptions">The <see cref="SetDataOptions"/> used to set new data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetData(SetDataOptions setDataOptions, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetDataAsync(CropperComponentId, setDataOptions, cancellationToken);
        }

        /// <summary>
        /// Change the canvas (image wrapper) position and size with new data.
        /// </summary>
        /// <param name="setCanvasDataOptions">The <see cref="SetCanvasDataOptions"/> used to set new canvas data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetCanvasDataAsync(CropperComponentId, setCanvasDataOptions, cancellationToken);
        }

        /// <summary>
        /// Output the crop box position and size data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropBoxData}"/> representing cropper box options asynchronous operation.</returns>
        public async ValueTask<CropBoxData> GetCropBoxDataAsync(CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetCropBoxDataAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Output the final cropped area position and size data (based on the natural size of the original image).
        /// </summary>
        /// <param name="rounded">Indicate if round the data values or not.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropperData}"/> representing cropper options asynchronous operation.</returns>
        public async ValueTask<CropperData> GetDataAsync(bool rounded, CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetDataAsync(CropperComponentId, rounded, cancellationToken);
        }

        /// <summary>
        /// Output the final cropped area position and size data (based on the natural size of the original image).
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ContainerData}"/> representing container options asynchronous operation.</returns>
        public async ValueTask<ContainerData> GetContainerDataAsync(CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetContainerDataAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Replace the image's src and rebuild the cropper.
        /// </summary>
        /// <param name="url">The new URL.</param>
        /// <param name="hasSameSize">If the new image has the same size as the old one, then it will not rebuild the cropper and only update the URLs of all related images. This can be used for applying filters.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ReplaceAsync(
            string url,
            bool hasSameSize = true,
            CancellationToken cancellationToken = default)
        {
            Src = url;
            await CropperJsIntertop!.ReplaceAsync(CropperComponentId, url, hasSameSize, cancellationToken);
        }

        /// <summary>
        /// Output the image position, size and other related data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ImageData}"/> representing image options asynchronous operation.</returns>
        public async ValueTask<ImageData> GetImageDataAsync(CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetImageDataAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Output the canvas (image wrapper) position and size data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CanvasData}"/> representing canvas options asynchronous operation.</returns>
        public async ValueTask<CanvasData> GetCanvasDataAsync(CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetCanvasDataAsync(CropperComponentId, cancellationToken);
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
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RevokeObjectUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            await CropperJsIntertop!.RevokeObjectUrlAsync(url, cancellationToken);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a cropped canvas.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing canvas drawn the cropped image asynchronous operation.</returns>
        public async ValueTask<CroppedCanvas> GetCroppedCanvasAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetCroppedCanvasAsync(CropperComponentId, getCroppedCanvasOptions, cancellationToken);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a cropped canvas.</param>
        /// <param name="type">A string indicating the image format. The default type is image/png; this image format will be also used if the specified type is not supported.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality to be used when creating images using file formats that support lossy compression (such as image/jpeg or image/webp).
        /// Different browsers have different image encoder compression, usually it is 92 or 80 percent of the full image quality. The default value is 1 with maximum image quality.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="number"/> is outside the range of 0 and 1.</exception>
        /// <returns>A <see cref="ValueTask{String}"/> representing canvas drawn the cropped image in URL format asynchronous operation.</returns>
        [Obsolete("This method blocks the UI thread. Use GetCroppedCanvasDataBackgroundAsync instead for background operation.")]
        public async ValueTask<string> GetCroppedCanvasDataURLAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            string type = "image/png",
            float number = 1,
            CancellationToken cancellationToken = default)
        {
            return number switch
            {
                < 0 or > 1 => throw new ArgumentException($"The given number should be between 0 and 1 for indication the image quality, but found {number}.", nameof(number)),
                _ => await CropperJsIntertop!.GetCroppedCanvasDataURLAsync(
                    CropperComponentId,
                    getCroppedCanvasOptions,
                    type,
                    number,
                    cancellationToken)
            };
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression) in background.
        /// If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a cropped canvas.</param>
        /// <param name="type">A string indicating the image format. The default type is image/png; this image format will be also used if the specified type is not supported.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality to be used when creating images using file formats that support lossy compression (such as image/jpeg or image/webp).
        /// Different browsers have different image encoder compression, usually it is 92 or 80 percent of the full image quality. The default value is 1 with maximum image quality.
        /// </param>
        /// <param name="maximumReceiveChunkSize">
        /// The maximum size of each image chunk to receive, in bytes. For example, 65536 equals 64 KB.
        /// If specified, incoming image data will be split into chunks of this size during transmission.
        /// If null, the chunk size will be handled automatically based on the stream's native chunking behavior.
        /// This helps control memory usage and ensures compatibility with interop limits.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="number"/> is outside the range of 0 and 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maximumReceiveChunkSize"/> is less than or equal to 0.</exception>
        /// <returns>A <see cref="ValueTask{ImageReceiver}"/> representing the asynchronous operation to retrieve cropped canvas data.</returns>
        public async ValueTask<ImageReceiver> GetCroppedCanvasDataBackgroundAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            string type = "image/png",
            float number = 1,
            int? maximumReceiveChunkSize = null,
            CancellationToken cancellationToken = default)
        {
            if (number < 0 || number > 1)
            {
                throw new ArgumentException($"The given number should be between 0 and 1 for indicating the image quality, but found {number}.", nameof(number));
            }

            if (maximumReceiveChunkSize is not null && maximumReceiveChunkSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumReceiveChunkSize), "Chunk size must be greater than 0 bytes when specified.");
            }

            ImageReceiver imageReceiver = new();

            await CropperJsIntertop.GetCroppedCanvasDataBackgroundAsync(
                CropperComponentId,
                getCroppedCanvasOptions,
                DotNetObjectReference.Create(imageReceiver),
                type,
                number,
                maximumReceiveChunkSize,
                cancellationToken);

            return imageReceiver;
        }

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            Destroy();
            await CropperJsIntertop!.DisposeAsync();
        }

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        public void Dispose()
        {
            DisposeAsync();
        }
    }
}
