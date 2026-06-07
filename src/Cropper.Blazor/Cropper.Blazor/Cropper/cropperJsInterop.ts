import Cropper, {
  ACTION_MOVE,
  ACTION_NONE,
  ACTION_SCALE,
  ACTION_SELECT,
  CropperCanvas,
  CropperImage,
  CropperOptions,
  CropperSelection,
  CropperViewer,
  CROPPER_SELECTION,
  DEFAULT_TEMPLATE,
} from "cropperjs";
import type { CropperBlazor as CropperComponentBaseTypes } from "./types/components/cropper-component-base.custom";
import type { CropperBlazor as CroppedCanvasReceiverTypes } from "./types/components/cropped-canvas-receiver.custom";
import type { CropperBlazor as ImageReceiverTypes } from "./types/components/image-receiver.custom";
import type { CropperBlazor as DataEventTypes } from "./types/data/cropper-event-data";
import type { CropperBlazor as DataOptionsTypes } from "./types/data/cropper-extended-options";
import type { CropperBlazor as DotNetTypes } from "./types/global/dotnet-global.custom";
import { CropperBlazor as BlobHelper } from "./helpers/blob-helper";
import { CropperBlazor as UrlImageHelper } from "./helpers/cropper-url-image-helper";
import { CropperCanvasInteropCommands } from "./cropperCanvasInteropCommands";
import { CropperDataInteropCommands } from "./cropperDataInteropCommands";
import { CropperImageInteropCommands } from "./cropperImageInteropCommands";
import { CropperSelectionInteropCommands } from "./cropperSelectionInteropCommands";
import { CropperViewerInteropCommands } from "./cropperViewerInteropCommands";
import type {
  CropperId,
  CropperInstanceParts,
  CropperRuntimeState,
  CropperSelectionActionDetail,
  CropperSelectionShape,
  CropperWithSelections,
  LegacyCanvasData,
  LegacyCroppedCanvasOptions,
  LegacyCropBoxData,
  LegacyCropperData,
  LegacyCropperInstanceOptions,
  LegacySelectionData,
  NullableNumber,
  SanitizedOriginalEvent,
} from "./cropperJsInterop.types";

declare global {
  interface Window {
    cropper: CropperDecorator;
    cropperUrlImageHelper: UrlImageHelper.Helpers.CropperUrlImageHelper;
  }
}

declare const DotNet: DotNetTypes.Global.DotNetNamespace;

export class CropperDecorator {
  private readonly state: CropperRuntimeState = {
    instances: {},
    eventHandlers: {},
    instanceOptions: {},
    viewers: new WeakMap<HTMLElement, CropperViewer>(),
    zoomLimits: {},
    zoomEventRatios: {},
  };
  private readonly canvasCommands = new CropperCanvasInteropCommands(
    (id) => this.getCropperParts(id),
    (action) => this.getCanvasAction(action),
  );
  private readonly imageCommands = new CropperImageInteropCommands(
    (id) => this.getCropperParts(id),
    (id) => this.state.instanceOptions[id],
    (id, value) => { this.state.zoomLimits[id] = value; },
    (image) => this.getImageZoomRatio(image),
    (id, ratio) => this.clampZoomRatio(id, ratio),
    (id, canvas, oldRatio, ratio) => this.dispatchZoomEvent(id, canvas, oldRatio, ratio),
    (type) => this.getSyntheticEvent(type),
  );
  private readonly selectionCommands = new CropperSelectionInteropCommands(
    (id) => this.getCropperParts(id),
    (id) => this.getSelections(this.getCropperParts(id).instance),
    (id, selection) => this.getLimitedSelection(id, selection),
    (selection) => this.ensureVisibleSelection(selection),
    (options) => this.getCropperSelectionCanvasOptions(options),
  );
  private readonly dataCommands = new CropperDataInteropCommands(
    (id) => this.getCropperParts(id),
    (id, selection) => this.getLimitedSelection(id, selection),
    (selection) => this.ensureVisibleSelection(selection),
    (data) => this.roundNumericProperties(data),
    (transform) => this.getRotationInDegrees(transform),
    (options) => this.getCropperSelectionCanvasOptions(options),
  );
  private readonly viewerCommands = new CropperViewerInteropCommands(
    (id) => this.tryGetCropperParts(id),
    this.state.viewers,
  );

  clear(id: CropperId) {
    const { selection } = this.getCropperParts(id);

    selection.$clear();

    selection.dispatchEvent(this.getSyntheticEvent("change"));
  }

  crop(id: CropperId) {
    const { selection } = this.getCropperParts(id);

    this.ensureVisibleSelection(selection);

    selection.dispatchEvent(this.getSyntheticEvent("change"));
  }

  destroy(id: CropperId) {
    const instance: Cropper = this.state.instances[id];

    if (instance) {
      this.unregisterEvents(id);
      instance.destroy();

      delete this.state.instances[id];
      delete this.state.instanceOptions[id];
      delete this.state.zoomLimits[id];
      delete this.state.zoomEventRatios[id];
    }
  }

  disable(id: CropperId) {
    this.canvasCommands.disable(id);
  }

  enable(id: CropperId) {
    this.canvasCommands.enable(id);
  }

  getCanvasData(id: CropperId) {
    return this.dataCommands.getCanvasData(id);
  }

  getContainerData(id: CropperId) {
    return this.dataCommands.getContainerData(id);
  }

  getCropBoxData(id: CropperId) {
    return this.dataCommands.getCropBoxData(id);
  }

  async getCroppedCanvas(id: CropperId, options: LegacyCroppedCanvasOptions) {
    return this.dataCommands.getCroppedCanvas(id, options);
  }

  async getCroppedCanvasInBackground(
    id: CropperId,
    options: LegacyCroppedCanvasOptions,
    dotNetCanvasReceiverRef: DotNetTypes.Global.DotNetObjectReference<CroppedCanvasReceiverTypes.Components.CroppedCanvasReceiver>,
  ) {
    setTimeout(async () => {
      const canvas: HTMLCanvasElement = await this.getCroppedCanvas(id, options);
      const jsRef: DotNetTypes.Global.JsObjectReference = DotNet.createJSObjectReference(canvas);

      await dotNetCanvasReceiverRef.invokeMethodAsync("ReceiveCanvasReference", jsRef);
    }, 0);
  }

