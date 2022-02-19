using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Cropper.Blazor.Services
{
    public class CropperJsInterop : ICropperJsInterop, IAsyncDisposable
    {
        private readonly IJSRuntime jsRuntime;
        private IJSObjectReference? module = null;

        public CropperJsInterop(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task LoadAsync()
        {
            module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Cropper.Blazor/cropperJsInterop.js");
        }

        public async ValueTask InitCropper([NotNull] ElementReference image, [NotNull] Options options, [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase)
        {
            if(module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.initCropper", image, options, cropperComponentBase);
        }

        public async ValueTask DisposeAsync()
        {
            if (module!=null)
            {
                await module.DisposeAsync();
            }
        }
    }
}