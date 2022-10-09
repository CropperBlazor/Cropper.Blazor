using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Base
{
    public interface ICropperComponentBase
    {
        [JSInvokable(nameof(ICropperComponentBase.CropperIsCroped))]
        void CropperIsCroped(CropEvent cropEvent);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsEnded))]
        void CropperIsEnded(CropEndEvent cropEndEvent);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsMoved))]
        void CropperIsMoved(CropMoveEvent cropMoveEvent);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsStarted))]
        void CropperIsStarted(CropStartEvent cropStartEvent);

        [JSInvokable(nameof(ICropperComponentBase.CropperIsZoomed))]
        void CropperIsZoomed(ZoomEvent zoomEvent);

        [JSInvokable(nameof(ICropperComponentBase.IsReady))]
        void IsReady(CropReadyEvent cropReadyEvent);
    }
}
