using System.Threading;
using Cropper.Blazor.Models;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
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
    }
}
