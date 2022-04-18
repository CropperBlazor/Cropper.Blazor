using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
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

        public async ValueTask Clear()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeAsync<object>("cropper.clear");
        }

        public async ValueTask Crop()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.crop");
        }

        public async ValueTask Destroy()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.destroy");
        }

        public async ValueTask Disable()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.disable");
        }

        public async ValueTask Enable()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.enable");
        }

        public async ValueTask<CanvasData> GetCanvasData()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<CanvasData>("cropper.getCanvasData");
        }

        public async ValueTask<ContainerData> GetContainerData()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<ContainerData>("cropper.getContainerData");
        }

        public async ValueTask<CropBoxData> GetCropBoxData()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<CropBoxData>("cropper.getCropBoxData");
        }

        public async ValueTask<object> GetCroppedCanvas(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<object>("cropper.getCroppedCanvas", getCroppedCanvasOptions);
        }

        public async ValueTask<string> GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<string>("cropper.getCroppedCanvasDataURL", getCroppedCanvasOptions);
        }

        public async ValueTask<CropperData> GetData(bool rounded)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<CropperData>("cropper.getData", rounded);
        }

        public async ValueTask<ImageData> GetImageData()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            return await jsRuntime!.InvokeAsync<ImageData>("cropper.getImageData");
        }

        public async ValueTask Move(decimal offsetX, decimal? offsetY)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.move", offsetX, offsetY);
        }

        public async ValueTask MoveTo(decimal x, decimal? y)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.moveTo", x, y);
        }

        public async ValueTask Replace(string url, bool onlyColorChanged)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.replace", url, onlyColorChanged);
        }

        public async ValueTask Reset()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.reset");
        }

        public async ValueTask Rotate(decimal degree)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.rotate", degree);
        }

        public async ValueTask RotateTo(decimal degree)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.rotateTo", degree);
        }

        public async ValueTask Scale(decimal scaleX, decimal scaleY)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.scale", scaleX, scaleY);
        }

        public async ValueTask ScaleX(decimal scaleX)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.scaleX", scaleX);
        }

        public async ValueTask ScaleY(decimal scaleY)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.scaleY", scaleY);
        }

        public async ValueTask SetAspectRatio(decimal aspectRatio)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setAspectRatio", aspectRatio);
        }

        public async ValueTask SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setCanvasData", setCanvasDataOptions);
        }

        public async ValueTask SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setCropBoxData", cropBoxDataOptions);
        }

        public async ValueTask SetData(SetDataOptions setDataOptions)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setData", setDataOptions);
        }

        public async ValueTask SetDragMode(DragMode dragMode)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setDragMode", dragMode.ToEnumString());
        }

        public async ValueTask Zoom(decimal ratio)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.zoom", ratio);
        }

        public async ValueTask ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.zoomTo", ratio, pivotX, pivotY);
        }

        public async ValueTask NoConflict()
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.noConflict");
        }

        public async ValueTask SetDefaults([NotNull] Options options)
        {
            if (module == null)
            {
                await LoadAsync();
            }
            await jsRuntime!.InvokeVoidAsync("cropper.setDefaults", options);
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