import type { CropperBlazor as DotNetTypes } from '../../types/global/dotnet-global';

export namespace CropperBlazor.Data {
    export type ZoomEventDataJS = {
        oldRatio: number;
        ratio: number;
        originalEvent: DotNetTypes.Global.JsObjectReference | null;
    };
}