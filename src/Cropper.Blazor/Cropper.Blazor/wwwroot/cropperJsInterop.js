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

    initCropper(image, optionsImage, imageObject) {
        if (image == null) {
            throw "Parameter 'image' must be is not null!";
        }
        const options = {};
        if (imageObject != null) {
            options['ready'] = function (event) {
                console.log("Ready: ");
                console.log(event);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('IsReady', eref).then(() => {
                    DotNet.disposeJSObjectReference(eref);
                });
            };
            options['cropstart'] = function (event) {
                console.log("cropstart: ");
                console.log(event);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsStarted', eref).then(() => {
                    DotNet.disposeJSObjectReference(eref);
                });
            };
            options['cropmove'] = function (event) {
                console.log("cropmove: ");
                console.log(event);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsMoved', eref).then(() => {
                    DotNet.disposeJSObjectReference(eref);
                });
            };
            options['cropend'] = function (event) {
                console.log("cropend: ");
                console.log(event);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsEnded', eref).then(() => {
                    DotNet.disposeJSObjectReference(eref);
                });
            };
            options['crop'] = function (event) {
                console.log("crop: ");
                console.log(event);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsCroped', eref).then(() => {
                    DotNet.disposeJSObjectReference(eref);
                });
            };
            options['zoom'] = function (event) {
                console.log("zoom: ");
                console.log(event);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsZoomed', eref).then(() => {
                    DotNet.disposeJSObjectReference(eref);
                });
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

class JsObject {
    getPropertyList(path) {
        let res = path.replace('[', '.').replace(']', '').split('.');

        if (res[0] === "") { // if we pass "[0].id" we want to return [0,'id']
            res.shift();
        }

        return res;
    }

    getInstanceProperty(instance, propertyPath) {

        if (propertyPath === '') {
            return instance;
        }

        let currentProperty = instance;
        let splitProperty = this.getPropertyList(propertyPath);

        for (let i = 0; i < splitProperty.length; i++) {
            if (splitProperty[i] in currentProperty) {
                currentProperty = currentProperty[splitProperty[i]];
            } else {
                return null;
            }
        }

        return currentProperty;
    }
}

window.cropper = new CropperDecorator();
window.jsObject = new JsObject();