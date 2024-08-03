window.overrideOnZoomCropperEvent = (minZoomRatio, maxZoomRatio) => {
  window.cropper.onZoom = function (imageObject, event, correlationId) {
    const jSEventData = this.getJSEventData(event, correlationId);

    const isApplyPreventZoomMinRatio =
      minZoomRatio != null && minZoomRatio > event.detail.ratio;
    const isApplyPreventZoomMaxRatio =
      maxZoomRatio != null && event.detail.ratio > maxZoomRatio;

    if (isApplyPreventZoomMinRatio || isApplyPreventZoomMaxRatio) {
      event.preventDefault();
    } else {
      imageObject.invokeMethodAsync("CropperIsZoomed", jSEventData);
    }
  };
};
