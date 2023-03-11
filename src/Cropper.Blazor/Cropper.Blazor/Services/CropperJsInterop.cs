using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// This service listens to cropper js events and allows you to manage calls from cropper.
    /// </summary>
    public class CropperJsInterop : ICropperJsInterop, IAsyncDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;
        private IJSObjectReference? Module = null;

        /// <summary>
        /// Path to cropper js interop module.
        /// </summary>
        public const string PathToCropperModule = "_content/Cropper.Blazor/cropperJsInterop.min.js";

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
        public CropperJsInterop(IJSRuntime jsRuntime, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Load JavaScript object into .NET.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        public async Task LoadModuleAsync(CancellationToken cancellationToken = default)
        {
            Uri baseUri = new Uri(_navigationManager.BaseUri);
            string pathAndQuery = baseUri.PathAndQuery;
            string hostName = baseUri.ToString().Replace(pathAndQuery, string.Empty);
            string globalPathToCropperModule = Path.Combine(hostName, PathToCropperModule);

            Module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", cancellationToken, globalPathToCropperModule);
        }

        /// <summary>
        /// Initializes cropper. 
        /// </summary>
        /// <param name="image">Reference to img html-DOM</param>
        /// <param name="options">Cropper options</param>
        /// <param name="cropperComponentBase">Reference to base cropper component. Default equal to 'this' object.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask InitCropperAsync(
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.initCropper",
                cancellationToken,
                image,
                options,
                cropperComponentBase);
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeAsync<object>("cropper.clear", cancellationToken);
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask CropAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.crop", cancellationToken);
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DestroyAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.destroy", cancellationToken);
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisableAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.disable", cancellationToken);
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask EnableAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.enable", cancellationToken);
        }

        /// <summary>
        /// Get the canvas position and size data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CanvasData}"/> representing result canvas data asynchronous operation.</returns>
        public async ValueTask<CanvasData> GetCanvasDataAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<CanvasData>("cropper.getCanvasData", cancellationToken);
        }

        /// <summary>
        /// Get the container size data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ContainerData}"/> representing result container data asynchronous operation.</returns>
        public async ValueTask<ContainerData> GetContainerDataAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<ContainerData>("cropper.getContainerData", cancellationToken);
        }

        /// <summary>
        /// Get the crop box position and size data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropBoxData}"/> representing result crop box data asynchronous operation.</returns>
        public async ValueTask<CropBoxData> GetCropBoxDataAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<CropBoxData>("cropper.getCropBoxData", cancellationToken);
        }

        /// <summary>
        /// Get a canvas drawn the cropped image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing result canvas asynchronous operation.</returns>
        public async ValueTask<CroppedCanvas> GetCroppedCanvasAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            IJSObjectReference jSCanvas = await _jsRuntime!.InvokeAsync<IJSObjectReference>("cropper.getCroppedCanvas", cancellationToken, getCroppedCanvasOptions);

            return new CroppedCanvas(jSCanvas);
        }

        /// <summary>
        /// Get a canvas drawn the cropped image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL result canvas asynchronous operation.</returns>
        public async ValueTask<string> GetCroppedCanvasDataURLAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<string>("cropper.getCroppedCanvasDataURL", cancellationToken, getCroppedCanvasOptions);
        }

        /// <summary>
        /// Get the cropped area position and size data (base on the original image).
        /// </summary>
        /// <param name="rounded">Indicate if round the data values or not.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropperData}"/> representing result cropped data asynchronous operation.</returns>
        public async ValueTask<CropperData> GetDataAsync(bool rounded, CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<CropperData>("cropper.getData", cancellationToken, rounded);
        }

        /// <summary>
        /// Get the image position and size data.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ImageData}"/> representing result image data asynchronous operation.</returns>
        public async ValueTask<ImageData> GetImageDataAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<ImageData>("cropper.getImageData", cancellationToken);
        }

        /// <summary>
        /// Move the canvas with relative offsets.
        /// </summary>
        /// <param name="offsetX">The relative offset distance on the x-axis.</param>
        /// <param name="offsetY">The relative offset distance on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveAsync(
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.move", cancellationToken, offsetX, offsetY);
        }

        /// <summary>
        /// Move the canvas to an absolute point.
        /// </summary>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveToAsync(
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.moveTo", cancellationToken, x, y);
        }

        /// <summary>
        /// Replace the image's src and rebuild the cropper.
        /// </summary>
        /// <param name="url">The new URL.</param>
        /// <param name="onlyColorChanged">Indicate if the new image has the same size as the old one.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ReplaceAsync(
            string url,
            bool onlyColorChanged,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.replace", cancellationToken, url, onlyColorChanged);
        }

        /// <summary>
        /// Reset the image and crop box to their initial states.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ResetAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.reset", cancellationToken);
        }

        /// <summary>
        /// Rotate the canvas with a relative degree.
        /// </summary>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RotateAsync(
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.rotate", cancellationToken, degree);
        }

        /// <summary>
        /// Rotate the canvas to an absolute degree.
        /// </summary>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RotateToAsync(
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.rotateTo", cancellationToken, degree);
        }

        /// <summary>
        /// Scale the image.
        /// </summary>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleAsync(
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.scale", cancellationToken, scaleX, scaleY);
        }

        /// <summary>
        /// Scale the image on the x-axis.
        /// </summary>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleXAsync(
            decimal scaleX,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.scaleX", cancellationToken, scaleX);
        }

        /// <summary>
        /// Scale the image on the y-axis.
        /// </summary>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleYAsync(
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.scaleY", cancellationToken, scaleY);
        }

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="aspectRatio">The new aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetAspectRatioAsync(
            decimal aspectRatio,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.setAspectRatio", cancellationToken, aspectRatio);
        }

        /// <summary>
        /// Set the canvas position and size with new data.
        /// </summary>
        /// <param name="setCanvasDataOptions">The new canvas data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetCanvasDataAsync(
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.setCanvasData", cancellationToken, setCanvasDataOptions);
        }

        /// <summary>
        /// Set the crop box position and size with new data.
        /// </summary>
        /// <param name="cropBoxDataOptions">The new crop box data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetCropBoxDataAsync(
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.setCropBoxData", cancellationToken, cropBoxDataOptions);
        }

        /// <summary>
        /// Set the cropped area position and size with new data.
        /// </summary>
        /// <param name="setDataOptions">The new data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDataAsync(
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.setData", cancellationToken, setDataOptions);
        }

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="dragMode">The new drag mode.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDragModeAsync(
            DragMode dragMode,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.setDragMode", cancellationToken, dragMode.ToEnumString());
        }

        /// <summary>
        /// Zoom the canvas with a relative ratio.
        /// </summary>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ZoomAsync(
            decimal ratio,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.zoom", cancellationToken, ratio);
        }

        /// <summary>
        /// Zoom the canvas to an absolute ratio.
        /// </summary>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="pivotX">The zoom pivot point X coordinate.</param>
        /// <param name="pivotY">The zoom pivot point Y coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ZoomToAsync(
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.zoomTo", cancellationToken, ratio, pivotX, pivotY);
        }

        /// <summary>
        /// Get the no conflict cropper class.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask NoConflictAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.noConflict", cancellationToken);
        }

        /// <summary>
        /// Change the default options.
        /// </summary>
        /// <param name="options">The new default options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDefaultsAsync(
            [NotNull] Options options,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync("cropper.setDefaults", cancellationToken, options);
        }

        /// <summary>
        /// Used to get an image from a stream.
        /// <br/>
        /// Converts JavaScript stream to .NET stream uses <see cref="DotNetStreamReference"/> and then creates a URL blob reference.
        /// </summary>
        /// <param name="imageFile">The <see cref="IBrowserFile"/> to convert to a new image file.</param>
        /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream. Defaults to 500 KB.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL blob reference asynchronous operation.</returns>
        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            var jsImageStream = imageFile.OpenReadStream(maxAllowedSize, cancellationToken);
            var dotnetImageStream = new DotNetStreamReference(jsImageStream);
            return await _jsRuntime.InvokeAsync<string>("cropper.getImageUsingStreaming", cancellationToken, dotnetImageStream);
        }

        /// <summary>
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime.InvokeVoidAsync("cropper.revokeObjectUrl", cancellationToken, url);
        }

        /// <summary>
        /// Called to dispose this instance.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called to dispose js module.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (Module is not null)
            {
                await Module.DisposeAsync();
            }

            Module = null;
        }
    }
}