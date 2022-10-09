using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Cropper.Blazor.Services
{
    public interface ICropperJsInterop
    {
        Task LoadModuleAsync();
        ValueTask InitCropperAsync(
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase);
        ValueTask ClearAsync();
        ValueTask CropAsync();
        ValueTask DestroyAsync();
        ValueTask DisableAsync();
        ValueTask EnableAsync();
        ValueTask<CanvasData> GetCanvasDataAsync();
        ValueTask<ContainerData> GetContainerDataAsync();
        ValueTask<CropBoxData> GetCropBoxDataAsync();
        ValueTask<CropperData> GetDataAsync(bool rounded);
        ValueTask<ImageData> GetImageDataAsync();
        ValueTask MoveAsync(decimal offsetX, decimal? offsetY);
        ValueTask MoveToAsync(decimal x, decimal? y);
        ValueTask ReplaceAsync(string url, bool onlyColorChanged);
        ValueTask ResetAsync();
        ValueTask RotateAsync(decimal degree);
        ValueTask RotateToAsync(decimal degree);
        ValueTask ScaleAsync(decimal scaleX, decimal scaleY);
        ValueTask ScaleXAsync(decimal scaleX);
        ValueTask ScaleYAsync(decimal scaleY);
        ValueTask SetCanvasDataAsync(SetCanvasDataOptions setCanvasDataOptions);
        ValueTask SetCropBoxDataAsync(SetCropBoxDataOptions cropBoxDataOptions);
        ValueTask SetDataAsync(SetDataOptions setDataOptions);
        ValueTask SetDragModeAsync(DragMode dragMode);
        ValueTask ZoomAsync(decimal ratio);
        ValueTask ZoomToAsync(decimal ratio, decimal pivotX, decimal pivotY);
        ValueTask NoConflictAsync();
        ValueTask SetDefaultsAsync([NotNull] Options options);
        ValueTask SetAspectRatioAsync(decimal aspectRatio);
        ValueTask<object> GetCroppedCanvasAsync(GetCroppedCanvasOptions getCroppedCanvasOptions);
        ValueTask<string> GetCroppedCanvasDataURLAsync(GetCroppedCanvasOptions getCroppedCanvasOptions);
        ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default(CancellationToken));
        ValueTask RevokeObjectUrlAsync(string url);
    }
}
