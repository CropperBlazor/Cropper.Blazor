using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Cropper.Blazor.Services
{
    public class CropperJsInterop : ICropperJsInterop, IAsyncDisposable
    {
        private readonly IJSRuntime jsRuntime;
        private IJSObjectReference? module = null;
        public const string PathToCropperModule = "./_content/Cropper.Blazor/cropperJsInterop.js";

        public CropperJsInterop(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task LoadModuleAsync()
        {
            module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", PathToCropperModule);
        }

        public async ValueTask InitCropperAsync(
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.initCropper", image, options, cropperComponentBase);
        }

        public async ValueTask ClearAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeAsync<object>("cropper.clear");
        }

        public async ValueTask CropAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.crop");
        }

        public async ValueTask DestroyAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.destroy");
        }

        public async ValueTask DisableAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.disable");
        }

        public async ValueTask EnableAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.enable");
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<CanvasData>("cropper.getCanvasData");
        }

        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<ContainerData>("cropper.getContainerData");
        }

        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<CropBoxData>("cropper.getCropBoxData");
        }

        public async ValueTask<object> GetCroppedCanvasAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<object>("cropper.getCroppedCanvas", getCroppedCanvasOptions);
        }

        public async ValueTask<string> GetCroppedCanvasDataURLAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<string>("cropper.getCroppedCanvasDataURL", getCroppedCanvasOptions);
        }

        public async ValueTask<CropperData> GetDataAsync(bool rounded)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<CropperData>("cropper.getData", rounded);
        }

        public async ValueTask<ImageData> GetImageDataAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            return await jsRuntime!.InvokeAsync<ImageData>("cropper.getImageData");
        }

        public async ValueTask MoveAsync(decimal offsetX, decimal? offsetY)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.move", offsetX, offsetY);
        }

        public async ValueTask MoveToAsync(decimal x, decimal? y)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.moveTo", x, y);
        }

        public async ValueTask ReplaceAsync(string url, bool onlyColorChanged)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.replace", url, onlyColorChanged);
        }

        public async ValueTask ResetAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.reset");
        }

        public async ValueTask RotateAsync(decimal degree)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.rotate", degree);
        }

        public async ValueTask RotateToAsync(decimal degree)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.rotateTo", degree);
        }

        public async ValueTask ScaleAsync(decimal scaleX, decimal scaleY)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.scale", scaleX, scaleY);
        }

        public async ValueTask ScaleXAsync(decimal scaleX)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.scaleX", scaleX);
        }

        public async ValueTask ScaleYAsync(decimal scaleY)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.scaleY", scaleY);
        }

        public async ValueTask SetAspectRatioAsync(decimal aspectRatio)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setAspectRatio", aspectRatio);
        }

        public async ValueTask SetCanvasDataAsync(SetCanvasDataOptions setCanvasDataOptions)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setCanvasData", setCanvasDataOptions);
        }

        public async ValueTask SetCropBoxDataAsync(SetCropBoxDataOptions cropBoxDataOptions)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setCropBoxData", cropBoxDataOptions);
        }

        public async ValueTask SetDataAsync(SetDataOptions setDataOptions)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setData", setDataOptions);
        }

        public async ValueTask SetDragModeAsync(DragMode dragMode)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setDragMode", dragMode.ToEnumString());
        }

        public async ValueTask ZoomAsync(decimal ratio)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.zoom", ratio);
        }

        public async ValueTask ZoomToAsync(decimal ratio, decimal pivotX, decimal pivotY)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.zoomTo", ratio, pivotX, pivotY);
        }

        public async ValueTask NoConflictAsync()
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.noConflict");
        }

        public async ValueTask SetDefaultsAsync([NotNull] Options options)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setDefaults", options);
        }

        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }

            var jsImageStream = imageFile.OpenReadStream(maxAllowedSize, cancellationToken);
            var dotnetImageStream = new DotNetStreamReference(jsImageStream);
            return await jsRuntime.InvokeAsync<string>("cropper.getImageUsingStreaming", dotnetImageStream);
        }

        public async ValueTask RevokeObjectUrlAsync(string url)
        {
            if (module is null)
            {
                await LoadModuleAsync();
            }

            await jsRuntime.InvokeVoidAsync("cropper.revokeObjectUrl", url);
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