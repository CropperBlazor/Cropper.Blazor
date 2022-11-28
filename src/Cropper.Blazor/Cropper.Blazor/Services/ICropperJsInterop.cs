using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Cropper.Blazor.Services
{
    public interface ICropperJsInterop
    {
        Task LoadModuleAsync(CancellationToken cancellationToken = default);
        ValueTask InitCropperAsync(
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken = default);
        ValueTask ClearAsync(CancellationToken cancellationToken = default);
        ValueTask CropAsync(CancellationToken cancellationToken = default);
        ValueTask DestroyAsync(CancellationToken cancellationToken = default);
        ValueTask DisableAsync(CancellationToken cancellationToken = default);
        ValueTask EnableAsync(CancellationToken cancellationToken = default);
        ValueTask<CanvasData> GetCanvasDataAsync(CancellationToken cancellationToken = default);
        ValueTask<ContainerData> GetContainerDataAsync(CancellationToken cancellationToken = default);
        ValueTask<CropBoxData> GetCropBoxDataAsync(CancellationToken cancellationToken = default);
        ValueTask<CropperData> GetDataAsync(
            bool rounded,
            CancellationToken cancellationToken = default);
        ValueTask<ImageData> GetImageDataAsync(CancellationToken cancellationToken = default);
        ValueTask MoveAsync(
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default);
        ValueTask MoveToAsync(
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default);
        ValueTask ReplaceAsync(
            string url,
            bool onlyColorChanged,
            CancellationToken cancellationToken = default);
        ValueTask ResetAsync(CancellationToken cancellationToken = default);
        ValueTask RotateAsync(
            decimal degree,
            CancellationToken cancellationToken = default);
        ValueTask RotateToAsync(
            decimal degree,
            CancellationToken cancellationToken = default);
        ValueTask ScaleAsync(
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default);
        ValueTask ScaleXAsync(
            decimal scaleX,
            CancellationToken cancellationToken = default);
        ValueTask ScaleYAsync(
            decimal scaleY,
            CancellationToken cancellationToken = default);
        ValueTask SetCanvasDataAsync(
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default);
        ValueTask SetCropBoxDataAsync(
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default);
        ValueTask SetDataAsync(
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default);
        ValueTask SetDragModeAsync(
            DragMode dragMode,
            CancellationToken cancellationToken = default);
        ValueTask ZoomAsync(
            decimal ratio,
            CancellationToken cancellationToken = default);
        ValueTask ZoomToAsync(
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default);
        ValueTask NoConflictAsync(CancellationToken cancellationToken = default);
        ValueTask SetDefaultsAsync(
            [NotNull] Options options,
            CancellationToken cancellationToken = default);
        ValueTask SetAspectRatioAsync(
            decimal aspectRatio,
            CancellationToken cancellationToken = default);
        ValueTask<object> GetCroppedCanvasAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default);
        ValueTask<string> GetCroppedCanvasDataURLAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default);
        ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default);
        ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default);
    }
}