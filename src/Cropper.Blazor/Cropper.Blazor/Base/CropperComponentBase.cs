using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Base
{
    public class CropperComponentBase : ICropperComponentBase
    {
        [JSInvokable(nameof(CropperComponentBase.IsReady))]
        public void IsReady(CropReadyEvent cropReadyEvent)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsStarted))]
        public void CropperIsStarted(CropStartEvent cropStartEvent)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsMoved))]
        public void CropperIsMoved(CropMoveEvent cropMoveEvent)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsEnded))]
        public void CropperIsEnded(CropEndEvent cropEndEvent)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsCroped))]
        public void CropperIsCroped(CropEvent cropEvent)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsZoomed))]
        public void CropperIsZoomed(ZoomEvent zoomEvent)
        {

        }
    }
}
