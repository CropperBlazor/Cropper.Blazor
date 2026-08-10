using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    public partial class CropperJsInterop
    {
        /// <summary>
        /// Initializes a viewer for an existing cropper component.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="viewerElement">Reference to the viewer host element.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <param name="selectionIndex">The optional selection index to bind the viewer to. When null, the active selection is used.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask InitializeViewerAsync(
            [NotNull] Guid cropperComponentId,
            [NotNull] ElementReference viewerElement,
            CancellationToken cancellationToken = default,
            int? selectionIndex = null)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.initializeViewer",
                cancellationToken,
                cropperComponentId,
                viewerElement,
                selectionIndex);
        }
    }
}
