using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropEndEvent
{
    /// <summary>
    /// Provides the metadata of a Crop End JS Event.
    /// </summary>
    public class CropEndJSEvent : BaseJSEvent
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CropEndJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
            : base(jsRuntime, jsRuntimeObjectRef)
        {
        }

        /// <summary>
        /// Represents a Crop End JavaScript Event object.
        /// </summary>
        public JSEventData<CropEndEvent> EventData { get; internal set; } = null!;
    }
}
