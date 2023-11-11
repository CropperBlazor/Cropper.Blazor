window.overrideOnZoomCropperEvent = (minZoomRatio: number, maxZoomRatio: number): void => {
  window.cropper.onZoom = function (imageObject: any, event: any, correlationId: any): void {
    const jSEventData: any = this.getJSEventData(event, correlationId)

    const isApplyPreventZoomMinRatio: boolean = (minZoomRatio != null) && (minZoomRatio > event.detail.ratio)
    const isApplyPreventZoomMaxRatio: boolean = (maxZoomRatio != null) && (event.detail.ratio > maxZoomRatio)

    if (isApplyPreventZoomMinRatio || isApplyPreventZoomMaxRatio) {
      event.preventDefault()
    } else {
      imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData)
    }
  }
}
