import { afterEach, beforeEach, describe, expect, it, vi } from "vitest";
import { CropperBlazor } from "./cropper-url-image-helper";

class MockDotNetStreamReference {
  constructor(private readonly data: ArrayBuffer) { }

  async arrayBuffer(): Promise<ArrayBuffer> {
    return this.data;
  }
}

describe("CropperUrlImageHelper coverage", () => {
  const mockObjectUrl = "blob:http://localhost/mock-url";

  beforeEach(() => {
    URL.createObjectURL = vi.fn();
    URL.revokeObjectURL = vi.fn();

    vi.spyOn(URL, "createObjectURL").mockReturnValue(mockObjectUrl);
    vi.spyOn(URL, "revokeObjectURL").mockImplementation(() => { });
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  it("creates object URL from streamed image", async () => {
    const buffer = new Uint8Array([1, 2, 3]).buffer;
    const stream = new MockDotNetStreamReference(buffer) as any;

    const result = await new CropperBlazor.Helpers.CropperUrlImageHelper().getImageUsingStreaming(stream);

    expect(result).toBe(mockObjectUrl);
    expect(URL.createObjectURL).toHaveBeenCalledOnce();
    expect(URL.createObjectURL).toHaveBeenCalledWith(expect.any(Blob));
  });

  it("revokes object URL", () => {
    new CropperBlazor.Helpers.CropperUrlImageHelper().revokeObjectUrl(mockObjectUrl);

    expect(URL.revokeObjectURL).toHaveBeenCalledOnce();
    expect(URL.revokeObjectURL).toHaveBeenCalledWith(mockObjectUrl);
  });
});