  async getCroppedCanvasDataURL(
    id: CropperId,
    options: LegacyCroppedCanvasOptions,
    type?: string,
    encoderOptions?: number,
  ) {
    return this.dataCommands.getCroppedCanvasDataURL(id, options, type, encoderOptions);
  }

  getData(id: CropperId, rounded?: boolean) {
    return this.dataCommands.getData(id, rounded);
  }

  getImageData(id: CropperId) {
    return this.dataCommands.getImageData(id);
  }

  move(id: CropperId, offsetX: number, offsetY?: number) {
    return this.imageCommands.move(id, offsetX, offsetY);
  }

  moveTo(id: CropperId, x: number, y?: number) {
    return this.imageCommands.moveTo(id, x, y);
  }

  center(id: CropperId, size: string) {
    return this.imageCommands.center(id, size);
  }

  replace(id: CropperId, url: string) {
    this.imageCommands.replace(id, url);
  }

  reset(id: CropperId) {
    const { image, selection } = this.getCropperParts(id);

    image.$resetTransform();
    selection.$reset();
  }

  resetSelection(id: CropperId) {
    return this.selectionCommands.resetSelection(id);
  }

  centerSelection(id: CropperId) {
    return this.selectionCommands.centerSelection(id);
  }

  clearSelection(id: CropperId) {
    this.selectionCommands.clearSelection(id);
  }

  moveSelectionTo(id: CropperId, x: number, y: number) {
    return this.selectionCommands.moveSelectionTo(id, x, y);
  }

  changeSelection(id: CropperId, x: number, y: number, width: number, height: number, aspectRatio?: NullableNumber) {
    return this.selectionCommands.changeSelection(id, x, y, width, height, aspectRatio);
  }

  changeSelectionByIndex(id: CropperId, index: number, x: number, y: number, width: number, height: number, aspectRatio?: NullableNumber) {
    return this.selectionCommands.changeSelectionByIndex(id, index, x, y, width, height, aspectRatio);
  }

  createSelection(id: CropperId, x: number, y: number, width: number, height: number) {
    this.selectionCommands.createSelection(id, x, y, width, height);
  }

  removeSelectionByIndex(id: CropperId, index: number) {
    this.selectionCommands.removeSelectionByIndex(id, index);
  }

  setSelectionShapeByIndex(id: CropperId, index: number, shape: CropperSelectionShape) {
    this.selectionCommands.setSelectionShapeByIndex(id, index, shape);
  }

  getSelectionCount(id: CropperId) {
    return this.selectionCommands.getSelectionCount(id);
  }

  getSelectionsData(id: CropperId) {
    return this.selectionCommands.getSelectionsData(id);
  }

  async selectionToCanvasDataURL(
    id: CropperId,
    options: LegacyCroppedCanvasOptions,
    type?: string,
    encoderOptions?: number,
  ) {
    return this.selectionCommands.selectionToCanvasDataURL(id, options, type, encoderOptions);
  }

  async selectionToCanvasByIndex(id: CropperId, index: number, options: LegacyCroppedCanvasOptions) {
    const canvas = await this.selectionCommands.selectionToCanvasByIndex(id, index, options);

    return canvas ? DotNet.createJSObjectReference(canvas) : null;
  }

  initializeViewer(id: CropperId, viewerElement: HTMLElement | null, selectionIndexOrViewerElementId?: number | string, viewerElementId?: string) {
    this.viewerCommands.initializeViewer(id, viewerElement, selectionIndexOrViewerElementId, viewerElementId);
  }

  rotate(id: CropperId, degree: number) {
    return this.imageCommands.rotate(id, degree);
  }

  rotateTo(id: CropperId, degree: number) {
    return this.imageCommands.rotateTo(id, degree);
  }

  scale(id: CropperId, x: number, y?: number) {
    return this.imageCommands.scale(id, x, y);
  }

  scaleX(id: CropperId, x: number) {
    return this.imageCommands.scaleX(id, x);
  }

  scaleY(id: CropperId, y: number) {
    return this.imageCommands.scaleY(id, y);
  }

  setAspectRatio(id: CropperId, ratio: number) {
    this.dataCommands.setAspectRatio(id, ratio);
  }

  setCanvasData(id: CropperId, data: LegacyCanvasData) {
    return this.dataCommands.setCanvasData(id, data);
  }

  setCropBoxData(id: CropperId, data: LegacyCropBoxData) {
    return this.dataCommands.setCropBoxData(id, data);
  }

  setData(id: CropperId, data: LegacyCropperData) {
    this.dataCommands.setData(id, data);
  }

  setDragMode(id: CropperId, mode: string) {
    this.canvasCommands.setDragMode(id, mode);
  }

  zoom(id: CropperId, ratio: number) {
    return this.imageCommands.zoom(id, ratio);
  }

  zoomTo(id: CropperId, ratio: number, pivotX: number, pivotY: number) {
    return this.imageCommands.zoomTo(id, ratio, pivotX, pivotY);
  }

  setZoomLimits(id: CropperId, minRatio?: NullableNumber, maxRatio?: NullableNumber) {
    this.imageCommands.setZoomLimits(id, minRatio, maxRatio);
  }

  setSelectionShape(id: CropperId, shape: CropperSelectionShape) {
    this.selectionCommands.setSelectionShape(id, shape);
  }

  get cropperInstances() {
    return this.state.instances;
  }

  noConflict() {
    return Cropper;
  }

  private clampZoomRatio(id: CropperId, ratio: number) {
    const limits = this.state.zoomLimits[id];

    return this.clamp(ratio, limits?.minRatio, limits?.maxRatio)
      ?? /* istanbul ignore next -- clamp only returns undefined when the caller passes undefined; zoom callers always pass a number. */ ratio;
  }

  private dispatchZoomEvent(id: CropperId, canvas: CropperCanvas, oldRatio: number, ratio: number) {
    this.state.zoomEventRatios[id] = { oldRatio, ratio };
    const bounds = canvas.getBoundingClientRect();
    const relatedEvent = this.getSyntheticMouseEvent("wheel", canvas, {
      bubbles: true,
      cancelable: true,
      clientX: bounds.left + bounds.width / 2,
      clientY: bounds.top + bounds.height / 2,
      shiftKey: false,
    });

    canvas.dispatchEvent(new CustomEvent("action", {
      bubbles: true,
      cancelable: true,
      detail: {
        action: ACTION_SCALE,
        oldRatio,
        ratio,
        relatedEvent,
      },
    }));
  }

