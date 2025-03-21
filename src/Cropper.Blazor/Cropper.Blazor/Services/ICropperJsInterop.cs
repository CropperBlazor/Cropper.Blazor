using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Components;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// Provides the metadata of a ICropperJsInterop.
    /// </summary>
    public interface ICropperJsInterop
    {
        /// <summary>
        /// Loads cropper JS module.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task LoadModuleAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Initializes cropper. 
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="image">Reference to img html-DOM</param>
        /// <param name="options">Cropper options</param>
        /// <param name="cropperComponentBase">Reference to base cropper component. Default equal to 'this' object.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask InitCropperAsync(
            Guid cropperComponentId,
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ClearAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask CropAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask DestroyAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask DisableAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask EnableAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the canvas position and size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CanvasData}"/> representing result canvas data asynchronous operation.</returns>
        ValueTask<CanvasData> GetCanvasDataAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the container size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ContainerData}"/> representing result container data asynchronous operation.</returns>
        ValueTask<ContainerData> GetContainerDataAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the crop box position and size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropBoxData}"/> representing result crop box data asynchronous operation.</returns>
        ValueTask<CropBoxData> GetCropBoxDataAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the cropped area position and size data (base on the original image).
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="rounded">Indicate if round the data values or not.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropperData}"/> representing result cropped data asynchronous operation.</returns>
        ValueTask<CropperData> GetDataAsync(
            Guid cropperComponentId,
            bool rounded,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the image position and size data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{ImageData}"/> representing result image data asynchronous operation.</returns>
        ValueTask<ImageData> GetImageDataAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Move the canvas with relative offsets.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="offsetX">The relative offset distance on the x-axis.</param>
        /// <param name="offsetY">The relative offset distance on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask MoveAsync(
            Guid cropperComponentId,
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Move the canvas to an absolute point.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask MoveToAsync(
            Guid cropperComponentId,
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Replace the image's src and rebuild the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="url">The new URL.</param>
        /// <param name="hasSameSize">If the new image has the same size as the old one, then it will not rebuild the cropper and only update the URLs of all related images. This can be used for applying filters.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ReplaceAsync(
            Guid cropperComponentId,
            string url,
            bool hasSameSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Reset the image and crop box to their initial states.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ResetAsync(
            Guid cropperComponentId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Rotate the canvas with a relative degree.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask RotateAsync(
            Guid cropperComponentId,
            decimal degree,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Rotate the canvas to an absolute degree.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask RotateToAsync(
            Guid cropperComponentId,
            decimal degree,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Scale the image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ScaleAsync(
            Guid cropperComponentId,
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Scale the image on the x-axis.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ScaleXAsync(
            Guid cropperComponentId,
            decimal scaleX,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Scale the image on the y-axis.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ScaleYAsync(
            Guid cropperComponentId,
            decimal scaleY,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the canvas position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="setCanvasDataOptions">The new canvas data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask SetCanvasDataAsync(
            Guid cropperComponentId,
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the crop box position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cropBoxDataOptions">The new crop box data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask SetCropBoxDataAsync(
            Guid cropperComponentId,
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the cropped area position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="setDataOptions">The new data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask SetDataAsync(
            Guid cropperComponentId,
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="dragMode">The new drag mode.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask SetDragModeAsync(
            Guid cropperComponentId,
            DragMode dragMode,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Zoom the canvas with a relative ratio.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ZoomAsync(
            Guid cropperComponentId,
            decimal ratio,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Zoom the canvas to an absolute ratio.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="pivotX">The zoom pivot point X coordinate.</param>
        /// <param name="pivotY">The zoom pivot point Y coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask ZoomToAsync(
            Guid cropperComponentId,
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the no conflict cropper class.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask NoConflictAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Change the default options.
        /// </summary>
        /// <param name="options">The new default options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask SetDefaultsAsync(
            [NotNull] Options options,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="aspectRatio">The new aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask SetAspectRatioAsync(
            Guid cropperComponentId,
            decimal aspectRatio,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a canvas drawn the cropped image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing result canvas asynchronous operation.</returns>
        ValueTask<CroppedCanvas> GetCroppedCanvasAsync(
            Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default);

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
        ValueTask<string> GetCroppedCanvasDataURLAsync(
            Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            string type,
            float number,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Used to get an image from a stream.
        /// <br/>
        /// Converts JavaScript stream to .NET stream uses <see cref="DotNetStreamReference"/> and then creates a URL blob reference.
        /// </summary>
        /// <param name="imageFile">The <see cref="IBrowserFile"/> to convert to a new image file.</param>
        /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream. Defaults to 500 KB.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL blob reference asynchronous operation.</returns>
        ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Called to dispose this instance.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        ValueTask DisposeAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="getCroppedCanvasOptions">The config options.</param>
        /// <param name="imageReceiverReference">Reference to image receiver.</param>
        /// <param name="type">A string indicating the image format. The default type is image/png; this image format will be also used if the specified type is not supported.</param>
        /// <param name="number">A number between 0 and 1 indicating the image quality to be used when creating images using file formats that support lossy compression (such as image/jpeg or image/webp). A user agent will use its default quality value if this option is not specified, or if the number is outside the allowed range.
        /// Different browsers have different image encoder compression, usually it is 92 or 80 percent of the full image quality.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CroppedCanvas}"/> representing result canvas asynchronous operation.</returns>
        ValueTask<CroppedCanvas> StartImageTransferAsync(
            [NotNull] Guid cropperComponentId,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            [NotNull] DotNetObjectReference<ImageReceiver> imageReceiverReference,
            string type,
            float number,
            CancellationToken cancellationToken = default);
    }
}
