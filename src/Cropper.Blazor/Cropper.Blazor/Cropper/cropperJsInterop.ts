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

    async readBlobInChunks(
        blob: Blob,
        receiverRef: any,
        maximumReceiveChunkSize?: number
    ) {
        if (!(blob instanceof Blob)) {
            throw new TypeError("blob must be a valid Blob");
        }

        const reader = maximumReceiveChunkSize
            ? this.buildChunkedReader(blob, maximumReceiveChunkSize)
            : blob.stream().getReader();

        try {
            while (true) {
                const { done, value } = await reader.read();
                if (done) break;

                await receiverRef.invokeMethodAsync("ReceiveImageChunk", value);
            }

            await receiverRef.invokeMethodAsync("CompleteImageTransfer");
        } catch (err) {
            await receiverRef.invokeMethodAsync(
                "HandleImageProcessingError",
                String(err)
            );
        }
    }

    private buildChunkedReader(blob: Blob, maxBytes: number): ReadableStreamDefaultReader<Uint8Array> {
        const blobReader = blob.stream().getReader();

        const stream = new ReadableStream<Uint8Array>({
            async pull(controller) {
                const { done, value } = await blobReader.read();
                if (done || !value) {
                    controller.close();
                    return;
                }

                let offset = 0;

                while (offset < value.length) {
                    const slice = value.slice(offset, offset + maxBytes);
                    offset += slice.length;
                    controller.enqueue(slice);
                }
            }
        });

        return stream.getReader();
    }

    sendImageInChunks(
        id: CropperId,
        options: any,
        receiverRef: any,
        type?: string,
        encoderOptions?: number,
        maximumReceiveChunkSize?: number
    ) {
        const instance = this.cropperInstances[id];
        options.maxWidth ??= Infinity;
        options.maxHeight ??= Infinity;

        setTimeout(() => {
            instance.getCroppedCanvas(options).toBlob(async (blob) => {
                if (blob) {
                    await this.readBlobInChunks(blob, receiverRef, maximumReceiveChunkSize);
                }
            }, type, encoderOptions);
        }, 0);
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
