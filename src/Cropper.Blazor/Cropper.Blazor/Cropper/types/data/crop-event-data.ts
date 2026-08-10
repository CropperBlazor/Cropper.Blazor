export namespace CropperBlazor.Data {
  export type OriginalEventDataJS = {
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

  export type CropEventDataJS = {
    action: string;
    originalEvent: OriginalEventDataJS | null;
  };
}
