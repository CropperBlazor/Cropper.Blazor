window.overrideCropperJsInteropModule = () => {
    window.cropper.onZoom = function (imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        event.preventDefault();
        imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData);
    };
};