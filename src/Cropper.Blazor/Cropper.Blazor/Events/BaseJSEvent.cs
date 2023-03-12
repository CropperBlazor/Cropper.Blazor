using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events
{
    /// <summary>
    /// Provides the metadata of a Base JS Event.
    /// </summary>
    public class BaseJSEvent
    {
        /// <summary>
        /// Represents an instance of a JavaScript runtime to which calls may be dispatched.
        /// </summary>
        protected readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Represents a reference to a Crop JavaScript Event object.
        /// </summary>
        public IJSObjectReference JSRuntimeObjectRef { get; }

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="jsRuntimeObjectRef">The <see cref="IJSObjectReference"/>.</param>
        public BaseJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
        {
            _jsRuntime = jsRuntime;
            JSRuntimeObjectRef = jsRuntimeObjectRef;
        }

        /// <summary>
        /// Prevent the event default behavior
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask PreventDefaultAsync(CancellationToken cancellationToken = default)
        {
            await _jsRuntime!.InvokeVoidAsync("jsObject.callInstanceMethod", cancellationToken, JSRuntimeObjectRef, "preventDefault");
        }

        /// <summary>
        /// Prevents other listeners of the same event from being called.
        /// If several listeners are attached to the same element for the same event type, they are called in the order in which they were added.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask StopImmediatePropagationAsync(CancellationToken cancellationToken = default)
        {
            await _jsRuntime!.InvokeVoidAsync("jsObject.callInstanceMethod", cancellationToken, JSRuntimeObjectRef, "stopImmediatePropagation");
        }

        /// <summary>
        /// Prevents further propagation of the current event in the capturing and bubbling phases.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask StopPropagationAsync(CancellationToken cancellationToken = default)
        {
            await _jsRuntime!.InvokeVoidAsync("jsObject.callInstanceMethod", cancellationToken, JSRuntimeObjectRef, "stopPropagation");
        }

        /// <summary>
        /// Get JavaScript Event object.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{JSEventData}"/> representing JavaScript event object asynchronous operation.</returns>
        public async ValueTask<JSEventData<Event>> GetJSEventDataAsync<Event>(CancellationToken cancellationToken = default)
        {
            return await _jsRuntime!.InvokeAsync<JSEventData<Event>>("cropper.getJSEventData", cancellationToken, JSRuntimeObjectRef);
        }
    }
}
