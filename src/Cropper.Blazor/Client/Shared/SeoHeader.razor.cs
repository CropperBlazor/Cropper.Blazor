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

        string GetSubTitle()
        {
            if (string.IsNullOrEmpty(Overview))
                return "";
            return Overview.TrimEnd('.') + ".";
        }

        string GetKeywords()
        {
            var keywords = new List<string>();

            keywords.AddRange(Keywords);

            keywords.AddRange(new[]
            {
                "Cropper.Blazor",
                "blazor",
                "component",
                "crop-image",
                "cropper",
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
                "blazor-cropper"
            });

            return string.Join(", ", keywords);
        }
    }
}
