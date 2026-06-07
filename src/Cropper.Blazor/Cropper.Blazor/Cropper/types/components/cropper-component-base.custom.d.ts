import Cropper, { CropperImage } from "cropperjs";

export namespace CropperBlazor.Components {
  /** Represents the C# cropper component methods callable from JS */
  export interface ICropperComponentBase {
    /** Called when the cropper is ready */
    IsReady(eventData: unknown): void;

    /** Called when cropping starts */
    CropperIsStarted(eventData: unknown): void;

    /** Called when cropping is moving */
    CropperIsMoved(eventData: unknown): void;

    /** Called when cropping ends */
    CropperIsEnded(eventData: unknown): void;

    /** Called on crop event */
    CropperIsCroped(eventData: unknown): void;

    /** Called on zoom event */
    CropperIsZoomed(eventData: unknown): void;
  }
}
