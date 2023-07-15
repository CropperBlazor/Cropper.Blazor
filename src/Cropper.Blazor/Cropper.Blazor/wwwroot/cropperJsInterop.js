class CropperDecorator {

    constructor() {
        this.cropperInstance = {};
    }

    clear() {
        return this.cropperInstance.clear();
    }

    crop() {
        return this.cropperInstance.crop();
    }

    destroy() {
        return this.cropperInstance.destroy();
    }

    disable() {
        return this.cropperInstance.disable();
    }

    enable() {
        return this.cropperInstance.enable();
    }

    getCanvasData() {
        return this.cropperInstance.getCanvasData();
    }

    getContainerData() {
        return this.cropperInstance.getContainerData();
    }

    getCropBoxData() {
        return this.cropperInstance.getCropBoxData();
    }

    getCroppedCanvas(options) {
        return this.cropperInstance.getCroppedCanvas(options);
    }

    getCroppedCanvasDataURL(options, type, encoderOptions) {
        return this.cropperInstance.getCroppedCanvas(options).toDataURL(type, encoderOptions);
    }

    getData(rounded) {
        return this.cropperInstance.getData(rounded);
    }

    getImageData() {
        return this.cropperInstance.getImageData();
    }

    move(offsetX, offsetY) {
        return this.cropperInstance.move(offsetX, offsetY);
    }

    moveTo(x, y) {
        return this.cropperInstance.moveTo(x, y);
    }

    replace(url, onlyColorChanged) {
        return this.cropperInstance.replace(url, onlyColorChanged);
    }

    reset() {
        return this.cropperInstance.reset();
    }

    rotate(degree) {
        return this.cropperInstance.rotate(degree);
    }

    rotateTo(degree) {
        return this.cropperInstance.rotateTo(degree);
    }

    scale(scaleX, scaleY) {
        return this.cropperInstance.scale(scaleX, scaleY);
    }

    scaleX(scaleX) {
        return this.cropperInstance.scaleX(scaleX);
    }

    scaleY(scaleY) {
        return this.cropperInstance.scaleY(scaleY);
    }

    setAspectRatio(aspectRatio) {
        return this.cropperInstance.setAspectRatio(aspectRatio);
    }

    setCanvasData(data) {
        return this.cropperInstance.setCanvasData(data);
    }

    setCropBoxData(data) {
        return this.cropperInstance.setCropBoxData(data);
    }

    setData(data) {
        return this.cropperInstance.setData(data);
    }

    setDragMode(dragMode) {
        return this.cropperInstance.setDragMode(dragMode);
    }

    zoom(ratio) {
        return this.cropperInstance.zoom(ratio);
    }

    zoomTo(ratio, pivotX, pivotY) {
        return this.cropperInstance.zoomTo(ratio, { pivotX, pivotY });
    }

    noConflict() {
        return Cropper.noConflict();
    }

    setDefaults(options) {
        return Cropper.setDefaults(options);
    }

    async getImageUsingStreaming(imageStream) {
        const arrayBuffer = await imageStream.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        return URL.createObjectURL(blob);
    }

    revokeObjectUrl(url) {
        URL.revokeObjectURL(url);
    }

    getJSEventData(instance, correlationId) {
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
            correlationId: correlationId
        };
    }

    getJSEventDataDetail(instance) {
        if (instance.type === "zoom") {
            return {
                    oldRatio: instance.detail.oldRatio,
                    ratio: instance.detail.ratio,
                    originalEvent: instance.detail.originalEvent ?
                        DotNet.createJSObjectReference(instance.detail.originalEvent) : null
            };
        }
        else if (instance.type === "cropstart" || instance.type === "cropend" || instance.type === "cropmove") {
            return {
                action: instance.detail.action,
                originalEvent: instance.detail.originalEvent ?
                    DotNet.createJSObjectReference(instance.detail.originalEvent) : null
            };
        }

        return instance.detail;
    }

    onReady(imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        imageObject.invokeMethodAsync('IsReady', jSEventData);
    }

    onCropStart(imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        imageObject.invokeMethodAsync('CropperIsStarted', jSEventData);
    }

    onCropMove(imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        imageObject.invokeMethodAsync('CropperIsMoved', jSEventData);
    }

    onCropEnd(imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        imageObject.invokeMethodAsync('CropperIsEnded', jSEventData);
    }

    onCrop(imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        imageObject.invokeMethodAsync('CropperIsCroped', jSEventData);
    }

    onZoom(imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData);
    }

    initCropper(image, optionsImage, imageObject) {
        if (image == null) {
            throw "Parameter 'image' must be is not null!";
        }

        if (optionsImage == null) {
            throw "Parameter 'optionsImage' must be is not null!";
        }

        const options = {};
        const correlationId = optionsImage["correlationId"];

        if (imageObject != null) {
            const self = this;

            options['ready'] = function (event) {
                self.onReady(imageObject, event, correlationId);
            };
            options['cropstart'] = function (event) {
                self.onCropStart(imageObject, event, correlationId);
            };
            options['cropmove'] = function (event) {
                self.onCropMove(imageObject, event, correlationId);
            };
            options['cropend'] = function (event) {
                self.onCropEnd(imageObject, event, correlationId);
            };
            options['crop'] = function (event) {
                self.onCrop(imageObject, event, correlationId);
            };
            options['zoom'] = function (event) {
                self.onZoom(imageObject, event, correlationId);
            };
        }

        if (optionsImage != null) {
            Object.entries(optionsImage)?.forEach(([key, value]) => {
                options[key] = value;
            });
        }

        this.cropperInstance = new Cropper(image, options);
    }
}

window.cropper = new CropperDecorator();