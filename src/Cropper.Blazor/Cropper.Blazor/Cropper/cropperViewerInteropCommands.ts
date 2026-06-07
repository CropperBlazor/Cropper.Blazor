import { CropperSelection, CropperViewer } from "cropperjs";
import { CropperId, CropperInstanceParts, CropperWithSelections } from "./cropperJsInterop.types";

type TryGetParts = (id: CropperId) => CropperInstanceParts | undefined;

export class CropperViewerInteropCommands {
  constructor(
    private readonly tryGetParts: TryGetParts,
    private readonly viewers: WeakMap<HTMLElement, CropperViewer>,
  ) {
  }

  initializeViewer(id: CropperId, viewerElement: HTMLElement | null, selectionIndexOrViewerElementId?: number | string, viewerElementId?: string) {
    const selectionIndex = typeof selectionIndexOrViewerElementId === "number" ? selectionIndexOrViewerElementId : undefined;
    const resolvedViewerElementId = typeof selectionIndexOrViewerElementId === "string" ? selectionIndexOrViewerElementId : viewerElementId;
    const element = viewerElement ?? (resolvedViewerElementId ? document.getElementById(resolvedViewerElementId) : null);

    if (!element) {
      throw new Error("Viewer element was not found.");
    }

    const parts = this.tryGetParts(id);

    if (!parts) {
      return;
    }

    const selections = selectionIndex === undefined
      ? []
      : this.getSelections(id);
    const selection = selectionIndex === undefined
      ? parts.selection
      : selections[selectionIndex];

    if (!selection) {
      element.replaceChildren();
      return;
    }

    const selectionId = this.getSelectionId(id, selection, selectionIndex, selections.length);
    selection.setAttribute("id", selectionId);
    const selectionSelector = `#${selectionId}`;

    let viewer = this.viewers.get(element);

    if (!viewer || viewer.parentElement !== element || viewer.getAttribute("selection") !== selectionSelector) {
      viewer = new CropperViewer();
      viewer.setAttribute("selection", selectionSelector);

      if (viewer.parentElement !== element) {
        element.replaceChildren(viewer);
      }

      this.viewers.set(element, viewer);
    } else {
      viewer.setAttribute("selection", selectionSelector);
      (viewer as any).$render(selection);
    }
  }

  private tryGetSelectionByIndex(id: CropperId, selectionIndex: number): CropperSelection | undefined {
    return this.getSelections(id)[selectionIndex];
  }

  private getSelections(id: CropperId): CropperSelection[] {
    const instance = this.tryGetParts(id)?.instance;

    return Array.from((instance as CropperWithSelections | undefined)?.getCropperSelections?.() ?? []);
  }

  private getSelectionId(id: CropperId, selection: CropperSelection, selectionIndex?: number, selectionCount?: number): string {
    if (selectionIndex === undefined) {
      return selection.id || `cropper-selection-${id}`;
    }

    if (selectionIndex === 0 && selectionCount === 1) {
      return `cropper-selection-${id}`;
    }

    return selection.active && selection.id
      ? selection.id
      : `cropper-selection-${id}-${selectionIndex}`;
  }
}
