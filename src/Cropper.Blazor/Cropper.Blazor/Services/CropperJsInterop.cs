using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Components;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Cropper.Blazor.ModuleOptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// This service listens to cropper js events and allows you to manage calls from cropper.
    /// </summary>
    public class CropperJsInterop : ICropperJsInterop, IAsyncDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly ICropperJsInteropOptions _cropperJsInteropOptions;
        private readonly IJSRuntime _jsRuntime;
        private IJSObjectReference? Module = null;

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
        /// <param name="cropperJsInteropOptions">The <see cref="ICropperJsInteropOptions"/>.</param>
        public CropperJsInterop(
            IJSRuntime jsRuntime,
            NavigationManager navigationManager,
            ICropperJsInteropOptions cropperJsInteropOptions)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _cropperJsInteropOptions = cropperJsInteropOptions;
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
            string globalPathToCropperModule = GetGlobalPathToCropperModule();

            Module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", cancellationToken, globalPathToCropperModule);
        }

        /// <summary>
        /// Finds path to the cropper module.
        /// </summary>
        /// <returns>The path to the cropper module.</returns>
        private string GetGlobalPathToCropperModule()
        {
            if (_cropperJsInteropOptions.IsActiveGlobalPath)
            {
                return _cropperJsInteropOptions.GlobalPathToCropperModule;
            }
            else
            {
                Uri baseUri = new(_navigationManager.BaseUri);
                string hostName = baseUri.GetHostName();

                return Path.Combine(hostName, _cropperJsInteropOptions.DefaultInternalPathToCropperModule);
            }
        }

        /// <summary>
        /// Initializes cropper. 
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="image">Reference to img html-DOM</param>
        /// <param name="options">Cropper options</param>
        /// <param name="cropperComponentBase">Reference to base cropper component. Default equal to 'this' object.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask InitCropperAsync(
            [NotNull] Guid cropperComponentId,
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
                cropperComponentId,
                image,
                options,
                cropperComponentBase);
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ClearAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.clear",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask CropAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.crop",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DestroyAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.destroy",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisableAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.disable",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask EnableAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.enable",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Get the canvas position and size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CanvasData}"/> representing result canvas data asynchronous operation.</returns>
        public async ValueTask<CanvasData> GetCanvasDataAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<CanvasData>(
                "cropper.getCanvasData",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Get the container size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ContainerData}"/> representing result container data asynchronous operation.</returns>
        public async ValueTask<ContainerData> GetContainerDataAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<ContainerData>(
                "cropper.getContainerData",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Get the crop box position and size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropBoxData}"/> representing result crop box data asynchronous operation.</returns>
        public async ValueTask<CropBoxData> GetCropBoxDataAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<CropBoxData>(
                "cropper.getCropBoxData",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Get a canvas drawn the cropped image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing result canvas asynchronous operation.</returns>
        public async ValueTask<CroppedCanvas> GetCroppedCanvasAsync(
            [NotNull] Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            IJSObjectReference jSCanvas = await _jsRuntime!.InvokeAsync<IJSObjectReference>(
                "cropper.getCroppedCanvas",
                cancellationToken,
                cropperComponentId,
                getCroppedCanvasOptions);

            return new CroppedCanvas(jSCanvas);
        }

        /// <summary>
        /// Get a canvas drawn the cropped image in background.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="croppedCanvasReceiverReference">Reference to cropped canvas receiver.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing result canvas asynchronous operation.</returns>
        public async ValueTask GetCroppedCanvasInBackgroundAsync(
            [NotNull] Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            DotNetObjectReference<CroppedCanvasReceiver> croppedCanvasReceiverReference,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.getCroppedCanvasInBackground",
                cancellationToken,
                cropperComponentId,
                getCroppedCanvasOptions,
                croppedCanvasReceiverReference);
        }

        /// <summary>
        /// Get a canvas drawn the cropped image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="type">A string indicating the image format. The default type is image/png; this image format will be also used if the specified type is not supported.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality to be used when creating images using file formats that support lossy compression (such as image/jpeg or image/webp). A user agent will use its default quality value if this option is not specified, or if the number is outside the allowed range.
        /// Different browsers have different image encoder compression, usually it is 92 or 80 percent of the full image quality.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL result canvas asynchronous operation.</returns>
        public async ValueTask<string> GetCroppedCanvasDataURLAsync(
            [NotNull] Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            string type,
            float number,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<string>(
                "cropper.getCroppedCanvasDataURL",
                cancellationToken,
                cropperComponentId,
                getCroppedCanvasOptions,
                type,
                number);
        }

        /// <summary>
        /// Get the cropped area position and size data (base on the original image).
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="rounded">Indicate if round the data values or not.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropperData}"/> representing result cropped data asynchronous operation.</returns>
        public async ValueTask<CropperData> GetDataAsync(
            [NotNull] Guid cropperComponentId,
            bool rounded,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<CropperData>(
                "cropper.getData",
                cancellationToken,
                cropperComponentId,
                rounded);
        }

        /// <summary>
        /// Get the image position and size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ImageData}"/> representing result image data asynchronous operation.</returns>
        public async ValueTask<ImageData> GetImageDataAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await _jsRuntime!.InvokeAsync<ImageData>(
                "cropper.getImageData",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Move the canvas with relative offsets.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="offsetX">The relative offset distance on the x-axis.</param>
        /// <param name="offsetY">The relative offset distance on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveAsync(
            [NotNull] Guid cropperComponentId,
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.move",
                cancellationToken,
                cropperComponentId,
                offsetX,
                offsetY);
        }

        /// <summary>
        /// Move the canvas to an absolute point.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveToAsync(
            [NotNull] Guid cropperComponentId,
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.moveTo",
                cancellationToken,
                cropperComponentId,
                x,
                y);
        }

        /// <summary>
        /// Replace the image's src and rebuild the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="url">The new URL.</param>
        /// <param name="hasSameSize">If the new image has the same size as the old one, then it will not rebuild the cropper and only update the URLs of all related images. This can be used for applying filters.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ReplaceAsync(
            [NotNull] Guid cropperComponentId,
            string url,
            bool hasSameSize,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.replace",
                cancellationToken,
                cropperComponentId,
                url,
                hasSameSize);
        }

        /// <summary>
        /// Reset the image and crop box to their initial states.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ResetAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.reset",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Rotate the canvas with a relative degree.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RotateAsync(
            [NotNull] Guid cropperComponentId,
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.rotate",
                cancellationToken,
                cropperComponentId,
                degree);
        }

        /// <summary>
        /// Rotate the canvas to an absolute degree.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RotateToAsync(
            [NotNull] Guid cropperComponentId,
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.rotateTo",
                cancellationToken,
                cropperComponentId,
                degree);
        }

        /// <summary>
        /// Scale the image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleAsync(
            [NotNull] Guid cropperComponentId,
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.scale",
                cancellationToken,
                cropperComponentId,
                scaleX,
                scaleY);
        }

        /// <summary>
        /// Scale the image on the x-axis.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleXAsync(
            [NotNull] Guid cropperComponentId,
            decimal scaleX,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.scaleX",
                cancellationToken,
                cropperComponentId,
                scaleX);
        }

        /// <summary>
        /// Scale the image on the y-axis.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleYAsync(
            [NotNull] Guid cropperComponentId,
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.scaleY",
                cancellationToken,
                cropperComponentId,
                scaleY);
        }

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="aspectRatio">The new aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetAspectRatioAsync(
            [NotNull] Guid cropperComponentId,
            decimal aspectRatio,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setAspectRatio",
                cancellationToken,
                cropperComponentId,
                aspectRatio);
        }

        /// <summary>
        /// Set the canvas position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="setCanvasDataOptions">The new canvas data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetCanvasDataAsync(
            [NotNull] Guid cropperComponentId,
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setCanvasData",
                cancellationToken,
                cropperComponentId,
                setCanvasDataOptions);
        }

        /// <summary>
        /// Set the crop box position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cropBoxDataOptions">The new crop box data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetCropBoxDataAsync(
            [NotNull] Guid cropperComponentId,
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setCropBoxData",
                cancellationToken,
                cropperComponentId,
                cropBoxDataOptions);
        }

        /// <summary>
        /// Set the cropped area position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="setDataOptions">The new data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDataAsync(
            [NotNull] Guid cropperComponentId,
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setData",
                cancellationToken,
                cropperComponentId,
                setDataOptions);
        }

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="dragMode">The new drag mode.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDragModeAsync(
            [NotNull] Guid cropperComponentId,
            DragMode dragMode,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setDragMode",
                cancellationToken,
                cropperComponentId,
                dragMode.ToEnumString());
        }

        /// <summary>
        /// Zoom the canvas with a relative ratio.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ZoomAsync(
            [NotNull] Guid cropperComponentId,
            decimal ratio,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.zoom",
                cancellationToken,
                cropperComponentId,
                ratio);
        }

        /// <summary>
        /// Zoom the canvas to an absolute ratio.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="pivotX">The zoom pivot point X coordinate.</param>
        /// <param name="pivotY">The zoom pivot point Y coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ZoomToAsync(
            [NotNull] Guid cropperComponentId,
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.zoomTo",
                cancellationToken,
                cropperComponentId,
                ratio,
                pivotX,
                pivotY);
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

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.noConflict",
                cancellationToken);
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

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setDefaults",
                cancellationToken,
                options);
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

            return await _jsRuntime.InvokeAsync<string>(
                "cropper.getImageUsingStreaming",
                cancellationToken,
                dotnetImageStream);
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

            await _jsRuntime.InvokeVoidAsync(
                "cropper.revokeObjectUrl",
                cancellationToken,
                url);
        }

        /// <summary>
        /// Get a canvas drawn the cropped image in background.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="imageReceiverReference">Reference to image receiver.</param>
        /// <param name="type">A string indicating the image format. The default type is image/png; this image format will be also used if the specified type is not supported.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality to be used when creating images using file formats that support lossy compression (such as image/jpeg or image/webp). A user agent will use its default quality value if this option is not specified, or if the number is outside the allowed range.
        /// Different browsers have different image encoder compression, usually it is 92 or 80 percent of the full image quality.
        /// </param>
        /// <param name="maximumReceiveChunkSize">
        /// The maximum size of each image chunk to receive, in bytes. For example, 65536 equals 64 KB.
        /// If specified, incoming image data will be split into chunks of this size during transmission.
        /// If null, the chunk size will be handled automatically based on the stream's native chunking behavior.
        /// This helps control memory usage and ensures compatibility with interop limits.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask GetCroppedCanvasDataInBackgroundAsync(
            [NotNull] Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            [NotNull] DotNetObjectReference<ImageReceiver> imageReceiverReference,
            string type,
            float number,
            int? maximumReceiveChunkSize,
            CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await _jsRuntime.InvokeVoidAsync(
                "cropper.sendImageInChunks",
                cancellationToken,
                cropperComponentId,
                getCroppedCanvasOptions,
                imageReceiverReference,
                type,
                number,
                maximumReceiveChunkSize);
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
