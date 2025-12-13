import Cropper from 'cropperjs';
import { ICropperComponentBase } from './types/components/cropper-component-base';
import { CroppedCanvasReceiver } from './types/components/cropped-canvas-receiver';
import { ImageReceiver } from './types/components/image-receiver';
import type { CropperBlazor as CropperEventTypes } from './types/cropper-event-data';
import type { CropperBlazor as CropperOptionsTypes } from './types/cropper-extended-options';
import { CropperUrlImageHelper } from './helpers/cropper-url-image-helper';

declare global {
    interface Window {
        cropper: CropperDecorator;
        cropperUrlImageHelper: typeof CropperUrlImageHelper;
    }
}

type CropperId = string;

export class CropperDecorator {
    private cropperInstances: Record<CropperId, Cropper> = {};

    clear(id: CropperId) {
        return this.cropperInstances[id].clear();
    }

    crop(id: CropperId) {
        return this.cropperInstances[id].crop();
    }

    destroy(id: CropperId) {
        const instance = this.cropperInstances[id];

        if (instance) {
            instance.destroy();

            delete this.cropperInstances[id];
        }
    }

    disable(id: CropperId) {
        return this.cropperInstances[id].disable();
    }

    enable(id: CropperId) {
        return this.cropperInstances[id].enable();
    }

    getCanvasData(id: CropperId): Cropper.CanvasData {
        return this.cropperInstances[id].getCanvasData();
    }

    getContainerData(id: CropperId): Cropper.ContainerData {
        return this.cropperInstances[id].getContainerData();
    }

    getCropBoxData(id: CropperId) {
        return this.cropperInstances[id].getCropBoxData();
    }

    getCroppedCanvas(id: CropperId, options: Cropper.GetCroppedCanvasOptions) {
        options.maxWidth ??= Infinity;
        options.maxHeight ??= Infinity;

        return this.cropperInstances[id].getCroppedCanvas(options);
    }

    async getCroppedCanvasInBackground(
        id: CropperId,
        options: Cropper.GetCroppedCanvasOptions,
        dotNetCanvasReceiverRef: DotNetObjectReference<CroppedCanvasReceiver>
    ) {
        setTimeout(async () => {
            const canvas = this.getCroppedCanvas(id, options);
            const jsRef = DotNet.createJSObjectReference(canvas);
            await dotNetCanvasReceiverRef.invokeMethodAsync(
                "ReceiveCanvasReference",
                jsRef
            );
        }, 0);
    }

    getCroppedCanvasDataURL(
        id: CropperId,
        options: Cropper.GetCroppedCanvasOptions,
        type?: string,
        encoderOptions?: number
    ) {
        options.maxWidth ??= Infinity;
        options.maxHeight ??= Infinity;

        return this.cropperInstances[id]
            .getCroppedCanvas(options)
            .toDataURL(type, encoderOptions);
    }

    getData(id: CropperId, rounded?: boolean): Cropper.Data {
        return this.cropperInstances[id].getData(rounded);
    }

    getImageData(id: CropperId): Cropper.ImageData {
        return this.cropperInstances[id].getImageData();
    }

    move(id: CropperId, offsetX: number, offsetY: number) {
        return this.cropperInstances[id].move(offsetX, offsetY);
    }

    moveTo(id: CropperId, x: number, y: number) {
        return this.cropperInstances[id].moveTo(x, y);
    }

    replace(id: CropperId, url: string, onlyColorChanged?: boolean) {
        return this.cropperInstances[id].replace(url, onlyColorChanged);
    }

    reset(id: CropperId) {
        return this.cropperInstances[id].reset();
    }

    rotate(id: CropperId, degree: number) {
        return this.cropperInstances[id].rotate(degree);
    }

    rotateTo(id: CropperId, degree: number) {
        return this.cropperInstances[id].rotateTo(degree);
    }

    scale(id: CropperId, x: number, y: number) {
        return this.cropperInstances[id].scale(x, y);
    }

    scaleX(id: CropperId, x: number) {
        return this.cropperInstances[id].scaleX(x);
    }

