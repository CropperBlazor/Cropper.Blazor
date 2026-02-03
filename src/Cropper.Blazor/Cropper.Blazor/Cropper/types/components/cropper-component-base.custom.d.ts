import Cropper from "cropperjs";

export namespace CropperBlazor.Components {
  /** Represents the C# cropper component methods callable from JS */
  export interface ICropperComponentBase {
    /** Called when the cropper is ready */
    IsReady(eventData: Cropper.CropEventData | Cropper.ZoomEventData): void;

    /** Called when cropping starts */
    CropperIsStarted(eventData: Cropper.CropEventData): void;

    /** Called when cropping is moving */
    CropperIsMoved(eventData: Cropper.CropEventData): void;

    /** Called when cropping ends */
    CropperIsEnded(eventData: Cropper.CropEventData): void;

    /** Called on crop event */
    CropperIsCroped(eventData: Cropper.CropEventData): void;

    /** Called on zoom event */
    CropperIsZoomed(eventData: Cropper.ZoomEventData): void;
  }
}
