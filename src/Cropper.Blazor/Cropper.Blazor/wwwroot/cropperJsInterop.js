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

    getCroppedCanvasDataURL(options) {
        return this.cropperInstance.getCroppedCanvas(options).toDataURL();
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

    getJSEventData(instance) {
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
            cancelBubble: instance.cancelBubble
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

    onReady (imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        imageObject.invokeMethodAsync('IsReady', jSEventData);
    }

    onCropStart(imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        imageObject.invokeMethodAsync('CropperIsStarted', jSEventData);
    }

    onCropMove(imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        imageObject.invokeMethodAsync('CropperIsMoved', jSEventData);
    }

    onCropEnd(imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        imageObject.invokeMethodAsync('CropperIsEnded', jSEventData);
    }

    onCrop(imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        imageObject.invokeMethodAsync('CropperIsCroped', jSEventData);
    }

    onZoom(imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData);
    }

    initCropper(image, optionsImage, imageObject) {
        if (image == null) {
            throw "Parameter 'image' must be is not null!";
        }

        const options = {};

        if (imageObject != null) {
            const self = this;

            options['ready'] = function (event) {
                self.onReady(imageObject, event);
            };
            options['cropstart'] = function (event) {
                self.onCropStart(imageObject, event);
            };
            options['cropmove'] = function (event) {
                self.onCropMove(imageObject, event);
            };
            options['cropend'] = function (event) {
                self.onCropEnd(imageObject, event);
            };
            options['crop'] = function (event) {
                self.onCrop(imageObject, event);
            };
            options['zoom'] = function (event) {
                self.onZoom(imageObject, event);
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