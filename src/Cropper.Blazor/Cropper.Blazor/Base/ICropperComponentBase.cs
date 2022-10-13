using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;

namespace Cropper.Blazor.Base
{
    public interface ICropperComponentBase
    {
        void CropperIsCroped(CropEvent cropEvent);
        void CropperIsEnded(CropEndEvent cropEndEvent);
        void CropperIsMoved(CropMoveEvent cropMoveEvent);
        void CropperIsStarted(CropStartEvent cropStartEvent);
        void CropperIsZoomed(ZoomEvent zoomEvent);
        void IsReady(CropReadyEvent cropReadyEvent);
    }
}
