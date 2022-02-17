using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Cropper.Blazor.Services
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

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