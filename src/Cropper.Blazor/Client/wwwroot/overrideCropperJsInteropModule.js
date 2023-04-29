window.overrideCropperJsInteropModule = (minZoomRatio, maxZoomRatio) => {
    window.cropper.onZoom = function (imageObject, event) {
        const jSEventData = this.getJSEventData(event);
        const isApplyPreventZoomRatio = minZoomRatio != null || maxZoomRatio != null;

        if (isApplyPreventZoomRatio && (event.detail.ratio < minZoomRatio || event.detail.ratio > maxZoomRatio)) {
            event.preventDefault();
        }
        else {
            imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData);
        }
    };
};