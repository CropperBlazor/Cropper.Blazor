using System.Threading;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
        /// <summary>
        /// Center the image within the cropper canvas.
        /// </summary>
        /// <param name="size">The center sizing mode. Supported values are <c>contain</c> and <c>cover</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Center(string size, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.CenterAsync(CropperComponentId, size, cancellationToken);
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
        /// <param name="ratio">Requires a positive number (ratio &gt; 0)</param>
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
    }
}