    scaleY(id: CropperId, y: number) {
        return this.cropperInstances[id].scaleY(y);
    }

    setAspectRatio(id: CropperId, ratio: number) {
        return this.cropperInstances[id].setAspectRatio(ratio);
    }

    setCanvasData(id: CropperId, data: Cropper.CanvasData) {
        return this.cropperInstances[id].setCanvasData(data);
    }

    setCropBoxData(id: CropperId, data: Cropper.SetCropBoxDataOptions) {
        return this.cropperInstances[id].setCropBoxData(data);
    }

    setData(id: CropperId, data: Cropper.Data) {
        return this.cropperInstances[id].setData(data);
    }

    setDragMode(id: CropperId, mode: Cropper.DragMode) {
        return this.cropperInstances[id].setDragMode(mode);
    }

    zoom(id: CropperId, ratio: number) {
        return this.cropperInstances[id].zoom(ratio);
    }

    zoomTo(id: CropperId, ratio: number, pivotX: number, pivotY: number) {
        return this.cropperInstances[id].zoomTo(ratio, { x: pivotX, y: pivotY });
    }

    noConflict() {
        return Cropper.noConflict();
    }

    setDefaults(options: Cropper.Options) {
        Cropper.setDefaults(options);
    }

    // --------------------------
    // Event serialization helpers
    // --------------------------

    getJSEventData<T extends EventTarget>(
        instance: Cropper.CropperEvent<T> | Cropper.CropEvent<T> | Cropper.CropStartEvent<T> | Cropper.CropMoveEvent<T> | Cropper.CropEndEvent<T>,
        correlationId: string | undefined): CropperEventTypes.CropperJSEventData {

        return {
            isTrusted: instance.isTrusted,
            detail: this.getJSEventDataDetail(instance),
            type: instance.type,
            eventPhase: instance.eventPhase,
            bubbles: instance.bubbles,
            cancelable: instance.cancelable,
            defaultPrevented: instance.defaultPrevented,
            composed: instance.composed,
            timeStamp: instance.timeStamp,
            returnValue: instance.returnValue,
            cancelBubble: instance.cancelBubble,
            correlationId
        };
    }

    getJSEventDataDetail<T extends EventTarget>(
        instance: Cropper.CropperEvent<T> | Cropper.CropEvent<T> | Cropper.CropStartEvent<T> | Cropper.CropMoveEvent<T> | Cropper.CropEndEvent<T>)
        : CropperEventTypes.CropperEventDataJS {
        if (instance.type === "zoom") {
            const zoomEventData: CropperEventTypes.CropperEventDataJS = {
                oldRatio: instance.detail.oldRatio,
                ratio: instance.detail.ratio,
                originalEvent: instance.detail.originalEvent
                    ? DotNet.createJSObjectReference(instance.detail.originalEvent)
                    : null
            };

            return zoomEventData;
        }
        else if (["cropstart", "cropend", "cropmove"].includes(instance.type)) {
            const cropEventData: CropperEventTypes.CropperEventDataJS = {
                action: instance.detail.action,
                originalEvent: instance.detail.originalEvent
                    ? DotNet.createJSObjectReference(instance.detail.originalEvent)
                    : null
            };

            return cropEventData;
        }

        return instance.detail;
    }

    onReady(
        imageObject: DotNetObjectReference<ICropperComponentBase>,
        e: Cropper.ReadyEvent<HTMLImageElement | HTMLCanvasElement>, id: string | undefined)
    {
        imageObject.invokeMethodAsync("IsReady", this.getJSEventData(e, id));
    }

    onCropStart(
        imageObject: DotNetObjectReference<ICropperComponentBase>,
        e: Cropper.CropStartEvent<HTMLImageElement | HTMLCanvasElement>, id: string | undefined)
    {
        imageObject.invokeMethodAsync("CropperIsStarted", this.getJSEventData(e, id));
    }

