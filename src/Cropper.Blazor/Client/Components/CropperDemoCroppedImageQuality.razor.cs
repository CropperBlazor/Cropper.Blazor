using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components;

public partial class CropperDemoCroppedImageQuality
{
    [Parameter]
    public double CroppedImageQuality { get; set; }

    [Parameter]
    public ImageSmoothingQuality CroppedImageSmoothingQuality { get; set; }

    [Parameter]
    public decimal? CroppedImageMinimumWidth { get; set; }

    [Parameter]
    public decimal? CroppedImageMaximumWidth { get; set; }

    [Parameter]
    public decimal? CroppedImageMinimumHeight { get; set; }

    [Parameter]
    public decimal? CroppedImageMaximumHeight { get; set; }

    [Parameter]
    public EventCallback<double> CroppedImageQualityChanged { get; set; }

    [Parameter]
    public EventCallback<ImageSmoothingQuality> CroppedImageSmoothingQualityChanged { get; set; }

    [Parameter]
    public EventCallback<decimal?> CroppedImageMinimumWidthChanged { get; set; }

    [Parameter]
    public EventCallback<decimal?> CroppedImageMaximumWidthChanged { get; set; }

    [Parameter]
    public EventCallback<decimal?> CroppedImageMinimumHeightChanged { get; set; }

    [Parameter]
    public EventCallback<decimal?> CroppedImageMaximumHeightChanged { get; set; }

    private async Task SetCroppedImageQualityAsync(double value)
    {
        CroppedImageQuality = value;
        await CroppedImageQualityChanged.InvokeAsync(value);
    }

    private async Task SetCroppedImageMinimumWidthAsync(decimal? value)
    {
        CroppedImageMinimumWidth = value;
        await CroppedImageMinimumWidthChanged.InvokeAsync(value);
    }

    private async Task SetCroppedImageMaximumWidthAsync(decimal? value)
    {
        CroppedImageMaximumWidth = value;
        await CroppedImageMaximumWidthChanged.InvokeAsync(value);
    }

    private async Task SetCroppedImageMinimumHeightAsync(decimal? value)
    {
        CroppedImageMinimumHeight = value;
        await CroppedImageMinimumHeightChanged.InvokeAsync(value);
    }

    private async Task SetCroppedImageMaximumHeightAsync(decimal? value)
    {
        CroppedImageMaximumHeight = value;
        await CroppedImageMaximumHeightChanged.InvokeAsync(value);
    }

}
