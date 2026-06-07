import { beforeEach, describe, expect, it, vi } from "vitest";

type MockCropperPart = HTMLElement & Record<string, any>;

function createStreamableBlob(bytes: Uint8Array<ArrayBuffer>): Blob {
  const blob = new Blob([bytes.buffer]);

  blob.stream = () => {
    return new ReadableStream<Uint8Array<ArrayBuffer>>({
      start(controller) {
        controller.enqueue(bytes);
        controller.close();
      },
    });

  };

  return blob;
}

const state = vi.hoisted(() => ({
  instances: [] as any[],
}));

function assignRect(element: Element, rect: Partial<DOMRect>) {
  Object.defineProperty(element, "getBoundingClientRect", {
    configurable: true,
    value: () => ({
      left: rect.left ?? 0,
      top: rect.top ?? 0,
      width: rect.width ?? 300,
      height: rect.height ?? 200,
      right: (rect.left ?? 0) + (rect.width ?? 300),
      bottom: (rect.top ?? 0) + (rect.height ?? 200),
      x: rect.left ?? 0,
      y: rect.top ?? 0,
      toJSON: () => ({}),
    }),
  });
}

function patchCanvas(canvas: MockCropperPart) {
  canvas.$setAction = vi.fn((action: string) => {
    canvas.action = action;
  });
  assignRect(canvas, { left: 10, top: 20, width: 300, height: 200 });
}

function patchImage(image: MockCropperPart) {
  let transform = [1, 0, 0, 1, 0, 0];
  image.$image = { naturalWidth: 640, naturalHeight: 320 };
  image.$center = vi.fn((size?: string) => {
    image.centerSize = size;
    return image;
  });
  image.$move = vi.fn((x: number, y?: number) => {
    transform[4] += x;
    transform[5] += y ?? x;
    return image;
  });
  image.$moveTo = vi.fn((x: number, y?: number) => {
    transform[4] = x;
    transform[5] = y ?? x;
    return image;
  });
  image.$resetTransform = vi.fn(() => {
    transform = [1, 0, 0, 1, 0, 0];
  });
  image.$rotate = vi.fn(() => image);
  image.$getTransform = vi.fn(() => [...transform]);
  image.$setTransform = vi.fn((...next: number[] | [number[]]) => {
    transform = Array.isArray(next[0]) ? [...next[0]] : next as number[];
    return image;
  });
  image.$scale = vi.fn((x: number, y?: number) => {
    transform[0] = x;
    transform[3] = y ?? x;
    return image;
  });
  image.$zoom = vi.fn((ratio: number) => {
    const next = 1 + ratio;
    transform[0] *= next;
    transform[3] *= next;
    return image;
  });
  image.$ready = vi.fn((callback: () => void) => callback());
  assignRect(image, { left: 5, top: 6, width: 320, height: 160 });
}

function patchSelection(selection: MockCropperPart) {
  selection.x = 0;
  selection.y = 0;
  selection.width = 160;
  selection.height = 90;
  selection.aspectRatio = Number.NaN;
  selection.$change = vi.fn((x: number, y: number, width: number, height: number, aspectRatio?: number) => {
    selection.x = x;
    selection.y = y;
    selection.width = width;
    selection.height = height;
    selection.aspectRatio = aspectRatio ?? selection.aspectRatio;
    selection.left = x;
    selection.top = y;
    return selection;
  });
  selection.$clear = vi.fn(() => {
    selection.width = 0;
    selection.height = 0;
    selection.hidden = true;
  });
  selection.$reset = vi.fn(() => {
    selection.hidden = false;
    selection.width = 160;
    selection.height = 90;
    return selection;
  });
  selection.$center = vi.fn(() => selection);
  selection.$moveTo = vi.fn((x: number, y: number) => {
    selection.x = x;
    selection.y = y;
    return selection;
  });
  selection.$toCanvas = vi.fn(async (options?: { beforeDraw?: (context: any, canvas: HTMLCanvasElement) => void }) => {
    const canvas = document.createElement("canvas");
    canvas.width = 10;
    canvas.height = 10;
    canvas.toDataURL = vi.fn(() => "data:image/png;base64,test");
    canvas.toBlob = vi.fn((callback: BlobCallback) => callback(createStreamableBlob(new Uint8Array([1]))));
    options?.beforeDraw?.({ fillRect: vi.fn(), fillStyle: "", imageSmoothingEnabled: false }, canvas);
    return canvas;
  });
  assignRect(selection, { left: 0, top: 0, width: 160, height: 90 });
}

