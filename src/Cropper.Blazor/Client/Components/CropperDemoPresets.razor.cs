using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components;

public partial class CropperDemoPresets
{
    [Parameter]
    public Options Options { get; set; } = null!;

    [Parameter]
    public CropperFace CropperFace { get; set; }

    [Parameter]
    public CropperViewMode ViewMode { get; set; }

    [Parameter]
    public EventCallback GetPolygonFilterCanvas { get; set; }

    [Parameter]
    public EventCallback GetPolygonFilterCanvasInBackground { get; set; }

    [Parameter]
    public EventCallback<(string Property, bool? Value)> OptionChecked { get; set; }

    [Parameter]
    public EventCallback<decimal> AspectRatioChanged { get; set; }

    [Parameter]
    public EventCallback<CropperViewMode> ViewModeChanged { get; set; }

    [Parameter]
    public EventCallback<CropperFace> CropperFaceChanged { get; set; }
}
