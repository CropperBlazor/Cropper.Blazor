using System.Threading;
using Cropper.Blazor.Models;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="dragMode">The <see cref="DragMode"/> used to set new drag mode.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetDragMode(DragMode dragMode = DragMode.None, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetDragModeAsync(CropperComponentId, dragMode, cancellationToken);
        }
    }
}