vi.mock("cropperjs", () => {
  class MockCropperViewer extends HTMLElement { }

  if (!customElements.get("cropper-viewer")) {
    customElements.define("cropper-viewer", MockCropperViewer);
  }

  class MockCropper {
    readonly container: HTMLElement;
    readonly root: HTMLElement;
    readonly canvas: MockCropperPart;
    readonly image: MockCropperPart;
    readonly selection: MockCropperPart;
    readonly template: string;

    constructor(image: HTMLImageElement | HTMLCanvasElement, options: any) {
      this.container = options.container ?? document.createElement("div");
      this.template = options.template;
      this.root = document.createElement("div");
      this.root.innerHTML = options.template;
      this.container.appendChild(this.root);
      this.canvas = this.root.querySelector("cropper-canvas") as MockCropperPart;
      this.image = this.root.querySelector("cropper-image") as MockCropperPart;
      this.selection = this.root.querySelector("cropper-selection") as MockCropperPart;
      patchCanvas(this.canvas);
      patchImage(this.image);
      patchSelection(this.selection);
      image.dataset.cropperInitialized = "true";
      state.instances.push(this);
    }

    static noConflict() {
      return MockCropper;
    }

    getCropperCanvas() {
      return this.canvas;
    }

    getCropperImage() {
      return this.image;
    }

    getCropperSelection() {
      return this.selection;
    }

    getCropperSelections() {
      return Array.from(this.root.querySelectorAll("cropper-selection"));
    }

    destroy = vi.fn();
  }

  return {
    default: MockCropper,
    CropperViewer: MockCropperViewer,
    ACTION_MOVE: "move",
    ACTION_NONE: "none",
    ACTION_SCALE: "scale",
    ACTION_SELECT: "select",
    DEFAULT_TEMPLATE: "<cropper-canvas><cropper-image></cropper-image><cropper-selection></cropper-selection></cropper-canvas>",
  };
});

