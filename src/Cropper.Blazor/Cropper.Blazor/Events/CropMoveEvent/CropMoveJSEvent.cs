using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropMoveEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Move JS Event.
    /// </summary>
    public class CropMoveJSEvent : BaseJSEvent
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CropMoveJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
            : base(jsRuntime, jsRuntimeObjectRef)
        {
        }

        /// <summary>
        /// Represents a Crop Move JavaScript Event object.
        /// </summary>
        public JSEventData<CropMoveEvent> EventData { get; internal set; } = null!;
    }
}
