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
        void CropperIsCroped(object data);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsEnded))]
        void CropperIsEnded(object data);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsMoved))]
        void CropperIsMoved(object data);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsStarted))]
        void CropperIsStarted(object data);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsZoomed))]
        void CropperIsZoomed(object data);

        [JSInvokable(nameof(ICropperComponentBase.IsReady))]
        void IsReady(object data);
    }
}
