import type { CropperBlazor as DotNetTypes } from '../../types/global/dotnet-global';

export namespace CropperBlazor.Data {
    export type CropEventDataJS = {
        action: string;
        originalEvent: DotNetTypes.Global.JsObjectReference | null;
    };
}