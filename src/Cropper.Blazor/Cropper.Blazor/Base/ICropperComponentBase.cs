using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;

namespace Cropper.Blazor.Base
{
    /// <summary>
    /// Provides the metadata of a ICropperComponentBase
    /// </summary>
    public interface ICropperComponentBase
    {
        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box changes.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   When the autoCrop option is set to the true, a crop event will be triggered before the ready event.
        ///  </para>
        ///  <para>
        ///   When the data option is set, another crop event will be triggered before the ready event
        ///  </para>
        /// </remarks>
        /// <param name="cropEvent">CropEvent</param>
        void CropperIsCroped(CropEvent cropEvent);

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box stops changing.
        /// </summary>
        /// <param name="cropEndEvent">CropEndEvent</param>
        void CropperIsEnded(CropEndEvent cropEndEvent);

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box is changing.
        /// </summary>
        /// <param name="cropMoveEvent">CropMoveEvent</param>
        void CropperIsMoved(CropMoveEvent cropMoveEvent);

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box starts to change.
        /// </summary>
        /// <param name="cropStartEvent">CropStartEvent</param>
        void CropperIsStarted(CropStartEvent cropStartEvent);

        /// <summary>
        /// This event fires when a cropper instance starts to zoom in or zoom out its canvas (image wrapper).
        /// </summary>
        /// <param name="zoomEvent">ZoomEvent</param>
        void CropperIsZoomed(ZoomEvent zoomEvent);

        /// <summary>
        /// This event fires when the target image has been loaded and the cropper instance is ready for operating.
        /// </summary>
        /// <param name="cropReadyEvent">CropReadyEvent</param>
        void IsReady(CropReadyEvent cropReadyEvent);
    }
}
