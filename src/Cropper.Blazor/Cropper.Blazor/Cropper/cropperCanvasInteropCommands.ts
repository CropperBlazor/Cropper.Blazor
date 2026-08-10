import { CropperCanvas } from "cropperjs";
import { CropperId, CropperInstanceParts } from "./cropperJsInterop.types";

type GetParts = (id: CropperId) => CropperInstanceParts;
type GetCanvasAction = (action: string | null | undefined) => string;

export class CropperCanvasInteropCommands {
  constructor(
    private readonly getParts: GetParts,
    private readonly getCanvasAction: GetCanvasAction,
  ) {
  }

  disable(id: CropperId) {
    const { canvas } = this.getParts(id);

    canvas.disabled = true;
  }

  enable(id: CropperId) {
    const { canvas } = this.getParts(id);

    canvas.disabled = false;
  }

  setDragMode(id: CropperId, mode: string) {
    const { canvas } = this.getParts(id);
    const action = this.getCanvasAction(mode);

    canvas.$setAction(action);
    this.setCanvasHandleAction(canvas, action);
  }

  private setCanvasHandleAction(canvas: CropperCanvas, action: string) {
    const handle = canvas.querySelector("cropper-handle[plain]") as HTMLElement | null;

    if (handle) {
      handle.setAttribute("action", action);
    }
  }
}
