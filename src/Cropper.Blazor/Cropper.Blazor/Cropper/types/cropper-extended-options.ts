import Cropper from 'cropperjs';

export namespace CropperBlazor {
    export type CropperExtendedOptions<T extends EventTarget = HTMLImageElement | HTMLCanvasElement> =
        Cropper.Options<T> & {
            correlationId?: string;
        };
}