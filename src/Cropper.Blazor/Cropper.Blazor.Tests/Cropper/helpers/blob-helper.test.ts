import { beforeEach, describe, it, expect, vi } from "vitest";
import { CropperBlazor } from "../../../Cropper.Blazor/Cropper/helpers/blob-helper";

class MockDotNetImageReceiver {
  readonly invokeMethodAsync = vi.fn(async () => undefined);
}

describe("readBlobInChunks", () => {
  beforeEach(() => {
    Object.defineProperty(Blob.prototype, "stream", {
      configurable: true,
      value: function stream(this: Blob) {
        const blob = this;

        return new ReadableStream<Uint8Array>({
          async start(controller) {
            const text = await new Response(blob).text();
            const bytes = new TextEncoder().encode(text);

            controller.enqueue(bytes);
            controller.close();
          },
        });
      },
    });
  });

  it("throws when blob is null", async () => {
    const receiver = new MockDotNetImageReceiver();

    await expect(CropperBlazor.Helpers.readBlobInChunks(null, receiver as any)).rejects.toThrow(
      "blob must be a valid Blob object.",
    );
  });

  it("throws when receiver is null", async () => {
    const blob = new Blob([new Uint8Array([1])]);

    await expect(CropperBlazor.Helpers.readBlobInChunks(blob, null)).rejects.toThrow(
      "dotNetImageReceiverRef must be a valid .NET object reference with an invokeMethodAsync function.",
    );
  });

  it("throws when maximum receive chunk size is not positive", async () => {
    const blob = new Blob([new Uint8Array([1])]);
    const receiver = new MockDotNetImageReceiver();

    await expect(CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any, 0)).rejects.toThrow(
      "maximumReceiveChunkSize must be greater than 0 bytes when specified.",
    );
  });

  it("streams blob with default chunking", async () => {
    const blob = new Blob([new Uint8Array([1, 2, 3])]);
    const receiver = new MockDotNetImageReceiver();

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any);

    expect(receiver.invokeMethodAsync.mock.calls[0][0]).toBe("ReceiveImageChunk");
    expect(receiver.invokeMethodAsync.mock.calls[0][1].constructor.name).toBe("Uint8Array");
    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("streams blob with custom maximum chunk size", async () => {
    const blob = new Blob([new Uint8Array([1, 2, 3, 4, 5, 6])]);
    const receiver = new MockDotNetImageReceiver();

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any, 8);

    const chunkCalls = receiver.invokeMethodAsync.mock.calls.filter(([method]) => method === "ReceiveImageChunk");
    expect(chunkCalls.length).toBeGreaterThan(1);
    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith("CompleteImageTransfer");
  });

  it("reports processing errors to the receiver", async () => {
    const blob = new Blob([new Uint8Array([1, 2, 3])]);
    const receiver = new MockDotNetImageReceiver();
    receiver.invokeMethodAsync.mockImplementationOnce(async () => {
      throw new Error("receiver failed");
    }).mockImplementationOnce(async () => undefined);

    await CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any);

    expect(receiver.invokeMethodAsync).toHaveBeenCalledWith(
      "HandleImageProcessingError",
      expect.stringContaining("receiver failed"),
    );
  });

  it("bubbles error when error reporting fails", async () => {
    const blob = new Blob([new Uint8Array([1, 2, 3])]);
    const receiver = new MockDotNetImageReceiver();
    receiver.invokeMethodAsync.mockImplementation(async () => {
      throw new Error("receiver failed");
    });

    await expect(CropperBlazor.Helpers.readBlobInChunks(blob, receiver as any)).rejects.toThrow(
      "receiver failed",
    );
  });
});
