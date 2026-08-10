import Cropper, { CropperCanvas, CropperImage, CropperSelection, CropperViewer } from "cropperjs";

export type CropperId = string;

export type CropperEventHandlerRegistration = {
  element: EventTarget;
  type: string;
  listener: EventListener;
};

export type NullableNumber = number | null | undefined;

export type LegacyCropperData = {
  x?: NullableNumber;
  y?: NullableNumber;
  width?: NullableNumber;
  height?: NullableNumber;
  rotate?: NullableNumber;
  scaleX?: NullableNumber;
  scaleY?: NullableNumber;
};

export type LegacyCanvasData = {
  left?: NullableNumber;
  top?: NullableNumber;
  width?: NullableNumber;
  height?: NullableNumber;
};

export type LegacyCropBoxData = LegacyCanvasData;

export type LegacySelectionData = LegacyCropBoxData & {
  x?: NullableNumber;
  y?: NullableNumber;
};

export type LegacyCroppedCanvasOptions = {
  width?: NullableNumber;
  height?: NullableNumber;
  minWidth?: NullableNumber;
  minHeight?: NullableNumber;
  maxWidth?: NullableNumber;
  maxHeight?: NullableNumber;
  fillColor?: string | null;
  imageSmoothingEnabled?: boolean | null;
  imageSmoothingQuality?: ImageSmoothingQuality | null;
  rounded?: boolean | null;
};

export type CropperInstanceParts = {
  instance: Cropper;
  canvas: CropperCanvas;
  image: CropperImage;
  selection: CropperSelection;
};

export type CropperWithSelections = Cropper & {
  getCropperSelections?: () => NodeListOf<CropperSelection>;
};

export type CropperSelectionActionDetail = {
  action?: string;
  endX?: NullableNumber;
  endY?: NullableNumber;
  relatedEvent?: Event | null;
  startX?: NullableNumber;
  startY?: NullableNumber;
};

export type SanitizedOriginalEvent = {
  button?: number;
  buttons?: number;
  clientX?: number;
  clientY?: number;
  deltaX?: number;
  deltaY?: number;
  pageX?: number;
  pageY?: number;
  pointerType?: string;
  shiftKey?: boolean;
  type: string;
};

export type CropperSelectionShape = "default" | "close" | "pentagon" | "circle" | "arrow";

export type CropperSelectionData = {
  index: number;
  active: boolean;
  x: number;
  y: number;
  width: number;
  height: number;
  aspectRatio?: NullableNumber;
  shape?: string | null;
};

export type LegacyCropperInstanceOptions = {
  selectionMaximumCount?: NullableNumber;
  zoomOnTouch: boolean;
  zoomOnWheel: boolean;
};

export type ZoomLimits = {
  minRatio?: NullableNumber;
  maxRatio?: NullableNumber;
};

export type ZoomEventRatio = {
  oldRatio: number;
  ratio: number;
};

export type CropperRuntimeState = {
  instances: Record<CropperId, Cropper>;
  eventHandlers: Record<CropperId, CropperEventHandlerRegistration[]>;
  instanceOptions: Record<CropperId, LegacyCropperInstanceOptions>;
  viewers: WeakMap<HTMLElement, CropperViewer>;
  zoomLimits: Record<CropperId, ZoomLimits>;
  zoomEventRatios: Record<CropperId, ZoomEventRatio>;
};

export type LimitedSelection = {
  x: number;
  y: number;
  width: number;
  height: number;
  changed: boolean;
};
