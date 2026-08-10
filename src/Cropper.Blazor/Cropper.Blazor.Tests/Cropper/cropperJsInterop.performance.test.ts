import { describe, expect, it } from "vitest";
import { CropperSelectionInteropCommands } from "../../Cropper.Blazor/Cropper/cropperSelectionInteropCommands";

describe("cropper selection performance behavior", () => {
  function createCommands(selection: any) {
    return new CropperSelectionInteropCommands(
      () => ({ selection } as any),
      () => [selection],
      (_id, nextSelection) => ({
        x: nextSelection.x ?? 0,
        y: nextSelection.y ?? 0,
        width: nextSelection.width ?? 0,
        height: nextSelection.height ?? 0,
      }),
      (currentSelection) => {
        currentSelection.hidden = false;
        currentSelection.$reset();
      },
      () => ({ beforeDraw: () => undefined }),
    );
  }

  function createSelection() {
    return {
      x: 1,
      y: 2,
      width: 10,
      height: 20,
      aspectRatio: Number.NaN,
      hidden: true,
      $change(x: number, y: number, width: number, height: number, aspectRatio: number) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.aspectRatio = aspectRatio;
        return this;
      },
      $reset() {
        this.width = 10;
        this.height = 20;
        this.hidden = false;
        return this;
      },
      $toCanvas: async () => document.createElement("canvas"),
      getAttribute: () => null,
      removeAttribute: () => undefined,
      setAttribute: () => undefined,
    };
  }

  it("keeps zero-size selection changes at zero", () => {
    const selection = createSelection();
    const commands = createCommands(selection);

    commands.changeSelection("id", 0, 0, 0, 0);

    expect(selection.width).toBe(0);
    expect(selection.height).toBe(0);
    expect(selection.hidden).toBe(true);
  });

  it("restores visibility only for non-zero selection changes", () => {
    const selection = createSelection();
    const commands = createCommands(selection);

    commands.changeSelection("id", 3, 4, 30, 40);

    expect(selection.width).toBe(30);
    expect(selection.height).toBe(40);
    expect(selection.hidden).toBe(false);
  });
});