    onCropMove(
        imageObject: DotNetObjectReference<ICropperComponentBase>,
        e: Cropper.CropMoveEvent<HTMLImageElement | HTMLCanvasElement>, id: string | undefined)
    {
        imageObject.invokeMethodAsync("CropperIsMoved", this.getJSEventData(e, id));
    }

    onCropEnd(
        imageObject: DotNetObjectReference<ICropperComponentBase>,
        e: Cropper.CropEndEvent<HTMLImageElement | HTMLCanvasElement>, id: string | undefined)
    {
        imageObject.invokeMethodAsync("CropperIsEnded", this.getJSEventData(e, id));
    }

    onCrop(
        imageObject: DotNetObjectReference<ICropperComponentBase>,
        e: Cropper.CropEvent<HTMLImageElement | HTMLCanvasElement>, id: string | undefined)
    {
        imageObject.invokeMethodAsync("CropperIsCroped", this.getJSEventData(e, id));
    }

    onZoom(
        imageObject: DotNetObjectReference<ICropperComponentBase>,
        e: Cropper.ZoomEvent<HTMLImageElement | HTMLCanvasElement>, id: string | undefined)
    {
        imageObject.invokeMethodAsync("CropperIsZoomed", this.getJSEventData(e, id));
    }

    initCropper(
        id: CropperId,
        image: HTMLImageElement | HTMLCanvasElement,
        optionsImage: CropperOptionsTypes.CropperExtendedOptions,
        imageObject?: DotNetObjectReference<ICropperComponentBase>
    ) {
        if (!image) throw new Error("Parameter 'image' must not be null");
        if (!optionsImage) throw new Error("Parameter 'optionsImage' must not be null");

        const options: Cropper.Options<HTMLImageElement> | Cropper.Options<HTMLCanvasElement> = {};
        const correlationId: string | undefined = optionsImage.correlationId;

        if (imageObject) {
            options.ready = (e: Cropper.ReadyEvent<HTMLImageElement | HTMLCanvasElement>) => this.onReady(imageObject, e, correlationId);
            options.cropstart = (e: Cropper.CropStartEvent<HTMLImageElement | HTMLCanvasElement>) => this.onCropStart(imageObject, e, correlationId);
            options.cropmove = (e: Cropper.CropMoveEvent<HTMLImageElement | HTMLCanvasElement>) => this.onCropMove(imageObject, e, correlationId);
            options.cropend = (e: Cropper.CropEndEvent<HTMLImageElement | HTMLCanvasElement>) => this.onCropEnd(imageObject, e, correlationId);
            options.crop = (e: Cropper.CropEvent<HTMLImageElement | HTMLCanvasElement>) => this.onCrop(imageObject, e, correlationId);
            options.zoom = (e: Cropper.ZoomEvent<HTMLImageElement | HTMLCanvasElement>) => this.onZoom(imageObject, e, correlationId);
        }

        Object.assign(options, optionsImage);

        if (image instanceof HTMLImageElement) {
            const cropper: Cropper = new Cropper(image, options as Cropper.Options<HTMLImageElement>);
            this.cropperInstances[id] = cropper;
        } else if (image instanceof HTMLCanvasElement) {
            const cropper: Cropper = new Cropper(image, options as Cropper.Options<HTMLCanvasElement>);
            this.cropperInstances[id] = cropper;
        } else {
            throw new Error(
                `Unsupported element type for Cropper: ${Object.prototype.toString.call(image)}`
            );
        }
    }

    // --------------------------
    // Chunked Blob Streaming
    // --------------------------

