class CropperDecorator {

    constructor() {
        this.cropperInstance = new Object();
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

    initCropper(image, optionsImage, imageObject) {
        if (image == null) {
            throw "Parameter 'image' must be is not null!";
        }
        const options = {};
        if (imageObject != null) {
            options['ready'] = function (event) {
                imageObject.invokeMethodAsync('IsReady', event);
            };
            options['cropstart'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsStarted', event.detail);
            };
            options['cropmove'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsMoved', event.detail);
            };
            options['cropend'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsEnded', event.detail);
            };
            options['crop'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsCroped', event.detail);
            };
            options['zoom'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsZoomed', event.detail);
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