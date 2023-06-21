using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Shared
{
    public partial class CroppedCanvasDialog
    {
        [Parameter]
        [System.ComponentModel.DataAnnotations.Required]
        public string Src { get; set; } = null!;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        public async Task DownloadImageSrcAsync()
        {
            await JSRuntime!.InvokeVoidAsync(
              "downloadFromUrl",
              new { Url = Src, FileName = $"{Guid.NewGuid()}.png" });
        }
    }
}
