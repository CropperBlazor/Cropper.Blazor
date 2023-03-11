using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropReadyEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Ready JS Event.
    /// </summary>
    public class CropReadyJSEvent : BaseJSEvent
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CropReadyJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
            : base(jsRuntime, jsRuntimeObjectRef)
        {
        }
    }
}
