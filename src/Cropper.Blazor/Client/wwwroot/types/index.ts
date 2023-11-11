export { }

declare global {
  interface DownloadOptions {
    url: string
    fileName?: string
  }

  interface Window {
    overrideOnZoomCropperEvent(minZoomRatio: number, maxZoomRatio: number): void
    cropper: any
    jsObject: JsObject
    updateAvailable: Promise<boolean>
    downloadFromUrl(options: DownloadOptions): void
    addClipPathPolygon(sourceCanvas: HTMLCanvasElement, path: number[]): string
    addClipPathEllipse(sourceCanvas: HTMLCanvasElement): string
  }
}
