using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropStartEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Start JS Event.
    /// </summary>
    public class CropStartJSEvent : BaseJSEvent
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CropStartJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
            : base(jsRuntime, jsRuntimeObjectRef)
        {
        }

        /// <summary>
        /// Represents a Crop Start JavaScript Event object.
        /// </summary>
        public JSEventData<CropStartEvent> EventData { get; internal set; } = null!;
    }
}
