using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Base
{
    public interface ICropperComponentBase
    {
        [JSInvokable(nameof(ICropperComponentBase.CropperIsCroped))]
        void CropperIsCroped(EventArgs eventArgs);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsEnded))]
        void CropperIsEnded(EventArgs eventArgs);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsMoved))]
        void CropperIsMoved(EventArgs eventArgs);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsStarted))]
        void CropperIsStarted(EventArgs eventArgs);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsZoomed))]
        void CropperIsZoomed(EventArgs eventArgs);

        [JSInvokable(nameof(ICropperComponentBase.IsReady))]
        void IsReady(EventArgs eventArgs);
    }
}
