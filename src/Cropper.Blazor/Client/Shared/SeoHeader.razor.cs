using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Shared
{
    public partial class SeoHeader
    {
        [Parameter]
        public IEnumerable<string> Keywords { get; set; } = [];

        [Parameter]
        public string? Overview { get; set; }

        [Parameter]
        public string? Title { get; set; }

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
                "blazor image editor",
                "image editor",
                "blazor image",
                "blazor image cropper",
                "free image cropper",
                "online cropper",
                "photo cropper",
                "avatar cropper",
                "photo cropper",
                ..Keywords,
            ];

            return string.Join(", ", keywords);
        }

        private string GetSubTitle()
        {
            if (string.IsNullOrWhiteSpace(Overview))
            {
                return string.Empty;
            }

            return Overview.TrimEnd('.') + ".";
        }
    }
}