    async readBlobInChunks(
        blob: Blob | null,
        dotNetImageReceiverRef: DotNetObjectReference<ImageReceiver>,
        maximumReceiveChunkSize?: number)
    {
        // Validate blob
        if (!(blob instanceof Blob)) {
            throw new TypeError('blob must be a valid Blob object.')
        }

        // Validate dotNetImageReceiverRef
        if (!dotNetImageReceiverRef || typeof dotNetImageReceiverRef.invokeMethodAsync !== 'function') {
            throw new TypeError('dotNetImageReceiverRef must be a valid .NET object reference with an invokeMethodAsync function.')
        }

        // Validate maximumReceiveChunkSize
        if (maximumReceiveChunkSize != null && maximumReceiveChunkSize <= 0) {
            throw new RangeError('maximumReceiveChunkSize must be greater than 0 bytes when specified.')
        }

        // By default, blob.stream() reads the blob using internal chunking (typically 65536 bytes per chunk).
        // To enforce a custom chunk size, especially to control serialized message size for JS interop or SignalR limits, we wrap it in a transformed ReadableStream.
        // This allows us to split the default chunks further to stay within a maximum size constraint (e.g., for Blazor's JS interop or SignalR message limits).
        let reader: ReadableStreamDefaultReader<Uint8Array> | null = null

        if (maximumReceiveChunkSize == null) {
            reader = blob.stream().getReader()
        } else {
            const blobStream = blob.stream().getReader()

            // Binary estimation of JSON size
            const getJsonSizeBinary = (chunk) => {
                const length = chunk.length

                // Max 3 digits for the number (0 to 255)
                const bytesPerElement = 3
                // Comma between elements
                const commas = length - 1
                // For '[' and ']'
                const brackets = 2

                return (length * bytesPerElement) + commas + brackets
            }

            // Create a custom stream that enforces max chunk size
            const transformedStream = new ReadableStream({
                async pull(controller) {
                    const { done, value } = await blobStream.read()

                    if (done) {
                        controller.close()

                        return
                    }

                    // Function to calculate JSON size for the current chunk using binary estimation
                    let offset = 0
                    let lastGoodChunkSize = maximumReceiveChunkSize

                    while (offset < value.length) {
                        // Start with the last known good chunk size, or the remaining length
                        let chunkSize = Math.min(lastGoodChunkSize, value.length - offset)
                        let chunk = value.slice(offset, offset + chunkSize)
                        let jsonSize = getJsonSizeBinary(chunk)

                        // If the JSON size is too large, reduce the chunk size gradually
                        while (jsonSize > maximumReceiveChunkSize && chunkSize > 1) {
                            // Reduce the chunk size in steps of 512 bytes, but not below 1 byte
                            chunkSize = Math.max(chunkSize - 512, 1)
                            chunk = value.slice(offset, offset + chunkSize)
                            jsonSize = getJsonSizeBinary(chunk)

                            // Stop reducing if the chunk size is already very small
                            if (chunkSize <= 512) {
                                break
                            }
                        }

                        // Move the offset forward by the size of the chunk just sent with update the last good chunk size
                        lastGoodChunkSize = chunkSize

                        offset += chunkSize

                        controller.enqueue(chunk)
                    }
                }
            })

            reader = transformedStream.getReader()
        }

        try {
            while (true) {
                const { done, value } = await reader.read()
                if (done) break

                await dotNetImageReceiverRef.invokeMethodAsync('ReceiveImageChunk', value)
            }

            await dotNetImageReceiverRef.invokeMethodAsync('CompleteImageTransfer')
        } catch (error) {
            await dotNetImageReceiverRef.invokeMethodAsync('HandleImageProcessingError', String(error))
        }
    }

    sendImageInChunks(
        cropperComponentId: CropperId,
        options: Cropper.GetCroppedCanvasOptions,
        dotNetImageReceiverRef: DotNetObjectReference<ImageReceiver>,
        type?: string,
        encoderOptions?: number,
        maximumReceiveChunkSize?: number)
    {
        options.maxWidth ??= Infinity
        options.maxHeight ??= Infinity

        const cropperInstance = this.cropperInstances[cropperComponentId]

        setTimeout(() => {
            cropperInstance.getCroppedCanvas(options).toBlob(async (blob) => {
                await this.readBlobInChunks(blob, dotNetImageReceiverRef, maximumReceiveChunkSize)
            }, type, encoderOptions)
        }, 0)
    }
}


window.cropper = new CropperDecorator();
window.cropperUrlImageHelper = CropperUrlImageHelper;
