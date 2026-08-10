using System.Reflection;
using System.Text.Json;
using Cropper.Blazor.Client.Components;
using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Exceptions;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.Client.Pages
{
    public partial class CropperDemo : IDisposable, IAsyncDisposable
    {
        public CropperComponent? CropperComponent = null!;
        private readonly string _errorLoadImageSrc = "not-found-image.jpg";
        private CropperDemoLiveSelection? LiveSelection = null!;
        private CropperFace CropperFace = CropperFace.Default;
        private CropperViewMode ViewMode = CropperViewMode.Vm0;
        private CropperState CropperState = new();
        private GetSetCropperData? GetSetCropperData = null!;
        private bool IsFreeAspectRatioEnabled;
        private Options Options = null!;
        private bool _isSelectionClearing;
        private int ActiveSelectionVersion { get; set; }
        private string CanvasThemeColor { get; set; } = "#3399ff";
        private string ShadeThemeColor { get; set; } = "#000000";
        private decimal ShadeThemeAlpha { get; set; } = 0.65m;
        private string GridThemeColor { get; set; } = "#eeeeee";
        private decimal GridThemeAlpha { get; set; } = 0.5m;
        private string CrosshairThemeColor { get; set; } = "#eeeeee";
        private decimal CrosshairThemeAlpha { get; set; } = 0.5m;
        private string HandleThemeColor { get; set; } = "#3399ff";
        private decimal HandleThemeAlpha { get; set; } = 0.5m;
        private string MoveHandleThemeColor { get; set; } = "#ffffff";
        private decimal MoveHandleThemeAlpha { get; set; } = 0.35m;
        private string ResizeHandleThemeColor { get; set; } = "#3399ff";
        private decimal ResizeHandleThemeAlpha { get; set; } = 0.5m;
        private decimal? ScaleXValue;
        private decimal? ScaleYValue;
        private int AdvancedFigureLineWidth { get; set; } = 4;
        private string AdvancedFigureColor { get; set; } = "#3399ff";
        private string AdvancedFigureBackgroundColor { get; set; } = "#3399ff";
        private decimal AdvancedFigureBackgroundAlpha { get; set; } = 0.18m;
        private readonly Dictionary<int, CropperDemoSelectionLimitSettings> SelectionLimits = [];
        private int MinimumSelectionCount { get; set; }
        private int MaximumSelectionCount { get; set; } = 5;
        private double CroppedImageQuality { get; set; } = 0.92;
        private ImageSmoothingQuality CroppedImageSmoothingQuality { get; set; } = ImageSmoothingQuality.High;
        private decimal? CroppedImageMinimumWidth { get; set; }
        private decimal? CroppedImageMaximumWidth { get; set; }
        private decimal? CroppedImageMinimumHeight { get; set; }
        private decimal? CroppedImageMaximumHeight { get; set; }
        private IReadOnlyList<CropperSelectionData> SelectionItems { get; set; } = [];
        private bool IsAdvancedSelectionFiguresEnabled => Options?.SelectionMultiple == true;
        private string Src = "https://fengyuanchen.github.io/cropperjs/v2/picture.jpg";
        private readonly IReadOnlyList<string> RelatedImageSources = new[]
        {
            "https://fengyuanchen.github.io/cropperjs/v2/picture.jpg",
            "https://fengyuanchen.github.io/cropperjs/images/picture.jpg"
        };
        private Breakpoint Start;
        public Dictionary<string, object> InputAttributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "loading", "lazy" },
                { "test-Attribute", "123-test" },
                { "crossorigin", "anonymous" },
                { "alt", "Cropper.Blazor demo image" }
            };

        private async Task ResetAllDemoState()
        {
            ResetMainCropperSettings();
            CropperFace = CropperFace.Default;
            ViewMode = CropperViewMode.Vm0;
            IsFreeAspectRatioEnabled = false;
            Options.AspectRatio = 1.7777777777777777m;
            await ApplyViewModeAsync();
            await GetSetCropperData!.ResetSettingsAsync();
            CropperComponent?.Reset();
        }

        private async Task CenterContain()
        {
            CropperComponent!.Center("contain");
        }

        private async Task CenterCover()
        {
            CropperComponent!.Center("cover");
        }

        private async Task ChangeSelectionAsync()
        {
            await ChangeSelectionAsync(160, 90);
        }

        private async Task ChangeSelectionAsync(decimal width, decimal height)
        {
            (width, height) = ClampSelectionDimensions(width, height);

            CropperComponent!.ChangeSelection(0, 0, width, height);
            UpdateCurrentAspectRatio(width, height);
        }

        private async Task CreateMultipleSelectionFiguresAsync()
        {
            Options.SelectionMultiple = true;
            Options.SelectionMaximumCount = MaximumSelectionCount;
            Options.HandleAction = CropperAction.Select;
            ReinitializeCropper();
            await Task.Delay(150);

            CropperComponent!.ChangeSelectionByIndex(0, 24, 24, 120, 90);
            CropperComponent.SetSelectionShapeByIndex(0, GetCropperFaceValue(CropperFace.Circle));

            await CreateSelectionOnCanvasAsync(168, 40, 110, 110);
            CropperComponent.ChangeSelectionByIndex(1, 168, 40, 110, 110);
            CropperComponent.SetSelectionShapeByIndex(1, GetCropperFaceValue(CropperFace.Pentagon));

            await CreateSelectionOnCanvasAsync(300, 48, 130, 80);
            CropperComponent.ChangeSelectionByIndex(2, 300, 48, 130, 80);
            CropperComponent.SetSelectionShapeByIndex(2, GetCropperFaceValue(CropperFace.Arrow));
            await RefreshSelectionItemsAsync();
        }

        private async Task CreateAdvancedMultipleSelectionFiguresAsync()
        {
            await AllowAdvancedSelectionFiguresAsync();

            await ChangeSelectionByIndexAsync(0, 24, 24, 120, 90, CropperFace.Circle);
            await ChangeSelectionByIndexAsync(1, 168, 40, 110, 110, CropperFace.Pentagon);
            await ChangeSelectionByIndexAsync(2, 300, 48, 130, 80, CropperFace.Arrow);
            await RefreshSelectionItemsAsync();
        }

        private async Task AllowAdvancedSelectionFiguresAsync()
        {
            Options.SelectionMultiple = true;
            Options.SelectionMaximumCount = MaximumSelectionCount;
            Options.HandleAction = CropperAction.Select;
            Options.SelectionOptions = new SelectionElementOptions
            {
                Multiple = true,
                Movable = true,
                Resizable = true,
                Keyboard = true,
                Outlined = true,
                Precise = true
            };

            ReinitializeCropper();
            await Task.Delay(150);
            await RefreshSelectionItemsAsync();
        }

        private async Task SetSelectionFigureAsync(int selectionIndex, CropperFace cropperFace)
        {
            await ChangeSelectionByIndexAsync(
                selectionIndex,
                24 + (selectionIndex * 136),
                24 + (selectionIndex * 12),
                selectionIndex == 1 ? 110 : 120,
                selectionIndex == 2 ? 80 : 100,
                cropperFace);
        }

        private async Task AddFiveSelectionFiguresAsync()
        {
            await CreateAdvancedMultipleSelectionFiguresAsync();
            await ChangeSelectionByIndexAsync(3, 436, 60, 96, 96, CropperFace.Circle);
            await ChangeSelectionByIndexAsync(4, 560, 72, 96, 72, CropperFace.Pentagon);
        }

        private async Task RemoveSelectionByIndexAsync(int selectionIndex)
        {
            int selectionCount = await CropperComponent!.GetSelectionCountAsync();

            if (selectionCount <= MinimumSelectionCount)
            {
                return;
            }

            CropperComponent!.RemoveSelectionByIndex(selectionIndex);
            await Task.Delay(50);
            await RefreshSelectionItemsAsync();
            StateHasChanged();
        }

        private async Task SetMinimumSelectionCountAsync(int value)
        {
            MinimumSelectionCount = Math.Clamp(value, 0, MaximumSelectionCount);
            await TrimSelectionsToMaximumAsync();
        }

        private async Task SetMaximumSelectionCountAsync(int value)
        {
            MaximumSelectionCount = Math.Max(value, MinimumSelectionCount);
            Options.SelectionMaximumCount = MaximumSelectionCount;
            await TrimSelectionsToMaximumAsync();
        }

        private async Task TrimSelectionsToMaximumAsync()
        {
            if (CropperComponent is null)
            {
                return;
            }

            int selectionCount = await CropperComponent.GetSelectionCountAsync();

            for (int selectionIndex = selectionCount - 1; selectionIndex >= MaximumSelectionCount; selectionIndex--)
            {
                CropperComponent.RemoveSelectionByIndex(selectionIndex);
            }

            await Task.Delay(50);
            await RefreshSelectionItemsAsync();
        }

        private async Task ChangeSelectionByIndexAsync(int selectionIndex, decimal x, decimal y, decimal width, decimal height, CropperFace cropperFace)
        {
            if (selectionIndex >= MaximumSelectionCount)
            {
                return;
            }

            if (Options.SelectionMultiple != true)
            {
                Options.SelectionMultiple = true;
                ReinitializeCropper();
                await Task.Delay(150);
            }

            while (await CropperComponent!.GetSelectionCountAsync() <= selectionIndex)
            {
                if (await CropperComponent.GetSelectionCountAsync() >= MaximumSelectionCount)
                {
                    return;
                }

                await CreateSelectionOnCanvasAsync(x, y, width, height);
            }

            (decimal limitedWidth, decimal limitedHeight) = ClampSelectionDimensions(selectionIndex, width, height);

            CropperComponent.ChangeSelectionByIndex(selectionIndex, x, y, limitedWidth, limitedHeight, GetSelectionAspectRatio(selectionIndex, cropperFace));
            CropperComponent.SetSelectionShapeByIndex(selectionIndex, GetCropperFaceValue(cropperFace));
            await RefreshSelectionItemsAsync();
        }

        private async Task RefreshSelectionItemsAsync()
        {
            if (CropperComponent is null)
            {
                if (SelectionItems.Count != 0)
                {
                    ActiveSelectionVersion++;
                }

                SelectionItems = [];
                return;
            }

            IReadOnlyList<CropperSelectionData> nextSelectionItems = await CropperComponent.GetSelectionsDataAsync();

            if (SelectionItems.Count != nextSelectionItems.Count)
            {
                ActiveSelectionVersion++;
            }

            SelectionItems = nextSelectionItems;
            await InvokeAsync(StateHasChanged);
        }

        private async Task MoveSelectionCardAsync(int selectionIndex, decimal offsetX, decimal offsetY)
        {
            CropperSelectionData? selection = SelectionItems.FirstOrDefault(item => item.Index == selectionIndex);

            if (selection is null)
            {
                return;
            }

            (decimal width, decimal height) = ClampSelectionDimensions(selectionIndex, selection.Width, selection.Height, selection.AspectRatio);

            CropperComponent!.ChangeSelectionByIndex(
                selectionIndex,
                Math.Max(0, selection.X + offsetX),
                Math.Max(0, selection.Y + offsetY),
                width,
                height,
                GetSelectionAspectRatio(selectionIndex, selection.AspectRatio));

            ActiveSelectionVersion++;
            await RefreshSelectionItemsAsync();
        }

        private async Task MoveSelectionCardAsync((int SelectionIndex, decimal OffsetX, decimal OffsetY) request)
        {
            await MoveSelectionCardAsync(request.SelectionIndex, request.OffsetX, request.OffsetY);
        }

        private async Task ResizeSelectionCardAsync(int selectionIndex, decimal width, decimal height)
        {
            CropperSelectionData? selection = SelectionItems.FirstOrDefault(item => item.Index == selectionIndex);

            if (selection is null)
            {
                return;
            }

            (width, height) = ClampSelectionDimensions(selectionIndex, width, height, selection.AspectRatio);

            CropperComponent!.ChangeSelectionByIndex(
                selectionIndex,
                selection.X,
                selection.Y,
                width,
                height,
                GetSelectionAspectRatio(selectionIndex, selection.AspectRatio));

            ActiveSelectionVersion++;
            await RefreshSelectionItemsAsync();
        }

        private async Task ResizeSelectionCardAsync((int SelectionIndex, decimal Width, decimal Height) request)
        {
            await ResizeSelectionCardAsync(request.SelectionIndex, request.Width, request.Height);
        }

        private async Task SetSelectionCardFigureAsync(int selectionIndex, string? shape)
        {
            CropperComponent!.SetSelectionShapeByIndex(selectionIndex, shape ?? GetCropperFaceValue(CropperFace.Default));
            ActiveSelectionVersion++;
            await RefreshSelectionItemsAsync();
        }

        private async Task SetSelectionCardFigureAsync((int SelectionIndex, string? Shape) request)
        {
            await SetSelectionCardFigureAsync(request.SelectionIndex, request.Shape);
        }

        private CropperDemoSelectionLimitSettings GetSelectionLimits(int selectionIndex)
        {
            if (!SelectionLimits.TryGetValue(selectionIndex, out CropperDemoSelectionLimitSettings? limits))
            {
                limits = new CropperDemoSelectionLimitSettings();
                SelectionLimits[selectionIndex] = limits;
            }

            return limits;
        }

        private async Task SetSelectionMinimumWidthAsync(int selectionIndex, decimal? value)
        {
            GetSelectionLimits(selectionIndex).MinimumWidth = value;
            await ApplySelectionLimitsAsync(selectionIndex);
        }

        private async Task SetSelectionMinimumWidthAsync((int SelectionIndex, decimal? Value) request)
        {
            await SetSelectionMinimumWidthAsync(request.SelectionIndex, request.Value);
        }

        private async Task SetSelectionMaximumWidthAsync(int selectionIndex, decimal? value)
        {
            GetSelectionLimits(selectionIndex).MaximumWidth = value;
            await ApplySelectionLimitsAsync(selectionIndex);
        }

        private async Task SetSelectionMaximumWidthAsync((int SelectionIndex, decimal? Value) request)
        {
            await SetSelectionMaximumWidthAsync(request.SelectionIndex, request.Value);
        }

        private async Task SetSelectionMinimumAspectRatioAsync(int selectionIndex, decimal? value)
        {
            GetSelectionLimits(selectionIndex).MinimumAspectRatio = value;
            await ApplySelectionLimitsAsync(selectionIndex);
        }

        private async Task SetSelectionMinimumAspectRatioAsync((int SelectionIndex, decimal? Value) request)
        {
            await SetSelectionMinimumAspectRatioAsync(request.SelectionIndex, request.Value);
        }

        private async Task SetSelectionMaximumAspectRatioAsync(int selectionIndex, decimal? value)
        {
            GetSelectionLimits(selectionIndex).MaximumAspectRatio = value;
            await ApplySelectionLimitsAsync(selectionIndex);
        }

        private async Task SetSelectionMaximumAspectRatioAsync((int SelectionIndex, decimal? Value) request)
        {
            await SetSelectionMaximumAspectRatioAsync(request.SelectionIndex, request.Value);
        }

        private async Task ApplySelectionLimitsAsync(int selectionIndex)
        {
            CropperSelectionData? selection = SelectionItems.FirstOrDefault(item => item.Index == selectionIndex);

            if (selection is null)
            {
                return;
            }

            (decimal width, decimal height) = ClampSelectionDimensions(selectionIndex, selection.Width, selection.Height, selection.AspectRatio);
            decimal? aspectRatio = GetSelectionConstrainedAspectRatio(selectionIndex, selection.AspectRatio);

            CropperComponent!.ChangeSelectionByIndex(selectionIndex, selection.X, selection.Y, width, height, aspectRatio);
            ActiveSelectionVersion++;
            await RefreshSelectionItemsAsync();
        }

        private async Task CreateSelectionOnCanvasAsync(decimal x, decimal y, decimal width, decimal height)
        {
            CropperComponent!.CreateSelection(x, y, width, height);
            ActiveSelectionVersion++;
            await Task.Delay(50);
        }

        private void UpdateCurrentAspectRatio(decimal width, decimal height)
        {
            if (!IsFreeAspectRatioEnabled || GetSetCropperData?.AspectRatioSettings is null)
            {
                return;
            }

            decimal aspectRatio = height == 0 ? 0 : width / height;
            Options.AspectRatio = aspectRatio;
            GetSetCropperData.AspectRatioSettings.SetUpAspectRatio(aspectRatio);
        }

        private (decimal Width, decimal Height) ClampSelectionDimensions(decimal width, decimal height)
        {
            if (GetSetCropperData?.CroppedDimensionsSettings is null)
            {
                return (width, height);
            }

            decimal minWidth = GetSetCropperData.CroppedDimensionsSettings.MinimumWidth ?? 0M;
            decimal maxWidth = GetSetCropperData.CroppedDimensionsSettings.MaximumWidth ?? decimal.MaxValue;
            decimal minHeight = GetSetCropperData.CroppedDimensionsSettings.MinimumHeight ?? 0M;
            decimal maxHeight = GetSetCropperData.CroppedDimensionsSettings.MaximumHeight ?? decimal.MaxValue;

            return (
                Math.Max(minWidth, Math.Min(maxWidth, width)),
                Math.Max(minHeight, Math.Min(maxHeight, height)));
        }

        private (decimal Width, decimal Height) ClampSelectionDimensions(int selectionIndex, decimal width, decimal height, decimal? aspectRatio = null)
        {
            CropperDemoSelectionLimitSettings limits = GetSelectionLimits(selectionIndex);
            decimal minWidth = limits.MinimumWidth ?? 0M;
            decimal maxWidth = limits.MaximumWidth ?? decimal.MaxValue;
            decimal nextWidth = Math.Max(minWidth, Math.Min(maxWidth, width));
            decimal nextHeight = height;
            decimal? nextAspectRatio = GetSelectionAspectRatio(selectionIndex, aspectRatio ?? (height == 0 ? null : width / height));

            if (nextAspectRatio is > 0)
            {
                nextHeight = nextWidth / nextAspectRatio.Value;
            }

            return (nextWidth, nextHeight);
        }

        private decimal? GetSelectionAspectRatio(int selectionIndex, CropperFace cropperFace)
        {
            decimal? aspectRatio = cropperFace switch
            {
                CropperFace.Circle => 1M,
                _ => null
            };

            return GetSelectionAspectRatio(selectionIndex, aspectRatio);
        }

        private decimal? GetSelectionAspectRatio(int selectionIndex, decimal? aspectRatio)
        {
            CropperDemoSelectionLimitSettings limits = GetSelectionLimits(selectionIndex);

            if (limits.MinimumAspectRatio is null && limits.MaximumAspectRatio is null)
            {
                return aspectRatio;
            }

            decimal nextAspectRatio = aspectRatio is > 0 ? aspectRatio.Value : limits.MinimumAspectRatio ?? limits.MaximumAspectRatio ?? 0M;

            if (limits.MinimumAspectRatio is not null)
            {
                nextAspectRatio = Math.Max(limits.MinimumAspectRatio.Value, nextAspectRatio);
            }

            if (limits.MaximumAspectRatio is not null)
            {
                nextAspectRatio = Math.Min(limits.MaximumAspectRatio.Value, nextAspectRatio);
            }

            return nextAspectRatio > 0 ? nextAspectRatio : aspectRatio;
        }

        private decimal? GetSelectionConstrainedAspectRatio(int selectionIndex, decimal? aspectRatio)
        {
            CropperDemoSelectionLimitSettings limits = GetSelectionLimits(selectionIndex);

            return limits.MinimumAspectRatio is null && limits.MaximumAspectRatio is null
                ? null
                : GetSelectionAspectRatio(selectionIndex, aspectRatio);
        }

        private async Task CenterSelectionAsync()
        {
            CropperComponent!.CenterSelection();
        }

        private async Task ClearSelectionAsync()
        {
            _isSelectionClearing = true;
            LiveSelection?.ClearCropData();
            CropperComponent!.ClearSelection();
        }

        private async Task MoveSelectionToOriginAsync()
        {
            CropperComponent!.MoveSelectionTo(0, 0);
        }

        private async Task GetSelectionCanvasDataUrlAsync()
        {
            GetCroppedCanvasOptions options = CreateCroppedCanvasOptions(16096, 16096);

            string croppedCanvasDataURL = await CropperComponent!.GetSelectionCanvasDataURLAsync(
                options,
                GetCroppedImageMimeType(),
                GetCroppedImageQuality());

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        private async Task RefreshCurrentCropperAsync()
        {
            await CropperState.NotifyCropperInitializedAsync(CropperComponent!);
        }

        private async Task ResetSelectionAsync()
        {
            CropperComponent!.ResetSelection();
        }

        [Inject] private IBrowserViewportService BrowserViewportService { get; set; } = null!;

        private bool IsAvailableInitCropper { get; set; } = true;
        //private decimal AspectRatio = 1.7777777777777777m;
        private bool IsErrorLoadImage { get; set; } = false;

        [Inject] private IJSRuntime? JSRuntime { get; set; }
        [Inject] private IUrlImageInterop UrlImageInterop { get; set; } = null!;
        private ElementReference CropperContainer { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await CropperComponent!.GetCanvasDataAsync();
        }

        public string GetClassNameCropper() =>
            "img-container" + CropperFace switch
            {
                CropperFace.Default => string.Empty,
                CropperFace.Close => " cropper-face-close",
                CropperFace.Pentagon => " cropper-face-pentagon",
                CropperFace.Circle => " cropper-face-circle",
                CropperFace.Arrow => " cropper-face-arrow",
                _ => string.Empty,
            };

        private string GetAdvancedFigureStyle() =>
            $"--cropper-advanced-selection-color: {AdvancedFigureColor}; --cropper-advanced-selection-background: {ToRgba(AdvancedFigureBackgroundColor, AdvancedFigureBackgroundAlpha)}; --cropper-advanced-selection-line-width: {AdvancedFigureLineWidth}px;";

        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await CropperComponent!.GetContainerDataAsync();
        }

        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await CropperComponent!.GetCropBoxDataAsync();
        }

        public async void GetCroppedCanvasData(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            CroppedCanvas croppedCanvas = await CropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            string croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL", GetCroppedImageMimeType(), GetCroppedImageQuality());

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async void GetCroppedCanvasDataByPolygonFilter(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            CroppedCanvas croppedCanvas = await CropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            string croppedCanvasDataURL;

            if (CropperFace == CropperFace.Default)
            {
                croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL", GetTransparentCroppedImageMimeType(), 1);
            }
            else if (CropperFace == CropperFace.Circle)
            {
                croppedCanvasDataURL = await JSRuntime!.InvokeAsync<string>("window.getEllipseImage", croppedCanvas!.JSRuntimeObjectRef);
            }
            else
            {
                IEnumerable<int> croppedPathToCanvasCropper = GetCroppedPathToCanvasCropper();

                croppedCanvasDataURL = await JSRuntime!.InvokeAsync<string>("window.getPolygonImage", croppedCanvas!.JSRuntimeObjectRef, croppedPathToCanvasCropper);
            }

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        private void ProcessAndOpenCroppedCanvasDialog(ImageReceiver imageReceiver, string imageMimeType)
        {
            InvokeAsync(async () =>
            {
                try
                {
                    using MemoryStream croppedCanvasDataStream = await imageReceiver.GetImageChunkStreamAsync();
                    byte[] croppedCanvasData = croppedCanvasDataStream.ToArray();

                    string croppedCanvasDataURL = $"data:{imageMimeType};base64," + Convert.ToBase64String(croppedCanvasData);

                    OpenCroppedCanvasDialog(croppedCanvasDataURL);
                }
                catch (ImageProcessingException ex)
                {
                    JSRuntime.InvokeVoidAsync("console.log", ex.ToString());
                }
            });
        }

        public async void GetCroppedCanvasDataByPolygonFilterInBackground(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            if (CropperFace == CropperFace.Default)
            {
                ImageReceiver imageReceiver = await CropperComponent!.GetCroppedCanvasDataInBackgroundAsync(getCroppedCanvasOptions);

                ProcessAndOpenCroppedCanvasDialog(imageReceiver, GetTransparentCroppedImageMimeType());
            }
            else if (CropperFace == CropperFace.Circle)
            {
                CroppedCanvasReceiver croppedCanvasReceiver = await CropperComponent!.GetCroppedCanvasInBackgroundAsync(
                    getCroppedCanvasOptions,
                    async (croppedCanvas, ct) =>
                    {
                        ImageReceiver imageReceiver = new ImageReceiver();

                        await JSRuntime!.InvokeVoidAsync(
                            "window.getEllipseImageInBackground",
                            croppedCanvas!.JSRuntimeObjectRef,
                            DotNetObjectReference.Create(imageReceiver));

                        ProcessAndOpenCroppedCanvasDialog(imageReceiver, "image/png");
                    });
            }
            else
            {
                IEnumerable<int> croppedPathToCanvasCropper = GetCroppedPathToCanvasCropper();

                CroppedCanvasReceiver croppedCanvasReceiver = await CropperComponent!.GetCroppedCanvasInBackgroundAsync(
                getCroppedCanvasOptions,
                async (croppedCanvas, ct) =>
                {
                    ImageReceiver imageReceiver = new ImageReceiver();

                    await JSRuntime!.InvokeVoidAsync(
                        "window.getPolygonImageInBackground",
                        croppedCanvas!.JSRuntimeObjectRef,
                        croppedPathToCanvasCropper,
                        DotNetObjectReference.Create(imageReceiver));

                    ProcessAndOpenCroppedCanvasDialog(imageReceiver, "image/png");
                });
            }
        }

        private async Task ReplaceImageFromRelatedSectionAsync(string src)
        {
            if (string.Equals(Src, src, StringComparison.Ordinal))
            {
                return;
            }

            string oldSrc = Src;
            IsAvailableInitCropper = false;

            await CropperComponent!.ReplaceAsync(src, false);

            Src = src;
            IsErrorLoadImage = false;

            if (oldSrc.StartsWith("blob:", StringComparison.OrdinalIgnoreCase))
            {
                await UrlImageInterop.RevokeObjectUrlAsync(oldSrc);
            }
        }

        public async void GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            string croppedCanvasDataURL = await CropperComponent!.GetCroppedCanvasDataURLAsync(
                getCroppedCanvasOptions,
                GetCroppedImageMimeType(),
                GetCroppedImageQuality());

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async void GetCroppedCanvasInBackgroundAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            await CropperComponent!.GetCroppedCanvasInBackgroundAsync(getCroppedCanvasOptions, async (croppedCanvas, ct) =>
            {
                ImageReceiver imageReceiver = await CropperComponent!.GetCroppedCanvasDataInBackgroundAsync(
                    getCroppedCanvasOptions,
                    GetCroppedImageMimeType(),
                    GetCroppedImageQuality());

                ProcessAndOpenCroppedCanvasDialog(imageReceiver, GetCroppedImageMimeType());
            });
        }

        public async void GetImageChunkStreamAsync(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            ImageReceiver imageReceiver = await CropperComponent!.GetCroppedCanvasDataInBackgroundAsync(
                getCroppedCanvasOptions,
                GetCroppedImageMimeType(),
                GetCroppedImageQuality());

            ProcessAndOpenCroppedCanvasDialog(imageReceiver, GetCroppedImageMimeType());
        }

        private GetCroppedCanvasOptions CreateCroppedCanvasOptions(decimal? maxWidth = null, decimal? maxHeight = null)
        {
            decimal? width = GetExplicitCroppedImageDimension(null, CroppedImageMinimumWidth, CroppedImageMaximumWidth ?? maxWidth);
            decimal? height = GetExplicitCroppedImageDimension(null, CroppedImageMinimumHeight, CroppedImageMaximumHeight ?? maxHeight);

            GetCroppedCanvasOptions options = new()
            {
                MinWidth = CroppedImageMinimumWidth,
                MaxWidth = width,
                MinHeight = CroppedImageMinimumHeight,
                MaxHeight = height,
                ImageSmoothingEnabled = true,
                ImageSmoothingQuality = CroppedImageSmoothingQuality.ToEnumString()
            };

            return options;
        }

        

        private static decimal? GetExplicitCroppedImageDimension(decimal? dimension, decimal? minimum, decimal? maximum)
        {
            if (dimension is not null)
            {
                return Math.Max(minimum ?? dimension.Value, Math.Min(maximum ?? dimension.Value, dimension.Value));
            }

            return maximum ?? minimum;
        }

        private GetCroppedCanvasOptions CreatePresetCroppedCanvasOptions(decimal width, decimal height) =>
            new()
            {
                Width = width,
                Height = height,
                MinWidth = width,
                MaxWidth = width,
                MinHeight = height,
                MaxHeight = height,
                ImageSmoothingEnabled = true,
                ImageSmoothingQuality = CroppedImageSmoothingQuality.ToEnumString()
            };

        private decimal? NormalizeMinimumCroppedImageWidth(decimal? value)
        {
            CroppedImageMinimumWidth = value;

            if (value is not null && CroppedImageMaximumWidth is not null && value > CroppedImageMaximumWidth)
            {
                CroppedImageMaximumWidth = value;
            }

            return CroppedImageMinimumWidth;
        }

        private decimal? NormalizeMaximumCroppedImageWidth(decimal? value)
        {
            CroppedImageMaximumWidth = value;

            if (value is not null && CroppedImageMinimumWidth is not null && value < CroppedImageMinimumWidth)
            {
                CroppedImageMinimumWidth = value;
            }

            return CroppedImageMaximumWidth;
        }

        private decimal? NormalizeMinimumCroppedImageHeight(decimal? value)
        {
            CroppedImageMinimumHeight = value;

            if (value is not null && CroppedImageMaximumHeight is not null && value > CroppedImageMaximumHeight)
            {
                CroppedImageMaximumHeight = value;
            }

            return CroppedImageMinimumHeight;
        }

        private decimal? NormalizeMaximumCroppedImageHeight(decimal? value)
        {
            CroppedImageMaximumHeight = value;

            if (value is not null && CroppedImageMinimumHeight is not null && value < CroppedImageMinimumHeight)
            {
                CroppedImageMinimumHeight = value;
            }

            return CroppedImageMaximumHeight;
        }

        private double NormalizeCroppedImageQuality(double value)
        {
            CroppedImageQuality = Math.Clamp(value, 0.1, 1);
            return CroppedImageQuality;
        }

        private void SetCroppedImageSmoothingQuality(ImageSmoothingQuality value)
        {
            CroppedImageSmoothingQuality = value;
        }

        private float GetCroppedImageQuality() => Convert.ToSingle(Math.Clamp(CroppedImageQuality, 0.1, 1));

        private static string GetCroppedImageMimeType() => "image/jpeg";

        private static string GetTransparentCroppedImageMimeType() => "image/png";

        public IEnumerable<int> GetCroppedPathToCanvasCropper() =>
            CropperFace switch
            {
                // That enumerable is equivalent css like that (the same for another paths) - clip-path: polygon(20% 0%, 0% 20%, 30% 50%, 0% 80%, 20% 100%, 50% 70%, 80% 100%, 100% 80%, 70% 50%, 100% 20%, 80% 0%, 50% 30%);
                CropperFace.Close => [20, 0, 0, 20, 30, 50, 0, 80, 20, 100, 50, 70, 80, 100, 100, 80, 70, 50, 100, 20, 80, 0, 50, 30],
                CropperFace.Pentagon => [50, 0, 100, 38, 82, 100, 18, 100, 0, 38],
                CropperFace.Arrow => [40, 0, 40, 40, 100, 40, 100, 60, 40, 60, 40, 100, 0, 50],
                _ => throw new InvalidOperationException()
            };

        public async ValueTask<CropperData> GetDataAsync(bool rounded)
        {
            return await CropperComponent!.GetDataAsync(rounded);
        }

        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await CropperComponent!.GetImageDataAsync();
        }

        public async Task InputFileChangeAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;

            if (imageFile != null)
            {
                string oldSrc = Src;

                Src = await UrlImageInterop.GetImageUsingStreamingAsync(imageFile, imageFile.Size);

                IsAvailableInitCropper = true;
                IsErrorLoadImage = false;

                CropperComponent?.Destroy();
                UrlImageInterop.RevokeObjectUrlAsync(oldSrc);
            }
        }

        public void MoveTo(decimal x, decimal? y)
        {
            CropperComponent?.MoveTo(x, y);
        }

        public async void OnCropEndEvent(JSEventData<CropEndEvent> cropEndJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndEvent, {JsonSerializer.Serialize(cropEndJSEvent)}");
            await RefreshSelectionItemsAsync();

            //if (cropEndJSEvent?.Detail?.OriginalEvent is not null)
            //{
            //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
            //        "jsObject.getInstanceProperty",
            //        cropEndJSEvent.Detail.OriginalEvent, "clientX");

            //    await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndJSEvent OriginalEvent clientX: {clientX}");
            //}
        }

        public async void OnCropEvent(JSEventData<CropEvent> cropJSEvent)
        {
            if (cropJSEvent?.Detail is not null)
            {
                ScaleXValue = cropJSEvent.Detail.ScaleX;
                ScaleYValue = cropJSEvent.Detail.ScaleY;

                decimal width = Math.Round(cropJSEvent.Detail.Width ?? 0);
                decimal height = Math.Round(cropJSEvent.Detail.Height ?? 0);

                if (_isSelectionClearing || (width == 0 && height == 0))
                {
                    _isSelectionClearing = width != 0 || height != 0;
                    LiveSelection?.ClearCropData();
                }
                else
                {
                    LiveSelection?.OnCropEvent(cropJSEvent.Detail);

                    if (width < GetSetCropperData!.CroppedDimensionsSettings.MinimumWidth
                    || height < GetSetCropperData!.CroppedDimensionsSettings.MinimumHeight
                    || width > GetSetCropperData!.CroppedDimensionsSettings.MaximumWidth
                    || height > GetSetCropperData!.CroppedDimensionsSettings.MaximumHeight
                    )
                    {
                        decimal minWidth = GetSetCropperData!.CroppedDimensionsSettings.MinimumWidth ?? 0M;
                        decimal maxWidth = GetSetCropperData!.CroppedDimensionsSettings.MaximumWidth ?? decimal.MaxValue;
                        decimal minHeight = GetSetCropperData!.CroppedDimensionsSettings.MinimumHeight ?? 0M;
                        decimal maxHeight = GetSetCropperData!.CroppedDimensionsSettings.MaximumHeight ?? decimal.MaxValue;
                        decimal nWidth = Math.Max(minWidth, Math.Min(maxWidth, width));
                        decimal nHeight = Math.Max(minHeight, Math.Min(maxHeight, height));

                        if (!IsFreeAspectRatioEnabled)
                        {
                            if (nWidth == 0)
                            {
                                nWidth = nHeight * Options.AspectRatio ?? 0M;
                            }
                            else if (nHeight == 0)
                            {
                                nHeight = nWidth / Options.AspectRatio ?? 0M;
                            }
                        }

                        if (Math.Abs(nWidth - width) <= 0.5m && Math.Abs(nHeight - height) <= 0.5m)
                        {
                            return;
                        }

                        CropperComponent!.SetCropBoxData(new SetCropBoxDataOptions
                        {
                            Width = nWidth,
                            Height = nHeight
                        });
                    }
                }
            }
        }

        public async void OnCropMoveEvent(JSEventData<CropMoveEvent> cropMoveJSEvent)
        {
            //if (cropMoveJSEvent?.Detail?.OriginalEvent is not null)
            //{
            //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
            //        "jsObject.getInstanceProperty",
            //        cropMoveJSEvent.Detail.OriginalEvent, "clientX");

            //    await JSRuntime!.InvokeVoidAsync("console.log", $"CropMoveJSEvent OriginalEvent clientX: {clientX}");
            //}
            await Task.CompletedTask;
        }

        public async void OnCropReadyEvent(JSEventData<CropReadyEvent> jSEventData)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropReadyJSEvent, {JsonSerializer.Serialize(jSEventData)}");
            await ApplyViewModeAsync();

            await InvokeAsync(async () =>
            {
                ImageData imageData = await CropperComponent!.GetImageDataAsync();
                decimal initZoomRatio = imageData.Width / imageData.NaturalWidth;

                GetSetCropperData!.SetRatio(initZoomRatio);
                await CropperState.NotifyCropperInitializedAsync(CropperComponent);
                await RefreshSelectionItemsAsync();
            });
        }

        public async void OnCropStartEvent(JSEventData<CropStartEvent> cropStartJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartEvent, {JsonSerializer.Serialize(cropStartJSEvent)}");
            ActiveSelectionVersion++;
            await RefreshSelectionItemsAsync();

            //if (cropStartJSEvent?.Detail?.OriginalEvent is not null)
            //{
            //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
            //        "jsObject.getInstanceProperty",
            //        cropStartJSEvent.Detail.OriginalEvent, "clientX");

            //    await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartJSEvent OriginalEvent clientX: {clientX}");
            //}
        }

        public void OnErrorLoadImageEvent(ErrorEventArgs errorEventArgs)
        {
            IsErrorLoadImage = true;
            Destroy();
            StateHasChanged();
        }

        public async void OnLoadImageEvent()
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Image Is loaded");
        }

        public async void OnZoomEvent(JSEventData<ZoomEvent> zoomJSEvent)
        {
            if (zoomJSEvent.Detail is not null)
            {
                await InvokeAsync(() =>
                {
                    JSRuntime!.InvokeVoidAsync("console.log", $"ZoomEvent {JsonSerializer.Serialize(zoomJSEvent)}");

                    GetSetCropperData!.OnZoomEvent(zoomJSEvent.Detail);
                });

                //if (zoomJSEvent.Detail.OriginalEvent is not null)
                //{
                //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
                //        "jsObject.getInstanceProperty",
                //        zoomJSEvent.Detail.OriginalEvent, "clientX");

                //    await JSRuntime!.InvokeVoidAsync("console.log", $"ZoomJSEvent clientX: {clientX}");
                //}
            }
        }

        public void OptionsChecked(string property, bool? newValue)
        {
            Type type = Options.GetType();
            PropertyInfo? propertyInfo = type!.GetProperty(property);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(Options, newValue, null);
                CropperComponent?.Destroy();
                CropperComponent?.InitCropper();
            }
        }

        private void OptionsChecked((string Property, bool? Value) option)
        {
            OptionsChecked(option.Property, option.Value);
        }

        private void GetCroppedCanvasDataByPolygonFilter()
        {
            GetCroppedCanvasDataByPolygonFilter(new GetCroppedCanvasOptions
            {
                MaxHeight = 4096,
                MaxWidth = 4096,
                ImageSmoothingQuality = ImageSmoothingQuality.High.ToEnumString()
            });
        }

        private void GetCroppedCanvasDataByPolygonFilterInBackground()
        {
            GetCroppedCanvasDataByPolygonFilterInBackground(new GetCroppedCanvasOptions
            {
                MaxHeight = 16096,
                MaxWidth = 16096,
                ImageSmoothingQuality = ImageSmoothingQuality.High.ToEnumString()
            });
        }

        public async Task ReplaceImageAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;

            if (imageFile != null)
            {
                string oldSrc = Src;
                string newSrc = await UrlImageInterop.GetImageUsingStreamingAsync(imageFile, imageFile.Size);

                if (IsErrorLoadImage)
                {
                    IsAvailableInitCropper = true;
                    IsErrorLoadImage = false;
                }
                else
                {
                    IsAvailableInitCropper = false;
                }

                await Task.WhenAll(
                    CropperComponent!.ReplaceAsync(newSrc, false).AsTask(),
                    UrlImageInterop.RevokeObjectUrlAsync(oldSrc).AsTask())
                    .ContinueWith(x =>
                    {
                        Src = newSrc;
                    });
            }
        }

        public void Scale(decimal? scaleX, decimal? scaleY)
        {
            CropperComponent?.Scale(scaleX ?? 0, scaleY ?? 0);
        }

        public void SetAspectRatio(decimal aspectRatio)
        {
            Options.AspectRatio = aspectRatio;
            IsFreeAspectRatioEnabled = aspectRatio == 0m;
            CropperComponent?.SetAspectRatio(aspectRatio);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperComponent?.SetCanvasData(setCanvasDataOptions);
        }

        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperComponent?.SetCropBoxData(cropBoxDataOptions);
        }

        public void SetCropperFace(CropperFace cropperFace)
        {
            CropperFace = cropperFace;
            JSRuntime!.InvokeVoidAsync("cropper.setSelectionShape", CropperComponent!.CropperComponentId, GetCropperFaceValue(cropperFace));
        }

        private static string GetCropperFaceValue(CropperFace cropperFace) => cropperFace switch
        {
            CropperFace.Close => "close",
            CropperFace.Pentagon => "pentagon",
            CropperFace.Circle => "circle",
            CropperFace.Arrow => "arrow",
            _ => "default"
        };

        public void SetData(SetDataOptions setDataOptions)
        {
            CropperComponent?.SetData(setDataOptions);
        }

        public async Task SetViewMode(CropperViewMode viewMode)
        {
            ViewMode = viewMode;
            await ApplyViewModeAsync();
        }

        private async Task ApplyViewModeAsync()
        {
            await JSRuntime!.InvokeVoidAsync("window.cropperViewMode.attach", CropperContainer, (int)ViewMode);
        }

        private void SetAutoCropArea(decimal value)
        {
            Options.AutoCropArea = value;
            ReinitializeCropper();
        }

        private void SetCanvasHidden(bool value)
        {
            Options.CanvasHidden = value;
            ReinitializeCropper();
        }

        private void SetCanvasBackground(bool value)
        {
            Options.Background = value;
            ReinitializeCropper();
        }

        private void SetCanvasDisabled(bool value)
        {
            Options.Disabled = value;
            ReinitializeCropper();
        }

        private void SetCanvasThemeColor(string value)
        {
            CanvasThemeColor = NormalizeHexColor(value, CanvasThemeColor);
            Options.CanvasThemeColor = CanvasThemeColor;
            ReinitializeCropper();
        }

        private void SetModalVisible(bool value)
        {
            Options.Modal = value;
            ReinitializeCropper();
        }

        private void SetShadeHidden(bool value)
        {
            Options.ShadeHidden = value;
            ReinitializeCropper();
        }

        private void SetShadeThemeColor(string value)
        {
            ShadeThemeColor = NormalizeHexColor(value, ShadeThemeColor);
            Options.ShadeThemeColor = ToRgba(ShadeThemeColor, ShadeThemeAlpha);
            ReinitializeCropper();
        }

        private void SetShadeThemeAlpha(decimal value)
        {
            ShadeThemeAlpha = Math.Clamp(value, 0, 1);
            Options.ShadeThemeColor = ToRgba(ShadeThemeColor, ShadeThemeAlpha);
            ReinitializeCropper();
        }

        private void SetCrosshairHidden(bool value)
        {
            Options.CrosshairHidden = value;
            ReinitializeCropper();
        }

        private void SetCrosshairCentered(bool value)
        {
            Options.CrosshairCentered = value;
            ReinitializeCropper();
        }

        private void SetCrosshairThemeColor(string value)
        {
            CrosshairThemeColor = NormalizeHexColor(value, CrosshairThemeColor);
            Options.CrosshairThemeColor = ToRgba(CrosshairThemeColor, CrosshairThemeAlpha);
            ReinitializeCropper();
        }

        private void SetCrosshairThemeAlpha(decimal value)
        {
            CrosshairThemeAlpha = Math.Clamp(value, 0, 1);
            Options.CrosshairThemeColor = ToRgba(CrosshairThemeColor, CrosshairThemeAlpha);
            ReinitializeCropper();
        }

        private void SetCrosshairVisible(bool value)
        {
            Options.Center = value;
            ReinitializeCropper();
        }

        private void SetGridHidden(bool value)
        {
            Options.GridHidden = value;
            ReinitializeCropper();
        }

        private void SetGridRows(decimal value)
        {
            Options.GridRows = value;
            ReinitializeCropper();
        }

        private void SetGridColumns(decimal value)
        {
            Options.GridColumns = value;
            ReinitializeCropper();
        }

        private void SetGridBordered(bool value)
        {
            Options.GridBordered = value;
            ReinitializeCropper();
        }

        private void SetGridCovered(bool value)
        {
            Options.GridCovered = value;
            ReinitializeCropper();
        }

        private void SetGridThemeColor(string value)
        {
            GridThemeColor = NormalizeHexColor(value, GridThemeColor);
            Options.GridThemeColor = ToRgba(GridThemeColor, GridThemeAlpha);
            ReinitializeCropper();
        }

        private void SetGridThemeAlpha(decimal value)
        {
            GridThemeAlpha = Math.Clamp(value, 0, 1);
            Options.GridThemeColor = ToRgba(GridThemeColor, GridThemeAlpha);
            ReinitializeCropper();
        }

        private void SetGridVisible(bool value)
        {
            Options.Guides = value;
            ReinitializeCropper();
        }

        private void SetImageHidden(bool value)
        {
            Options.ImageHidden = value;
            ReinitializeCropper();
        }

        private void SetImageMovable(bool value)
        {
            Options.Movable = value;
            ReinitializeCropper();
        }

        private void SetImageInitialCenterSize(CropperImageInitialCenterSize value)
        {
            Options.ImageInitialCenterSize = value;
            ReinitializeCropper();
        }

        private void SetImageAlt(string value)
        {
            Options.ImageAlt = value;
            ReinitializeCropper();
        }

        private void SetImageRotatable(bool value)
        {
            Options.Rotatable = value;
            ReinitializeCropper();
        }

        private void SetImageScalable(bool value)
        {
            Options.Scalable = value;
            ReinitializeCropper();
        }

        private void SetImageSkewable(bool value)
        {
            Options.Skewable = value;
            ReinitializeCropper();
        }

        private void SetHandleHidden(bool value)
        {
            Options.HandleHidden = value;
            ReinitializeCropper();
        }

        private void SetHandleAction(CropperAction value)
        {
            Options.HandleAction = value;
            ReinitializeCropper();
        }

        private void SetHandlePlain(bool value)
        {
            Options.HandlePlain = value;
            ReinitializeCropper();
        }

        private void SetHandleThemeColor(string value)
        {
            HandleThemeColor = NormalizeHexColor(value, HandleThemeColor);
            Options.HandleThemeColor = ToRgba(HandleThemeColor, HandleThemeAlpha);
            ReinitializeCropper();
        }

        private void SetHandleThemeAlpha(decimal value)
        {
            HandleThemeAlpha = Math.Clamp(value, 0, 1);
            Options.HandleThemeColor = ToRgba(HandleThemeColor, HandleThemeAlpha);
            ReinitializeCropper();
        }

        private void SetMoveHandleHidden(bool value)
        {
            Options.MoveHandleHidden = value;
            ReinitializeCropper();
        }

        private void SetMoveHandleAction(CropperAction value)
        {
            Options.MoveHandleAction = value;
            ReinitializeCropper();
        }

        private void SetMoveHandleThemeColor(string value)
        {
            MoveHandleThemeColor = NormalizeHexColor(value, MoveHandleThemeColor);
            Options.MoveHandleThemeColor = ToRgba(MoveHandleThemeColor, MoveHandleThemeAlpha);
            ReinitializeCropper();
        }

        private void SetMoveHandleThemeAlpha(decimal value)
        {
            MoveHandleThemeAlpha = Math.Clamp(value, 0, 1);
            Options.MoveHandleThemeColor = ToRgba(MoveHandleThemeColor, MoveHandleThemeAlpha);
            ReinitializeCropper();
        }

        private void SetResizeHandleHidden(bool value)
        {
            Options.ResizeHandleHidden = value;
            ReinitializeCropper();
        }

        private void SetResizeHandleThemeColor(string value)
        {
            ResizeHandleThemeColor = NormalizeHexColor(value, ResizeHandleThemeColor);
            Options.ResizeHandleThemeColor = ToRgba(ResizeHandleThemeColor, ResizeHandleThemeAlpha);
            ReinitializeCropper();
        }

        private void SetResizeHandleThemeAlpha(decimal value)
        {
            ResizeHandleThemeAlpha = Math.Clamp(value, 0, 1);
            Options.ResizeHandleThemeColor = ToRgba(ResizeHandleThemeColor, ResizeHandleThemeAlpha);
            ReinitializeCropper();
        }

        private static string NormalizeHexColor(string? value, string fallback) =>
            string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

        private static string ToRgba(string hexColor, decimal alpha)
        {
            string color = hexColor.TrimStart('#');

            if (color.Length == 3)
            {
                color = string.Concat(color.Select(character => new string(character, 2)));
            }

            if (color.Length != 6
                || !int.TryParse(color[..2], System.Globalization.NumberStyles.HexNumber, null, out int red)
                || !int.TryParse(color[2..4], System.Globalization.NumberStyles.HexNumber, null, out int green)
                || !int.TryParse(color[4..6], System.Globalization.NumberStyles.HexNumber, null, out int blue))
            {
                return hexColor;
            }

            return $"rgba({red}, {green}, {blue}, {Math.Clamp(alpha, 0, 1):0.##})";
        }

        private void SetMoveHandleVisible(bool value)
        {
            Options.Highlight = value;
            ReinitializeCropper();
        }

        private void SetSelectionMovable(bool value)
        {
            Options.CropBoxMovable = value;
            ReinitializeCropper();
        }

        private void SetSelectionHidden(bool value)
        {
            Options.SelectionHidden = value;
            ReinitializeCropper();
        }

        private void SetSelectionDynamic(bool value)
        {
            Options.SelectionDynamic = value;
            ReinitializeCropper();
        }

        private void SetSelectionMultiple(bool value)
        {
            Options.SelectionMultiple = value;
            ReinitializeCropper();
        }

        private void SetSelectionKeyboard(bool value)
        {
            Options.Keyboard = value;
            ReinitializeCropper();
        }

        private void SetSelectionOutlined(bool value)
        {
            Options.Outlined = value;
            ReinitializeCropper();
        }

        private void SetSelectionPrecise(bool value)
        {
            Options.SelectionPrecise = value;
            ReinitializeCropper();
        }

        private void SetSelectionResizable(bool value)
        {
            Options.CropBoxResizable = value;
            ReinitializeCropper();
        }

        private void SetSelectionZoomable(bool value)
        {
            Options.Zoomable = value;
            ReinitializeCropper();
        }

        private void SetWheelZoomRatio(decimal value)
        {
            Options.WheelZoomRatio = value;
            ReinitializeCropper();
        }

        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperComponent?.ZoomTo(ratio, pivotX, pivotY);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Destroy();
                CropperComponent?.DisposeAsync();
                JSRuntime!.InvokeVoidAsync("console.log", "Cropper Demo component is destroyed");
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await BrowserViewportService.SubscribeAsync(this, fireImmediately: true);

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async ValueTask DisposeAsync() => await BrowserViewportService.UnsubscribeAsync(this);

        Guid IBrowserViewportObserver.Id { get; } = Guid.NewGuid();

        ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
        {
            ReportRate = 250,
            NotifyOnBreakpointOnly = true
        };

        Task IBrowserViewportObserver.NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
        {
            if (browserViewportEventArgs.IsImmediate)
            {
                Start = browserViewportEventArgs.Breakpoint;
            }

            return InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            Options = new Options()
            {
                //Preview = ".img-preview",
                AspectRatio = 1.7777777777777777m
            };
        }

        private void Clear()
        {
            CropperComponent?.Clear();
        }

        private void Crop()
        {
            CropperComponent?.Crop();
        }

        private void Destroy()
        {
            CropperComponent?.Destroy();
            UrlImageInterop.RevokeObjectUrlAsync(Src);
        }

        private void Disable()
        {
            CropperComponent?.Disable();
        }

        private void Enable()
        {
            CropperComponent?.Enable();
        }

        private void Move(decimal offsetX, decimal? offsetY)
        {
            CropperComponent?.Move(offsetX, offsetY);
        }

        private void OpenCroppedCanvasDialog(string croppedCanvasDataURL)
        {
            DialogParameters parameters = new()
            {
                { "Src", croppedCanvasDataURL }
            };

            DialogOptions options = new()
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                BackdropClick = false
            };

            _dialogService.ShowAsync<Shared.CroppedCanvasDialog>("CroppedCanvasDialog", parameters, options);
        }

        private void Reset()
        {
            _ = GetSetCropperData?.ResetSettingsAsync();
            SelectionLimits.Clear();
            SelectionItems = [];
            CropperComponent?.Reset();
        }

        private void ResetCropperSelection()
        {
            CropperComponent?.Reset();
        }

        private void ResetMainCropperSettings()
        {
            Options.CanvasHidden = false;
            Options.Background = true;
            Options.Modal = true;
            Options.Disabled = false;
            Options.WheelZoomRatio = 0.1m;
            CanvasThemeColor = "#3399ff";
            Options.CanvasThemeColor = CanvasThemeColor;
            Options.ImageHidden = false;
            Options.Rotatable = true;
            Options.Scalable = true;
            Options.Skewable = true;
            Options.Movable = true;
            Options.ImageInitialCenterSize = CropperImageInitialCenterSize.Contain;
            Options.ImageAlt = "Cropper.Blazor demo image";
            Options.ShadeHidden = true;
            ShadeThemeColor = "#000000";
            ShadeThemeAlpha = 0.65m;
            Options.ShadeThemeColor = ToRgba(ShadeThemeColor, ShadeThemeAlpha);
            Options.HandleHidden = false;
            Options.HandleAction = CropperAction.Select;
            Options.HandlePlain = true;
            HandleThemeColor = "#3399ff";
            HandleThemeAlpha = 0.5m;
            Options.HandleThemeColor = ToRgba(HandleThemeColor, HandleThemeAlpha);
            Options.SelectionHidden = false;
            Options.SelectionMaximumCount = MaximumSelectionCount;
            Options.AutoCropArea = 0.5m;
            Options.SelectionDynamic = false;
            Options.CropBoxMovable = true;
            Options.CropBoxResizable = true;
            Options.Zoomable = false;
            Options.SelectionMultiple = false;
            Options.Keyboard = false;
            Options.Outlined = false;
            Options.SelectionPrecise = false;
            Options.Guides = true;
            Options.GridHidden = false;
            Options.GridRows = 3;
            Options.GridColumns = 3;
            Options.GridBordered = true;
            Options.GridCovered = true;
            GridThemeColor = "#eeeeee";
            GridThemeAlpha = 0.5m;
            Options.GridThemeColor = ToRgba(GridThemeColor, GridThemeAlpha);
            Options.Center = true;
            Options.CrosshairHidden = false;
            Options.CrosshairCentered = true;
            CrosshairThemeColor = "#eeeeee";
            CrosshairThemeAlpha = 0.5m;
            Options.CrosshairThemeColor = ToRgba(CrosshairThemeColor, CrosshairThemeAlpha);
            Options.Highlight = true;
            Options.MoveHandleHidden = false;
            Options.MoveHandleAction = CropperAction.Select;
            MoveHandleThemeColor = "#ffffff";
            MoveHandleThemeAlpha = 0.35m;
            Options.MoveHandleThemeColor = ToRgba(MoveHandleThemeColor, MoveHandleThemeAlpha);
            Options.ResizeHandleHidden = false;
            ResizeHandleThemeColor = "#3399ff";
            ResizeHandleThemeAlpha = 0.5m;
            Options.ResizeHandleThemeColor = ToRgba(ResizeHandleThemeColor, ResizeHandleThemeAlpha);
            SelectionLimits.Clear();
            SelectionItems = [];
            MinimumSelectionCount = 0;
            MaximumSelectionCount = 5;
            ReinitializeCropper();
        }

        private void ReinitializeCropper()
        {
            Options.SelectionMaximumCount = MaximumSelectionCount;
            CropperComponent?.Destroy();
            CropperComponent?.InitCropper();
        }

        private void Rotate(decimal degree)
        {
            CropperComponent?.Rotate(degree);
        }

        private void ScaleX(decimal? scaleX)
        {
            CropperComponent?.ScaleX(scaleX ?? 0);
        }

        private void ScaleY(decimal? scaleY)
        {
            CropperComponent?.ScaleY(scaleY ?? 0);
        }

        private void SetCropDragMode()
        {
            CropperComponent?.SetDragMode(DragMode.Crop);
        }

        private void SetMoveDragMode()
        {
            CropperComponent?.SetDragMode(DragMode.Move);
        }
        private void Zoom(decimal ratio)
        {
            CropperComponent?.Zoom(ratio);
        }

    }
}
