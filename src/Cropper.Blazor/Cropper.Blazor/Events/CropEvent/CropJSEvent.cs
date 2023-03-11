using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropEvent
{
    /// <summary>
    /// Provides the metadata of a Crop JS Event.
    /// </summary>
    public class CropJSEvent : BaseJSEvent
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public CropJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
            : base(jsRuntime, jsRuntimeObjectRef)
        {
        }

        /// <summary>
        /// Represents a a Crop JavaScript Event object.
        /// </summary>
        public JSEventData<CropEvent> EventData { get; internal set; } = null!;

        /// <summary>
        /// Get Crop JavaScript Event object.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{CropJSEventData}"/> representing cropper JavaScript event object asynchronous operation.</returns>
        public async ValueTask<JSEventData<CropEvent>> GetCropJSEventDataAsync(CancellationToken cancellationToken = default)
        {
            return await _jsRuntime!.InvokeAsync<JSEventData<CropEvent>>("cropper.getCropJSEventData", cancellationToken, JSRuntimeObjectRef);
        }
    }
}
