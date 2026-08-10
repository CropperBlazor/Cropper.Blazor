import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { CropperBlazor } from "../../../Cropper.Blazor/Cropper/helpers/cropper-url-image-helper";

// Minimal mock of DotNetStreamReference
class MockDotNetStreamReference {
  constructor(private data: ArrayBuffer) {}

  async arrayBuffer(): Promise<ArrayBuffer> {
    return this.data;
  }
}

//1.	Debug via Node inspector:
//•	npm run test:debug:watch --prefix Cropper.Blazor.Tests
//•	Attach VS to 127.0.0.1:9229
//2.	Debug using VS Code JavaScript debugger.
//3.	Keep VS Test Explorer only for run/verify, not breakpoint debugging for these Vitest TS tests.

describe("CropperUrlImageHelper", () => {
  const mockObjectUrl = "blob:http://localhost/mock-url";

  beforeEach(() => {
    URL.createObjectURL = vi.fn();
    URL.revokeObjectURL = vi.fn();

    vi.spyOn(URL, "createObjectURL").mockReturnValue(mockObjectUrl);
    vi.spyOn(URL, "revokeObjectURL").mockImplementation(() => {});
  });

  afterEach(() => {
    vi.restoreAllMocks();
  });

  it("should create object URL from streamed image", async () => {
    const buffer = new Uint8Array([1, 2, 3]).buffer;
    const stream = new MockDotNetStreamReference(buffer) as any;

    const result = await new CropperBlazor.Helpers.CropperUrlImageHelper().getImageUsingStreaming(
      stream,
    );

    expect(result).toBe(mockObjectUrl);
    expect(URL.createObjectURL).toHaveBeenCalledOnce();
    expect(URL.createObjectURL).toHaveBeenCalledWith(expect.any(Blob));
  });

  it("should revoke object URL", () => {
    new CropperBlazor.Helpers.CropperUrlImageHelper().revokeObjectUrl(mockObjectUrl);

    expect(URL.revokeObjectURL).toHaveBeenCalledOnce();
    expect(URL.revokeObjectURL).toHaveBeenCalledWith(mockObjectUrl);
  });
});
