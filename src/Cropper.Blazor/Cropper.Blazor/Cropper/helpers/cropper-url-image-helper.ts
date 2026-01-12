import type { CropperBlazor as DotNetTypes } from "../types/global/dotnet-global.custom";

export namespace CropperBlazor.Helpers {
  export class CropperUrlImageHelper {
    async getImageUsingStreaming(
      imageStream: DotNetTypes.Global.DotNetStreamReference,
    ): Promise<string> {
      const buf: ArrayBuffer = await imageStream.arrayBuffer();
      const blob: Blob = new Blob([buf]);

      return URL.createObjectURL(blob);
    }

    revokeObjectUrl(url: string) {
      URL.revokeObjectURL(url);
    }
  }
}
