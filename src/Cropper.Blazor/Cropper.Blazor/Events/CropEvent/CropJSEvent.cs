using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IJSObjectReference _jsRuntimeObjectRef;

        /// <summary>
        /// 
        /// </summary>

        public CropJSEvent(IJSRuntime jsRuntime, IJSObjectReference jsRuntimeObjectRef)
        {
            _jsRuntime = jsRuntime;
            _jsRuntimeObjectRef = jsRuntimeObjectRef;
        }

        /// <summary>
        /// 
        /// </summary>
        public ValueTask<CropEvent> CropEvent =>
            _jsRuntime!.InvokeAsync<CropEvent>("jsObject.getInstanceProperty", _jsRuntimeObjectRef, "detail");

        /// <summary>
        /// Prevent the event default behavior
        /// </summary>
        /// <returns></returns>
        public async ValueTask PreventDefault()
        {
            await _jsRuntime!.InvokeVoidAsync("jsObject.callInstanceMethod", _jsRuntimeObjectRef, "preventDefault");
        }
    }
}