describe("cropperJsInterop coverage", () => {
  beforeEach(() => {
    document.body.innerHTML = "";
    state.instances.length = 0;
    (globalThis as any).DotNet = {
      createJSObjectReference: vi.fn((value) => ({ value })),
    };
  });

  async function createDecorator() {
    vi.resetModules();
    const module = await import("./cropperJsInterop");
    return new module.CropperDecorator();
  }

  function createImage() {
    const container = document.createElement("div");
    const image = document.createElement("img");
    container.appendChild(image);
    document.body.appendChild(container);
    return image;
  }

  function latestInstance() {
    return state.instances[state.instances.length - 1]!;
  }

  it("registers cropper globals", async () => {
    await import("./cropperJsInterop");

    expect(window.cropper).toBeDefined();
    expect(window.cropperUrlImageHelper).toBeDefined();
  });

  it("initializes templates and applies v2 options", async () => {
    const cropper = await createDecorator();
    const image = createImage();

    expect((cropper as any).getTemplate(null)).toContain("cropper-canvas");
    expect((cropper as any).getTemplate({
      canvasOptions: { hidden: true, background: false, disabled: true, scaleStep: 0.2, themeColor: "red" },
      imageOptions: { hidden: true, rotatable: false, scalable: false, skewable: false, translatable: false, initialCenterSize: "cover", alt: "Nested" },
      shadeOptions: { hidden: false, themeColor: "black" },
      handleOptions: { hidden: true, action: "select", plain: false, themeColor: "blue" },
      selectionOptions: { hidden: true, dynamic: true, movable: false, resizable: false, zoomable: true, multiple: true, keyboard: true, outlined: true, precise: true },
      gridOptions: { hidden: true, rows: 6, columns: 7, bordered: false, covered: false, themeColor: "white" },
      crosshairOptions: { hidden: true, centered: false, themeColor: "green" },
      moveHandleOptions: { hidden: true, action: "none", themeColor: "yellow" },
      resizeHandleOptions: { hidden: true, themeColor: "purple" },
    } as any)).toContain('alt="Nested"');

    cropper.initCropper("id", image, {
      autoCropArea: 0.5,
      canvasHidden: true,
      background: false,
      disabled: true,
      wheelZoomRatio: 0.2,
      canvasThemeColor: "<red&blue>",
      imageHidden: true,
      rotatable: false,
      scalable: false,
      skewable: false,
      movable: false,
      imageInitialCenterSize: "cover",
      imageAlt: "Alt",
      shadeHidden: false,
      shadeThemeColor: "#000000",
      handleHidden: true,
      handleAction: "move",
      handlePlain: false,
      handleThemeColor: "#111111",
      selectionHidden: true,
      initialAspectRatio: 1,
      aspectRatio: 2,
      selectionDynamic: true,
      cropBoxMovable: false,
      cropBoxResizable: true,
      zoomable: false,
      selectionMultiple: true,
      keyboard: false,
      outlined: false,
      selectionPrecise: true,
      gridHidden: true,
      gridRows: 4,
      gridColumns: 5,
      gridBordered: false,
      gridCovered: false,
      gridThemeColor: "#222222",
      crosshairHidden: true,
      crosshairCentered: false,
      crosshairThemeColor: "#333333",
      moveHandleHidden: true,
      moveHandleAction: "none",
      moveHandleThemeColor: "#444444",
      resizeHandleHidden: true,
      resizeHandleThemeColor: "#555555",
      minCanvasWidth: 10,
      minCanvasHeight: 20,
      minCropBoxWidth: 30,
      minCropBoxHeight: 40,
      preview: ".preview",
      setDataOptions: { x: 1, y: 2, width: 3, height: 4, rotate: 45, scaleX: 2, scaleY: 3 },
    } as any);

    expect(latestInstance().template).toContain("&lt;red&amp;blue&gt;");
    expect(latestInstance().canvas.hidden).toBe(true);
    expect(latestInstance().image.alt).toBe("Alt");
    expect(latestInstance().selection.width).toBe(3);
    expect(latestInstance().selection.dynamic).toBe(true);

    cropper.initCropper("auto-crop-off", createImage(), { autoCrop: false, cropBoxResizable: false, modal: false, guides: false, center: false, highlight: false } as any);
    expect(latestInstance().template).toContain('initial-coverage="0"');
    expect(latestInstance().template).not.toContain("-resize");

    cropper.initCropper("static-selection", createImage(), { selectionDynamic: false } as any);
    expect(latestInstance().template).not.toContain("dynamic");
    expect(latestInstance().selection.dynamic).toBe(false);

    cropper.initCropper("dynamic-selection", createImage(), { selectionDynamic: true } as any);
    expect(latestInstance().template).toContain("dynamic");
    expect(latestInstance().selection.dynamic).toBe(true);
  });

  it("executes data commands and transformations", async () => {
    const cropper = await createDecorator();
    cropper.initCropper("id", createImage(), {} as any);
    const instance = latestInstance();

    expect(cropper.center("id", "cover")).toBe(instance.image);
    cropper.changeSelection("id", 1, 2, 10, 20);
    cropper.clear("id");
    cropper.crop("id");
    cropper.disable("id");
    cropper.enable("id");
    expect(cropper.getCanvasData("id").width).toBe(300);
    expect(cropper.getContainerData("id").width).toBe(0);
    expect(cropper.getCropBoxData("id").width).toBe(160);
    expect(cropper.getData("id", true).width).toBe(160);
    expect(cropper.getImageData("id").aspectRatio).toBe(2);
    instance.image.$image.naturalHeight = 0;
    expect(cropper.getImageData("id").aspectRatio).toBe(0);
    instance.image.$image.naturalHeight = 320;
    expect(cropper.move("id", 1, 2)).toBe(instance.image);
    expect(cropper.moveTo("id", 3, 4)).toBe(instance.image);
    cropper.replace("id", "next.png");
    cropper.reset("id");
    cropper.resetSelection("id");
    cropper.centerSelection("id");
    cropper.clearSelection("id");
    cropper.moveSelectionTo("id", 5, 6);
    cropper.changeSelection("id", 1, 2, 10, 20, 2);
    expect(instance.selection.aspectRatio).toBe(2);
    instance.getCropperSelections = () => [instance.selection];
    cropper.changeSelectionByIndex("id", 0, 2, 3, 11, 22, 3);
    expect(instance.selection.aspectRatio).toBe(3);
    cropper.changeSelectionByIndex("id", 0, 2, 3, 11, 22);
    expect(cropper.changeSelectionByIndex("id", 2, 2, 3, 11, 22, 3)).toBeUndefined();
    cropper.createSelection("id", 1, 2, 3, 4);
    cropper.setSelectionShapeByIndex("id", 0, "circle");
    expect(instance.selection.getAttribute("data-cropper-face")).toBe("circle");
    cropper.setSelectionShapeByIndex("id", 2, "circle");
    expect(cropper.getSelectionCount("id")).toBe(1);
    expect(cropper.getSelectionsData("id")).toEqual([
      {
        index: 0,
        active: undefined,
        x: 2,
        y: 3,
        width: 11,
        height: 22,
        aspectRatio: 3,
        shape: "circle",
      },
    ]);
    cropper.setSelectionShape("id", "default");
    expect(instance.selection.hasAttribute("data-cropper-face")).toBe(false);
    cropper.setSelectionShape("id", "pentagon");
    expect(instance.selection.getAttribute("data-cropper-face")).toBe("pentagon");
    expect(await cropper.selectionToCanvasDataURL("id", { fillColor: "red", imageSmoothingEnabled: true, imageSmoothingQuality: "high" }, "image/png", 1)).toContain("data:image/png");
    cropper.rotate("id", 45);
    cropper.rotateTo("id", 90);
    instance.image.$setTransform(0, 0, 0, 0, 0, 0);
    cropper.rotateTo("id", 180);
    cropper.scale("id", 2, 3);
    instance.image.$setTransform(1, 0, 0, 0, 0, 0);
    cropper.scaleX("id", -1);
    instance.image.$setTransform(0, 0, 0, 1, 0, 0);
    cropper.scaleY("id", -1);
    cropper.setAspectRatio("id", 1);
    cropper.setCanvasData("id", { left: 1, top: 2, width: 100, height: 50 });
    cropper.setCanvasData("id", {});
    assignRect(instance.image, { left: 5, top: 6, width: 0, height: 0 });
    cropper.setCanvasData("id", { width: 10, height: 10 });
    assignRect(instance.image, { left: 5, top: 6, width: 320, height: 160 });
    cropper.setCropBoxData("id", { left: 1, top: 2, width: 20, height: 30 });
    cropper.setCropBoxData("id", {});
    cropper.setData("id", { x: 1, y: 2, width: 20, height: 30, rotate: 30, scaleX: 2, scaleY: 2 });
    cropper.setData("id", {});
    cropper.setData("id", { rotate: 10 });
    cropper.setData("id", { scaleX: 3 });
    cropper.setData("id", { scaleY: 4 });
    cropper.setDragMode("id", "move");
    cropper.setDragMode("id", "select");
    cropper.setDragMode("id", "invalid");

    expect(instance.canvas.disabled).toBe(false);
  });

  it("executes crop canvas and streaming commands", async () => {
    vi.useFakeTimers();
    const cropper = await createDecorator();
    cropper.initCropper("id", createImage(), {} as any);
    const receiver = { invokeMethodAsync: vi.fn(async () => undefined) };
    const imageReceiver = { invokeMethodAsync: vi.fn(async () => undefined) };

    expect(await cropper.getCroppedCanvas("id", { width: 1, minWidth: 2, maxWidth: 3, height: 1, minHeight: 2, maxHeight: 3 })).toBeInstanceOf(HTMLCanvasElement);
    expect(await cropper.getCroppedCanvasDataURL("id", {}, "image/png", 1)).toContain("data:image/png");
    await cropper.readBlobInChunks(createStreamableBlob(new Uint8Array([1, 2, 3])), imageReceiver as any, 10);
    cropper.getCroppedCanvasInBackground("id", {}, receiver as any);
    cropper.sendImageInChunks("id", {}, imageReceiver as any, "image/png", 1, 10);
    await vi.runAllTimersAsync();
    vi.useRealTimers();

    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("ReceiveCanvasReference", expect.anything());
    expect(imageReceiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("executes zoom limits and event serialization", async () => {
    const cropper = await createDecorator();
    const dotNet = { invokeMethodAsync: vi.fn(async () => undefined) };
    cropper.initCropper("id", createImage(), { zoomOnWheel: true, zoomOnTouch: true } as any, dotNet);
    const instance = latestInstance();

    cropper.setZoomLimits("id", 0.5, 2);
    instance.selection.width = 400;
    instance.selection.height = 300;
    cropper.zoom("id", 0.5);
    instance.selection.width = 10;
    instance.selection.height = 10;
    cropper.zoom("id", 0);
    instance.image.$setTransform(0, 0, 0, 0, 0, 0);
    cropper.setZoomLimits("id", undefined, undefined);
    cropper.zoom("id", 0.5);
    cropper.zoomTo("id", 1, 0, 0);
    cropper.setDefaults({} as any);
    expect(cropper.noConflict()).toBeDefined();

    cropper.initCropper("disabled-zoom", createImage(), { zoomOnWheel: false, zoomOnTouch: false } as any);
    expect(cropper.zoom("disabled-zoom", 1)).toBe(latestInstance().image);

    expect(cropper.getJSEventData(new CustomEvent("action", { detail: { action: "scale", oldRatio: 1, ratio: 2 } }), "id").type).toBe("zoom");
    expect(cropper.getJSEventData(new CustomEvent("actionstart", { detail: { action: "move" } }), "id").type).toBe("cropstart");
    expect(cropper.getJSEventData(new CustomEvent("actionmove", { detail: { action: "move" } }), "id").type).toBe("cropmove");
    expect(cropper.getJSEventData(new CustomEvent("actionend", { detail: { action: "move" } }), "id").type).toBe("cropend");
    expect(cropper.getJSEventData(new CustomEvent("change", { detail: { action: "move" } }), "id").type).toBe("crop");
    expect(cropper.getJSEventData(new CustomEvent("change", { detail: { action: "move" } }), "id").correlationId).toBe("id");
    cropper.onReady(dotNet as any, "id");
    cropper.onCropStart(dotNet as any, new CustomEvent("actionstart", { detail: { action: "move" } }), "id");
    cropper.onCropMove(dotNet as any, new CustomEvent("actionmove", { detail: { action: "move" } }), "id");
    cropper.onCropEnd(dotNet as any, new CustomEvent("actionend", { detail: { action: "move" } }), "id");
    cropper.onZoom(dotNet as any, new CustomEvent("action", { detail: { action: "scale", oldRatio: 1, ratio: 1.2 } }), "id");
    expect(dotNet.invokeMethodAsync).toHaveBeenLastCalledWith("CropperIsZoomed", expect.objectContaining({ correlationId: "id", detail: expect.objectContaining({ oldRatio: 1, ratio: 1.2 }) }));
    instance.image.$setTransform(1, 0, 0, 1, 0, 0);
    cropper.setZoomLimits("id", undefined, undefined);
    cropper.onZoom(dotNet as any, new CustomEvent("action", { detail: { action: "scale", scale: 0.1 } }), "id");
    expect(dotNet.invokeMethodAsync).toHaveBeenLastCalledWith("CropperIsZoomed", expect.objectContaining({ detail: expect.objectContaining({ oldRatio: 1 / 1.1, ratio: 1 }) }));
    expect(cropper.getJSEventData(new CustomEvent("action", { detail: { action: "scale", relatedEvent: new Event("wheel") } }), "id").detail.originalEvent).toEqual(expect.anything());
    expect(cropper.getJSEventData(new CustomEvent("action", { detail: { originalEvent: new Event("pointerdown") } }), "id").detail.originalEvent).toEqual(expect.anything());
    expect(cropper.getJSEventData(new CustomEvent("actionstart", { detail: { action: "select", relatedEvent: new Event("pointerdown") } }), "id").detail.originalEvent).toEqual(expect.anything());
    cropper.getJSEventData(new CustomEvent("custom"), "id");
    cropper.initCropper("", createImage(), {} as any);
    cropper.onCrop(dotNet as any, new CustomEvent("change", { detail: { action: "move" } }), undefined);
    cropper.initCropper("id", createImage(), { zoomOnWheel: true, zoomOnTouch: true } as any, dotNet);
    const registeredInstance = latestInstance();
    registeredInstance.canvas.dispatchEvent(new CustomEvent("actionstart", { detail: { action: "move" } }));
    registeredInstance.canvas.dispatchEvent(new CustomEvent("actionmove", { detail: { action: "move" } }));
    registeredInstance.canvas.dispatchEvent(new CustomEvent("actionend", { detail: { action: "move" } }));
    registeredInstance.canvas.dispatchEvent(new CustomEvent("action", { cancelable: true, detail: { action: "select", startX: 30, startY: 50, endX: 70, endY: 100 } }));
    expect(registeredInstance.selection.$change).toHaveBeenLastCalledWith(20, 30, 40, 50, expect.any(Number), true);
    cropper.removeSelectionByIndex("id", 10);
    cropper.removeSelectionByIndex("id", 0);
    expect(registeredInstance.selection.$clear).toHaveBeenCalled();
    const removableSelection = document.createElement("cropper-selection") as MockCropperPart;
    registeredInstance.canvas.appendChild(removableSelection);
    patchSelection(removableSelection);
    cropper.removeSelectionByIndex("id", 1);
    expect(removableSelection.isConnected).toBe(false);
    registeredInstance.canvas.dispatchEvent(new CustomEvent("action", { detail: { action: "move" } }));
    registeredInstance.canvas.dispatchEvent(new CustomEvent("action", { detail: { action: "scale", ratio: 1.5 } }));
    registeredInstance.selection.dispatchEvent(new CustomEvent("change", { cancelable: true, detail: { left: -10, top: -20, width: 400, height: 300 } }));
    registeredInstance.selection.dispatchEvent(new CustomEvent("change", { cancelable: true, detail: { x: 1, y: 2, width: 10, height: 10 } }));
    cropper.destroy("id");

    expect(dotNet.invokeMethodAsync).toHaveBeenCalled();
    expect((cropper as any).state.eventHandlers.id).toBeUndefined();
    expect((cropper as any).state.instances.id).toBeUndefined();
    expect((cropper as any).state.instanceOptions.id).toBeUndefined();
    expect((cropper as any).state.zoomLimits.id).toBeUndefined();
    expect((cropper as any).state.zoomEventRatios.id).toBeUndefined();
  });

  it("routes events to the registered cropper component with correlation ids", async () => {
    const cropper = await createDecorator();
    const firstDotNet = { invokeMethodAsync: vi.fn(async () => undefined) };
    const secondDotNet = { invokeMethodAsync: vi.fn(async () => undefined) };

    cropper.initCropper("first-id", createImage(), {} as any, firstDotNet);
    const firstInstance = latestInstance();
    cropper.initCropper("second-id", createImage(), {} as any, secondDotNet);
    const secondInstance = latestInstance();

    firstDotNet.invokeMethodAsync.mockClear();
    secondDotNet.invokeMethodAsync.mockClear();

    firstInstance.canvas.dispatchEvent(new CustomEvent("action", { bubbles: true, detail: { action: "move" } }));
    secondInstance.canvas.dispatchEvent(new CustomEvent("action", { bubbles: true, detail: { action: "scale", ratio: 1.1 } }));

    expect(firstDotNet.invokeMethodAsync).toHaveBeenCalledWith("CropperIsCroped", expect.objectContaining({ correlationId: "first-id" }));
    expect(secondDotNet.invokeMethodAsync).toHaveBeenCalledWith("CropperIsZoomed", expect.objectContaining({ correlationId: "second-id" }));
    expect(firstDotNet.invokeMethodAsync).not.toHaveBeenCalledWith("CropperIsZoomed", expect.anything());
    expect(secondDotNet.invokeMethodAsync).not.toHaveBeenCalledWith("CropperIsCroped", expect.anything());
  });

  it("covers helper fallback branches", async () => {
    const cropper = await createDecorator();
    cropper.initCropper("id", createImage(), {} as any);
    const instance = latestInstance();

    instance.selection.style.minWidth = "5px";
    instance.selection.style.minHeight = "6px";
    cropper.changeSelection("id", 1, 2, 1, 1);
    expect((cropper as any).getTemplate({})).toContain("cropper-selection");
    expect((cropper as any).getLimitedSelection("id", { left: 1, top: 2 })).toEqual({ x: 1, y: 2, width: 0, height: 0, changed: false });
    expect((cropper as any).getLimitedSelection("id", { width: 1, height: 1 })).toEqual({ x: 0, y: 0, width: 1, height: 1, changed: false });
    expect((cropper as any).getLimitedSelection("id", { top: 2, width: 1, height: 1 })).toEqual({ x: 0, y: 2, width: 1, height: 1, changed: false });
    expect((cropper as any).getCropperSelectionCanvasOptions().width).toBeUndefined();
    expect((cropper as any).clamp(1, null, null)).toBe(1);
    expect((cropper as any).roundNumericProperties({ value: 1.2, text: "x" })).toEqual({ value: 1, text: "x" });
    instance.canvas.querySelector("cropper-handle[plain]")?.remove();
    cropper.setDragMode("id", "move");

    instance.image.$setTransform(0, 0, 0, 0, 0, 0);
    cropper.setData("id", { rotate: 10 });
    expect(() => cropper.initCropper("bad-image", null as any, {} as any)).toThrow("Parameter 'image' must not be null");
    expect(() => cropper.initCropper("bad-options", createImage(), null as any)).toThrow("Parameter 'optionsImage' must not be null");
  });

  it("handles viewers, preview inputs, errors, and destroy", async () => {
    const cropper = await createDecorator();
    const image = createImage();
    const preview = document.createElement("div");
    preview.className = "preview";
    document.body.appendChild(preview);
    cropper.initCropper("id", image, { preview: ".preview" } as any);
    const idInstance = latestInstance();
    cropper.initCropper("element-preview", createImage(), { preview } as any);
    cropper.initCropper("array-preview", createImage(), { preview: [preview] } as any);
    cropper.initCropper("no-preview", createImage(), { preview: null } as any);
    expect((cropper as any).getPreviewElements(null)).toEqual([]);

    const viewer = document.createElement("div");
    cropper.initializeViewer("id", viewer, "viewer-id");
    const pendingViewer = document.createElement("div");
    pendingViewer.innerHTML = "pending";
    cropper.initializeViewer("pending", pendingViewer);
    expect(pendingViewer.innerHTML).toBe("");
    idInstance.selection.removeAttribute("id");
    idInstance.selection.id = "";
    idInstance.selection.id = "existing-selection";
    const viewerById = document.createElement("div");
    viewerById.id = "viewer-by-id";
    document.body.appendChild(viewerById);
    cropper.initializeViewer("id", null, "viewer-by-id");
    expect(viewer.children.length).toBe(1);
    expect(() => cropper.initializeViewer("id", null)).toThrow("Viewer element was not found.");
    expect(() => cropper.getCanvasData("missing")).toThrow("Cropper instance 'missing' was not found.");

    const consoleLog = vi.spyOn(console, "log").mockImplementation(() => undefined);
    cropper.initCropper("bad", {} as any, {} as any);
    expect(consoleLog).toHaveBeenCalled();
    consoleLog.mockRestore();

    idInstance.getCropperSelection = () => null;
    expect(() => cropper.getCanvasData("id")).toThrow("Cropper instance 'id' was not found.");
    idInstance.getCropperSelection = () => idInstance.selection;

    const defaultOptionsDotNet = { invokeMethodAsync: vi.fn(async () => undefined) };
    cropper.initCropper("default-options", createImage(), {} as any, defaultOptionsDotNet);
    latestInstance().selection.dispatchEvent(new CustomEvent("change", { cancelable: true, detail: { width: 1, height: 1 } }));

    cropper.destroy("id");
    cropper.destroy("id");
    expect(idInstance.destroy).toHaveBeenCalled();
  });
});
