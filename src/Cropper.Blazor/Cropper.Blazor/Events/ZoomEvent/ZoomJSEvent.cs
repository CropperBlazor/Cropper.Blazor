using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.ZoomEvent
{
    /// <summary>
    /// Provides the metadata of a Zoom JS Event.
    /// </summary>
    public class ZoomJSEvent : BaseJSEvent
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public ZoomJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
            : base(jsRuntime, jsRuntimeObjectRef)
        {
        }

        /// <summary>
        /// Represents a Zoom JavaScript Event object.
        /// </summary>
        public JSEventData<ZoomEvent> EventData { get; internal set; } = null!;
    }
}
