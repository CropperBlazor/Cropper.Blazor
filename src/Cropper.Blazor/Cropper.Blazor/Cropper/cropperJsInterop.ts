import * as Cropper from 'cropperjs/src';


declare global {
    interface DotNet {
        createJSObjectReference(obj: any): any;
    }

    const DotNet: DotNet;

    interface Window {
        cropper: CropperDecorator;
        cropperUrlImageHelper: typeof CropperUrlImageHelper;
    }
}

type CropperId = string;

interface CropperExtendedOptions<T extends EventTarget = HTMLImageElement>
    extends Cropper.default.Options<T> {
    correlationId?: string;
}

export class CropperDecorator {
    private cropperInstances: Record<CropperId, Cropper.default> = {};

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

    getCanvasData(id: CropperId): Cropper.default.CanvasData {
        return this.cropperInstances[id].getCanvasData();
    }

    getContainerData(id: CropperId): Cropper.default.ContainerData {
        return this.cropperInstances[id].getContainerData();
    }

    getCropBoxData(id: CropperId) {
        return this.cropperInstances[id].getCropBoxData();
    }

    getCroppedCanvas(id: CropperId, options: any) {
        options.maxWidth ??= Infinity;
        options.maxHeight ??= Infinity;
        return this.cropperInstances[id].getCroppedCanvas(options);
    }

    async getCroppedCanvasInBackground(
        id: CropperId,
        options: any,
        dotNetCanvasReceiverRef: any
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
        options: any,
        type?: string,
        encoderOptions?: number
    ) {
        options.maxWidth ??= Infinity;
        options.maxHeight ??= Infinity;

        return this.cropperInstances[id]
            .getCroppedCanvas(options)
            .toDataURL(type, encoderOptions);
    }

    getData(id: CropperId, rounded?: boolean): Cropper.default.Data {
        return this.cropperInstances[id].getData(rounded);
    }

    getImageData(id: CropperId): Cropper.default.ImageData {
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

    setCanvasData(id: CropperId, data: Cropper.default.CanvasData) {
        return this.cropperInstances[id].setCanvasData(data);
    }

    setCropBoxData(id: CropperId, data: any) {
        return this.cropperInstances[id].setCropBoxData(data);
    }

    setData(id: CropperId, data: Cropper.default.Data) {
        return this.cropperInstances[id].setData(data);
    }

    setDragMode(id: CropperId, mode: Cropper.default.DragMode) {
        return this.cropperInstances[id].setDragMode(mode);
    }

    zoom(id: CropperId, ratio: number) {
        return this.cropperInstances[id].zoom(ratio);
    }

    zoomTo(id: CropperId, ratio: number, pivotX: number, pivotY: number) {
        return this.cropperInstances[id].zoomTo(ratio, { x: pivotX, y: pivotY });
    }

    noConflict() {
        return Cropper.default.noConflict();
    }

    setDefaults(options: Cropper.default.Options) {
        Cropper.default.setDefaults(options);
    }

    // --------------------------
    // Event serialization helpers
    // --------------------------

    getJSEventData(instance: any, correlationId: any) {
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

    getJSEventDataDetail(instance: any): any {
        if (instance.type === "zoom") {
            return {
                oldRatio: instance.detail.oldRatio,
                ratio: instance.detail.ratio,
                originalEvent: instance.detail.originalEvent
                    ? DotNet.createJSObjectReference(instance.detail.originalEvent)
                    : null
            };
        }

        if (["cropstart", "cropend", "cropmove"].includes(instance.type)) {
            return {
                action: instance.detail.action,
                originalEvent: instance.detail.originalEvent
                    ? DotNet.createJSObjectReference(instance.detail.originalEvent)
                    : null
            };
        }

        return instance.detail;
    }

    onReady(imageObject: any, e: any, id: any) {
        imageObject.invokeMethodAsync("IsReady", this.getJSEventData(e, id));
    }

    onCropStart(imageObject: any, e: any, id: any) {
        imageObject.invokeMethodAsync("CropperIsStarted", this.getJSEventData(e, id));
    }

    onCropMove(imageObject: any, e: any, id: any) {
        imageObject.invokeMethodAsync("CropperIsMoved", this.getJSEventData(e, id));
    }

    onCropEnd(imageObject: any, e: any, id: any) {
        imageObject.invokeMethodAsync("CropperIsEnded", this.getJSEventData(e, id));
    }

    onCrop(imageObject: any, e: any, id: any) {
        imageObject.invokeMethodAsync("CropperIsCroped", this.getJSEventData(e, id));
    }

    onZoom(imageObject: any, e: any, id: any) {
        imageObject.invokeMethodAsync("CropperIsZoomed", this.getJSEventData(e, id));
    }

    initCropper(
        id: CropperId,
        image: HTMLImageElement,
        optionsImage: CropperExtendedOptions,
        imageObject?: any
    ) {
        if (!image) throw new Error("Parameter 'image' must not be null");
        if (!optionsImage) throw new Error("Parameter 'optionsImage' must not be null");

        const options: Cropper.default.Options<HTMLImageElement> = {};
        const correlationId = optionsImage.correlationId;

        if (imageObject) {
            options.ready = (e: any) => this.onReady(imageObject, e, correlationId);
            options.cropstart = (e: any) => this.onCropStart(imageObject, e, correlationId);
            options.cropmove = (e: any) => this.onCropMove(imageObject, e, correlationId);
            options.cropend = (e: any) => this.onCropEnd(imageObject, e, correlationId);
            options.crop = (e: any) => this.onCrop(imageObject, e, correlationId);
            options.zoom = (e: any) => this.onZoom(imageObject, e, correlationId);
        }

        Object.assign(options, optionsImage);

        const cropper = new Cropper.default(image, options);
        this.cropperInstances[id] = cropper;
    }

    // --------------------------
    // Chunked Blob Streaming
    // --------------------------

    async readBlobInChunks(blob: Blob | null, dotNetImageReceiverRef: any, maximumReceiveChunkSize?: number) {
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

    sendImageInChunks(cropperComponentId: CropperId, options, dotNetImageReceiverRef: any, type?: string, encoderOptions?: number, maximumReceiveChunkSize?: number) {
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

// ---------------------------------------------------
// URL Image Helper
// ---------------------------------------------------

export class CropperUrlImageHelper {
    static async getImageUsingStreaming(imageStream: any): Promise<string> {
        const buf = await imageStream.arrayBuffer();
        const blob = new Blob([buf]);
        return URL.createObjectURL(blob);
    }

    static revokeObjectUrl(url: string) {
        URL.revokeObjectURL(url);
    }
}

window.cropper = new CropperDecorator();
window.cropperUrlImageHelper = CropperUrlImageHelper;
