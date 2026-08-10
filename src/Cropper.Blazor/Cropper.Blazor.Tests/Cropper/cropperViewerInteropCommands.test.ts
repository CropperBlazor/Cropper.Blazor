import { describe, expect, it } from "vitest";
import { CropperViewerInteropCommands } from "../../Cropper.Blazor/Cropper/cropperViewerInteropCommands";

describe("cropper viewer performance behavior", () => {
  function createSelection(id = "selection-1") {
    return {
      id,
      x: 0,
      y: 0,
      width: 100,
      height: 100,
      setAttribute(name: string, value: string) {
        if (name === "id") {
          this.id = value;
        }
      },
    } as any;
  }

  function createSelectionWithoutId() {
    return createSelection("");
  }

  it("reuses existing viewer instead of replacing it", () => {
    const host = document.createElement("div");
    const selection = createSelection();
    const commands = new CropperViewerInteropCommands(
      () => ({ selection } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", host);
    const firstViewer = host.querySelector("cropper-viewer");

    commands.initializeViewer("cropper-id", host);
    const secondViewer = host.querySelector("cropper-viewer");

    expect(firstViewer).toBeTruthy();
    expect(secondViewer).toBe(firstViewer);
    expect(host.querySelectorAll("cropper-viewer")).toHaveLength(1);
    expect(secondViewer?.getAttribute("selection")).toBe("#selection-1");
  });

  it("keeps existing viewer when cropper parts are temporarily unavailable", () => {
    const host = document.createElement("div");
    const selection = createSelection();
    let hasParts = true;
    const commands = new CropperViewerInteropCommands(
      () => hasParts ? ({ selection } as any) : undefined,
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", host);
    const viewer = host.querySelector("cropper-viewer");

    hasParts = false;
    commands.initializeViewer("cropper-id", host);

    expect(host.querySelector("cropper-viewer")).toBe(viewer);
  });

  it("replaces viewer when the active selection changes", () => {
    const host = document.createElement("div");
    let selection = createSelection("selection-1");
    const commands = new CropperViewerInteropCommands(
      () => ({ selection } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", host);
    const firstViewer = host.querySelector("cropper-viewer");

    selection = createSelection("selection-2");
    commands.initializeViewer("cropper-id", host);
    const secondViewer = host.querySelector("cropper-viewer");

    expect(secondViewer).toBeTruthy();
    expect(secondViewer).not.toBe(firstViewer);
    expect(host.querySelectorAll("cropper-viewer")).toHaveLength(1);
    expect(secondViewer?.getAttribute("selection")).toBe("#selection-2");
  });

  it("binds viewer to selection index", () => {
    const host = document.createElement("div");
    const selections = [createSelection("selection-1"), createSelection("selection-2")];
    const commands = new CropperViewerInteropCommands(
      () => ({
        instance: {
          getCropperSelections: () => selections,
        },
        selection: selections[0],
      } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", host, 1);

    expect(host.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id-1");
    expect(selections[1].id).toBe("cropper-selection-cropper-id-1");
  });

  it("rebases inactive indexed selections away from the active fallback id", () => {
    const activeHost = document.createElement("div");
    const secondHost = document.createElement("div");
    const selections = [createSelection("cropper-selection-cropper-id"), createSelection("cropper-selection-cropper-id")];
    selections[0].active = true;
    selections[1].active = false;
    const commands = new CropperViewerInteropCommands(
      () => ({
        instance: {
          getCropperSelections: () => selections,
        },
        selection: selections[0],
      } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", activeHost);
    commands.initializeViewer("cropper-id", secondHost, 1);

    expect(activeHost.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id");
    expect(secondHost.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id-1");
    expect(selections[0].id).toBe("cropper-selection-cropper-id");
    expect(selections[1].id).toBe("cropper-selection-cropper-id-1");
  });

  it("assigns unique fallback selection ids for indexed viewers", () => {
    const firstHost = document.createElement("div");
    const secondHost = document.createElement("div");
    const selections = [createSelectionWithoutId(), createSelectionWithoutId()];
    const commands = new CropperViewerInteropCommands(
      () => ({
        instance: {
          getCropperSelections: () => selections,
        },
        selection: selections[0],
      } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", firstHost, 0);
    commands.initializeViewer("cropper-id", secondHost, 1);

    expect(firstHost.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id-0");
    expect(secondHost.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id-1");
    expect(selections[0].id).toBe("cropper-selection-cropper-id-0");
    expect(selections[1].id).toBe("cropper-selection-cropper-id-1");
  });

  it("uses active fallback id for first indexed viewer when only one selection exists", () => {
    const liveHost = document.createElement("div");
    const firstCardHost = document.createElement("div");
    const selections = [createSelectionWithoutId()];
    selections[0].active = true;
    const commands = new CropperViewerInteropCommands(
      () => ({
        instance: {
          getCropperSelections: () => selections,
        },
        selection: selections[0],
      } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", liveHost);
    commands.initializeViewer("cropper-id", firstCardHost, 0);

    expect(liveHost.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id");
    expect(firstCardHost.querySelector("cropper-viewer")?.getAttribute("selection")).toBe("#cropper-selection-cropper-id");
    expect(selections[0].id).toBe("cropper-selection-cropper-id");
  });

  it("re-renders reused viewer with current selection data", () => {
    const host = document.createElement("div");
    const selection = createSelection();
    const commands = new CropperViewerInteropCommands(
      () => ({ selection } as any),
      new WeakMap(),
    );

    commands.initializeViewer("cropper-id", host);
    const viewer = host.querySelector("cropper-viewer") as any;
    viewer.$render = (renderedSelection: any) => {
      viewer.renderedSelection = renderedSelection;
    };
    selection.x = 24;
    selection.y = 32;
    selection.width = 160;
    selection.height = 90;

    commands.initializeViewer("cropper-id", host);

    expect(host.querySelector("cropper-viewer")).toBe(viewer);
    expect(viewer.renderedSelection).toBe(selection);
  });
});
