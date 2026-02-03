using System;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
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
            DestroyAsync(cancellationToken);
        }

        private async ValueTask DestroyAsync(CancellationToken cancellationToken = default)
        {
            await CropperJsIntertop!.DestroyAsync(CropperComponentId, cancellationToken);
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
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// <br/>
        /// Obsolete — use <see cref="Services.IUrlImageInterop.RevokeObjectUrlAsync(string, CancellationToken)"/> 
        /// for centralized JS interop handling.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        [Obsolete("⚠️ This method will be removed in future versions. Use IUrlImageInterop.RevokeObjectUrlAsync instead for proper JS interop management.")]
        public async ValueTask RevokeObjectUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            await CropperJsIntertop!.RevokeObjectUrlAsync(url, cancellationToken);
        }
    }
}
