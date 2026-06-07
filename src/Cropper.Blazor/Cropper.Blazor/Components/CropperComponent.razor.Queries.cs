using System;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
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
        /// <br/>
        /// Obsolete — use <see cref="Services.IUrlImageInterop.GetImageUsingStreamingAsync(IBrowserFile, long, CancellationToken)"/> 
        /// for centralized JS interop handling.
        /// </summary>
        /// <param name="imageFile">The <see cref="IBrowserFile"/> to convert to a new image file.</param>
        /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream. Defaults to 500 KB.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing Blob URL asynchronous operation.</returns>
        [Obsolete("Use IUrlImageInterop.GetImageUsingStreamingAsync instead for centralized JS interop handling.")]
        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
        }

        /// <summary>
        /// Get a canvas drawn from the cropped image (lossy compression). If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a cropped canvas.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing canvas drawn the cropped image asynchronous operation.</returns>
        [Obsolete("This method blocks the UI thread. Use GetCroppedCanvasInBackgroundAsync instead for background operation.")]
        public async ValueTask<CroppedCanvas> GetCroppedCanvasAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            return await CropperJsIntertop!.GetCroppedCanvasAsync(
                CropperComponentId,
                getCroppedCanvasOptions,
                cancellationToken);
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
        [Obsolete("This method blocks the UI thread. Use GetCroppedCanvasDataInBackgroundAsync instead for background operation.")]
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
        /// Get a data URL from the current cropper selection canvas.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a cropped canvas.</param>
        /// <param name="type">A string indicating the image format. The default type is image/png; this image format will be also used if the specified type is not supported.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality to be used when creating images using file formats that support lossy compression.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="number"/> is outside the range of 0 and 1.</exception>
        /// <returns>A <see cref="ValueTask{String}"/> representing canvas drawn the cropped image in URL format asynchronous operation.</returns>
        public async ValueTask<string> GetSelectionCanvasDataURLAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            string type = "image/png",
            float number = 1,
            CancellationToken cancellationToken = default)
        {
            return number switch
            {
                < 0 or > 1 => throw new ArgumentException($"The given number should be between 0 and 1 for indication the image quality, but found {number}.", nameof(number)),
                _ => await CropperJsIntertop!.GetSelectionCanvasDataURLAsync(
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
        public async ValueTask<ImageReceiver> GetCroppedCanvasDataInBackgroundAsync(
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

            await CropperJsIntertop.GetCroppedCanvasDataInBackgroundAsync(
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
        /// Get a canvas drawn from the cropped image (lossy compression).
        /// If it is not cropped, then returns a canvas drawn the whole image.
        /// </summary>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a cropped canvas.</param>
        /// <param name="onReceive">A function that takes a <see cref="CroppedCanvas"/> as input and returns a <see cref="Task"/>. Used for handling the cropped image result asynchronously.</param>        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvasReceiver}"/> representing the asynchronous operation to retrieve cropped canvas receiver reference.</returns>
        public async ValueTask<CroppedCanvasReceiver> GetCroppedCanvasInBackgroundAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            Func<CroppedCanvas, CancellationToken, Task> onReceive,
            CancellationToken cancellationToken = default)
        {
            CroppedCanvasReceiver croppedCanvasReceiver = new CroppedCanvasReceiver(onReceive, cancellationToken);

            await CropperJsIntertop!.GetCroppedCanvasInBackgroundAsync(
                CropperComponentId,
                getCroppedCanvasOptions,
                DotNetObjectReference.Create(croppedCanvasReceiver),
                cancellationToken);

            return croppedCanvasReceiver;
        }
    }
}
