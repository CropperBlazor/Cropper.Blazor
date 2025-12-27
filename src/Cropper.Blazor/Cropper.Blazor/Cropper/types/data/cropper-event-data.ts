import type { CropperBlazor as CropEventDataTypes } from './crop-event-data';
import type { CropperBlazor as ZoomEventDataTypes } from './zoom-event-data';

export namespace CropperBlazor.Data {
    export type CropperEventDataJS = CropEventDataTypes.Data.CropEventDataJS | ZoomEventDataTypes.Data.ZoomEventDataJS;

    export type CropperJSEventData = {
        isTrusted: boolean;
        detail: CropperEventDataJS;
        type: string;
        eventPhase: number;
        bubbles: boolean;
        cancelable: boolean;
        defaultPrevented: boolean;
        composed: boolean;
        timeStamp: number;
        returnValue: any;
        cancelBubble: boolean;
        correlationId?: string;
    };
}