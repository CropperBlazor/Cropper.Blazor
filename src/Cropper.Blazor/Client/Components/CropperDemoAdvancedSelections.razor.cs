using Cropper.Blazor.Models;
using Cropper.Blazor.Components;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components;

public partial class CropperDemoAdvancedSelections
{
    [Parameter]
    public bool IsEnabled { get; set; }

    [Parameter]
    public IReadOnlyList<CropperSelectionData> SelectionItems { get; set; } = [];

    [Parameter]
    public CropperState CropperState { get; set; } = null!;

    [Parameter]
    public int ActiveSelectionVersion { get; set; }

    [Parameter]
    public int MinimumSelectionCount { get; set; }

    [Parameter]
    public int MaximumSelectionCount { get; set; }

    [Parameter]
    public int AdvancedFigureLineWidth { get; set; }

    [Parameter]
    public string AdvancedFigureColor { get; set; } = string.Empty;

    [Parameter]
    public string AdvancedFigureBackgroundColor { get; set; } = string.Empty;

    [Parameter]
    public decimal AdvancedFigureBackgroundAlpha { get; set; }

    [Parameter]
    public EventCallback AllowAdvancedSelectionFigures { get; set; }

    [Parameter]
    public EventCallback CreateAdvancedMultipleSelectionFigures { get; set; }

    [Parameter]
    public EventCallback AddFiveSelectionFigures { get; set; }

    [Parameter]
    public EventCallback RefreshSelectionItems { get; set; }

    [Parameter]
    public EventCallback<int> RemoveSelection { get; set; }

    [Parameter]
    public EventCallback<int> SetMinimumSelectionCount { get; set; }

    [Parameter]
    public EventCallback<int> SetMaximumSelectionCount { get; set; }

    [Parameter]
    public EventCallback<int> AdvancedFigureLineWidthChanged { get; set; }

    [Parameter]
    public EventCallback<string> AdvancedFigureColorChanged { get; set; }

    [Parameter]
    public EventCallback<string> AdvancedFigureBackgroundColorChanged { get; set; }

    [Parameter]
    public EventCallback<decimal> AdvancedFigureBackgroundAlphaChanged { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, decimal OffsetX, decimal OffsetY)> MoveSelection { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, decimal Width, decimal Height)> ResizeSelection { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, string? Shape)> SetSelectionFigure { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, decimal? Value)> SetSelectionMinimumWidth { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, decimal? Value)> SetSelectionMaximumWidth { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, decimal? Value)> SetSelectionMinimumAspectRatio { get; set; }

    [Parameter]
    public EventCallback<(int SelectionIndex, decimal? Value)> SetSelectionMaximumAspectRatio { get; set; }

    [Parameter]
    public Func<int, CropperDemoSelectionLimitSettings> GetSelectionLimits { get; set; } = _ => new CropperDemoSelectionLimitSettings();
}