  private getImageZoomRatio(image: CropperImage) {
    const transform = image.$getTransform();

    return Math.hypot(transform[0], transform[1]) || 1;
  }

  setDefaults(_options: DataOptionsTypes.Data.CropperExtendedOptions) {
    console.warn("Cropper.js 2 does not support global setDefaults. Pass options to each CropperComponent instance instead.");
  }

  // --------------------------
  // Event serialization helpers
  // --------------------------

  getJSEventData(
    instance: Event | CustomEvent,
    correlationId: string | undefined,
    detail?: DataEventTypes.Data.CropperEventDataJS,
  ): DataEventTypes.Data.CropperJSEventData {
    return {
      isTrusted: instance.isTrusted,
      detail: detail ?? this.getJSEventDataDetail(instance),
      type: this.getLegacyEventName(instance),
      eventPhase: instance.eventPhase,
      bubbles: instance.bubbles,
      cancelable: instance.cancelable,
      defaultPrevented: instance.defaultPrevented,
      composed: instance.composed,
      timeStamp: instance.timeStamp,
      returnValue: instance.returnValue,
      cancelBubble: instance.cancelBubble,
      correlationId,
    };
  }

  getJSEventDataDetail(instance: Event | CustomEvent): DataEventTypes.Data.CropperEventDataJS {
    const customEvent = instance as CustomEvent;
    const originalEvent = customEvent.detail?.relatedEvent ?? customEvent.detail?.originalEvent;

    if (customEvent.detail?.action === ACTION_SCALE) {
      return {
        oldRatio: customEvent.detail?.oldRatio ?? customEvent.detail?.ratio ?? 0,
        ratio: customEvent.detail?.ratio ?? customEvent.detail?.scale ?? 0,
        originalEvent: this.getSanitizedOriginalEvent(originalEvent),
      };
    }

    return {
      action: customEvent.detail?.action ?? ACTION_NONE,
      originalEvent: this.getSanitizedOriginalEvent(originalEvent),
    };
  }

  private getSanitizedOriginalEvent(originalEvent: Event | null | undefined): SanitizedOriginalEvent | null {
    if (!originalEvent) {
      return null;
    }

    const mouseEvent = originalEvent instanceof MouseEvent ? originalEvent : null;
    const pointerEvent = "PointerEvent" in window && originalEvent instanceof PointerEvent ? originalEvent : null;
    const wheelEvent = originalEvent instanceof WheelEvent ? originalEvent : null;

    return {
      button: mouseEvent?.button,
      buttons: mouseEvent?.buttons,
      clientX: mouseEvent?.clientX,
      clientY: mouseEvent?.clientY,
      deltaX: wheelEvent?.deltaX,
      deltaY: wheelEvent?.deltaY,
      pageX: mouseEvent?.pageX,
      pageY: mouseEvent?.pageY,
      pointerType: pointerEvent?.pointerType,
      shiftKey: mouseEvent?.shiftKey,
      type: originalEvent.type,
    };
  }

  onReady(
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    id: string | undefined,
  ) {
    imageObject.invokeMethodAsync("IsReady", this.getSyntheticJSEventData("ready", id));
  }

  onCropStart(
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    e: Event,
    id: string | undefined,
  ) {
    imageObject.invokeMethodAsync("CropperIsStarted", this.getJSEventData(e, id));
  }

  onCropMove(
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    e: Event,
    id: string | undefined,
  ) {
    imageObject.invokeMethodAsync("CropperIsMoved", this.getJSEventData(e, id));
  }

  onCropEnd(
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    e: Event,
    id: string | undefined,
  ) {
    imageObject.invokeMethodAsync("CropperIsEnded", this.getJSEventData(e, id));
  }

  onCrop(
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    e: Event,
    id: string | undefined,
  ) {
    const detail = this.getData(id ?? "", false) as unknown as DataEventTypes.Data.CropperEventDataJS;

    imageObject.invokeMethodAsync("CropperIsCroped", this.getJSEventData(e, id, detail));
  }

  onZoom(
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    e: Event,
    id: string | undefined,
  ) {
    const customEvent = e as CustomEvent;
    const image = id ? this.getCropperParts(id).image : undefined;
    const cachedRatios = id ? this.state.zoomEventRatios[id] : undefined;
    const imageRatio = image ? this.getImageZoomRatio(image) : undefined;
    const scale = customEvent.detail?.scale;
    const oldRatio = customEvent.detail?.oldRatio
      ?? (scale !== undefined && imageRatio !== undefined ? imageRatio / (1 + scale) : undefined)
      ?? customEvent.detail?.ratio
      ?? cachedRatios?.oldRatio
      ?? imageRatio
      ?? 0;
    const requestedRatio = customEvent.detail?.ratio
      ?? (scale !== undefined ? oldRatio * (1 + scale) : undefined)
      ?? cachedRatios?.ratio
      ?? imageRatio
      ?? 0;
    const ratio = id ? this.clampZoomRatio(id, requestedRatio) : requestedRatio;

    if (id && image && imageRatio && Math.abs(requestedRatio - ratio) > 0.0001) {
      e.preventDefault();
      e.stopImmediatePropagation();
      image.$zoom((ratio / imageRatio) - 1);
    }
    else if (image && imageRatio && Math.abs(imageRatio - ratio) > 0.0001) {
      image.$zoom((ratio / imageRatio) - 1);
    }

    imageObject.invokeMethodAsync("CropperIsZoomed", this.getJSEventData(e, id, {
      oldRatio,
      ratio,
      originalEvent: customEvent.detail?.relatedEvent ?? customEvent.detail?.originalEvent ?? null,
    }));
  }

  initCropper(
    id: CropperId,
    image: HTMLImageElement | HTMLCanvasElement,
    optionsImage: DataOptionsTypes.Data.CropperExtendedOptions,
    imageObject?: any,
  ) {
    if (!image) throw new Error("Parameter 'image' must not be null");
    if (!optionsImage) throw new Error("Parameter 'optionsImage' must not be null");
    optionsImage.correlationId = id;
    const container = image.parentElement ?? undefined;
    const options: CropperOptions = {
      container,
      template: this.getTemplate(optionsImage),
    };
    const correlationId: string | undefined = optionsImage.correlationId;

    try {
      if (image instanceof HTMLImageElement || image instanceof HTMLCanvasElement) {
        this.destroy(id);

        const cropper: Cropper = new Cropper(image, options);
        this.state.instances[id] = cropper;
        this.applyOptions(id, optionsImage);

        if (imageObject) {
          this.registerEvents(id, imageObject, correlationId);
        }
      } else {
        throw new TypeError(
          `Unsupported element type for Cropper: ${Object.prototype.toString.call(image)}`,
        );
      }
    }
    catch (error) {
      console.log(error);
    }

  }

