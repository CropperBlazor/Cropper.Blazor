export namespace CropperBlazor.Components {
  /** Represents a receiver for image data streamed from JS to .NET */
  export interface ImageReceiver {
    /** Called when an error occurs during image processing */
    HandleImageProcessingError(errorMessage: string): void;

    /** Called to receive a chunk of image data from JS */
    ReceiveImageChunk(chunk: Uint8Array): Promise<void>;

    /** Called when the image transfer is complete */
    CompleteImageTransfer(): void;
  }
}
