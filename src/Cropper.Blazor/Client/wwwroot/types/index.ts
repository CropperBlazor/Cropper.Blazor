import { IDownloadOptions } from "./IDownloadOptions"
import { JsObject } from "./JsObject"

declare global {
  interface Window {
    overrideOnZoomCropperEvent(minZoomRatio: number, maxZoomRatio: number): void
    cropper: any
    jsObject: JsObject
    updateAvailable: Promise<boolean>
    downloadFromUrl(options: IDownloadOptions): void
    addClipPathPolygon(sourceCanvas: HTMLCanvasElement, path: number[]): string
    addClipPathEllipse(sourceCanvas: HTMLCanvasElement): string
  }
}
