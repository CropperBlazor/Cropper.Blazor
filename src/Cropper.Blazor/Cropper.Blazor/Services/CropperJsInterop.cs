using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Cropper.Blazor.Services
{
    public class CropperJsInterop : ICropperJsInterop, IAsyncDisposable
    {
        private readonly IJSRuntime jsRuntime;
        private IJSObjectReference? module = null;

        /// <summary>
        /// Path to cropper js interop module.
        /// </summary>
        public const string PathToCropperModule = "./_content/Cropper.Blazor/cropperJsInterop.min.js";

        public CropperJsInterop(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task LoadModuleAsync(CancellationToken cancellationToken = default)
        {
            module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", cancellationToken, PathToCropperModule);
        }

        public async ValueTask InitCropperAsync(
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync(
                "cropper.initCropper",
                cancellationToken,
                image,
                options,
                cropperComponentBase);
        }

        public async ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeAsync<object>("cropper.clear", cancellationToken);
        }

        public async ValueTask CropAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.crop", cancellationToken);
        }

        public async ValueTask DestroyAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.destroy", cancellationToken);
        }

        public async ValueTask DisableAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.disable", cancellationToken);
        }

        public async ValueTask EnableAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.enable", cancellationToken);
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<CanvasData>("cropper.getCanvasData", cancellationToken);
        }

        public async ValueTask<ContainerData> GetContainerDataAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<ContainerData>("cropper.getContainerData", cancellationToken);
        }

        public async ValueTask<CropBoxData> GetCropBoxDataAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<CropBoxData>("cropper.getCropBoxData", cancellationToken);
        }

        public async ValueTask<object> GetCroppedCanvasAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<object>("cropper.getCroppedCanvas", cancellationToken, getCroppedCanvasOptions);
        }

        public async ValueTask<string> GetCroppedCanvasDataURLAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<string>("cropper.getCroppedCanvasDataURL", cancellationToken, getCroppedCanvasOptions);
        }

        public async ValueTask<CropperData> GetDataAsync(bool rounded, CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<CropperData>("cropper.getData", cancellationToken, rounded);
        }

        public async ValueTask<ImageData> GetImageDataAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            return await jsRuntime!.InvokeAsync<ImageData>("cropper.getImageData", cancellationToken);
        }

        public async ValueTask MoveAsync(
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.move", cancellationToken, offsetX, offsetY);
        }

        public async ValueTask MoveToAsync(
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.moveTo", cancellationToken, x, y);
        }

        public async ValueTask ReplaceAsync(
            string url,
            bool onlyColorChanged,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.replace", cancellationToken, url, onlyColorChanged);
        }

        public async ValueTask ResetAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.reset", cancellationToken);
        }

        public async ValueTask RotateAsync(
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.rotate", cancellationToken, degree);
        }

        public async ValueTask RotateToAsync(
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.rotateTo", cancellationToken, degree);
        }

        public async ValueTask ScaleAsync(
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.scale", cancellationToken, scaleX, scaleY);
        }

        public async ValueTask ScaleXAsync(
            decimal scaleX,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.scaleX", cancellationToken, scaleX);
        }

        public async ValueTask ScaleYAsync(
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.scaleY", cancellationToken, scaleY);
        }

        public async ValueTask SetAspectRatioAsync(
            decimal aspectRatio,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.setAspectRatio", cancellationToken, aspectRatio);
        }

        public async ValueTask SetCanvasDataAsync(
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.setCanvasData", cancellationToken, setCanvasDataOptions);
        }

        public async ValueTask SetCropBoxDataAsync(
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.setCropBoxData", cancellationToken, cropBoxDataOptions);
        }

        public async ValueTask SetDataAsync(
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.setData", cancellationToken, setDataOptions);
        }

        public async ValueTask SetDragModeAsync(
            DragMode dragMode,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.setDragMode", cancellationToken, dragMode.ToEnumString());
        }

        public async ValueTask ZoomAsync(
            decimal ratio,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.zoom", cancellationToken, ratio);
        }

        public async ValueTask ZoomToAsync(
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.zoomTo", cancellationToken, ratio, pivotX, pivotY);
        }

        public async ValueTask NoConflictAsync(CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.noConflict", cancellationToken);
        }

        public async ValueTask SetDefaultsAsync(
            [NotNull] Options options,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime!.InvokeVoidAsync("cropper.setDefaults", cancellationToken, options);
        }

        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            var jsImageStream = imageFile.OpenReadStream(maxAllowedSize, cancellationToken);
            var dotnetImageStream = new DotNetStreamReference(jsImageStream);
            return await jsRuntime.InvokeAsync<string>("cropper.getImageUsingStreaming", cancellationToken, dotnetImageStream);
        }

        public async ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default)
        {
            if (module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }

            await jsRuntime.InvokeVoidAsync("cropper.revokeObjectUrl", cancellationToken, url);
        }

        /// <summary>
        /// Called to dispose this instance.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called to dispose js module.
        /// </summary>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }

            module = null;
        }
    }
}