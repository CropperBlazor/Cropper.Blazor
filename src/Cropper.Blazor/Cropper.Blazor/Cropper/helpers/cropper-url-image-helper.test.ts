import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { CropperBlazor } from './cropper-url-image-helper';

// Minimal mock of DotNetStreamReference
class MockDotNetStreamReference {
    constructor(private data: ArrayBuffer) { }

    async arrayBuffer(): Promise<ArrayBuffer> {
        return this.data;
    }
}

describe('CropperUrlImageHelper', () => {
    const mockObjectUrl = 'blob:http://localhost/mock-url';

    beforeEach(() => {
        vi.spyOn(URL, 'createObjectURL').mockReturnValue(mockObjectUrl);
        vi.spyOn(URL, 'revokeObjectURL').mockImplementation(() => { });
    });

    afterEach(() => {
        vi.restoreAllMocks();
    });

    it('should create object URL from streamed image', async () => {
        const buffer = new Uint8Array([1, 2, 3]).buffer;
        const stream = new MockDotNetStreamReference(buffer) as any;

        const result = await CropperBlazor.Helpers.CropperUrlImageHelper.getImageUsingStreaming(stream);

        expect(result).toBe(mockObjectUrl);
        expect(URL.createObjectURL).toHaveBeenCalledOnce();
        expect(URL.createObjectURL).toHaveBeenCalledWith(
            expect.any(Blob)
        );
    });

    it('should revoke object URL', () => {
        CropperBlazor.Helpers.CropperUrlImageHelper.revokeObjectUrl(mockObjectUrl);

        expect(URL.revokeObjectURL).toHaveBeenCalledOnce();
        expect(URL.revokeObjectURL).toHaveBeenCalledWith(mockObjectUrl);
    });
});
