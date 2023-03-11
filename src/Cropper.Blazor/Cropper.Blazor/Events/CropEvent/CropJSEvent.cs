using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropEvent
{
    /// <summary>
    /// Provides the metadata of a Crop JS Event.
    /// </summary>
    public class CropJSEvent
    {
        private readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Represents a reference to a Crop JavaScript Event object.
        /// </summary>
        public IJSObjectReference JSRuntimeObjectRef { get; }

        /// <summary>
        /// 
        /// </summary>
        public CropJSEventData CropJSEventData { get; internal set; } = null!;

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CropJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
        {
            _jsRuntime = jsRuntime;
            JSRuntimeObjectRef = jsRuntimeObjectRef;
        }

        /// <summary>
        /// 
        /// </summary>
        public ValueTask<object> getMethods =>
            _jsRuntime!.InvokeAsync<object>("jsObject.getMethods", JSRuntimeObjectRef);

        /// <summary>
        /// 
        /// </summary>
        public ValueTask<CropJSEventData> GetPropertiesValueAsync() =>
            _jsRuntime!.InvokeAsync<CropJSEventData>("jsObject.getPropertiesValue", JSRuntimeObjectRef);

        /// <summary>
        /// Prevent the event default behavior
        /// </summary>
        /// <returns></returns>
        public async ValueTask PreventDefaultAsync()
        {
            await _jsRuntime!.InvokeVoidAsync("jsObject.callInstanceMethod", JSRuntimeObjectRef, "preventDefault");
        }
    }
}
