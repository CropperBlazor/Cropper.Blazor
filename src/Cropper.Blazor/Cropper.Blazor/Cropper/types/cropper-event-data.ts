export namespace CropperBlazor {
    export type CropEventDataJS = {
        action: string;
        originalEvent: JsObjectReference | null;
    };

    export type ZoomEventDataJS = {
        oldRatio: number;
        ratio: number;
        originalEvent: JsObjectReference | null;
    };

    export type CropperEventDataJS = CropEventDataJS | ZoomEventDataJS;

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