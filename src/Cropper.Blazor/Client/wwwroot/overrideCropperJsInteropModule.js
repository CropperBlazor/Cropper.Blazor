window.overrideCropperJsInteropModule = (minZoomRatio, maxZoomRatio) => {
    window.cropper.onZoom = function (imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        const isApplyPreventZoomRatio = minZoomRatio != null || maxZoomRatio != null;

        if (isApplyPreventZoomRatio && (event.detail.ratio < minZoomRatio || event.detail.ratio > maxZoomRatio)) {
            event.preventDefault();
        }
        else {
            imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData);
        }
    };
};
