/** Represents a receiver for a cropped canvas streamed from JS to .NET */
export interface CroppedCanvasReceiver {
    /** Called when a cropped canvas is received from JS */
    ReceiveCanvasReference(canvasRef: HTMLCanvasElement): void;
}