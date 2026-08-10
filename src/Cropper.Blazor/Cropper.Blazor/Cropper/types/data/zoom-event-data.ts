import type { CropperBlazor as CropEventDataTypes } from "./crop-event-data";

export namespace CropperBlazor.Data {
  export type ZoomEventDataJS = {
    oldRatio: number;
    ratio: number;
    originalEvent: CropEventDataTypes.Data.OriginalEventDataJS | null;
  };
}
