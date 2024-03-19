using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Shared
{
    public partial class SeoHeader
    {
        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public string? Overview { get; set; }

        [Parameter]
        public IEnumerable<string> Keywords { get; set; } = new List<string>();

        private string GetSubTitle()
        {
            if (string.IsNullOrEmpty(Overview))
                return "";
            return Overview.TrimEnd('.') + ".";
        }

        private string GetKeywords()
        {
            List<string> keywords =
            [
                "Cropper.Blazor",
                "Blazor.Cropper",
                "cropper",
                "blazor",
                "component",
                "crop-image",
                "cropperjs",
                "Cropper.js",
                "Blazor Components",
                "Blazor Library",
                "Blazor Cropper",
                "Cropper",
                "Image",
                "Crop",
                "Resize",
                "image-cropper",
                "crop-image",
                "csharp",
                "blazor-cropper",
                ".net",
                ".net core",
                "pwa",
                "webassembly",
                .. Keywords,
            ];

            return string.Join(", ", keywords);
        }
    }
}
