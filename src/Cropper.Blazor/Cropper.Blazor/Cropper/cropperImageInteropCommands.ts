import { CropperCanvas, CropperImage } from "cropperjs";
import { CropperId, CropperInstanceParts, LegacyCropperInstanceOptions, NullableNumber, ZoomLimits } from "./cropperJsInterop.types";

type GetParts = (id: CropperId) => CropperInstanceParts;
type GetImageZoomRatio = (image: CropperImage) => number;
type ClampZoomRatio = (id: CropperId, ratio: number) => number;
type DispatchZoomEvent = (id: CropperId, canvas: CropperCanvas, oldRatio: number, ratio: number) => void;
type GetSyntheticEvent = (type: string) => Event;

export class CropperImageInteropCommands {
  constructor(
    private readonly getParts: GetParts,
    private readonly getInstanceOptions: (id: CropperId) => LegacyCropperInstanceOptions | undefined,
    private readonly setZoomLimitsValue: (id: CropperId, value: ZoomLimits) => void,
    private readonly getImageZoomRatio: GetImageZoomRatio,
    private readonly clampZoomRatio: ClampZoomRatio,
    private readonly dispatchZoomEvent: DispatchZoomEvent,
    private readonly getSyntheticEvent: GetSyntheticEvent,
  ) {
  }

  center(id: CropperId, size: string) {
    const { image } = this.getParts(id);

    return image.$center(size);
  }

  move(id: CropperId, offsetX: number, offsetY?: number) {
    const { image } = this.getParts(id);

    return image.$move(offsetX, offsetY);
  }

  moveTo(id: CropperId, x: number, y?: number) {
    const { image } = this.getParts(id);

    return image.$moveTo(x, y);
  }

  replace(id: CropperId, url: string) {
    const { image } = this.getParts(id);

    image.src = url;
  }

  rotate(id: CropperId, degree: number) {
    const { image, selection } = this.getParts(id);
    const result = image.$rotate(`${degree}deg`);

    selection.dispatchEvent(this.getSyntheticEvent("change"));

    return result;
  }

  rotateTo(id: CropperId, degree: number) {
    const { image } = this.getParts(id);
    const transform = image.$getTransform();
    const scaleX = Math.hypot(transform[0], transform[1]) || 1;
    const scaleY = Math.hypot(transform[2], transform[3]) || 1;
    const radians = (degree * Math.PI) / 180;

    return image.$setTransform(
      Math.cos(radians) * scaleX,
      Math.sin(radians) * scaleX,
      -Math.sin(radians) * scaleY,
      Math.cos(radians) * scaleY,
      transform[4],
      transform[5],
    );
  }

  scale(id: CropperId, x: number, y?: number) {
    const { image } = this.getParts(id);

    return image.$scale(x, y);
  }

  scaleX(id: CropperId, x: number) {
    const { image, selection } = this.getParts(id);
    const transform = image.$getTransform();
    const currentScaleY = transform[3] || 1;
    const result = image.$scale(x, currentScaleY);

    selection.dispatchEvent(this.getSyntheticEvent("change"));

    return result;
  }

  scaleY(id: CropperId, y: number) {
    const { image } = this.getParts(id);
    const transform = image.$getTransform();
    const currentScaleX = transform[0] || 1;

    return image.$scale(currentScaleX, y);
  }

  zoom(id: CropperId, ratio: number) {
    const { canvas, image } = this.getParts(id);
    const options = this.getInstanceOptions(id);

    if (options?.zoomOnWheel === false && options?.zoomOnTouch === false) {
      return image;
    }

    const oldRatio = this.getImageZoomRatio(image);
    const nextRatio = this.clampZoomRatio(id, oldRatio * (1 + ratio));
    const result = image.$zoom((nextRatio / oldRatio) - 1);

    this.dispatchZoomEvent(id, canvas, oldRatio, this.getImageZoomRatio(image));

    return result;
  }

  zoomTo(id: CropperId, ratio: number, pivotX: number, pivotY: number) {
    const { canvas, image } = this.getParts(id);
    const currentScale = this.getImageZoomRatio(image);
    const nextRatio = this.clampZoomRatio(id, ratio);
    const relativeRatio = nextRatio / currentScale - 1;
    const result = image.$zoom(relativeRatio, pivotX, pivotY);

    this.dispatchZoomEvent(id, canvas, currentScale, this.getImageZoomRatio(image));

    return result;
  }

  setZoomLimits(id: CropperId, minRatio?: NullableNumber, maxRatio?: NullableNumber) {
    this.setZoomLimitsValue(id, { minRatio, maxRatio });
  }
}
