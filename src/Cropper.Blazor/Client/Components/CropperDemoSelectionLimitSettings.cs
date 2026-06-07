namespace Cropper.Blazor.Client.Components;

public sealed class CropperDemoSelectionLimitSettings
{
    public decimal? MinimumWidth { get; set; }

    public decimal? MaximumWidth { get; set; }

    public decimal? MinimumAspectRatio { get; set; }

    public decimal? MaximumAspectRatio { get; set; }
}
