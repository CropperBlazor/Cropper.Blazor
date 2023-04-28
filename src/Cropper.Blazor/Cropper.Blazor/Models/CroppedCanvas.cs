using System;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of Cropped Canvas.
    /// </summary>
    public class CroppedCanvas : IDisposable
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

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        public void Dispose()
        {
            JSRuntimeObjectRef?.DisposeAsync();
        }
    }
}
