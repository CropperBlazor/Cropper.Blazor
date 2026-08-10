import { ACTION_SELECT, CropperSelection } from "cropperjs";
import { CropperId, CropperInstanceParts, CropperSelectionData, CropperSelectionShape, LegacyCroppedCanvasOptions, LegacySelectionData, NullableNumber } from "./cropperJsInterop.types";

type GetParts = (id: CropperId) => CropperInstanceParts;
type GetLimitedSelection = (id: CropperId, selection: LegacySelectionData) => { x: number; y: number; width: number; height: number };
type EnsureVisibleSelection = (selection: CropperSelection) => void;
type GetSelections = (id: CropperId) => CropperSelection[];
type GetSelectionCanvasOptions = (options?: LegacyCroppedCanvasOptions) => { width?: number; height?: number; beforeDraw: (context: CanvasRenderingContext2D, canvas: HTMLCanvasElement) => void };

export class CropperSelectionInteropCommands {
  constructor(
    private readonly getParts: GetParts,
    private readonly getSelections: GetSelections,
    private readonly getLimitedSelection: GetLimitedSelection,
    private readonly ensureVisibleSelection: EnsureVisibleSelection,
    private readonly getSelectionCanvasOptions: GetSelectionCanvasOptions,
  ) {
  }

  resetSelection(id: CropperId) {
    const { selection } = this.getParts(id);

    return selection.$reset();
  }

  centerSelection(id: CropperId) {
    const { selection } = this.getParts(id);

    return selection.$center();
  }

  clearSelection(id: CropperId) {
    const { selection } = this.getParts(id);

    selection.$clear();
  }

  moveSelectionTo(id: CropperId, x: number, y: number) {
    const { selection } = this.getParts(id);

    this.ensureVisibleSelection(selection);

    return selection.$moveTo(x, y);
  }

  changeSelection(id: CropperId, x: number, y: number, width: number, height: number, aspectRatio?: NullableNumber) {
    const { selection } = this.getParts(id);
    const nextSelection = this.getLimitedSelection(id, { x, y, width, height });

    if (width > 0 && height > 0) {
      this.ensureVisibleSelection(selection);
    }

    return selection.$change(
      nextSelection.x,
      nextSelection.y,
      nextSelection.width,
      nextSelection.height,
      aspectRatio ?? selection.aspectRatio,
      true,
    );
  }

  changeSelectionByIndex(id: CropperId, index: number, x: number, y: number, width: number, height: number, aspectRatio?: NullableNumber) {
    const selections = this.getSelections(id);
    const selection = selections[index];

    if (!selection) {
      return undefined;
    }

    selections.forEach((currentSelection) => {
      if (currentSelection.width > 0 && currentSelection.height > 0) {
        this.ensureVisibleSelection(currentSelection);
      }
    });
    const nextSelection = this.getLimitedSelection(id, { x, y, width, height });

    if (width > 0 && height > 0) {
      this.ensureVisibleSelection(selection);
    }

    const result = selection.$change(
      nextSelection.x,
      nextSelection.y,
      nextSelection.width,
      nextSelection.height,
      aspectRatio ?? selection.aspectRatio,
      true,
    );

    this.getSelections(id).forEach((currentSelection) => {
      if (currentSelection.width > 0 && currentSelection.height > 0) {
        this.ensureVisibleSelection(currentSelection);
      }
    });

    return result;
  }

  createSelection(id: CropperId, x: number, y: number, width: number, height: number) {
    const { canvas } = this.getParts(id);
    const bounds = canvas.getBoundingClientRect();
    const selections = this.getSelections(id);

    selections.forEach((selection) => this.ensureVisibleSelection(selection));

    canvas.dispatchEvent(new CustomEvent("action", {
      bubbles: true,
      cancelable: true,
      detail: {
        action: ACTION_SELECT,
        startX: bounds.left + x,
        startY: bounds.top + y,
        endX: bounds.left + x + width,
        endY: bounds.top + y + height,
      },
    }));

    this.getSelections(id).forEach((selection) => this.ensureVisibleSelection(selection));
  }

  removeSelectionByIndex(id: CropperId, index: number) {
    const selections = this.getSelections(id);
    const selection = selections[index];

    if (!selection) {
      return;
    }

    if (selections.length === 1) {
      selection.$clear();
      return;
    }

    selection.remove();
    this.getSelections(id).forEach((currentSelection) => this.ensureVisibleSelection(currentSelection));
  }

  setSelectionShapeByIndex(id: CropperId, index: number, shape: CropperSelectionShape) {
    const selection = this.getSelections(id)[index];

    if (!selection) {
      return;
    }

    this.applySelectionShape(selection, shape);
    this.getSelections(id).forEach((currentSelection) => this.ensureVisibleSelection(currentSelection));
  }

  getSelectionCount(id: CropperId) {
    return this.getSelections(id).length;
  }

  getSelectionsData(id: CropperId): CropperSelectionData[] {
    return this.getSelections(id).map((selection, index) => ({
      index,
      active: selection.active,
      x: selection.x,
      y: selection.y,
      width: selection.width,
      height: selection.height,
      aspectRatio: selection.aspectRatio,
      shape: selection.getAttribute("data-cropper-face"),
    }));
  }

  async selectionToCanvasDataURL(
    id: CropperId,
    options: LegacyCroppedCanvasOptions,
    type?: string,
    encoderOptions?: number,
  ) {
    const { selection } = this.getParts(id);
    this.ensureVisibleSelection(selection);
    const canvas = await selection.$toCanvas(this.getSelectionCanvasOptions(options));

    return canvas.toDataURL(type, encoderOptions);
  }

  async selectionToCanvasByIndex(id: CropperId, index: number, options: LegacyCroppedCanvasOptions) {
    const selection = this.getSelections(id)[index];

    if (!selection) {
      return null;
    }

    this.ensureVisibleSelection(selection);

    const canvas = await selection.$toCanvas(this.getSelectionCanvasOptions(options));
    canvas.dataset.selectionIndex = index.toString();
    canvas.dataset.selectionBounds = `${Math.round(selection.x)},${Math.round(selection.y)},${Math.round(selection.width)},${Math.round(selection.height)}`;

    return canvas;
  }

  setSelectionShape(id: CropperId, shape: CropperSelectionShape) {
    const { selection } = this.getParts(id);

    this.ensureVisibleSelection(selection);
    this.applySelectionShape(selection, shape);
    this.getSelections(id).forEach((currentSelection) => this.ensureVisibleSelection(currentSelection));
  }

  applySelectionShape(selection: CropperSelection, shape: CropperSelectionShape) {
    if (!shape || shape === "default") {
      selection.removeAttribute("data-cropper-face");
      return;
    }

    selection.setAttribute("data-cropper-face", shape);
  }
}
