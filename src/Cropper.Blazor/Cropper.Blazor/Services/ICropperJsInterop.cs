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
        Task LoadModuleAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask InitCropperAsync(
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ClearAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask CropAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask DestroyAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask DisableAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask EnableAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<CanvasData> GetCanvasDataAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<ContainerData> GetContainerDataAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<CropBoxData> GetCropBoxDataAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<CropperData> GetDataAsync(
            bool rounded,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<ImageData> GetImageDataAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask MoveAsync(
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask MoveToAsync(
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ReplaceAsync(
            string url,
            bool onlyColorChanged,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ResetAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask RotateAsync(
            decimal degree,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask RotateToAsync(
            decimal degree,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ScaleAsync(
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ScaleXAsync(
            decimal scaleX,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ScaleYAsync(
            decimal scaleY,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask SetCanvasDataAsync(
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask SetCropBoxDataAsync(
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask SetDataAsync(
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask SetDragModeAsync(
            DragMode dragMode,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ZoomAsync(
            decimal ratio,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask ZoomToAsync(
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask NoConflictAsync(CancellationToken cancellationToken = default(CancellationToken));
        ValueTask SetDefaultsAsync(
            [NotNull] Options options,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask SetAspectRatioAsync(
            decimal aspectRatio,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<object> GetCroppedCanvasAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<string> GetCroppedCanvasDataURLAsync(
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}