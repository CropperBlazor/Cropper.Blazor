using Microsoft.JSInterop;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of Cropped Canvas.
    /// </summary>
    public class CroppedCanvas
    {
        /// <summary>
        /// Represents a reference to a JavaScript Cropped Canvas object.
        /// </summary>
        public IJSObjectReference JSRuntimeObjectRef { get; }

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CroppedCanvas(IJSObjectReference jsRuntimeObjectRef)
        {
            JSRuntimeObjectRef = jsRuntimeObjectRef;
        }
    }
}