  // --------------------------
  // Chunked Blob Streaming
  // --------------------------

  async readBlobInChunks(
    blob: Blob | null,
    dotNetImageReceiverRef: DotNetTypes.Global.DotNetObjectReference<ImageReceiverTypes.Components.ImageReceiver>,
    maximumReceiveChunkSize?: number,
  ) {
    await BlobHelper.Helpers.readBlobInChunks(
      blob,
      dotNetImageReceiverRef,
      maximumReceiveChunkSize,
    );
  }

  sendImageInChunks(
    cropperComponentId: CropperId,
    options: LegacyCroppedCanvasOptions,
    dotNetImageReceiverRef: DotNetTypes.Global.DotNetObjectReference<ImageReceiverTypes.Components.ImageReceiver>,
    type?: string,
    encoderOptions?: number,
    maximumReceiveChunkSize?: number,
  ) {
    setTimeout(async () => {
      const canvas = await this.getCroppedCanvas(cropperComponentId, options);

      canvas.toBlob(
        async (blob: Blob | null) => {
          await this.readBlobInChunks(blob, dotNetImageReceiverRef, maximumReceiveChunkSize);
        },
        type,
        encoderOptions,
      );
    }, 0);
  }

  private getCropperParts(id: CropperId): CropperInstanceParts {
    const parts = this.tryGetCropperParts(id);

    if (!parts) {
      throw new Error(`Cropper instance '${id}' was not found.`);
    }

    return parts;
  }

  private tryGetCropperParts(id: CropperId): CropperInstanceParts | undefined {
    const instance = this.state.instances[id];

    if (!instance) {
      return undefined;
    }

    const canvas = instance.getCropperCanvas();
    const image = instance.getCropperImage();
    const selection = this.getActiveSelection(instance);

    if (!canvas || !image || !selection) {
      return undefined;
    }

    return { instance, canvas, image, selection };
  }

  private getActiveSelection(instance: Cropper): CropperSelection | null {
    const selections = this.getSelections(instance);

    return selections.find((selection) => selection.active)
      ?? instance.getCropperSelection();
  }

  private getSelections(instance: Cropper): CropperSelection[] {
    return Array.from((instance as CropperWithSelections).getCropperSelections?.() ?? []);
  }

  private getSelectionFromEvent(event: Event): CropperSelection | null {
    const target = event.target;

    return target instanceof Element && target.localName === CROPPER_SELECTION
      ? target as CropperSelection
      : null;
  }

