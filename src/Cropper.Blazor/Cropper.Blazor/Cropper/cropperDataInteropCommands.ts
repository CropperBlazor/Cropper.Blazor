import { CropperImage } from "cropperjs";
import {
  CropperId,
  CropperInstanceParts,
  LegacyCanvasData,
  LegacyCropBoxData,
  LegacyCroppedCanvasOptions,
  LegacyCropperData,
  LegacySelectionData,
} from "./cropperJsInterop.types";

type GetParts = (id: CropperId) => CropperInstanceParts;
type GetLimitedSelection = (id: CropperId, selection: LegacySelectionData) => { x: number; y: number; width: number; height: number };
type EnsureVisibleSelection = (selection: CropperInstanceParts["selection"]) => void;
type RoundNumericProperties = <T extends Record<string, unknown>>(data: T) => T;
type GetRotationInDegrees = (transform: number[]) => number;
type GetSelectionCanvasOptions = (options?: LegacyCroppedCanvasOptions) => { width?: number; height?: number; beforeDraw: (context: CanvasRenderingContext2D, canvas: HTMLCanvasElement) => void };

export class CropperDataInteropCommands {
  constructor(
    private readonly getParts: GetParts,
    private readonly getLimitedSelection: GetLimitedSelection,
    private readonly ensureVisibleSelection: EnsureVisibleSelection,
    private readonly roundNumericProperties: RoundNumericProperties,
    private readonly getRotationInDegrees: GetRotationInDegrees,
    private readonly getSelectionCanvasOptions: GetSelectionCanvasOptions,
  ) {
  }

  getCanvasData(id: CropperId) {
    const { canvas } = this.getParts(id);
    const containerBounds = canvas.getBoundingClientRect();

    return {
      left: containerBounds.left,
      top: containerBounds.top,
      width: containerBounds.width,
      height: containerBounds.height,
      naturalWidth: containerBounds.width,
      naturalHeight: containerBounds.height,
    };
  }

  getContainerData(id: CropperId) {
    const { instance } = this.getParts(id);
    const containerBounds = instance.container.getBoundingClientRect();

    return {
      width: containerBounds.width,
      height: containerBounds.height,
    };
  }

  getCropBoxData(id: CropperId) {
    const { selection } = this.getParts(id);

    return {
      left: selection.x,
      top: selection.y,
      width: selection.width,
      height: selection.height,
    };
  }

  async getCroppedCanvas(id: CropperId, options: LegacyCroppedCanvasOptions) {
    const { selection } = this.getParts(id);

    this.ensureVisibleSelection(selection);

    return selection.$toCanvas(this.getSelectionCanvasOptions(options));
  }

  async getCroppedCanvasDataURL(
    id: CropperId,
    options: LegacyCroppedCanvasOptions,
    type?: string,
    encoderOptions?: number,
  ) {
    const canvas = await this.getCroppedCanvas(id, options);

    return canvas.toDataURL(type, encoderOptions);
  }

  getData(id: CropperId, rounded?: boolean) {
    const { image, selection } = this.getParts(id);
    const transform = image.$getTransform();
    const data = {
      x: selection.x,
      y: selection.y,
      width: selection.width,
      height: selection.height,
      rotate: this.getRotationInDegrees(transform),
      scaleX: transform[0],
      scaleY: transform[3],
    };

    return rounded ? this.roundNumericProperties(data) : data;
  }

  getImageData(id: CropperId) {
    const { image } = this.getParts(id);
    const bounds = image.getBoundingClientRect();
    const transform = image.$getTransform();
    const naturalWidth = image.$image.naturalWidth;
    const naturalHeight = image.$image.naturalHeight;

    return {
      left: bounds.left,
      top: bounds.top,
      width: bounds.width,
      height: bounds.height,
      rotate: this.getRotationInDegrees(transform),
      scaleX: transform[0],
      scaleY: transform[3],
      naturalWidth,
      naturalHeight,
      aspectRatio: naturalHeight ? naturalWidth / naturalHeight : 0,
    };
  }

  setAspectRatio(id: CropperId, ratio: number) {
    const { selection } = this.getParts(id);

    selection.aspectRatio = ratio;
    selection.$change(selection.x, selection.y, selection.width, selection.height, ratio, true);
  }

  setCanvasData(id: CropperId, data: LegacyCanvasData) {
    const { image } = this.getParts(id);
    const transform = image.$getTransform();

    transform[4] = data.left ?? transform[4];
    transform[5] = data.top ?? transform[5];

    if (data.width) {
      const bounds = image.getBoundingClientRect();
      const scaleRatio = bounds.width ? data.width / bounds.width : 1;
      transform[0] *= scaleRatio;
      transform[1] *= scaleRatio;
    }

    if (data.height) {
      const bounds = image.getBoundingClientRect();
      const scaleRatio = bounds.height ? data.height / bounds.height : 1;
      transform[2] *= scaleRatio;
      transform[3] *= scaleRatio;
    }

    return image.$setTransform(transform);
  }

  setCropBoxData(id: CropperId, data: LegacyCropBoxData) {
    const { selection } = this.getParts(id);
    const nextSelection = this.getLimitedSelection(id, {
      x: data.left ?? selection.x,
      y: data.top ?? selection.y,
      width: data.width ?? selection.width,
      height: data.height ?? selection.height,
    });
    const aspectRatio = data.width !== undefined || data.height !== undefined ? NaN : selection.aspectRatio;

    if (nextSelection.width > 0 && nextSelection.height > 0) {
      this.ensureVisibleSelection(selection);
    }

    return selection.$change(
      nextSelection.x,
      nextSelection.y,
      nextSelection.width,
      nextSelection.height,
      aspectRatio,
      true,
    );
  }

  setData(id: CropperId, data: LegacyCropperData) {
    const { image, selection } = this.getParts(id);
    const nextSelection = this.getLimitedSelection(id, {
      x: data.x ?? selection.x,
      y: data.y ?? selection.y,
      width: data.width ?? selection.width,
      height: data.height ?? selection.height,
    });

    if (nextSelection.width > 0 && nextSelection.height > 0) {
      this.ensureVisibleSelection(selection);
    }

    selection.$change(
      nextSelection.x,
      nextSelection.y,
      nextSelection.width,
      nextSelection.height,
      selection.aspectRatio,
      true,
    );

    if (data.rotate !== undefined || data.scaleX !== undefined || data.scaleY !== undefined) {
      const transform = image.$getTransform();
      const radians = ((data.rotate ?? this.getRotationInDegrees(transform)) * Math.PI) / 180;
      const scaleX = data.scaleX ?? (Math.hypot(transform[0], transform[1]) || 1);
      const scaleY = data.scaleY ?? (Math.hypot(transform[2], transform[3]) || 1);

      image.$setTransform(
        Math.cos(radians) * scaleX,
        Math.sin(radians) * scaleX,
        -Math.sin(radians) * scaleY,
        Math.cos(radians) * scaleY,
        transform[4],
        transform[5],
      );
    }
  }
}
