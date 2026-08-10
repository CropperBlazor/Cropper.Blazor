import { describe, expect, it, vi } from "vitest";
import { CropperBlazor } from "./blob-helper";

class MockDotNetImageReceiver {
  readonly invokeMethodAsync = vi.fn<[string, unknown?], Promise<void>>(async () => undefined);
}

class StreamableBlob extends Blob {
  constructor(private readonly bytes: Uint8Array<ArrayBuffer>) {
    super([bytes.buffer]);
  }

  stream(): ReadableStream<Uint8Array<ArrayBuffer>> {
    const bytes = this.bytes;

    return new ReadableStream<Uint8Array<ArrayBuffer>>({
      start(controller) {
        controller.enqueue(bytes);
        controller.close();
      },
    });
  }
}

describe("readBlobInChunks coverage", () => {
  it("throws for invalid arguments", async () => {
    const blob = new StreamableBlob(new Uint8Array([1]));
    const receiver = new MockDotNetImageReceiver();

    await expect(CropperBlazor.Helpers.readBlobInChunks(null, receiver as any)).rejects.toThrow("blob must be a valid Blob object.");
    await expect(CropperBlazor.Helpers.readBlobInChunks(blob, null)).rejects.toThrow("dotNetImageReceiverRef must be a valid .NET object reference with an invokeMethodAsync function.");
    await expect(CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any, 0)).rejects.toThrow("maximumReceiveChunkSize must be greater than 0 bytes when specified.");
  });

  it("streams blob with default chunking", async () => {
    const blob = new StreamableBlob(new Uint8Array([1, 2, 3]));
    const receiver = new MockDotNetImageReceiver();

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any);

    const [methodName, chunk] = receiver.invokeMethodAsync.mock.calls[0];

    expect(methodName).toBe("ReceiveImageChunk");
    expect(chunk?.constructor.name).toBe("Uint8Array");
    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("streams blob with custom maximum chunk size", async () => {
    const blob = new StreamableBlob(new Uint8Array([1, 2, 3, 4, 5, 6]));
    const receiver = new MockDotNetImageReceiver();

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any, 8);

    const chunkCalls = receiver.invokeMethodAsync.mock.calls.filter(([method]) => method === "ReceiveImageChunk");
    expect(chunkCalls.length).toBeGreaterThan(1);
    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("streams blob with a custom chunk size above the reduction threshold", async () => {
    const blob = new StreamableBlob(new Uint8Array(1200).fill(1));
    const receiver = new MockDotNetImageReceiver();

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any, 4000);

    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("streams blob without entering custom chunk reduction", async () => {
    const blob = new StreamableBlob(new Uint8Array([1, 2]));
    const receiver = new MockDotNetImageReceiver();

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any, 100);

    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("reports processing errors to the receiver", async () => {
    const blob = new StreamableBlob(new Uint8Array([1, 2, 3]));
    const receiver = new MockDotNetImageReceiver();
    receiver.invokeMethodAsync.mockImplementationOnce(async () => {
      throw new Error("receiver failed");
    }).mockImplementationOnce(async () => undefined);

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any);

    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("HandleImageProcessingError", expect.stringContaining("receiver failed"));
  });

  it("bubbles when error reporting fails", async () => {
    const blob = new StreamableBlob(new Uint8Array([1, 2, 3]));
    const receiver = new MockDotNetImageReceiver();
    receiver.invokeMethodAsync.mockImplementation(async () => {
      throw new Error("receiver failed");
    });

    await expect(CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any)).rejects.toThrow("receiver failed");
  });
});