  private getTemplate(options: DataOptionsTypes.Data.CropperExtendedOptions): string {
    if (!options) {
      return DEFAULT_TEMPLATE;
    }

    const canvasAttributes = [
      this.booleanAttribute("hidden", options.canvasOptions?.hidden ?? options.canvasHidden ?? false),
      this.booleanAttribute("background", options.canvasOptions?.background ?? options.background ?? true),
      this.booleanAttribute("disabled", options.canvasOptions?.disabled ?? options.disabled ?? false),
      `scale-step="${options.canvasOptions?.scaleStep ?? options.wheelZoomRatio ?? 0.1}"`,
      this.stringAttribute("theme-color", options.canvasOptions?.themeColor ?? options.canvasThemeColor),
      this.booleanAttribute("slottable", options.canvasOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const imageAttributes = [
      this.booleanAttribute("hidden", options.imageOptions?.hidden ?? options.imageHidden ?? false),
      this.booleanAttribute("rotatable", options.imageOptions?.rotatable ?? options.rotatable ?? true),
      this.booleanAttribute("scalable", options.imageOptions?.scalable ?? options.scalable ?? true),
      this.booleanAttribute("skewable", options.imageOptions?.skewable ?? options.skewable ?? true),
      this.booleanAttribute("translatable", options.imageOptions?.translatable ?? options.movable ?? true),
      this.stringAttribute("initial-center-size", options.imageOptions?.initialCenterSize ?? options.imageInitialCenterSize),
      this.stringAttribute("alt", options.imageOptions?.alt ?? options.imageAlt),
      this.booleanAttribute("slottable", options.imageOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const shadeAttributes = [
      this.booleanAttribute("hidden", options.shadeOptions?.hidden ?? options.shadeHidden ?? true),
      this.numberAttribute("x", options.shadeOptions?.x),
      this.numberAttribute("y", options.shadeOptions?.y),
      this.numberAttribute("width", options.shadeOptions?.width),
      this.numberAttribute("height", options.shadeOptions?.height),
      this.stringAttribute("theme-color", options.shadeOptions?.themeColor ?? options.shadeThemeColor),
      this.booleanAttribute("slottable", options.shadeOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const handleAttributes = [
      `action="${this.getCanvasAction(options.handleOptions?.action ?? options.handleAction ?? options.dragMode)}"`,
      this.booleanAttribute("hidden", options.handleOptions?.hidden ?? options.handleHidden ?? false),
      this.booleanAttribute("plain", options.handleOptions?.plain ?? options.handlePlain ?? true),
      this.stringAttribute("theme-color", options.handleOptions?.themeColor ?? options.handleThemeColor ?? "rgba(51, 153, 255, 0.5)"),
      this.booleanAttribute("slottable", options.handleOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const selectionAttributes = [
      `id="cropper-selection-${options.correlationId ?? crypto.randomUUID()}"`,
      this.booleanAttribute("hidden", options.selectionOptions?.hidden ?? options.selectionHidden ?? options.autoCrop === false),
      this.numberAttribute("x", options.selectionOptions?.x),
      this.numberAttribute("y", options.selectionOptions?.y),
      this.numberAttribute("width", options.selectionOptions?.width),
      this.numberAttribute("height", options.selectionOptions?.height),
      `initial-coverage="${options.selectionOptions?.initialCoverage ?? this.getInitialCoverage(options)}"`,
      this.numberAttribute("aspect-ratio", options.selectionOptions?.aspectRatio ?? options.aspectRatio),
      this.numberAttribute("initial-aspect-ratio", options.selectionOptions?.initialAspectRatio ?? options.initialAspectRatio),
      this.booleanAttribute("dynamic", options.selectionOptions?.dynamic ?? options.selectionDynamic ?? false),
      this.booleanAttribute("movable", options.selectionOptions?.movable ?? options.cropBoxMovable ?? true),
      this.booleanAttribute("resizable", options.selectionOptions?.resizable ?? options.cropBoxResizable ?? true),
      this.booleanAttribute("zoomable", options.selectionOptions?.zoomable ?? options.zoomable ?? false),
      this.booleanAttribute("multiple", options.selectionOptions?.multiple ?? options.selectionMultiple ?? false),
      this.booleanAttribute("keyboard", options.selectionOptions?.keyboard ?? options.keyboard ?? false),
      this.booleanAttribute("outlined", options.selectionOptions?.outlined ?? options.outlined ?? false),
      this.booleanAttribute("precise", options.selectionOptions?.precise ?? options.selectionPrecise ?? false),
      this.booleanAttribute("slottable", options.selectionOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const gridAttributes = [
      "role=\"grid\"",
      this.booleanAttribute("hidden", options.gridOptions?.hidden ?? options.gridHidden ?? false),
      `rows="${options.gridOptions?.rows ?? options.gridRows ?? 3}"`,
      `columns="${options.gridOptions?.columns ?? options.gridColumns ?? 3}"`,
      this.booleanAttribute("bordered", options.gridOptions?.bordered ?? options.gridBordered ?? true),
      this.booleanAttribute("covered", options.gridOptions?.covered ?? options.gridCovered ?? true),
      this.stringAttribute("theme-color", options.gridOptions?.themeColor ?? options.gridThemeColor),
      this.booleanAttribute("slottable", options.gridOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const crosshairAttributes = [
      this.booleanAttribute("hidden", options.crosshairOptions?.hidden ?? options.crosshairHidden ?? false),
      this.booleanAttribute("centered", options.crosshairOptions?.centered ?? options.crosshairCentered ?? true),
      this.stringAttribute("theme-color", options.crosshairOptions?.themeColor ?? options.crosshairThemeColor),
      this.booleanAttribute("slottable", options.crosshairOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
    const moveHandleAttributes = [
      `action="${this.getSelectionMoveAction(options.moveHandleOptions?.action ?? options.moveHandleAction)}"`,
      this.booleanAttribute("hidden", options.moveHandleOptions?.hidden ?? options.moveHandleHidden ?? false),
      this.stringAttribute("theme-color", options.moveHandleOptions?.themeColor ?? options.moveHandleThemeColor ?? "rgba(255, 255, 255, 0.35)"),
      this.booleanAttribute("slottable", options.moveHandleOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");

    return `<cropper-canvas ${canvasAttributes}>`
      + `<cropper-image ${imageAttributes}></cropper-image>`
      + `${options.modal ?? true ? `<cropper-shade ${shadeAttributes}></cropper-shade>` : ""}`
      + `<cropper-handle ${handleAttributes}></cropper-handle>`
      + `<cropper-selection ${selectionAttributes}>`
      + `${options.guides ?? true ? `<cropper-grid ${gridAttributes}></cropper-grid>` : ""}`
      + `${options.center ?? true ? `<cropper-crosshair ${crosshairAttributes}></cropper-crosshair>` : ""}`
      + `${options.highlight ?? true ? `<cropper-handle ${moveHandleAttributes}></cropper-handle>` : ""}`
      + `${this.getResizeHandles(options)}`
      + "</cropper-selection>"
      + "</cropper-canvas>";
  }

  private applyOptions(id: CropperId, options: DataOptionsTypes.Data.CropperExtendedOptions) {
    const { canvas, image, selection } = this.getCropperParts(id);

    this.state.instanceOptions[id] = {
      selectionMaximumCount: options.selectionMaximumCount,
      zoomOnTouch: options.zoomOnTouch ?? true,
      zoomOnWheel: options.zoomOnWheel ?? true,
    };

    canvas.background = options.canvasOptions?.background ?? options.background ?? true;
    canvas.hidden = options.canvasOptions?.hidden ?? options.canvasHidden ?? false;
    canvas.themeColor = options.canvasOptions?.themeColor ?? options.canvasThemeColor ?? "#39f";
    this.applyOptionalElementOptions(canvas, {
      slottable: options.canvasOptions?.slottable,
    });
    canvas.scaleStep = options.canvasOptions?.scaleStep ?? options.wheelZoomRatio ?? 0.1;
    canvas.disabled = options.canvasOptions?.disabled ?? options.disabled ?? false;
    image.hidden = options.imageOptions?.hidden ?? options.imageHidden ?? false;
    image.translatable = options.imageOptions?.translatable ?? options.movable ?? true;
    image.rotatable = options.imageOptions?.rotatable ?? options.rotatable ?? true;
    image.scalable = options.imageOptions?.scalable ?? options.scalable ?? true;
    image.skewable = options.imageOptions?.skewable ?? options.skewable ?? true;
    image.initialCenterSize = options.imageOptions?.initialCenterSize ?? options.imageInitialCenterSize ?? "contain";
    this.applyOptionalElementOptions(image, {
      slottable: options.imageOptions?.slottable,
    });

    const imageAlt = options.imageOptions?.alt ?? options.imageAlt;

    if (imageAlt !== undefined && imageAlt !== null) {
      image.alt = imageAlt;
    }

    selection.initialCoverage = options.selectionOptions?.initialCoverage ?? this.getInitialCoverage(options);
    selection.aspectRatio = options.selectionOptions?.aspectRatio ?? options.aspectRatio ?? NaN;
    selection.initialAspectRatio = options.selectionOptions?.initialAspectRatio ?? options.initialAspectRatio ?? NaN;
    this.applyOptionalElementOptions(selection, {
      x: options.selectionOptions?.x,
      y: options.selectionOptions?.y,
      width: options.selectionOptions?.width,
      height: options.selectionOptions?.height,
      slottable: options.selectionOptions?.slottable,
    });
    selection.movable = options.selectionOptions?.movable ?? options.cropBoxMovable ?? true;
    selection.resizable = options.selectionOptions?.resizable ?? options.cropBoxResizable ?? true;
    selection.zoomable = options.selectionOptions?.zoomable ?? options.zoomable ?? false;
    selection.keyboard = options.selectionOptions?.keyboard ?? options.keyboard ?? false;
    selection.outlined = options.selectionOptions?.outlined ?? options.outlined ?? false;
    selection.hidden = options.selectionOptions?.hidden ?? options.selectionHidden ?? options.autoCrop === false;
    selection.dynamic = options.selectionOptions?.dynamic ?? options.selectionDynamic ?? false;
    selection.multiple = options.selectionOptions?.multiple ?? options.selectionMultiple ?? false;
    selection.precise = options.selectionOptions?.precise ?? options.selectionPrecise ?? false;

    this.applyOptionalElementOptions(canvas.querySelector("cropper-shade"), {
      hidden: options.shadeOptions?.hidden ?? options.shadeHidden,
      x: options.shadeOptions?.x,
      y: options.shadeOptions?.y,
      width: options.shadeOptions?.width,
      height: options.shadeOptions?.height,
      themeColor: options.shadeOptions?.themeColor ?? options.shadeThemeColor,
      slottable: options.shadeOptions?.slottable,
    });
    this.applyOptionalElementOptions(canvas.querySelector("cropper-handle[plain]"), {
      hidden: options.handleOptions?.hidden ?? options.handleHidden,
      action: this.getCanvasAction(options.handleOptions?.action ?? options.handleAction ?? options.dragMode),
      plain: options.handleOptions?.plain ?? options.handlePlain,
      themeColor: options.handleOptions?.themeColor ?? options.handleThemeColor,
      slottable: options.handleOptions?.slottable,
    });
    this.applyOptionalElementOptions(selection.querySelector("cropper-grid"), {
      hidden: options.gridOptions?.hidden ?? options.gridHidden,
      rows: options.gridOptions?.rows ?? options.gridRows,
      columns: options.gridOptions?.columns ?? options.gridColumns,
      bordered: options.gridOptions?.bordered ?? options.gridBordered,
      covered: options.gridOptions?.covered ?? options.gridCovered,
      themeColor: options.gridOptions?.themeColor ?? options.gridThemeColor,
      slottable: options.gridOptions?.slottable,
    });
    this.applyOptionalElementOptions(selection.querySelector("cropper-crosshair"), {
      hidden: options.crosshairOptions?.hidden ?? options.crosshairHidden,
      centered: options.crosshairOptions?.centered ?? options.crosshairCentered,
      themeColor: options.crosshairOptions?.themeColor ?? options.crosshairThemeColor,
      slottable: options.crosshairOptions?.slottable,
    });
    this.applyOptionalElementOptions(selection.querySelector("cropper-handle[action='move']"), {
      hidden: options.moveHandleOptions?.hidden ?? options.moveHandleHidden,
      action: this.getSelectionMoveAction(options.moveHandleOptions?.action ?? options.moveHandleAction),
      themeColor: options.moveHandleOptions?.themeColor ?? options.moveHandleThemeColor,
      slottable: options.moveHandleOptions?.slottable,
    });

    const resizeHandles = selection.querySelectorAll("cropper-handle[action$='-resize']") as NodeListOf<Element>;

    for (const handle of Array.from(resizeHandles)) {
      this.applyOptionalElementOptions(handle, {
        hidden: options.resizeHandleOptions?.hidden ?? options.resizeHandleHidden,
        themeColor: options.resizeHandleOptions?.themeColor ?? options.resizeHandleThemeColor,
        slottable: options.resizeHandleOptions?.slottable,
      });
    }

    if (options.minCanvasWidth) {
      canvas.style.minWidth = `${options.minCanvasWidth}px`;
    }

    if (options.minCanvasHeight) {
      canvas.style.minHeight = `${options.minCanvasHeight}px`;
    }

    if (options.minCropBoxWidth) {
      selection.style.minWidth = `${options.minCropBoxWidth}px`;
    }

    if (options.minCropBoxHeight) {
      selection.style.minHeight = `${options.minCropBoxHeight}px`;
    }

    if (options.setDataOptions) {
      this.setData(id, options.setDataOptions);
    }

    this.initializePreviewElements(id, options.preview);
  }

  private applyOptionalElementOptions(element: Element | null, options: Record<string, unknown>) {
    if (!element) {
      return;
    }

    const target = element as unknown as Record<string, unknown>;

    for (const key of Object.keys(options)) {
      const value = options[key];

      if (value !== undefined && value !== null) {
        target[key] = value;
      }
    }
  }

  private getNumericStyleValue(value: string) {
    const parsedValue = Number.parseFloat(value);

    return Number.isFinite(parsedValue) ? parsedValue : 0;
  }

  private getCanvasAction(action: string | null | undefined) {
    if (action === "crop" || action === ACTION_SELECT) {
      return ACTION_SELECT;
    }

    if (action === ACTION_MOVE) {
      return ACTION_MOVE;
    }

    return ACTION_NONE;
  }

  private getSelectionMoveAction(action: string | null | undefined) {
    if (action === ACTION_NONE || action === ACTION_MOVE) {
      return action;
    }

    return ACTION_SELECT;
  }

  private getElementOffset(element: Element) {
    const bounds = element.getBoundingClientRect();

    return {
      left: bounds.left + window.pageXOffset,
      top: bounds.top + window.pageYOffset,
    };
  }

  private registerEvents(
    id: CropperId,
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    correlationId: string | undefined,
  ) {
    const { canvas, image, selection } = this.getCropperParts(id);
    const handlers: Array<{ element: EventTarget; type: string; listener: EventListener }> = [];
    const add = (element: EventTarget, type: string, listener: EventListener) => {
      element.addEventListener(type, listener);
      handlers.push({ element, type, listener });
    };
    const addCapture = (element: EventTarget, type: string, listener: EventListener) => {
      element.addEventListener(type, listener, true);
      handlers.push({ element, type, listener });
    };

    image.$ready(() => {
      this.onReady(imageObject, correlationId);
      this.onCrop(imageObject, this.getSyntheticEvent("crop"), correlationId);
    });
    add(canvas, "actionstart", (event) => this.onCropStart(imageObject, event, correlationId));
    add(canvas, "actionmove", (event) => this.onCropMove(imageObject, event, correlationId));
    add(canvas, "actionend", (event) => this.onCropEnd(imageObject, event, correlationId));
    addCapture(canvas, "action", (event) => this.ensureActionRelatedEvent(event as CustomEvent));
    addCapture(canvas, "action", (event) => this.preventSelectionAboveMaximum(id, event as CustomEvent));
    addCapture(canvas, "action", (event) => this.ensureCanvasSelectAction(canvas, event as CustomEvent));
    addCapture(canvas, "action", (event) => this.preventLimitedNativeZoom(id, imageObject, event as CustomEvent, correlationId));
    add(canvas, "action", (event) => this.keepSelectionsVisibleOnCanvasSelect(id, event as CustomEvent));
    add(canvas, "action", (event) => this.replaceSelectionOnCanvasSelect(id, event as CustomEvent));
    add(canvas, "action", (event) => {
      const customEvent = event as CustomEvent;

      if (customEvent.detail?.action === ACTION_SCALE) {
        this.onZoom(imageObject, event, correlationId);
      } else {
        this.onCrop(imageObject, event, correlationId);
      }
    });
    add(canvas, "change", (event) => this.onCrop(imageObject, event, correlationId));

    this.state.eventHandlers[id] = handlers;
  }

  private preventSelectionAboveMaximum(id: CropperId, event: CustomEvent) {
    const detail = event.detail as CropperSelectionActionDetail | undefined;

    if (detail?.action !== ACTION_SELECT) {
      return;
    }

    const { instance, selection } = this.getCropperParts(id);

    if (!selection.multiple) {
      return;
    }

    const maximumCount = this.state.instanceOptions[id]?.selectionMaximumCount;

    if (!maximumCount || this.getSelections(instance).length < maximumCount) {
      return;
    }

    event.preventDefault();
    event.stopImmediatePropagation();
  }

  private keepSelectionsVisibleOnCanvasSelect(id: CropperId, event: CustomEvent) {
    if ((event.detail as CropperSelectionActionDetail | undefined)?.action !== ACTION_SELECT) {
      return;
    }

    const selections = this.getSelections(this.getCropperParts(id).instance);

    if (selections.length <= 1) {
      return;
    }

    queueMicrotask(() => {
      this.getSelections(this.getCropperParts(id).instance).forEach((selection) => this.ensureVisibleSelection(selection));
    });
  }

  private replaceSelectionOnCanvasSelect(id: CropperId, event: CustomEvent) {
    const detail = event.detail as CropperSelectionActionDetail;

    if (detail?.action !== ACTION_SELECT
      || detail.startX === null
      || detail.startY === null
      || detail.endX === null
      || detail.endY === null) {
      return;
    }

    this.ensureSelectionActionCoordinates(detail);

    if (detail.startX === undefined
      || detail.startY === undefined
      || detail.endX === undefined
      || detail.endY === undefined) {
      return;
    }

    const moveX = detail.endX - detail.startX;
    const moveY = detail.endY - detail.startY;

    if (moveX === 0 && moveY === 0) {
      return;
    }

    const { canvas, selection } = this.getCropperParts(id);

    if (selection.multiple) {
      return;
    }

    const offset = this.getElementOffset(canvas);
    const nextSelection = this.getLimitedSelection(id, {
      x: detail.startX - offset.left,
      y: detail.startY - offset.top,
      width: Math.abs(moveX),
      height: Math.abs(moveY),
    });

    event.preventDefault();
    selection.$change(nextSelection.x, nextSelection.y, nextSelection.width, nextSelection.height, selection.aspectRatio, true);
  }

  private ensureSelectionActionCoordinates(detail: CropperSelectionActionDetail) {
    if (detail.startX !== undefined
      && detail.startY !== undefined
      && detail.endX !== undefined
      && detail.endY !== undefined) {
      return;
    }

    if (!(detail.relatedEvent instanceof MouseEvent)) {
      return;
    }

    detail.endX ??= detail.relatedEvent.pageX;
    detail.endY ??= detail.relatedEvent.pageY;

    if (detail.startX === undefined || detail.startY === undefined) {
      detail.startX = detail.endX;
      detail.startY = detail.endY;
    }
  }

  private ensureActionRelatedEvent(event: CustomEvent) {
    if (!event.detail) {
      return;
    }

    const target = event.target instanceof Element ? event.target : document.body;

    if (event.detail.relatedEvent) {
      event.detail.relatedEvent = this.ensureMouseEventTarget(event.detail.relatedEvent, target);
      return;
    }

    const bounds = target.getBoundingClientRect();

    event.detail.relatedEvent = this.getSyntheticMouseEvent("pointermove", target, {
      bubbles: true,
      cancelable: true,
      clientX: event.detail.startX ?? event.detail.endX ?? bounds.left + bounds.width / 2,
      clientY: event.detail.startY ?? event.detail.endY ?? bounds.top + bounds.height / 2,
      shiftKey: false,
    });
  }

  private ensureCanvasSelectAction(canvas: CropperCanvas, event: CustomEvent) {
    if (event.detail?.action !== ACTION_MOVE || !this.isCanvasSelectAction(canvas)) {
      return;
    }

    event.detail.action = ACTION_SELECT;
  }

  private isCanvasSelectAction(canvas: CropperCanvas) {
    const handle = canvas.querySelector("cropper-handle[plain]") as Element | null;

    return handle?.getAttribute("action") === ACTION_SELECT;
  }

  private preventLimitedNativeZoom(
    id: CropperId,
    imageObject: DotNetTypes.Global.DotNetObjectReference<CropperComponentBaseTypes.Components.ICropperComponentBase>,
    event: CustomEvent,
    correlationId: string | undefined,
  ) {
    if (event.detail?.action !== ACTION_SCALE) {
      return;
    }

    const { image } = this.getCropperParts(id);
    const imageRatio = this.getImageZoomRatio(image);
    const scale = event.detail?.scale;
    const requestedRatio = event.detail?.ratio
      ?? (typeof scale === "number" ? imageRatio * (1 + scale) : undefined)
      ?? imageRatio;
    const ratio = this.clampZoomRatio(id, requestedRatio);

    if (imageRatio === 0) {
      return;
    }

    event.detail.oldRatio ??= imageRatio;
    event.detail.ratio = ratio;
    event.detail.scale = (ratio / imageRatio) - 1;

    if (Math.abs(requestedRatio - ratio) <= 0.0001) {
      return;
    }

    event.preventDefault();
    event.stopImmediatePropagation();
    image.$zoom((ratio / imageRatio) - 1);
    this.onZoom(imageObject, event, correlationId);
  }

  private getSyntheticMouseEvent(type: string, target: Element, eventInit: MouseEventInit): MouseEvent {
    const event = new MouseEvent(type, eventInit);

    return this.ensureMouseEventTarget(event, target) as MouseEvent;
  }

  private ensureMouseEventTarget(event: Event, target: Element): Event {
    if (event.target instanceof Element) {
      return event;
    }

    Object.defineProperty(event, "target", {
      configurable: true,
      value: target,
    });

    return event;
  }

  private unregisterEvents(id: CropperId) {
    const handlers = this.state.eventHandlers[id] ?? [];

    for (const handler of handlers) {
      handler.element.removeEventListener(handler.type, handler.listener);
    }

    delete this.state.eventHandlers[id];
  }

  private initializePreviewElements(id: CropperId, preview: DataOptionsTypes.Data.CropperExtendedOptions["preview"]) {
    if (!preview) {
      return;
    }

    const elements = this.getPreviewElements(preview);

    for (const element of elements) {
      this.initializeViewer(id, element as HTMLElement);
    }
  }

  private getPreviewElements(preview: DataOptionsTypes.Data.CropperExtendedOptions["preview"]): Element[] {
    if (!preview) {
      return [];
    }

    if (typeof preview === "string") {
      return Array.from(document.querySelectorAll(preview));
    }

    if (preview instanceof Element) {
      return [preview];
    }

    return Array.from(preview as ArrayLike<Element>);
  }

  private getLimitedSelection(_id: CropperId, selection: LegacySelectionData) {
    return {
      x: selection.x ?? selection.left ?? 0,
      y: selection.y ?? selection.top ?? 0,
      width: selection.width ?? 0,
      height: selection.height ?? 0,
    };
  }

  private getCropperSelectionCanvasOptions(options: LegacyCroppedCanvasOptions = {}) {
    const width = this.clamp(options.width ?? undefined, options.minWidth, options.maxWidth);
    const height = this.clamp(options.height ?? undefined, options.minHeight, options.maxHeight);

    return {
      width,
      height,
      beforeDraw: (context: CanvasRenderingContext2D, canvas: HTMLCanvasElement) => {
        if (options.fillColor) {
          context.fillStyle = options.fillColor;
          context.fillRect(0, 0, canvas.width, canvas.height);
        }

        if (options.imageSmoothingEnabled !== undefined && options.imageSmoothingEnabled !== null) {
          context.imageSmoothingEnabled = options.imageSmoothingEnabled;
        }

        if (options.imageSmoothingQuality) {
          context.imageSmoothingQuality = options.imageSmoothingQuality;
        }
      },
    };
  }

  private ensureVisibleSelection(selection: CropperSelection) {
    if (!selection.hidden && selection.width > 0 && selection.height > 0) {
      return;
    }

    selection.hidden = false;
    selection.$reset();
  }

  private getInitialCoverage(options: DataOptionsTypes.Data.CropperExtendedOptions): number {
    if (options.autoCrop === false) {
      return 0;
    }

    return options.autoCropArea ?? 0.5;
  }

  private getResizeHandles(options: DataOptionsTypes.Data.CropperExtendedOptions): string {
    if ((options.selectionOptions?.resizable ?? options.cropBoxResizable) === false) {
      return "";
    }

    return ["n", "e", "s", "w", "ne", "nw", "se", "sw"]
      .map((action) => `<cropper-handle action="${action}-resize" ${this.resizeHandleAttributes(options)}></cropper-handle>`)
      .join("");
  }

  private resizeHandleAttributes(options: DataOptionsTypes.Data.CropperExtendedOptions): string {
    return [
      this.booleanAttribute("hidden", options.resizeHandleOptions?.hidden ?? options.resizeHandleHidden ?? false),
      this.stringAttribute("theme-color", options.resizeHandleOptions?.themeColor ?? options.resizeHandleThemeColor ?? "rgba(51, 153, 255, 0.5)"),
      this.booleanAttribute("slottable", options.resizeHandleOptions?.slottable ?? false),
    ].filter(Boolean).join(" ");
  }

  private booleanAttribute(name: string, value: boolean): string {
    return value ? name : "";
  }

  private numberAttribute(name: string, value: NullableNumber): string {
    return value === undefined || value === null ? "" : `${name}="${value}"`;
  }

  private stringAttribute(name: string, value: string | null | undefined): string {
    return value === undefined || value === null || value === "" ? "" : `${name}="${this.escapeAttribute(value)}"`;
  }

  private escapeAttribute(value: string): string {
    return value
      .replace(/&/g, "&amp;")
      .replace(/"/g, "&quot;")
      .replace(/</g, "&lt;")
      .replace(/>/g, "&gt;");
  }

  private clamp(value: number | undefined, min?: NullableNumber, max?: NullableNumber): number | undefined {
    if (value === undefined) {
      return value;
    }

    let result = value;

    if (min !== undefined && min !== null) {
      result = Math.max(result, min);
    }

    if (max !== undefined && max !== null) {
      result = Math.min(result, max);
    }

    return result;
  }

  private getRotationInDegrees(transform: number[]): number {
    return Math.atan2(transform[1], transform[0]) * (180 / Math.PI);
  }

  private getSyntheticEvent(type: string): Event {
    return new CustomEvent(type, { bubbles: true, cancelable: true, detail: { action: ACTION_NONE } });
  }

  private roundNumericProperties<T extends Record<string, unknown>>(data: T): T {
    const mutableData = data as Record<string, unknown>;

    for (const key of Object.keys(mutableData)) {
      if (typeof mutableData[key] === "number") {
        mutableData[key] = Math.round(mutableData[key] as number);
      }
    }

    return data;
  }

  private getLegacyEventName(instance: Event | CustomEvent): string {
    switch (instance.type) {
      case "actionstart":
        return "cropstart";
      case "actionmove":
        return "cropmove";
      case "actionend":
        return "cropend";
      case "action":
        return (instance as CustomEvent).detail?.action === "scale" ? "zoom" : "crop";
      case "change":
        return "crop";
      default:
        return instance.type;
    }
  }

  private getSyntheticJSEventData(type: string, correlationId: string | undefined): DataEventTypes.Data.CropperJSEventData {
    return {
      isTrusted: true,
      detail: { action: ACTION_NONE, originalEvent: null },
      type,
      eventPhase: 0,
      bubbles: false,
      cancelable: false,
      defaultPrevented: false,
      composed: false,
      timeStamp: Date.now(),
      returnValue: true,
      cancelBubble: false,
      correlationId,
    };
  }
}

window.cropper = new CropperDecorator();
window.cropperUrlImageHelper = new UrlImageHelper.Helpers.CropperUrlImageHelper();
