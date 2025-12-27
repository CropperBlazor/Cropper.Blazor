import type { CropperBlazor as DotNetTypes } from '../types/global/dotnet-global';

export namespace CropperBlazor.Helpers {
    export class CropperUrlImageHelper {
        static async getImageUsingStreaming(imageStream: DotNetTypes.Global.DotNetStreamReference): Promise<string> {
            const buf = await imageStream.arrayBuffer();
            const blob = new Blob([buf]);

            return URL.createObjectURL(blob);
        }

        static revokeObjectUrl(url: string) {
            URL.revokeObjectURL(url);
        }
    }
}
