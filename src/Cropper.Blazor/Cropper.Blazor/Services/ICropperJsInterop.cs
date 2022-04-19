﻿using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Cropper.Blazor.Services
{
    public interface ICropperJsInterop
    {
        Task LoadAsync();
        ValueTask InitCropper([NotNull] ElementReference image, [NotNull] Options options, [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase);
        ValueTask Clear();
        ValueTask Crop();
        ValueTask Destroy();
        ValueTask Disable();
        ValueTask Enable();
        ValueTask<CanvasData> GetCanvasData();
        ValueTask<ContainerData> GetContainerData();
        ValueTask<CropBoxData> GetCropBoxData();
        ValueTask<CropperData> GetData(bool rounded);
        ValueTask<ImageData> GetImageData();
        ValueTask Move(decimal offsetX, decimal? offsetY);
        ValueTask MoveTo(decimal x, decimal? y);
        ValueTask Replace(string url, bool onlyColorChanged);
        ValueTask Reset();
        ValueTask Rotate(decimal degree);
        ValueTask RotateTo(decimal degree);
        ValueTask Scale(decimal scaleX, decimal scaleY);
        ValueTask ScaleX(decimal scaleX);
        ValueTask ScaleY(decimal scaleY);
        ValueTask SetCanvasData(SetCanvasDataOptions setCanvasDataOptions);
        ValueTask SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions);
        ValueTask SetData(SetDataOptions setDataOptions);
        ValueTask SetDragMode(DragMode dragMode);
        ValueTask Zoom(decimal ratio);
        ValueTask ZoomTo(decimal ratio, decimal pivotX, decimal pivotY);
        ValueTask NoConflict();
        ValueTask SetDefaults([NotNull] Options options);
        ValueTask SetAspectRatio(decimal aspectRatio);
        ValueTask<object> GetCroppedCanvas(GetCroppedCanvasOptions getCroppedCanvasOptions);
        ValueTask<string> GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions);
        ValueTask<string> GetImageUsingStreaming(IBrowserFile imageFile, long maxAllowedSize = 512000L, CancellationToken cancellationToken = default(CancellationToken));
        ValueTask RevokeObjectUrl(string url);
    }
}
