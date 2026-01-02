import type { CropperBlazor as ImageReceiverTypes } from "../types/components/image-receiver.custom";
import type { CropperBlazor as DotNetTypes } from "../types/global/dotnet-global.custom";

// --------------------------
// Chunked Blob Streaming
// Provides a utility to read a Blob in chunks and stream it to a .NET receiver
// via JS interop. Supports optional maximum chunk size to respect message limits.
// Useful for Blazor interop, SignalR, or other large file transfers.
// --------------------------
export namespace CropperBlazor.Helpers {
  export async function readBlobInChunks(
    blob: Blob | null,
    dotNetImageReceiverRef: DotNetTypes.Global.DotNetObjectReference<ImageReceiverTypes.Components.ImageReceiver>,
    maximumReceiveChunkSize?: number,
  ) {
    // Validate blob
    if (!(blob instanceof Blob)) {
      throw new TypeError("blob must be a valid Blob object.");
    }

    // Validate dotNetImageReceiverRef
    if (!dotNetImageReceiverRef || typeof dotNetImageReceiverRef.invokeMethodAsync !== "function") {
      throw new TypeError(
        "dotNetImageReceiverRef must be a valid .NET object reference with an invokeMethodAsync function.",
      );
    }

    // Validate maximumReceiveChunkSize
    if (maximumReceiveChunkSize != null && maximumReceiveChunkSize <= 0) {
      throw new RangeError("maximumReceiveChunkSize must be greater than 0 bytes when specified.");
    }

    // By default, blob.stream() reads the blob using internal chunking (typically 65536 bytes per chunk).
    // To enforce a custom chunk size, especially to control serialized message size for JS interop or SignalR limits, we wrap it in a transformed ReadableStream.
    // This allows us to split the default chunks further to stay within a maximum size constraint (e.g., for Blazor's JS interop or SignalR message limits).
    let reader: ReadableStreamDefaultReader<Uint8Array> | null = null;

    if (maximumReceiveChunkSize == null) {
      reader = blob.stream().getReader();
    } else {
      const blobStream = blob.stream().getReader();

      // Binary estimation of JSON size
      const getJsonSizeBinary = (chunk: Uint8Array<ArrayBuffer>) => {
        const length = chunk.length;

        // Max 3 digits for the number (0 to 255)
        const bytesPerElement = 3;
        // Comma between elements
        const commas = length - 1;
        // For '[' and ']'
        const brackets = 2;

        return length * bytesPerElement + commas + brackets;
      };

      // Create a custom stream that enforces max chunk size
      const transformedStream = new ReadableStream({
        async pull(controller) {
          const { done, value } = await blobStream.read();

          if (done) {
            controller.close();

            return;
          }

          // Function to calculate JSON size for the current chunk using binary estimation
          let offset = 0;
          let lastGoodChunkSize = maximumReceiveChunkSize;

          while (offset < value.length) {
            // Start with the last known good chunk size, or the remaining length
            let chunkSize = Math.min(lastGoodChunkSize, value.length - offset);
            let chunk = value.slice(offset, offset + chunkSize);
            let jsonSize = getJsonSizeBinary(chunk);

            // If the JSON size is too large, reduce the chunk size gradually
            while (jsonSize > maximumReceiveChunkSize && chunkSize > 1) {
              // Reduce the chunk size in steps of 512 bytes, but not below 1 byte
              chunkSize = Math.max(chunkSize - 512, 1);
              chunk = value.slice(offset, offset + chunkSize);
              jsonSize = getJsonSizeBinary(chunk);

              // Stop reducing if the chunk size is already very small
              if (chunkSize <= 512) {
                break;
              }
            }

            // Move the offset forward by the size of the chunk just sent with update the last good chunk size
            lastGoodChunkSize = chunkSize;

            offset += chunkSize;

            controller.enqueue(chunk);
          }
        },
      });

      reader = transformedStream.getReader();
    }

    try {
      while (true) {
        const { done, value } = await reader.read();
        if (done) break;

        await dotNetImageReceiverRef.invokeMethodAsync("ReceiveImageChunk", value);
      }

      await dotNetImageReceiverRef.invokeMethodAsync("CompleteImageTransfer");
    } catch (error) {
      await dotNetImageReceiverRef.invokeMethodAsync("HandleImageProcessingError", String(error));
    }
  }
}
