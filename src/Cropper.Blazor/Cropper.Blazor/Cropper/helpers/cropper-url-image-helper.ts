export class CropperUrlImageHelper {
    static async getImageUsingStreaming(imageStream: DotNetStreamReference): Promise<string> {
        const buf = await imageStream.arrayBuffer();
        const blob = new Blob([buf]);

        return URL.createObjectURL(blob);
    }

    static revokeObjectUrl(url: string) {
        URL.revokeObjectURL(url);
    }
}