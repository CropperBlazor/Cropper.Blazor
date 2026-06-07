using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Components;
using Cropper.Blazor.Models;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    public partial class CropperJsInterop
    {
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
            await TryLoadModuleAsync(cancellationToken);

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
            await TryLoadModuleAsync(cancellationToken);

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
            await TryLoadModuleAsync(cancellationToken);

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
            await TryLoadModuleAsync(cancellationToken);

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
            await TryLoadModuleAsync(cancellationToken);

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
            await TryLoadModuleAsync(cancellationToken);

            return await _jsRuntime!.InvokeAsync<string>(
                "cropper.getCroppedCanvasDataURL",
                cancellationToken,
                cropperComponentId,
                getCroppedCanvasOptions,
                type,
                number);
        }

        /// <summary>
        /// Get a data URL from the current cropper selection canvas.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="type">A string indicating the image format.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL result canvas asynchronous operation.</returns>
        public async ValueTask<string> GetSelectionCanvasDataURLAsync(
            [NotNull] Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            string type,
            float number,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            return await _jsRuntime!.InvokeAsync<string>(
                "cropper.selectionToCanvasDataURL",
                cancellationToken,
                cropperComponentId,
                getCroppedCanvasOptions,
                type,
                number);
        }

        /// <summary>
        /// Get a canvas element reference from a Cropper.js v2 selection by zero-based index.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> containing a JavaScript object reference to the selection canvas element.</returns>
        public async ValueTask<IJSObjectReference?> GetSelectionCanvasReferenceByIndexAsync(
            [NotNull] Guid cropperComponentId,
            int selectionIndex,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            return await _jsRuntime!.InvokeAsync<IJSObjectReference?>(
                "cropper.selectionToCanvasByIndex",
                cancellationToken,
                cropperComponentId,
                selectionIndex,
                getCroppedCanvasOptions);
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
            await TryLoadModuleAsync(cancellationToken);

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
            await TryLoadModuleAsync(cancellationToken);

            return await _jsRuntime!.InvokeAsync<ImageData>(
                "cropper.getImageData",
                cancellationToken,
                cropperComponentId);
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
            await TryLoadModuleAsync(cancellationToken);

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
    }
}
