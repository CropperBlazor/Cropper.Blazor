using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Cropper.Blazor.IntegrationTests.Pages;
using Xunit;

namespace Cropper.Blazor.IntegrationTests;

public sealed class CropperDemoTests : IClassFixture<SeleniumTestFixture>
{
    private readonly SeleniumTestFixture _fixture;
    private readonly DemoPage _page;

    public CropperDemoTests(SeleniumTestFixture fixture)
    {
        _fixture = fixture;
        _page = new DemoPage(fixture);
    }

    [Fact]
    public void V2Demo_ExercisesCurrentCropperAndControls()
    {
        RunWithDiagnostics(nameof(V2Demo_ExercisesCurrentCropperAndControls), () =>
        {
            OpenDemo("/v2");

            WaitForPreviewImages();
            ClickButtonByText("Refresh selection");
            ScrollToCropper();
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("return document.querySelector('cropper-selection')?.width > 0;"));
            WaitForDataPreviewValue("X");
            WaitForDataPreviewValue("Y");
            WaitForDataPreviewValue("Width");
            WaitForDataPreviewValue("Height");
            WaitForDataPreviewValue("ScaleX");
            WaitForDataPreviewValue("ScaleY");
            ClickButtonByText("Center contain");
            ClickButtonByText("Center cover");
            ClickButtonByText("Selection 160");
            ScrollToCropper();
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("const selection = document.querySelector('cropper-selection'); return Math.round(selection?.width ?? 0) === 160 && Math.round(selection?.height ?? 0) === 90;"));
            ClickButtonByText("Center Selection");
            ClickButtonByText("Reset Selection");
            ClickButtonByText("Clear Selection");
            ScrollToCropper();
            WaitForDataPreviewValue("Width", "0.0000");
            ClickButtonByText("Selection to [0,0]");
            ClickAndWaitForCroppedCanvasDialog("Selection to Canvas");
            CloseDialog();
            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by URL");
        });
    }

    [Fact]
    public void V2Demo_MultipleSelection_DoesNotExceedMaximumWhenSelectingOnCanvas()
    {
        RunWithDiagnostics(nameof(V2Demo_MultipleSelection_DoesNotExceedMaximumWhenSelectingOnCanvas), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            SetSettingsInput("maximum-selection-count", "2");
            ClickButtonByText("Start 3 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>("return document.querySelectorAll('.img-container cropper-selection').length;") == 2);

            DispatchNativeSelectionDrag(40, 40, 80, 60);
            DispatchNativeSelectionDrag(140, 80, 80, 60);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>("return document.querySelectorAll('.img-container cropper-selection').length;") == 2);
        });
    }

    [Theory]
    [InlineData("0.2", "Low")]
    [InlineData("1", "High")]
    public void V2Demo_CroppedImageQualityControls_ProduceRequestedSizeForQualityValues(string quality, string smoothingQuality)
    {
        RunWithDiagnostics($"V2Demo_CroppedImageQualityControls_ProduceRequestedSizeForQualityValues_{quality}_{smoothingQuality}", () =>
        {
            OpenV2Demo();

            SetSettingsInput("cropped-image-quality", quality);
            SetImageSmoothingQuality(smoothingQuality);
            ClickButtonByText("320×180");

            _page.Dialog.WaitForCroppedCanvasDialog();
            (long width, long height) = _page.Dialog.GetCroppedCanvasDialogImageSize();
            string imageSource = _page.Dialog.GetCroppedCanvasDialogImageSource();

            Assert.InRange(width, 319, 320);
            Assert.InRange(height, 179, 180);
            Assert.StartsWith("data:image/jpeg;base64,", imageSource, StringComparison.Ordinal);
        });
    }

    [Fact]
    public void V2Demo_SelectionCardLimits_ApplyToIndividualSelection()
    {
        RunWithDiagnostics(nameof(V2Demo_SelectionCardLimits_ApplyToIndividualSelection), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");
            SetSelectionCardInput(0, "selection-card-min-width", "150");
            SetSelectionCardInput(1, "selection-card-max-width", "90");
            SetSelectionCardInput(2, "selection-card-max-aspect-ratio", "1");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "return Math.round(selections[0]?.width ?? 0) >= 150" +
                " && Math.round(selections[1]?.width ?? 0) <= 90" +
                " && ((selections[2]?.width ?? 0) / (selections[2]?.height ?? 1)) <= 1.05;"));
        });
    }

    [Theory]
    [InlineData(0, "selection-card-min-width", "170", 170, 1000)]
    [InlineData(1, "selection-card-max-width", "90", 0, 90)]
    public void V2Demo_SelectionCardWidthLimits_ApplyToTargetSelectionOnly(int cardIndex, string inputTestId, string value, int minimumWidth, int maximumWidth)
    {
        RunWithDiagnostics($"V2Demo_SelectionCardWidthLimits_ApplyToTargetSelectionOnly_{inputTestId}_{cardIndex}", () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");
            SetSelectionCardInput(cardIndex, inputTestId, value);

            _fixture.Wait.Until(_ =>
            {
                decimal targetWidth = GetSelectionWidth(cardIndex);

                return targetWidth >= minimumWidth
                    && targetWidth <= maximumWidth;
            });
        });
    }

    [Theory]
    [InlineData(0, "selection-card-min-aspect-ratio", "2.0", 1.95, 2.05)]
    [InlineData(2, "selection-card-max-aspect-ratio", "1.0", 0.95, 1.05)]
    public void V2Demo_SelectionCardAspectLimits_ApplyToTargetSelectionOnly(int cardIndex, string inputTestId, string value, decimal minimumRatio, decimal maximumRatio)
    {
        RunWithDiagnostics($"V2Demo_SelectionCardAspectLimits_ApplyToTargetSelectionOnly_{inputTestId}_{cardIndex}", () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");
            SetSelectionCardInput(cardIndex, inputTestId, value);

            _fixture.Wait.Until(_ =>
            {
                decimal targetRatio = GetSelectionRatio(cardIndex);

                return targetRatio >= minimumRatio
                    && targetRatio <= maximumRatio;
            });
        });
    }

    [Fact]
    public void V2Demo_AdvancedSelectionFiguresBlock_IsEnabledOnlyWhenMultipleFlagIsActive()
    {
        RunWithDiagnostics(nameof(V2Demo_AdvancedSelectionFiguresBlock_IsEnabledOnlyWhenMultipleFlagIsActive), () =>
        {
            OpenV2Demo();

            WaitForAdvancedSelectionFiguresEnabled(false);
            AssertAdvancedSelectionControlDisabled("add-five-selection-figures", true);
            AssertAdvancedSelectionControlDisabled("maximum-selection-count", true);

            EnableAdvancedSelectionFigures();

            WaitForAdvancedSelectionFiguresEnabled(true);
            AssertAdvancedSelectionControlDisabled("add-five-selection-figures", false);
            AssertAdvancedSelectionControlDisabled("maximum-selection-count", false);

            ClickButtonByText("Start 3 styled selections");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>("return document.querySelectorAll('[data-testid=\"selection-card\"]').length;") >= 3);

            ClickActionButtonByTitle("Reset");

            WaitForAdvancedSelectionFiguresEnabled(true);
            AssertAdvancedSelectionControlDisabled("add-five-selection-figures", false);
            AssertAdvancedSelectionControlDisabled("maximum-selection-count", false);

            ClickSettingsSwitch("v2-main-selection-multiple");

            WaitForAdvancedSelectionFiguresEnabled(false);
            AssertAdvancedSelectionControlDisabled("add-five-selection-figures", true);
            AssertAdvancedSelectionControlDisabled("maximum-selection-count", true);
        });
    }

    [Fact]
    public void V2Demo_SelectionCardList_ControlsIndividualSelectionsAndKeepsPreviewsVisible()
    {
        RunWithDiagnostics(nameof(V2Demo_SelectionCardList_ControlsIndividualSelectionsAndKeepsPreviewsVisible), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Add 5 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const cards = Array.from(document.querySelectorAll('[data-testid=\"selection-card\"]'));" +
                "const previews = Array.from(document.querySelectorAll('[data-testid=\"selection-card-preview\"]'));" +
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "const viewers = previews.map(preview => preview.querySelector('cropper-viewer'));" +
                "const viewerSelections = viewers.map(viewer => viewer?.getAttribute('selection') ?? '');" +
                "const viewerTargets = viewers.map(viewer => viewer?.$selection ?? null);" +
                "const viewerRects = viewers.map(viewer => viewer?.getBoundingClientRect());" +
                "const selectionIds = selections.slice(0, 5).map(selection => `#${selection.id}`);" +
                "const selectionBounds = selections.slice(0, 5).map(selection => `${Math.round(selection.x)},${Math.round(selection.y)},${Math.round(selection.width)},${Math.round(selection.height)}`);" +
                "return cards.length >= 5 && previews.length >= 5" +
                " && viewerRects.slice(0, 5).every(rect => rect && rect.width > 0 && rect.height > 0)" +
                " && viewerSelections.slice(0, 5).every((selection, index) => selection && selection === selectionIds[index])" +
                " && viewerSelections.slice(0, 5).every((selection, index) => document.querySelector(selection) === selections[index])" +
                " && viewerTargets.slice(0, 5).every((selection, index) => selection === selections[index])" +
                " && new Set(selectionBounds).size === 5" +
                " && new Set(viewerSelections.slice(0, 5)).size === 5;"));

            decimal firstSelectionX = Convert.ToDecimal(_fixture.ExecuteScript<object>("return document.querySelector('.img-container cropper-selection')?.x ?? 0;"));
            IWebElement firstMoveRight = _fixture.WaitForClickable(By.CssSelector("[data-testid='selection-card'] [data-testid='selection-card-move-right']"));

            ScrollIntoView(firstMoveRight);
            ClickElement(firstMoveRight);

            _fixture.Wait.Until(_ => Convert.ToDecimal(_fixture.ExecuteScript<object>("return document.querySelector('.img-container cropper-selection')?.x ?? 0;")) > firstSelectionX);

            IWebElement firstWideButton = _fixture.WaitForClickable(By.CssSelector("[data-testid='selection-card'] [data-testid='selection-card-size-wide']"));

            ScrollIntoView(firstWideButton);
            ClickElement(firstWideButton);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "const previews = Array.from(document.querySelectorAll('[data-testid=\"selection-card-preview\"]'));" +
                "const viewers = previews.map(preview => preview.querySelector('cropper-viewer'));" +
                "const viewerRects = viewers.map(viewer => viewer?.getBoundingClientRect());" +
                "return Math.round(selections[0]?.width ?? 0) === 160" +
                " && selections.slice(0, 5).every(selection => !selection.hidden && Math.round(selection.width || 0) > 0 && Math.round(selection.height || 0) > 0)" +
                " && viewerRects.slice(0, 5).every(rect => rect && rect.width > 0 && rect.height > 0)" +
                " && viewers.slice(0, 5).every((viewer, index) => viewer?.$selection === selections[index])" +
                " && viewers.slice(0, 5).every((viewer, index) => Math.round(viewer?.$selection?.width ?? 0) === Math.round(selections[index]?.width ?? 0))" +
                " && viewers.slice(0, 5).every((viewer, index) => Math.round(viewer?.$selection?.height ?? 0) === Math.round(selections[index]?.height ?? 0));"));

            _fixture.Wait.Until(_ => SelectionCardViewerCanvasesMatchSelections(5));
        });
    }

    [Fact]
    public void V2Demo_LiveSelectionPreviews_UpdateWhenActiveMultiSelectionChanges()
    {
        RunWithDiagnostics(nameof(V2Demo_LiveSelectionPreviews_UpdateWhenActiveMultiSelectionChanges), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "const liveViewers = Array.from(document.querySelectorAll('.img-preview')).map(preview => preview.querySelector('cropper-viewer')).filter(Boolean);" +
                "return selections.length >= 3 && liveViewers.length > 0 && liveViewers.every(viewer => !!viewer.$selection);"));

            string firstLivePreviewSelection = GetLivePreviewSelectionSelector();
            string firstLivePreviewBounds = GetLivePreviewSelectionBounds();

            ActivateSelectionByIndex(1);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "const activeSelection = selections[1];" +
                "const liveViewers = Array.from(document.querySelectorAll('.img-preview')).map(preview => preview.querySelector('cropper-viewer'));" +
                "return !!activeSelection && activeSelection.active && liveViewers.length > 0 && liveViewers.every(viewer => viewer.getAttribute('selection') === `#${activeSelection.id}` && viewer.$selection === activeSelection);"));

            string secondLivePreviewSelection = GetLivePreviewSelectionSelector();
            string secondLivePreviewBounds = GetLivePreviewSelectionBounds();

            Assert.NotEqual(firstLivePreviewSelection, secondLivePreviewSelection);
            Assert.NotEqual(firstLivePreviewBounds, secondLivePreviewBounds);
            Assert.Equal(GetSelectionSelector(1), secondLivePreviewSelection);

            ActivateSelectionByIndex(2);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "const activeSelection = selections[2];" +
                "const liveViewers = Array.from(document.querySelectorAll('.img-preview')).map(preview => preview.querySelector('cropper-viewer'));" +
                "return !!activeSelection && activeSelection.active && liveViewers.length > 0 && liveViewers.every(viewer => viewer.getAttribute('selection') === `#${activeSelection.id}` && viewer.$selection === activeSelection);"));

            string thirdLivePreviewSelection = GetLivePreviewSelectionSelector();
            string thirdLivePreviewBounds = GetLivePreviewSelectionBounds();

            Assert.NotEqual(secondLivePreviewSelection, thirdLivePreviewSelection);
            Assert.NotEqual(secondLivePreviewBounds, thirdLivePreviewBounds);
            Assert.Equal(GetSelectionSelector(2), thirdLivePreviewSelection);
        });
    }

    [Fact]
    public void V2Demo_CircleSelectionCardViewer_UpdatesPreviewDataAfterResize()
    {
        RunWithDiagnostics(nameof(V2Demo_CircleSelectionCardViewer_UpdatesPreviewDataAfterResize), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const previews = Array.from(document.querySelectorAll('[data-testid=\"selection-card-preview\"]'));" +
                "const circlePreview = previews.find(preview => preview.querySelector('.selection-card-viewer')?.classList.contains('selection-preview-circle'));" +
                "const viewer = circlePreview?.querySelector('cropper-viewer');" +
                "const rect = viewer?.getBoundingClientRect();" +
                "return !!viewer?.$selection && rect.width > 0 && rect.height > 0;"));

            string initialCirclePreviewData = GetSelectionCardViewerPreviewData("selection-preview-circle");
            int circleCardIndex = Convert.ToInt32(_fixture.ExecuteScript<long>(
                "const cards = Array.from(document.querySelectorAll('[data-testid=\"selection-card\"]'));" +
                "return cards.findIndex(card => card.querySelector('.selection-card-viewer')?.classList.contains('selection-preview-circle'));"));

            IWebElement wideButton = _fixture.WaitForClickable(By.CssSelector($"[data-testid='selection-card']:nth-of-type({circleCardIndex + 1}) [data-testid='selection-card-size-wide']"));

            ScrollIntoView(wideButton);
            ClickElement(wideButton);

            _fixture.Wait.Until(_ =>
            {
                string nextCirclePreviewData = GetSelectionCardViewerPreviewData("selection-preview-circle");

                return nextCirclePreviewData != initialCirclePreviewData
                    && nextCirclePreviewData.EndsWith(",160,90", StringComparison.Ordinal);
            });

            _fixture.Wait.Until(_ => SelectionCardViewerCanvasMatchesSelection("selection-preview-circle", 160, 90));
        });
    }

    [Fact]
    public void V2Demo_SelectionCardPreview_MatchesLivePreviewWhenMultiSelectionHasOneSelection()
    {
        RunWithDiagnostics(nameof(V2Demo_SelectionCardPreview_MatchesLivePreviewWhenMultiSelectionHasOneSelection), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            SetSettingsInput("maximum-selection-count", "1");
            ClickButtonByText("Start 3 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "const liveViewer = document.querySelector('.img-preview')?.querySelector('cropper-viewer');" +
                "const firstCardViewer = document.querySelector('[data-testid=\"selection-card-preview\"]')?.querySelector('cropper-viewer');" +
                "const cards = Array.from(document.querySelectorAll('[data-testid=\"selection-card\"]'));" +
                "return selections.length === 1 && !!liveViewer && !!firstCardViewer" +
                " && cards.length === 1" +
                " && liveViewer.getAttribute('selection') === firstCardViewer.getAttribute('selection')" +
                " && document.querySelector(liveViewer.getAttribute('selection')) === selections[0]" +
                " && document.querySelector(firstCardViewer.getAttribute('selection')) === selections[0];"));
        });
    }

    [Fact]
    public void V2Demo_ZoomOutsideSelection_WhenMaxLimitReached_PreventsImagePositionChange()
    {
        RunWithDiagnostics(nameof(V2Demo_ZoomOutsideSelection_WhenMaxLimitReached_PreventsImagePositionChange), () =>
        {
            OpenV2Demo();

            SetSettingsInput("zoom-min-ratio", "0.50");
            SetSettingsInput("zoom-max-ratio", "0.80");
            ChangeSelectionZone(24, 32, 180, 90);
            ScrollToCropper();

            _fixture.ExecuteScript<object>(
                "const image = document.querySelector('cropper-image');" +
                "if (!image) return;" +
                "const current = Math.hypot(...image.$getTransform().slice(0, 2)) || 1;" +
                "image.$zoom((0.8 / current) - 1);");
            _fixture.Wait.Until(_ => GetImageZoomRatio() >= 0.78m && GetImageZoomRatio() <= 0.82m);

            IReadOnlyCollection<object> transformBeforeWheel = GetImageTransform();

            DispatchNativeWheelZoomOutsideSelection();

            _fixture.Wait.Until(_ => GetImageZoomRatio() >= 0.78m && GetImageZoomRatio() <= 0.82m);

            IReadOnlyCollection<object> transformAfterWheel = GetImageTransform();

            Assert.InRange(GetImageZoomRatio(), 0.48m, 0.82m);
            Assert.True(TransformTranslationIsStable(transformBeforeWheel, transformAfterWheel, 2m));
            AssertNoBrowserConsoleErrorContaining("DeserializeUnableToConvertValue");
            AssertNoBrowserConsoleErrorContaining("Cropper.Blazor.Events.ActionEvent");
        });
    }

    [Fact]
    public void V2Demo_MainCropperSettings_UpdateActiveCropper()
    {
        RunWithDiagnostics(nameof(V2Demo_MainCropperSettings_UpdateActiveCropper), () =>
        {
            OpenV2Demo();

            ScrollToCustomElementsPlayground();
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("return document.querySelectorAll('cropper-canvas').length === 1;"));
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("return !!document.querySelector('[data-testid=\"v2-custom-elements-panel\"] [data-testid=\"v2-main-background\"]');"));

            ClickSettingsSwitch("v2-main-background");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("const canvas = document.querySelector('cropper-canvas'); return !!canvas && canvas.background === false;"));

            ClickSettingsSwitch("v2-main-background");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("const canvas = document.querySelector('cropper-canvas'); return !!canvas && canvas.background === true;"));
        });
    }

    [Fact]
    public void V2Demo_MultipleSelection_CreatesAdditionalCropperSelections()
    {
        RunWithDiagnostics(nameof(V2Demo_MultipleSelection_CreatesAdditionalCropperSelections), () =>
        {
            OpenV2Demo();

            ScrollToCustomElementsPlayground();
            ClickSettingsSwitch("v2-main-selection-multiple");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-selection", "multiple"));

            _fixture.ExecuteScript<object>(
                "const canvas = document.querySelector('cropper-canvas');" +
                "const bounds = canvas.getBoundingClientRect();" +
                "canvas.dispatchEvent(new CustomEvent('action', { bubbles: true, cancelable: true, detail: { action: 'select', startX: bounds.left + 20, startY: bounds.top + 30, endX: bounds.left + 100, endY: bounds.top + 90 } }));");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>(
                "const selections = document.querySelectorAll('cropper-selection');" +
                "return selections.length;") >= 2);
        });
    }

    [Fact]
    public void V2Demo_SelectionCardPreview_BindsToTargetSelectionWithoutRebindingMainViewer()
    {
        RunWithDiagnostics(nameof(V2Demo_SelectionCardPreview_BindsToTargetSelectionWithoutRebindingMainViewer), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
                "return selections.length >= 3 && selections.every(selection => selection.id);"));

            string initialViewerSelection = _fixture.ExecuteScript<string>(
                "return document.querySelector('.img-preview.preview-lg cropper-viewer')?.getAttribute('selection') ?? ''; ");
            string secondSelectionId = _fixture.ExecuteScript<string>(
                "const selection = document.querySelectorAll('.img-container cropper-selection')[1]; return selection ? `#${selection.id}` : ''; ");
            string secondPreviewSelection = _fixture.ExecuteScript<string>(
                "return document.querySelectorAll('[data-testid=\"selection-card-preview\"]')[1]?.querySelector('cropper-viewer')?.getAttribute('selection') ?? ''; ");
            string secondSelectionBounds = _fixture.ExecuteScript<string>(
                "const selection = document.querySelectorAll('.img-container cropper-selection')[1];" +
                "return selection ? `${Math.round(selection.x)},${Math.round(selection.y)},${Math.round(selection.width)},${Math.round(selection.height)}` : ''; ");
            string firstSelectionBounds = _fixture.ExecuteScript<string>(
                "const selection = document.querySelectorAll('.img-container cropper-selection')[0];" +
                "return selection ? `${Math.round(selection.x)},${Math.round(selection.y)},${Math.round(selection.width)},${Math.round(selection.height)}` : ''; ");
            string secondPreviewTargetBounds = _fixture.ExecuteScript<string>(
                "const viewer = document.querySelectorAll('[data-testid=\"selection-card-preview\"]')[1]?.querySelector('cropper-viewer');" +
                "const selection = viewer?.getAttribute('selection');" +
                "const target = selection ? document.querySelector(selection) : null;" +
                "return target ? `${Math.round(target.x)},${Math.round(target.y)},${Math.round(target.width)},${Math.round(target.height)}` : ''; ");
            bool secondPreviewTargetsSecondSelection = _fixture.ExecuteScript<bool>(
                "const viewer = document.querySelectorAll('[data-testid=\"selection-card-preview\"]')[1]?.querySelector('cropper-viewer');" +
                "const selection = document.querySelectorAll('.img-container cropper-selection')[1];" +
                "return !!viewer && viewer.$selection === selection;");

            _fixture.ExecuteScript<object>(
                "document.querySelectorAll('[data-testid=\"selection-card-preview\"]')[1]?.click();");

            Assert.NotEqual(initialViewerSelection, secondSelectionId);
            Assert.Equal(secondSelectionId, secondPreviewSelection);
            Assert.Equal(secondSelectionBounds, secondPreviewTargetBounds);
            Assert.True(secondPreviewTargetsSecondSelection);
            Assert.NotEqual(firstSelectionBounds, secondSelectionBounds);
            Assert.Equal(initialViewerSelection, _fixture.ExecuteScript<string>(
                "return document.querySelector('.img-preview.preview-lg cropper-viewer')?.getAttribute('selection') ?? ''; "));
        });
    }

    [Fact]
    public void V2Demo_MultipleSelectionControls_SetDifferentSelectionFigures()
    {
        RunWithDiagnostics(nameof(V2Demo_MultipleSelectionControls_SetDifferentSelectionFigures), () =>
        {
            OpenV2Demo();

            ClickButtonByText("Multi figures");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('cropper-selection'));" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow';"));

            ClickButtonByText("#1 Circle");
            ClickButtonByText("#2 Pentagon");
            ClickButtonByText("#3 Arrow");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('cropper-selection'));" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow'" +
                " && Math.round(selections[0]?.width ?? 0) > 0" +
                " && Math.round(selections[1]?.width ?? 0) > 0" +
                " && Math.round(selections[2]?.width ?? 0) > 0;"));
        });
    }

    [Fact]
    public void V2Demo_InitialSelection_CanBeRecreatedWithoutReset()
    {
        RunWithDiagnostics(nameof(V2Demo_InitialSelection_CanBeRecreatedWithoutReset), () =>
        {
            OpenV2Demo();
            ScrollToCropper();

            DragSelectionOnCanvas(12, 12, 160, 90);

            WaitForSelectionDimension("width", 160);
            WaitForSelectionDimension("height", 90);
        });
    }

    [Fact]
    public void V2Demo_V2CanvasAndImageSettings_UpdateElements()
    {
        RunWithDiagnostics(nameof(V2Demo_V2CanvasAndImageSettings_UpdateElements), () =>
        {
            OpenV2Demo();

            ScrollToCustomElementsPlayground();
            ClickSettingsSwitch("v2-main-canvas-hidden");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-canvas", "hidden"));

            ClickSettingsSwitch("v2-main-canvas-disabled");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-canvas", "disabled"));

            SetColorSettingsInput("v2-main-canvas-theme-color", "#ff0000");
            _fixture.Wait.Until(_ => GetCropperProperty<string>("cropper-canvas", "themeColor") == "#ff0000");

            ClickSettingsSwitch("v2-main-image-hidden");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-image", "hidden"));

            ClickSettingsSwitch("v2-main-skewable");
            _fixture.Wait.Until(_ => !GetCropperProperty<bool>("cropper-image", "skewable"));

            SetSettingsInput("v2-main-image-alt", "Updated v2 alt");
            _fixture.Wait.Until(_ => GetCropperAttribute("cropper-image", "alt") == "Updated v2 alt");
        });
    }

    [Fact]
    public void V2Demo_V2GridSettings_UpdateElement()
    {
        RunWithDiagnostics(nameof(V2Demo_V2GridSettings_UpdateElement), () =>
        {
            OpenV2Demo();

            ScrollToCustomElementsPlayground();
            ClickSettingsSwitch("v2-main-grid-hidden");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-grid", "hidden"));

            SetSettingsInput("v2-main-grid-rows", "5");
            _fixture.Wait.Until(_ => GetCropperAttribute("cropper-grid", "rows") == "5");

            SetSettingsInput("v2-main-grid-columns", "4");
            _fixture.Wait.Until(_ => GetCropperAttribute("cropper-grid", "columns") == "4");

            ClickSettingsSwitch("v2-main-grid-bordered");
            _fixture.Wait.Until(_ => !GetCropperProperty<bool>("cropper-grid", "bordered"));

            ClickSettingsSwitch("v2-main-grid-covered");
            _fixture.Wait.Until(_ => !GetCropperProperty<bool>("cropper-grid", "covered"));

            SetColorSettingsInput("v2-main-grid-theme-color", "#00ff00");
            _fixture.Wait.Until(_ => GetCropperProperty<string>("cropper-grid", "themeColor") == "rgba(0, 255, 0, 0.5)");
        });
    }

    [Fact]
    public void V2Demo_V2CrosshairAndHandleSettings_UpdateElements()
    {
        RunWithDiagnostics(nameof(V2Demo_V2CrosshairAndHandleSettings_UpdateElements), () =>
        {
            OpenV2Demo();

            ScrollToCustomElementsPlayground();
            ClickSettingsSwitch("v2-main-crosshair-hidden");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-crosshair", "hidden"));

            ClickSettingsSwitch("v2-main-crosshair-centered");
            _fixture.Wait.Until(_ => !GetCropperProperty<bool>("cropper-crosshair", "centered"));

            SetColorSettingsInput("v2-main-crosshair-theme-color", "#0000ff");
            _fixture.Wait.Until(_ => GetCropperProperty<string>("cropper-crosshair", "themeColor") == "rgba(0, 0, 255, 0.5)");

            ClickSettingsSwitch("v2-main-move-handle-hidden");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-selection cropper-handle[action='move'], cropper-selection cropper-handle[action='select']", "hidden"));

            SetColorSettingsInput("v2-main-move-handle-theme-color", "#ff00ff");
            _fixture.Wait.Until(_ => GetCropperProperty<string>("cropper-selection cropper-handle[action='move'], cropper-selection cropper-handle[action='select']", "themeColor") == "rgba(255, 0, 255, 0.35)");

            ClickSettingsSwitch("v2-main-resize-handle-hidden");
            _fixture.Wait.Until(_ => GetCropperProperty<bool>("cropper-handle[action='n-resize']", "hidden"));

            SetColorSettingsInput("v2-main-resize-handle-theme-color", "#00ffff");
            _fixture.Wait.Until(_ => GetCropperProperty<string>("cropper-handle[action='n-resize']", "themeColor") == "rgba(0, 255, 255, 0.5)");
        });
    }

    [Theory]
    [InlineData("dimensions-min-width", "200", "width", "100×90", 200)]
    [InlineData("dimensions-min-height", "120", "height", "160×60", 120)]
    [InlineData("dimensions-max-width", "120", "width", "200×90", 120)]
    [InlineData("dimensions-max-height", "70", "height", "160×120", 70)]
    public void V2Demo_DimensionsSettings_ClampSelectionSize(string settingTestId, string settingValue, string dimension, string buttonText, int expectedValue)
    {
        RunWithDiagnostics($"V2Demo_DimensionsSettings_ClampSelectionSize_{settingTestId}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            SetSettingsInput(settingTestId, settingValue);
            ClickButtonByText(buttonText);

            WaitForSelectionDimension(dimension, expectedValue);
        });
    }

    [Theory]
    [InlineData("dimensions-min-width", "200", "width", "100×90", 100)]
    [InlineData("dimensions-min-height", "120", "height", "160×60", 60)]
    [InlineData("dimensions-max-width", "120", "width", "200×90", 200)]
    [InlineData("dimensions-max-height", "70", "height", "160×120", 120)]
    public void V2Demo_DimensionsSettings_NullValueStopsClamping(string settingTestId, string settingValue, string dimension, string buttonText, int expectedValue)
    {
        RunWithDiagnostics($"V2Demo_DimensionsSettings_NullValueStopsClamping_{settingTestId}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            SetSettingsInput(settingTestId, settingValue);
            ClearSettingsInput(settingTestId);
            ClickButtonByText(buttonText);

            WaitForSelectionDimension(dimension, expectedValue);
        });
    }

    [Fact]
    public void V2Demo_DimensionsSettings_ClampSelectionSize_WhenSelectionIsStretched()
    {
        RunWithDiagnostics(nameof(V2Demo_DimensionsSettings_ClampSelectionSize_WhenSelectionIsStretched), () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            SetSettingsInput("dimensions-min-width", "120");
            SetSettingsInput("dimensions-min-height", "80");
            SetSettingsInput("dimensions-max-width", "220");
            SetSettingsInput("dimensions-max-height", "140");

            ClickButtonByText("320×200");
            WaitForSelectionDimension("width", 220);
            WaitForSelectionDimension("height", 140);

            ClickButtonByText("80×50");
            WaitForSelectionDimension("width", 120);
            WaitForSelectionDimension("height", 80);
        });
    }

    [Theory]
    [InlineData("dimensions-min-width", "150", "width", 40, 40, 150)]
    [InlineData("dimensions-min-height", "110", "height", 40, 40, 110)]
    [InlineData("dimensions-max-width", "180", "width", 320, 160, 180)]
    [InlineData("dimensions-max-height", "130", "height", 260, 240, 130)]
    public void V2Demo_DimensionsSettings_ClampSelectionSize_WhenNativeSelectionIsStretched(string settingTestId, string settingValue, string dimension, int width, int height, int expectedValue)
    {
        RunWithDiagnostics($"V2Demo_DimensionsSettings_ClampSelectionSize_WhenNativeSelectionIsStretched_{settingTestId}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            SetSettingsInput(settingTestId, settingValue);
            DispatchNativeSelectionDrag(30, 35, width, height);

            WaitForSelectionDimension(dimension, expectedValue);
        });
    }

    [Fact]
    public void V2Demo_DimensionsSettings_ClampSelectionSize_WithMinAndMax()
    {
        RunWithDiagnostics(nameof(V2Demo_DimensionsSettings_ClampSelectionSize_WithMinAndMax), () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            ClickButtonByText("Selection 160");
            SetSettingsInput("dimensions-min-width", "140");
            SetSettingsInput("dimensions-max-width", "150");
            ClickButtonByText("Selection 160");

            WaitForSelectionDimension("width", 150);
        });
    }

    [Theory]
    [InlineData("zoom-min-ratio", "0.75", "or")]
    [InlineData("zoom-max-ratio", "0.50", "or")]
    public void V2Demo_ZoomRatioSettings_ClampZoom(string settingTestId, string settingValue, string mode)
    {
        RunWithDiagnostics($"V2Demo_ZoomRatioSettings_ClampZoom_{settingTestId}_{mode}", () =>
        {
            OpenV2Demo();

            decimal beforeRatio = GetImageZoomRatio();
            if (settingTestId == "zoom-max-ratio")
            {
                ClickButtonByTitle("Zoom In");
                beforeRatio = GetImageZoomRatio();
            }

            SetSettingsInput(settingTestId, settingValue);
            decimal expectedRatio = decimal.Parse(settingValue, System.Globalization.CultureInfo.InvariantCulture);

            ClickButtonByTitle(settingTestId == "zoom-min-ratio" ? "Zoom Out" : "Zoom In");

            _fixture.Wait.Until(_ =>
            {
                decimal ratio = GetImageZoomRatio();
                return settingTestId == "zoom-min-ratio"
                    ? ratio >= expectedRatio - 0.02m
                    : ratio <= beforeRatio && ratio <= expectedRatio + 0.05m;
            });
        });
    }

    [Fact]
    public void V2Demo_AspectRatioSettings_UpdateCurrentAspectRatioWhenCropperChanges()
    {
        RunWithDiagnostics(nameof(V2Demo_AspectRatioSettings_UpdateCurrentAspectRatioWhenCropperChanges), () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            ClickButtonByText("Selection 160");
            string beforeRatio = GetSettingsInputValue("aspect-current-ratio");
            ClickButtonByText("160×120");

            _fixture.Wait.Until(_ =>
            {
                string currentRatio = GetSettingsInputValue("aspect-current-ratio");
                decimal selectionRatio = GetSelectionAspectRatio();

                return currentRatio != beforeRatio
                    && decimal.TryParse(currentRatio, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out decimal displayedRatio)
                    && Math.Abs(displayedRatio - selectionRatio) <= 0.05m;
            });
        });
    }

    [Fact]
    public void V2Demo_ZoomRatioSettings_UpdateVisibleZoomEventRatios()
    {
        RunWithDiagnostics(nameof(V2Demo_ZoomRatioSettings_UpdateVisibleZoomEventRatios), () =>
        {
            OpenV2Demo();

            string beforeRatio = GetSettingsInputValue("zoom-current-ratio");
            ClickButtonByTitle("Zoom In");

            _fixture.Wait.Until(_ =>
            {
                string oldRatio = GetSettingsInputValue("zoom-old-ratio");
                string currentRatio = GetSettingsInputValue("zoom-current-ratio");

                return !string.IsNullOrWhiteSpace(oldRatio)
                    && !string.IsNullOrWhiteSpace(currentRatio)
                    && currentRatio != beforeRatio;
            });
        });
    }

    [Fact]
    public void V2Demo_ZoomRatioSettings_UpdateVisibleZoomEventRatios_WhenNativeZoomIsClamped()
    {
        RunWithDiagnostics(nameof(V2Demo_ZoomRatioSettings_UpdateVisibleZoomEventRatios_WhenNativeZoomIsClamped), () =>
        {
            OpenV2Demo();

            SetSettingsInput("zoom-max-ratio", "0.80");
            _fixture.Wait.Until(_ => GetImageZoomRatio() <= 0.82m);
            DispatchLimitedNativeZoom(1.20m);

            _fixture.Wait.Until(_ =>
            {
                decimal oldRatio = GetSettingsInputDecimalValue("zoom-old-ratio");
                decimal currentRatio = GetSettingsInputDecimalValue("zoom-current-ratio");

                return oldRatio > 0
                    && currentRatio > 0
                    && currentRatio <= 0.82m
                    && Math.Abs(currentRatio - GetImageZoomRatio()) <= 0.02m;
            });
        });
    }

    [Fact]
    public void V2Demo_ZoomRatioSettings_ClampZoom_WithMinAndMax()
    {
        RunWithDiagnostics(nameof(V2Demo_ZoomRatioSettings_ClampZoom_WithMinAndMax), () =>
        {
            OpenV2Demo();

            SetSettingsInput("zoom-min-ratio", "0.50");
            SetSettingsInput("zoom-max-ratio", "0.80");

            _fixture.Wait.Until(_ =>
            {
                decimal ratio = GetImageZoomRatio();
                return ratio >= 0.48m && ratio <= 0.82m;
            });
        });
    }

    [Theory]
    [InlineData("VM0")]
    [InlineData("VM1")]
    [InlineData("VM2")]
    [InlineData("VM3")]
    public void V2Demo_ZoomingWithLimitedViewModes_DoesNotOverflowSelectionChange(string viewMode)
    {
        RunWithDiagnostics($"V2Demo_ZoomingWithLimitedViewModes_DoesNotOverflowSelectionChange_{viewMode}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText(viewMode);
            ClickButtonByExactText("Free");
            SetSettingsInput("dimensions-max-height", "40");
            SetSettingsInput("dimensions-max-width", "80");
            ClickButtonByText("320×200");

            for (int i = 0; i < 4; i++)
            {
                ClickButtonByTitle("Zoom In");
            }

            WaitForSelectionAvailable();
            AssertNoBrowserConsoleErrorContaining("Maximum call stack size exceeded");
            AssertNoBrowserConsoleErrorContaining("limitSelectionOnChange");
        });
    }

    [Theory]
    [InlineData("VM0")]
    [InlineData("VM1")]
    [InlineData("VM2")]
    [InlineData("VM3")]
    public void V2Demo_ZoomingWithViewModes_ReportsAbsoluteZoomEventRatios(string viewMode)
    {
        RunWithDiagnostics($"V2Demo_ZoomingWithViewModes_ReportsAbsoluteZoomEventRatios_{viewMode}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText(viewMode);
            decimal beforeRatio = GetImageZoomRatio();

            ClickButtonByTitle("Zoom In");

            _fixture.Wait.Until(_ =>
            {
                decimal imageRatio = GetImageZoomRatio();
                decimal oldRatio = GetSettingsInputDecimalValue("zoom-old-ratio");
                decimal currentRatio = GetSettingsInputDecimalValue("zoom-current-ratio");

                return oldRatio > 0
                    && currentRatio > oldRatio
                    && Math.Abs(oldRatio - beforeRatio) <= 0.02m
                    && Math.Abs(currentRatio - imageRatio) <= 0.02m;
            });
        });
    }

    [Fact]
    public void V2Demo_AspectRatioSettings_NullValueStopsClamping()
    {
        RunWithDiagnostics(nameof(V2Demo_AspectRatioSettings_NullValueStopsClamping), () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            SetSettingsInput("aspect-min-ratio", "2.0");
            ClearSettingsInput("aspect-min-ratio");
            ClickButtonByText("160×120");

            _fixture.Wait.Until(_ =>
            {
                decimal aspectRatio = GetSelectionAspectRatio();
                return aspectRatio >= 1.30m && aspectRatio <= 1.36m;
            });
        });
    }

    [Theory]
    [InlineData("aspect-min-ratio", "2.0", 2.0, "or")]
    [InlineData("aspect-max-ratio", "1.2", 1.2, "or")]
    public void V2Demo_AspectRatioSettings_ApplyOnlyInFreeMode(string settingTestId, string settingValue, decimal expectedRatio, string mode)
    {
        RunWithDiagnostics($"V2Demo_AspectRatioSettings_ApplyOnlyInFreeMode_{settingTestId}_{mode}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            _fixture.Wait.Until(_ => Math.Abs(Convert.ToDecimal(_fixture.ExecuteScript<object>("return document.querySelector('cropper-selection')?.aspectRatio || 0;"))) < 0.0001m);
            ClickButtonByText("Selection 160");
            SetSettingsInput(settingTestId, settingValue);

            _fixture.Wait.Until(_ =>
            {
                decimal aspectRatio = GetSelectionAspectRatio();
                return aspectRatio >= expectedRatio - 0.05m && aspectRatio <= expectedRatio + 0.05m;
            });
        });
    }

    [Fact]
    public void V2Demo_AspectRatioSettings_ApplyOnlyInFreeMode_WithMinAndMax()
    {
        RunWithDiagnostics(nameof(V2Demo_AspectRatioSettings_ApplyOnlyInFreeMode_WithMinAndMax), () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            ClickButtonByText("Selection 160");
            SetSettingsInput("aspect-min-ratio", "1.2");
            SetSettingsInput("aspect-max-ratio", "1.4");

            _fixture.Wait.Until(_ =>
            {
                decimal aspectRatio = GetSelectionAspectRatio();
                return aspectRatio >= 1.19m && aspectRatio <= 1.41m;
            });
        });
    }

    [Theory]
    [InlineData("16:9", 1.70, 1.85, 0.0, 0.0)]
    [InlineData("4:3", 1.25, 1.40, 0.0, 0.0)]
    [InlineData("1:1", 0.95, 1.05, 0.0, 0.0)]
    [InlineData("Free", 1.20, 1.40, 1.20, 1.40)]
    public void V2Demo_CropNewZone_UpdatesCurrentAspectRatioAndDoesNotLogErrors(string mode, decimal expectedMinRatio, decimal expectedMaxRatio, decimal minAspectRatio, decimal maxAspectRatio)
    {
        RunWithDiagnostics($"V2Demo_CropNewZone_UpdatesCurrentAspectRatioAndDoesNotLogErrors_{mode}_{minAspectRatio}_{maxAspectRatio}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText(mode);

            if (minAspectRatio > 0)
            {
                SetSettingsInput("aspect-min-ratio", minAspectRatio.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            if (maxAspectRatio > 0)
            {
                SetSettingsInput("aspect-max-ratio", maxAspectRatio.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            decimal beforeAspectRatio = GetCurrentAspectRatioSettingDecimalValue();

            ChangeSelectionZone(24, 32, 180, 90);

            _fixture.Wait.Until(_ => GetCurrentAspectRatioSettingDecimalValue() > 0);

            decimal aspectRatio = GetCurrentAspectRatioSettingDecimalValue();

            if (expectedMinRatio > 0 || expectedMaxRatio > 0)
            {
                Assert.InRange(aspectRatio, expectedMinRatio, expectedMaxRatio);
            }

            AssertNoBrowserConsoleErrorContaining("DeserializeUnableToConvertValue");
            AssertNoBrowserConsoleErrorContaining("Cropper.Blazor.Events.ActionEvent");
        });
    }

    [Theory]
    [InlineData("VM1", "Free", null, "1.40", 0.0, 1.40)]
    [InlineData("VM2", "Free", "1.20", null, 1.20, 10.0)]
    [InlineData("VM3", "Free", "1.20", "1.40", 1.20, 1.40)]
    [InlineData("VM1", "16:9", null, null, 1.70, 1.85)]
    [InlineData("VM2", "4:3", null, null, 1.25, 1.40)]
    [InlineData("VM3", "1:1", null, null, 0.95, 1.05)]
    public void V2Demo_CropSeveralNewZones_WithViewModesAndAspectLimits_DoesNotLogErrors(
        string viewMode,
        string aspectMode,
        string? minAspectRatio,
        string? maxAspectRatio,
        decimal expectedMinRatio,
        decimal expectedMaxRatio)
    {
        RunWithDiagnostics($"V2Demo_CropSeveralNewZones_WithViewModesAndAspectLimits_DoesNotLogErrors_{viewMode}_{aspectMode}_{minAspectRatio}_{maxAspectRatio}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText(viewMode);
            ClickButtonByExactText(aspectMode);
            SetNullableSettingsInput("aspect-min-ratio", minAspectRatio);
            SetNullableSettingsInput("aspect-max-ratio", maxAspectRatio);

            (decimal X, decimal Y, decimal Width, decimal Height)[] zones =
            [
                (24, 32, 180, 90),
                (48, 40, 120, 120),
                (12, 16, 220, 80)
            ];

            foreach ((decimal x, decimal y, decimal width, decimal height) in zones)
            {
                ChangeSelectionZone(x, y, width, height, dispatchActionEvents: true);

                _fixture.Wait.Until(_ => GetSelectionAspectRatio() > 0);

                decimal aspectRatio = GetSelectionAspectRatio();
                Assert.InRange(aspectRatio, expectedMinRatio, expectedMaxRatio);
                WaitForSelectionAvailable();
            }

            AssertNoBrowserConsoleErrorContaining("DeserializeUnableToConvertValue");
            AssertNoBrowserConsoleErrorContaining("Cropper.Blazor.Events.ActionEvent");
        });
    }

    [Theory]
    [InlineData("VM1", true, null, "0.80", 0.10, 0.80)]
    [InlineData("VM1", false, "0.50", null, 0.50, 10.0)]
    [InlineData("VM2", true, "0.50", "0.80", 0.50, 0.80)]
    [InlineData("VM2", false, null, "0.70", 0.10, 0.70)]
    [InlineData("VM3", true, "0.55", null, 0.55, 10.0)]
    [InlineData("VM3", false, "0.50", "0.75", 0.50, 0.75)]
    public void V2Demo_ZoomSeveralZones_WithViewModesAndNullableLimits_ReportsRatios(
        string viewMode,
        bool onSelection,
        string? minZoomRatio,
        string? maxZoomRatio,
        decimal expectedMinRatio,
        decimal expectedMaxRatio)
    {
        RunWithDiagnostics($"V2Demo_ZoomSeveralZones_WithViewModesAndNullableLimits_ReportsRatios_{viewMode}_{onSelection}_{minZoomRatio}_{maxZoomRatio}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText(viewMode);
            SetNullableSettingsInput("zoom-min-ratio", minZoomRatio);
            SetNullableSettingsInput("zoom-max-ratio", maxZoomRatio);
            ChangeSelectionZone(24, 32, 180, 90);
            ScrollToCropper();

            for (int i = 0; i < 3; i++)
            {
                decimal beforeRatio = GetImageZoomRatio();

                DispatchWheelZoom(onSelection, maxZoomRatio);

                _fixture.Wait.Until(_ =>
                {
                    decimal oldRatio = GetZoomRatioSettingDecimalValue("zoom-old-ratio");
                    decimal currentRatio = GetZoomRatioSettingDecimalValue("zoom-current-ratio");

                    return oldRatio > 0
                        && currentRatio > 0
                        && Math.Abs(oldRatio - beforeRatio) <= 0.03m;
                });

                decimal imageRatio = GetImageZoomRatio();
                decimal currentRatio = GetZoomRatioSettingDecimalValue("zoom-current-ratio");
                Assert.InRange(currentRatio, expectedMinRatio, expectedMaxRatio);
                Assert.InRange(imageRatio, expectedMinRatio, expectedMaxRatio);
            }

            AssertNoBrowserConsoleErrorContaining("DeserializeUnableToConvertValue");
            AssertNoBrowserConsoleErrorContaining("Cropper.Blazor.Events.ActionEvent");
        });
    }

    [Theory]
    [InlineData("VM1")]
    [InlineData("VM2")]
    [InlineData("VM3")]
    public void V2Demo_ZoomOutsideSelection_WithHighMinMaxLimits_ClampsNativeWheelZoom(string viewMode)
    {
        RunWithDiagnostics($"V2Demo_ZoomOutsideSelection_WithHighMinMaxLimits_ClampsNativeWheelZoom_{viewMode}", () =>
        {
            OpenV2Demo();

            ClickButtonByExactText(viewMode);
            SetSettingsInput("zoom-min-ratio", "3.00");
            SetSettingsInput("zoom-max-ratio", "4.00");
            ChangeSelectionZone(24, 32, 180, 90);
            ScrollToCropper();

            DispatchNativeWheelZoomOutsideSelection();

            _fixture.Wait.Until(_ =>
            {
                decimal currentRatio = GetZoomRatioSettingDecimalValue("zoom-current-ratio");
                decimal imageRatio = GetImageZoomRatio();

                return currentRatio >= 3.00m
                    && currentRatio <= 4.00m
                    && imageRatio >= 3.00m
                    && imageRatio <= 4.00m;
            });

            AssertNoBrowserConsoleErrorContaining("DeserializeUnableToConvertValue");
            AssertNoBrowserConsoleErrorContaining("Cropper.Blazor.Events.ActionEvent");
        });
    }

    [Theory]
    [InlineData("/v1")]
    [InlineData("/v2")]
    public void Demo_GetSetDataButtons_UpdateDataCards(string route)
    {
        RunWithDiagnostics($"Demo_GetSetDataButtons_UpdateDataCards_{route.Trim('/')}", () =>
        {
            OpenDemo(route);

            ClickCardButton("Cropper Data", "Get Data");
            WaitForCardInputValue("Cropper Data", "X");
            WaitForCardInputValue("Cropper Data", "Y");
            WaitForCardInputValue("Cropper Data", "Width");
            WaitForCardInputValue("Cropper Data", "Height");
            WaitForCardInputValue("Cropper Data", "ScaleX");
            WaitForCardInputValue("Cropper Data", "ScaleY");
            ClickCardButton("Cropper Data", "Set Data");
            WaitForSelectionAvailable();

            ClickCardButton("Crop Box Data", "Get Data");
            WaitForCardInputValue("Crop Box Data", "Left");
            WaitForCardInputValue("Crop Box Data", "Top");
            WaitForCardInputValue("Crop Box Data", "Width");
            WaitForCardInputValue("Crop Box Data", "Height");
            ClickCardButton("Crop Box Data", "Set Data");
            WaitForSelectionAvailable();

            ClickCardButton("Container Data", "Get Container Data");
            WaitForCardInputValue("Container Data", "Width");
            WaitForCardInputValue("Container Data", "Height");

            ClickCardButton("Image Data", "Get Image Data");
            WaitForCardInputValue("Image Data", "Width");
            WaitForCardInputValue("Image Data", "Height");
            WaitForCardInputValue("Image Data", "Natural Width");
            WaitForCardInputValue("Image Data", "Natural Height");
            WaitForCardInputValue("Image Data", "Aspect Ratio");

            ClickCardButton("Canvas Data", "Get Data");
            WaitForCardInputValue("Canvas Data", "Left");
            WaitForCardInputValue("Canvas Data", "Top");
            WaitForCardInputValue("Canvas Data", "Width");
            WaitForCardInputValue("Canvas Data", "Height");
            WaitForCardInputValue("Canvas Data", "Natural Width");
            WaitForCardInputValue("Canvas Data", "Natural Height");
            ClickCardButton("Canvas Data", "Set Data");
            WaitForSelectionAvailable();
        });
    }

    [Fact]
    public void V2Demo_ResetButtons_ClearLimitSettingFields()
    {
        RunWithDiagnostics(nameof(V2Demo_ResetButtons_ClearLimitSettingFields), () =>
        {
            OpenV2Demo();

            ClickButtonByExactText("Free");
            SetSettingsInput("dimensions-min-width", "100");
            SetSettingsInput("dimensions-max-width", "200");
            SetSettingsInput("zoom-min-ratio", "0.50");
            SetSettingsInput("zoom-max-ratio", "0.80");
            SetSettingsInput("aspect-min-ratio", "1.20");
            SetSettingsInput("aspect-max-ratio", "1.40");

            ClickActionButtonByTitle("Reset");

            _fixture.Wait.Until(_ =>
                string.IsNullOrWhiteSpace(GetSettingsInputValue("dimensions-min-width"))
                && string.IsNullOrWhiteSpace(GetSettingsInputValue("dimensions-max-width"))
                && string.IsNullOrWhiteSpace(GetSettingsInputValue("zoom-min-ratio"))
                && string.IsNullOrWhiteSpace(GetSettingsInputValue("zoom-max-ratio"))
                && string.IsNullOrWhiteSpace(GetSettingsInputValue("aspect-min-ratio"))
                && string.IsNullOrWhiteSpace(GetSettingsInputValue("aspect-max-ratio")));
        });
    }

    [Theory]
    [InlineData("Close", "polygon")]
    [InlineData("Pentagon", "polygon")]
    [InlineData("Circle", "50%")]
    [InlineData("Arrow", "polygon")]
    public void V2Demo_FaceButtons_ApplySelectionStyles(string faceTooltip, string expectedStyle)
    {
        RunWithDiagnostics($"V2Demo_FaceButtons_ApplySelectionStyles_{faceTooltip}", () =>
        {
            OpenV2Demo();

            ClickTooltipButton(faceTooltip);

            _fixture.Wait.Until(_ => GetSelectionFaceStyle().Contains(expectedStyle, StringComparison.OrdinalIgnoreCase));
        });
    }

    [Fact]
    public void V2Demo_FilterAndViewModeButtons_OpenCroppedDialogs()
    {
        RunWithDiagnostics(nameof(V2Demo_FilterAndViewModeButtons_OpenCroppedDialogs), () =>
        {
            OpenV2Demo();

            foreach (string aspectRatio in new[] { "16:9", "4:3", "1:1", "Free" })
            {
                ClickButtonByExactText(aspectRatio);
                WaitForSelectionAvailable();
            }

            foreach (string viewMode in new[] { "VM0", "VM1", "VM2", "VM3" })
            {
                ClickButtonByExactText(viewMode);
                WaitForSelectionAvailable();
            }

            foreach (string faceTooltip in new[] { "Default (Rectangle)", "Close", "Pentagon", "Circle", "Arrow" })
            {
                ClickTooltipButton(faceTooltip);
                ClickAndWaitForCroppedCanvasDialogByExactText("GET");
                CloseDialog();

                ClickTooltipButton(faceTooltip);
                ClickAndWaitForCroppedCanvasDialogByExactText("GET In Background");
                CloseDialog();
            }
        });
    }

    [Theory]
    [InlineData("/v1")]
    [InlineData("/v2")]
    public void Demo_DataPreview_UpdatesAfterTransformActions(string route)
    {
        RunWithDiagnostics($"Demo_DataPreview_UpdatesAfterTransformActions_{route.Trim('/')}", () =>
        {
            OpenDemo(route);
            WaitForDataPreviewValue("Width");
            WaitForDataPreviewValue("Height");

            string rotateBefore = GetDataPreviewValue("Rotate");
            ClickButtonByTitle("Rotate Right");
            WaitForDataPreviewValueChange("Rotate", rotateBefore);

            string scaleBefore = GetDataPreviewValue("ScaleX");
            ClickButtonByTitle("Flip Horizontal");
            WaitForDataPreviewValueChange("ScaleX", scaleBefore);
        });
    }

    [Theory]
    [InlineData("/v1")]
    [InlineData("/v2")]
    public void Demo_CroppedCanvasButtons_OpenImageDialog(string route)
    {
        RunWithDiagnostics($"Demo_CroppedCanvasButtons_OpenImageDialog_{route.Trim('/')}", () =>
        {
            OpenDemo(route);
            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by URL");
            CloseDialog();

            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by element");
            CloseDialog();
        });
    }

    [Theory]
    [InlineData("/v1")]
    [InlineData("/v2")]
    public void Demo_BackgroundAndChunkedButtons_OpenImageDialog(string route)
    {
        RunWithDiagnostics($"Demo_BackgroundAndChunkedButtons_OpenImageDialog_{route.Trim('/')}", () =>
        {
            OpenDemo(route);
            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by element In Background");
            CloseDialog();

            ClickAndWaitForCroppedCanvasDialog("Get Chunked Stream Image In Background");
            CloseDialog();
        });
    }

    [Fact]
    public void V2Demo_BackgroundGetButton_OpensDialogWithinExpectedTime()
    {
        RunWithDiagnostics(nameof(V2Demo_BackgroundGetButton_OpensDialogWithinExpectedTime), () =>
        {
            OpenV2Demo();

            DateTime start = DateTime.UtcNow;
            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by element In Background");
            TimeSpan elapsed = DateTime.UtcNow - start;
            string imageSource = _page.Dialog.GetCroppedCanvasDialogImageSource();

            Assert.StartsWith("data:image/jpeg;base64,", imageSource, StringComparison.Ordinal);
            Assert.True(elapsed < TimeSpan.FromSeconds(5), $"Background GET took {elapsed.TotalSeconds:0.00}s.");
        });
    }

    [Fact]
    public void V2Demo_LoadsCropperElements()
    {
        RunWithDiagnostics(nameof(V2Demo_LoadsCropperElements), () =>
        {
            OpenV2Demo();

            _fixture.WaitForElement(By.CssSelector("cropper-canvas"));
            _fixture.WaitForElement(By.CssSelector("cropper-image"));
            _fixture.WaitForElement(By.CssSelector("cropper-selection"));

            bool hasCropperInstance = _fixture.ExecuteScript<bool>(
                "return !!window.cropper && Object.keys(window.cropper.cropperInstances || {}).length > 0;");

            Assert.True(hasCropperInstance);
        });
    }

    [Fact]
    public void V2Demo_ActionButtons_UpdateImageTransform()
    {
        RunWithDiagnostics(nameof(V2Demo_ActionButtons_UpdateImageTransform), () =>
        {
            OpenV2Demo();

            _fixture.WaitForElement(By.CssSelector("cropper-image"));
            IReadOnlyCollection<object> initialTransform = GetImageTransform();

            ClickButtonByTitle("Zoom In");
            WaitForTransformChange(initialTransform);

            IReadOnlyCollection<object> zoomedTransform = GetImageTransform();

            ClickButtonByTitle("Rotate Right");
            WaitForTransformChange(zoomedTransform);

            IReadOnlyCollection<object> rotatedTransform = GetImageTransform();

            Assert.NotEqual(string.Join(",", initialTransform), string.Join(",", zoomedTransform));
            Assert.NotEqual(string.Join(",", zoomedTransform), string.Join(",", rotatedTransform));
        });
    }

    [Fact]
    public void V2Demo_MouseWheelZoom_UpdatesVisibleZoomEventRatiosAndHonorsLimits()
    {
        RunWithDiagnostics(nameof(V2Demo_MouseWheelZoom_UpdatesVisibleZoomEventRatiosAndHonorsLimits), () =>
        {
            OpenV2Demo();

            SetSettingsInput("zoom-min-ratio", "0.50");
            SetSettingsInput("zoom-max-ratio", "0.60");
            ScrollToCropper();

            decimal initialRatio = GetImageZoomRatio();

            DispatchWheelZoom();

            _fixture.Wait.Until(_ => GetZoomRatioSettingDecimalValue("zoom-old-ratio") > 0
                && GetZoomRatioSettingDecimalValue("zoom-current-ratio") > 0);

            decimal oldRatio = GetZoomRatioSettingDecimalValue("zoom-old-ratio");
            decimal currentRatio = GetZoomRatioSettingDecimalValue("zoom-current-ratio");
            decimal imageRatio = GetImageZoomRatio();

            Assert.InRange(oldRatio, initialRatio - 0.02m, initialRatio + 0.02m);
            Assert.InRange(currentRatio, 0.50m, 0.60m);
            Assert.InRange(imageRatio, 0.50m, 0.60m);
        });
    }

    [Fact]
    public void V2Demo_AspectRatioButton_UpdatesSelectionAspectRatio()
    {
        RunWithDiagnostics(nameof(V2Demo_AspectRatioButton_UpdatesSelectionAspectRatio), () =>
        {
            OpenV2Demo();

            _fixture.WaitForElement(By.CssSelector("cropper-selection"));
            IWebElement aspectRatioButton = _fixture.WaitForClickable(By.XPath("//button[.//*[normalize-space()='16:9'] or normalize-space()='16:9']"));

            ScrollIntoView(aspectRatioButton);
            ClickElement(aspectRatioButton);

            _fixture.Wait.Until(_ =>
            {
                object value = _fixture.ExecuteScript<object>("return document.querySelector('cropper-selection')?.aspectRatio ?? 0;");
                decimal ratio = Convert.ToDecimal(value);
                return Math.Abs(ratio - 1.7777777777777777m) < 0.0001m;
            });

            decimal aspectRatio = Convert.ToDecimal(_fixture.ExecuteScript<object>("return document.querySelector('cropper-selection')?.aspectRatio ?? 0;"));

            Assert.InRange(aspectRatio, 1.7777m, 1.7778m);
        });
    }

    [Fact]
    public void V2Demo_ClearAndCrop_ToggleSelectionVisibility()
    {
        RunWithDiagnostics(nameof(V2Demo_ClearAndCrop_ToggleSelectionVisibility), () =>
        {
            OpenV2Demo();

            _fixture.WaitForElement(By.CssSelector("cropper-selection"));
            ClickButtonByText("Selection 160");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("const selection = document.querySelector('cropper-selection'); return Math.round(selection?.width ?? 0) === 160 && Math.round(selection?.height ?? 0) === 90;"));

            ClickActionButtonByTitle("Clear");
            bool clearedAfterClear = WaitForDataPreviewValue("Width", "0.0000");

            ClickActionButtonByTitle("Crop");
            bool visibleAfterCrop = WaitForSelectionVisible();

            Assert.True(clearedAfterClear);
            Assert.True(visibleAfterCrop);
        });
    }

    [Fact]
    public void VersionedRoutes_RenderSeparateHomeAndSharedReleases()
    {
        RunWithDiagnostics(nameof(VersionedRoutes_RenderSeparateHomeAndSharedReleases), () =>
        {
            _fixture.NavigateTo("/");
            _fixture.WaitForElement(By.XPath("//*[contains(normalize-space(), 'version 1.6.2')]"));

            _fixture.Driver.Navigate().GoToUrl(_fixture.GetUri("/v2/home"));
            _fixture.WaitForElement(By.XPath("//*[contains(normalize-space(), 'version 2.1.1')]"));

            _fixture.Driver.Navigate().GoToUrl(_fixture.GetUri("/releases"));
            IWebElement heading = _fixture.WaitForElement(By.XPath("//*[normalize-space()='Releases']"));

            Assert.True(heading.Displayed);
            Assert.DoesNotContain("/v1", _fixture.Driver.Url, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("/v2", _fixture.Driver.Url, StringComparison.OrdinalIgnoreCase);
        });
    }

    [Fact]
    public void V1Demo_ButtonsRemainUsable()
    {
        RunWithDiagnostics(nameof(V1Demo_ButtonsRemainUsable), () =>
        {
            OpenDemo("/v1");
            ClickCoreDemoButtons();
        });
    }

    [Fact]
    public void V2Demo_ButtonsRemainUsable()
    {
        RunWithDiagnostics(nameof(V2Demo_ButtonsRemainUsable), () =>
        {
            OpenDemo("/v2");
            ClickCoreDemoButtons();
        });
    }

    [Fact]
    public void V2CropperJsApiExamples_RenderBlazorEquivalents()
    {
        RunWithDiagnostics(nameof(V2CropperJsApiExamples_RenderBlazorEquivalents), () =>
        {
            OpenDemo("/v2/examples/cropperjs-api");

            _fixture.WaitForElement(By.XPath("//*[normalize-space()='Cropper.js API in Blazor']"));
            _fixture.WaitForElement(By.XPath("//*[contains(normalize-space(), 'Configure Cropper.js internal elements as Blazor child components')]"));
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>("return document.querySelectorAll('cropper-canvas').length;") >= 11);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const root = document.querySelector('.cropper-js-api-elements-options');" +
                "const canvas = root?.querySelector('cropper-canvas');" +
                "const image = root?.querySelector('cropper-image');" +
                "const selection = root?.querySelector('cropper-selection');" +
                "const grid = root?.querySelector('cropper-grid');" +
                "const crosshair = root?.querySelector('cropper-crosshair');" +
                "const shade = root?.querySelector('cropper-shade');" +
                "const plainHandle = root?.querySelector('cropper-handle[plain]');" +
                "const moveHandle = root?.querySelector('cropper-handle[action=\"move\"]');" +
                "const viewer = root?.querySelector('[data-testid=\"cropper-js-api-viewer\"] cropper-viewer');" +
                "const resizeActions = Array.from(root?.querySelectorAll('cropper-handle[action$=\"-resize\"]') ?? []).map(handle => handle.getAttribute('action')).sort().join(',');" +
                "return !!root && canvas?.background === false" +
                " && image?.initialCenterSize === 'contain'" +
                " && image?.alt === 'Cropper.js API internal elements example'" +
                " && shade?.hidden === false" +
                " && selection?.dynamic === true" +
                " && selection?.keyboard === true" +
                " && selection?.outlined === true" +
                " && selection?.precise === true" +
                " && grid?.getAttribute('rows') === '4'" +
                " && grid?.getAttribute('columns') === '4'" +
                " && crosshair?.centered === true" +
                " && plainHandle?.getAttribute('action') === 'select'" +
                " && moveHandle?.getAttribute('action') === 'move'" +
                " && resizeActions === 'e-resize,n-resize,ne-resize,nw-resize,s-resize,se-resize,sw-resize,w-resize'" +
                " && !!viewer;"));

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('[data-testid=\"cropper-js-api-canvas-element\"] cropper-canvas');" +
                "const image = document.querySelector('[data-testid=\"cropper-js-api-image-element\"] cropper-image');" +
                "const shade = document.querySelector('[data-testid=\"cropper-js-api-shade-element\"] cropper-shade');" +
                "const handleRoot = document.querySelector('[data-testid=\"cropper-js-api-handle-element\"]');" +
                "const selection = document.querySelector('[data-testid=\"cropper-js-api-selection-element\"] cropper-selection');" +
                "const grid = document.querySelector('[data-testid=\"cropper-js-api-grid-element\"] cropper-grid');" +
                "const crosshair = document.querySelector('[data-testid=\"cropper-js-api-crosshair-element\"] cropper-crosshair');" +
                "const viewer = document.querySelector('[data-testid=\"cropper-js-api-dedicated-viewer\"] cropper-viewer');" +
                "const resizeActions = Array.from(handleRoot?.querySelectorAll('cropper-handle[action$=\"-resize\"]') ?? []).map(handle => handle.getAttribute('action')).sort().join(',');" +
                "return canvas?.background === false" +
                " && image?.initialCenterSize === 'cover'" +
                " && image?.alt === 'Cropper.js API image element example'" +
                " && !!shade" +
                " && handleRoot?.querySelector('cropper-handle[plain]')?.getAttribute('action') === 'select'" +
                " && handleRoot?.querySelector('cropper-handle[action=\"move\"]')?.getAttribute('action') === 'move'" +
                " && resizeActions === 'e-resize,n-resize,ne-resize,nw-resize,s-resize,se-resize,sw-resize,w-resize'" +
                " && selection?.multiple === true" +
                " && selection?.dynamic === true" +
                " && grid?.getAttribute('rows') === '4'" +
                " && grid?.getAttribute('columns') === '4'" +
                " && crosshair?.centered === true" +
                " && !!viewer;"));

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const code = document.body.innerText;" +
                "return code.includes('<CropperShade InputAttributes=\"shadeAttributes\" />')" +
                " && code.includes('<CropperGrid InputAttributes=\"gridAttributes\" />')" +
                " && code.includes('<CropperCrosshair InputAttributes=\"crosshairAttributes\" />')" +
                " && code.includes('GetSelectionsDataAsync()');"));

            ClickButtonByText("Create selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('.cropper-js-api-multiple-selections cropper-selection'));" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow'" +
                " && document.querySelector('[data-testid=\"cropper-js-api-selection-shapes\"]')?.textContent.includes('circle, pentagon, arrow');"));

            ClickButtonByText("Export selection to canvas data URL");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "return !!document.querySelector('img[alt=\"Cropped selection result\"]')?.src.startsWith('data:image/');"));
        });
    }

    [Fact]
    public void V1CropperJsApiExamples_RouteRendersBlazorExamples()
    {
        RunWithDiagnostics(nameof(V1CropperJsApiExamples_RouteRendersBlazorExamples), () =>
        {
            OpenDemo("/v1/examples/cropperjs-api");

            _fixture.WaitForElement(By.XPath("//*[normalize-space()='Cropper.js API in Blazor']"));
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "return !!document.querySelector('[data-testid=\"cropper-js-api-shade-element\"] cropper-shade')" +
                " && !!document.querySelector('[data-testid=\"cropper-js-api-grid-element\"] cropper-grid')" +
                " && !!document.querySelector('[data-testid=\"cropper-js-api-crosshair-element\"] cropper-crosshair');"));
        });
    }

    [Fact]
    public void V2CropperJsApiExamples_SelectionChildComponentEnablesMultipleSelections_WhenRootOptionIsDisabled()
    {
        RunWithDiagnostics(nameof(V2CropperJsApiExamples_SelectionChildComponentEnablesMultipleSelections_WhenRootOptionIsDisabled), () =>
        {
            OpenDemo("/v2/examples/cropperjs-api");

            _fixture.WaitForElement(By.XPath("//*[normalize-space()='cropper-selection multiple initialization']"));
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const root = document.querySelector('[data-testid=\"cropper-js-api-selection-multiple-init\"]');" +
                "const selection = root?.querySelector('cropper-selection');" +
                "return !!selection && selection.multiple === true && selection.getAttribute('multiple') !== null" +
                " && selection.dynamic === true && selection.movable === true && selection.resizable === true" +
                " && selection.zoomable === false && selection.keyboard === true && selection.outlined === true && selection.precise === true;"));

            ClickButtonByText("Create child-component selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('[data-testid=\"cropper-js-api-selection-multiple-init\"] cropper-selection'));" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow'" +
                " && document.querySelector('[data-testid=\"cropper-js-api-selection-multiple-init-count\"]')?.textContent.includes('3')" +
                " && document.querySelector('[data-testid=\"cropper-js-api-selection-multiple-init-shapes\"]')?.textContent.includes('circle, pentagon, arrow');"));
        });
    }

    [Fact]
    public void V2CropperJsApiExamples_SelectionChildComponentDisablesMultipleSelections_WhenRootOptionIsEnabled()
    {
        RunWithDiagnostics(nameof(V2CropperJsApiExamples_SelectionChildComponentDisablesMultipleSelections_WhenRootOptionIsEnabled), () =>
        {
            OpenDemo("/v2/examples/cropperjs-api");

            _fixture.WaitForElement(By.XPath("//*[normalize-space()='cropper-selection multiple disabled override']"));
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const root = document.querySelector('[data-testid=\"cropper-js-api-selection-multiple-disabled\"]');" +
                "const selection = root?.querySelector('cropper-selection');" +
                "return !!selection && selection.multiple === false && selection.getAttribute('multiple') === null" +
                " && selection.dynamic === true && selection.movable === true && selection.resizable === true" +
                " && selection.zoomable === false && selection.keyboard === true && selection.outlined === true && selection.precise === true;"));

            ClickButtonByText("Try create disabled multi selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('[data-testid=\"cropper-js-api-selection-multiple-disabled\"] cropper-selection'));" +
                "return selections.length === 1" +
                " && selections[0].multiple === false" +
                " && document.querySelector('[data-testid=\"cropper-js-api-selection-multiple-disabled-count\"]')?.textContent.includes('1');"));
        });
    }

    [Fact]
    public void V2CropperJsApiExamples_InitialMultipleSelections_RenderStyledBlazorElements()
    {
        RunWithDiagnostics(nameof(V2CropperJsApiExamples_InitialMultipleSelections_RenderStyledBlazorElements), () =>
        {
            OpenDemo("/v2/examples/cropperjs-api");

            _fixture.WaitForElement(By.XPath("//*[normalize-space()='Initial multiple cropper-selection elements']"));
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const root = document.querySelector('[data-testid=\"cropper-js-api-initial-multiple-selections\"]');" +
                "const selections = Array.from(root?.querySelectorAll('cropper-selection') ?? []);" +
                "const resizeHandles = Array.from(root?.querySelectorAll('cropper-handle[action$=\"-resize\"]') ?? []);" +
                "return selections.length >= 3" +
                " && selections.slice(0, 3).every(selection => selection.multiple === true && selection.keyboard === true && selection.outlined === true)" +
                " && selections.slice(0, 3).map(selection => selection.getAttribute('data-cropper-face')).join(',') === 'circle,polygon,arrow'" +
                " && selections.every(selection => !!selection.querySelector('cropper-grid') && !!selection.querySelector('cropper-crosshair') && !!selection.querySelector('cropper-handle[action=\"move\"]'))" +
                " && resizeHandles.length >= 24" +
                " && selections[0].classList.contains('initial-selection-circle')" +
                " && selections[1].classList.contains('initial-selection-polygon')" +
                " && selections[2].classList.contains('initial-selection-arrow');"));
        });
    }

    [Fact]
    public void V2Demo_AdvancedSelectionFigureControls_UpdateEachSelection()
    {
        RunWithDiagnostics(nameof(V2Demo_AdvancedSelectionFigureControls_UpdateEachSelection), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Start 3 styled selections");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('.img-container cropper-canvas');" +
                "const selections = Array.from(canvas?.querySelectorAll('cropper-selection') ?? []);" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow';"));

            ClickButtonByText("#1 Polygon");
            ClickButtonByText("#2 Arrow");
            ClickButtonByText("#3 Circle");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('.img-container cropper-canvas');" +
                "const selections = Array.from(canvas?.querySelectorAll('cropper-selection') ?? []);" +
                "const root = document.querySelector('.img-container');" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[1].getAttribute('data-cropper-face') === 'arrow'" +
                " && selections[2].getAttribute('data-cropper-face') === 'circle'" +
                " && getComputedStyle(root).getPropertyValue('--cropper-advanced-selection-line-width').trim() === '4px';"));
        });
    }

    [Fact]
    public void V2Demo_AdvancedSelectionControls_AddFiveVisibleSelectionsAndRemoveSelection()
    {
        RunWithDiagnostics(nameof(V2Demo_AdvancedSelectionControls_AddFiveVisibleSelectionsAndRemoveSelection), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            ClickButtonByText("Add 5 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('.img-container cropper-canvas');" +
                "const selections = Array.from(canvas?.querySelectorAll('cropper-selection') ?? []);" +
                "return selections.length >= 5" +
                " && selections.slice(0, 5).every(selection => !selection.hidden && Math.round(selection.width || 0) > 0 && Math.round(selection.height || 0) > 0);"));

            RemoveSelectionByIndex(1);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('.img-container cropper-canvas');" +
                "const selections = Array.from(canvas?.querySelectorAll('cropper-selection') ?? []);" +
                "return selections.length === 4" +
                " && selections.every(selection => !selection.hidden && Math.round(selection.width || 0) > 0 && Math.round(selection.height || 0) > 0);"));
        });
    }

    [Fact]
    public void V2Demo_AdvancedSelectionControls_RespectMinMaxAndKeepInactivePolygonsVisible()
    {
        RunWithDiagnostics(nameof(V2Demo_AdvancedSelectionControls_RespectMinMaxAndKeepInactivePolygonsVisible), () =>
        {
            OpenV2Demo();

            EnableAdvancedSelectionFigures();
            SetSettingsInput("maximum-selection-count", "3");
            ClickButtonByText("Add 5 styled selections");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('.img-container cropper-canvas');" +
                "const selections = Array.from(canvas?.querySelectorAll('cropper-selection') ?? []);" +
                "return selections.length === 3" +
                " && selections.every(selection => !selection.hidden && Math.round(selection.width || 0) > 0 && Math.round(selection.height || 0) > 0)" +
                " && selections.filter(selection => selection.getAttribute('data-cropper-face') === 'pentagon').every(selection => getComputedStyle(selection).clipPath !== 'none');"));

            SetSettingsInput("minimum-selection-count", "3");
            RemoveSelectionByIndex(0);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('.img-container cropper-canvas');" +
                "return (canvas?.querySelectorAll('cropper-selection')?.length ?? 0) === 3;"));
        });
    }

    [Fact]
    public void V2Demo_RemoveOnlySelection_ClearsSelectionWithoutConsoleErrors()
    {
        RunWithDiagnostics(nameof(V2Demo_RemoveOnlySelection_ClearsSelectionWithoutConsoleErrors), () =>
        {
            OpenV2Demo();

            RemoveSelectionByIndex(0);

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selection = document.querySelector('.img-container cropper-selection');" +
                "return !!selection && selection.hidden === true && Math.round(selection.width || 0) === 0;"));

            AssertNoBrowserConsoleErrorContaining("Cannot read");
            AssertNoBrowserConsoleErrorContaining("undefined");
        });
    }

    [Fact]
    public void V2Demo_CroppedImageQualityControls_AffectDialogImageAndBackgroundOutputMatches()
    {
        RunWithDiagnostics(nameof(V2Demo_CroppedImageQualityControls_AffectDialogImageAndBackgroundOutputMatches), () =>
        {
            OpenV2Demo();

            SetSettingsInput("cropped-image-quality", "1");
            SetSettingsInput("cropped-image-max-width", "320");
            SetSettingsInput("cropped-image-max-height", "180");
            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by URL");
            string urlImageSource = _page.Dialog.GetCroppedCanvasDialogImageSource();
            (long urlWidth, long urlHeight) = _page.Dialog.GetCroppedCanvasDialogImageSize();
            CloseDialog();

            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by element");
            string elementImageSource = _page.Dialog.GetCroppedCanvasDialogImageSource();
            (long elementWidth, long elementHeight) = _page.Dialog.GetCroppedCanvasDialogImageSize();
            CloseDialog();

            ClickAndWaitForCroppedCanvasDialog("Get Cropped Canvas by element In Background");
            string backgroundImageSource = _page.Dialog.GetCroppedCanvasDialogImageSource();
            (long backgroundWidth, long backgroundHeight) = _page.Dialog.GetCroppedCanvasDialogImageSize();
            CloseDialog();

            ClickAndWaitForCroppedCanvasDialog("Get Chunked Stream Image In Background");
            string chunkedImageSource = _page.Dialog.GetCroppedCanvasDialogImageSource();
            (long chunkedWidth, long chunkedHeight) = _page.Dialog.GetCroppedCanvasDialogImageSize();

            Assert.StartsWith("data:image/jpeg;base64,", urlImageSource, StringComparison.Ordinal);
            Assert.StartsWith("data:image/jpeg;base64,", elementImageSource, StringComparison.Ordinal);
            Assert.StartsWith("data:image/jpeg;base64,", backgroundImageSource, StringComparison.Ordinal);
            Assert.StartsWith("data:image/jpeg;base64,", chunkedImageSource, StringComparison.Ordinal);
            Assert.InRange(urlWidth, 319, 320);
            Assert.InRange(urlHeight, 179, 180);
            Assert.Equal(urlWidth, elementWidth);
            Assert.Equal(urlHeight, elementHeight);
            Assert.Equal(urlWidth, backgroundWidth);
            Assert.Equal(urlHeight, backgroundHeight);
            Assert.Equal(urlWidth, chunkedWidth);
            Assert.Equal(urlHeight, chunkedHeight);
            Assert.True(urlImageSource.Length > 128);
            Assert.True(elementImageSource.Length > 128);
            Assert.True(backgroundImageSource.Length > 128);
            Assert.True(chunkedImageSource.Length > 128);
        });
    }

    [Fact]
    public void V2Demo_CroppedImagePresetButtons_ProduceRequestedImageSize()
    {
        RunWithDiagnostics(nameof(V2Demo_CroppedImagePresetButtons_ProduceRequestedImageSize), () =>
        {
            OpenV2Demo();

            ClickButtonByText("320×180");
            _page.Dialog.WaitForCroppedCanvasDialog();
            (long width, long height) = _page.Dialog.GetCroppedCanvasDialogImageSize();

            Assert.InRange(width, 319, 320);
            Assert.InRange(height, 179, 180);
        });
    }

    [Fact]
    public void V2Demo_NavigationBetweenExamplesAndDemo_DisposesCropperResources()
    {
        RunWithDiagnostics(nameof(V2Demo_NavigationBetweenExamplesAndDemo_DisposesCropperResources), () =>
        {
            OpenDemo("/v2/examples/cropperjs-api");
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>("return document.querySelectorAll('cropper-canvas').length;") > 0);

            OpenV2Demo();
            _fixture.Wait.Until(_ => _fixture.ExecuteScript<long>("return document.querySelectorAll('cropper-canvas').length;") == 1);
            string demoImageSource = _fixture.ExecuteScript<string>("return document.querySelector('.img-container cropper-image')?.getAttribute('src') ?? '';");

            OpenDemo("/v2/examples/cropperjs-api");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvases = Array.from(document.querySelectorAll('cropper-canvas'));" +
                "const imageSources = Array.from(document.querySelectorAll('cropper-image')).map(image => image.getAttribute('src') ?? '');" +
                "return canvases.length > 0 && !imageSources.includes(arguments[0]);",
                demoImageSource));

            AssertNoBrowserConsoleErrorContaining("Cannot read");
            AssertNoBrowserConsoleErrorContaining("ObjectDisposedException");
            AssertNoBrowserConsoleErrorContaining("JSDisconnectedException");
        });
    }

    [Fact]
    public void V2Demo_PreviousMultiSelectionExamples_WorkProperly()
    {
        RunWithDiagnostics(nameof(V2Demo_PreviousMultiSelectionExamples_WorkProperly), () =>
        {
            OpenV2Demo();

            ClickButtonByText("Multi figures");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('cropper-selection'));" +
                "return selections.length >= 3" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow';"));

            ClickButtonByText("#1 Circle");
            ClickButtonByText("#2 Pentagon");
            ClickButtonByText("#3 Arrow");

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const selections = Array.from(document.querySelectorAll('cropper-selection'));" +
                "return selections.length >= 3" +
                " && selections.every(selection => selection.width > 0 && selection.height > 0)" +
                " && selections[0].getAttribute('data-cropper-face') === 'circle'" +
                " && selections[1].getAttribute('data-cropper-face') === 'pentagon'" +
                " && selections[2].getAttribute('data-cropper-face') === 'arrow';"));
        });
    }

    private void RunWithDiagnostics(string testName, Action test)
    {
        try
        {
            test();
            _fixture.AssertNoBrowserConsoleErrors();
        }
        catch
        {
            string artifactsDirectory = _fixture.CaptureDiagnostics(testName);
            _fixture.PauseForInspection($"Test '{testName}' failed. Diagnostics captured in: {artifactsDirectory}");
            throw;
        }
    }

    private void OpenV2Demo()
    {
        _page.OpenV2();
    }

    private void OpenDemo(string route)
    {
        _page.Open(route);
	}

	private void ClickCoreDemoButtons()
    {
        string[] buttonTitles =
        [
            "Move",
            "Crop",
            "Zoom In",
            "Zoom Out",
            "Move Left",
            "Move Right",
            "Move Up",
            "Move Down",
            "Rotate Left",
            "Rotate Right",
            "Flip Horizontal",
            "Flip Vertical",
            "Clear",
            "Disable",
            "Enable",
            "Destroy",
            "Reset"
        ];

        foreach (string title in buttonTitles)
        {
            ClickButtonByTitle(title);
        }
    }

    private void ClickButtonByTitle(string title)
    {
        _page.ClickButtonByTitle(title);
    }

    private void ClickActionButtonByTitle(string title)
    {
        _page.ClickActionButtonByTitle(title);
    }

    private void ClickButtonByText(string buttonText)
    {
        _page.ClickButtonByText(buttonText);
    }

    private void ClickButtonByExactText(string buttonText)
    {
        _page.ClickButtonByExactText(buttonText);
    }

    private void ClickTooltipButton(string tooltipText)
    {
        _page.ClickTooltipButton(tooltipText);
    }

    private void ClickCardButton(string cardTitle, string buttonText)
    {
        _page.ClickCardButton(cardTitle, buttonText);
    }

    private void ClickElement(IWebElement element)
    {
        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            _fixture.ExecuteScript<object>("arguments[0].click();", element);
        }
    }

    private void ScrollIntoView(IWebElement element)
    {
        _fixture.ExecuteScript<object>("arguments[0].scrollIntoView({ block: 'center', inline: 'center' });", element);
    }

    private void ScrollToCropper()
    {
        _page.Cropper.ScrollToCropper();
    }

    private void ScrollToCustomElementsPlayground()
    {
        _page.Cropper.ScrollToCustomElementsPlayground();
    }

    private void ClickSettingsSwitch(string testId)
    {
        _page.Settings.ClickSwitch(testId);
    }

    private void EnableAdvancedSelectionFigures()
    {
        if (AdvancedSelectionFiguresEnabled())
        {
            return;
        }

        ClickSettingsSwitch("v2-main-selection-multiple");
        WaitForAdvancedSelectionFiguresEnabled(true);
    }

    private bool AdvancedSelectionFiguresEnabled()
    {
        return _fixture.ExecuteScript<bool>(
            "return document.querySelector('[data-testid=\"advanced-selection-figures\"]')?.getAttribute('data-enabled') === 'true';");
    }

    private void WaitForAdvancedSelectionFiguresEnabled(bool enabled)
    {
        _fixture.Wait.Until(_ => AdvancedSelectionFiguresEnabled() == enabled);
    }

    private void AssertAdvancedSelectionControlDisabled(string testId, bool expectedDisabled)
    {
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
            "const field = document.querySelector(`[data-testid='${arguments[0]}']`);" +
            "const control = field?.matches('button,input') ? field : field?.querySelector('button,input');" +
            "return !!control && control.disabled === arguments[1];",
            testId,
            expectedDisabled));
    }

    private void SetSettingsInput(string testId, string value)
    {
        _page.Settings.SetInput(testId, value);
    }

    private void ClearSettingsInput(string testId)
    {
        _page.Settings.ClearInput(testId);
    }

    private void SetNullableSettingsInput(string testId, string? value)
    {
        if (value is null)
        {
            return;
        }
        else
        {
            SetSettingsInput(testId, value);
        }
    }

    private void SetColorSettingsInput(string testId, string value)
    {
        _page.Settings.SetColorInput(testId, value);
    }

    private void SetImageSmoothingQuality(string value)
    {
        string testId = $"cropped-image-smoothing-quality-{value.ToLowerInvariant()}-button";
        IWebElement button = _fixture.WaitForClickable(By.CssSelector($"[data-testid='{testId}']"));

        ScrollIntoView(button);
        ClickElement(button);
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<string>(
            "const marker = document.querySelector('[data-testid=\"cropped-image-smoothing-quality\"]');" +
            "return marker?.getAttribute('data-current-smoothing-quality') || '';").Contains(value, StringComparison.OrdinalIgnoreCase));
    }

    private void SetSelectionCardInput(int cardIndex, string testId, string value)
    {
        IWebElement input = _fixture.Wait.Until(driver =>
        {
            IReadOnlyCollection<IWebElement> cards = driver.FindElements(By.CssSelector("[data-testid='selection-card']"));

            if (cards.Count <= cardIndex)
            {
                return null;
            }

            IWebElement? field = cards.ElementAt(cardIndex).FindElements(By.CssSelector($"[data-testid='{testId}']")).FirstOrDefault();
            IWebElement? nestedInput = field?.TagName.Equals("input", StringComparison.OrdinalIgnoreCase) == true
                ? field
                : field?.FindElements(By.CssSelector("input")).FirstOrDefault();

            return nestedInput is not null && nestedInput.Displayed && nestedInput.Enabled ? nestedInput : null;
        })!;

        ScrollIntoView(input);
        _fixture.ExecuteScript<object>(
            "const input = arguments[0];" +
            "const value = arguments[1];" +
            "const setter = Object.getOwnPropertyDescriptor(HTMLInputElement.prototype, 'value').set;" +
            "setter.call(input, value);" +
            "input.dispatchEvent(new Event('input', { bubbles: true }));" +
            "input.dispatchEvent(new Event('change', { bubbles: true }));" +
            "input.dispatchEvent(new FocusEvent('blur', { bubbles: true }));",
            input,
            value);
        _fixture.Wait.Until(_ => input.GetAttribute("value") == value);
    }

    private string GetSettingsInputValue(string testId)
    {
        return _page.Settings.GetInputValue(testId);
    }

    private decimal GetSettingsInputDecimalValue(string testId)
    {
        string value = GetSettingsInputValue(testId);

        return decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out decimal result)
            ? result
            : 0;
    }

    private T GetCropperProperty<T>(string selector, string propertyName)
    {
        return _page.Cropper.GetProperty<T>(selector, propertyName);
    }

    private string GetCropperAttribute(string selector, string attributeName)
    {
        return _page.Cropper.GetAttribute(selector, attributeName);
    }


    private decimal GetImageZoomRatio()
    {
        return _page.Cropper.GetImageZoomRatio();
    }

    private void DispatchWheelZoom()
    {
        DispatchWheelZoom(onSelection: true, maxZoomRatio: "0.60");
    }

    private void DispatchWheelZoom(bool onSelection, string? maxZoomRatio)
    {
        _fixture.ExecuteScript<object>(
            "const canvas = document.querySelector('cropper-canvas');" +
            "const image = document.querySelector('cropper-image');" +
            "const selection = document.querySelector('cropper-selection');" +
            "if (!canvas || !image) return;" +
            "const oldRatio = Math.hypot(...image.$getTransform().slice(0, 2)) || 1;" +
            "const maxRatio = Number.parseFloat(arguments[1] ?? '10');" +
            "const nextRatio = Math.min(Number.isFinite(maxRatio) ? maxRatio : 10, oldRatio * 1.12);" +
            "const pivotSource = arguments[0] && selection ? selection : canvas;" +
            "const rect = pivotSource.getBoundingClientRect();" +
            "image.$zoom((nextRatio / oldRatio) - 1, rect.left + rect.width / 2, rect.top + rect.height / 2);" +
            "canvas.dispatchEvent(new CustomEvent('action', { bubbles: true, cancelable: true, detail: { action: 'scale', oldRatio, ratio: nextRatio } }));",
            onSelection,
            maxZoomRatio);
    }

    private void DispatchNativeSelectionDrag(decimal x, decimal y, decimal width, decimal height)
    {
        _fixture.ExecuteScript<object>(
            "const canvas = document.querySelector('cropper-canvas');" +
            "if (!canvas) return;" +
            "const rect = canvas.getBoundingClientRect();" +
            "const startX = rect.left + Number(arguments[0]);" +
            "const startY = rect.top + Number(arguments[1]);" +
            "const endX = startX + Number(arguments[2]);" +
            "const endY = startY + Number(arguments[3]);" +
            "canvas.dispatchEvent(new CustomEvent('action', { bubbles: true, cancelable: true, detail: { action: 'select', startX, startY, endX, endY, relatedEvent: new MouseEvent('pointermove', { bubbles: true, cancelable: true, clientX: endX, clientY: endY, pageX: endX, pageY: endY }) } }));",
            Convert.ToDouble(x),
            Convert.ToDouble(y),
            Convert.ToDouble(width),
            Convert.ToDouble(height));
    }

    private void DragSelectionOnCanvas(int x, int y, int width, int height)
    {
        DispatchNativeSelectionDrag(x, y, width, height);
    }

    private void DispatchLimitedNativeZoom(decimal ratio)
    {
        _fixture.ExecuteScript<object>(
            "const canvas = document.querySelector('cropper-canvas');" +
            "const image = document.querySelector('cropper-image');" +
            "if (!canvas || !image) return;" +
            "const rect = canvas.getBoundingClientRect();" +
            "const oldRatio = Math.hypot(...image.$getTransform().slice(0, 2)) || 1;" +
            "const requestedRatio = Number(arguments[0]);" +
            "const relatedEvent = new WheelEvent('wheel', { bubbles: true, cancelable: true, deltaY: -120, clientX: rect.left + rect.width / 2, clientY: rect.top + rect.height / 2, pageX: rect.left + rect.width / 2, pageY: rect.top + rect.height / 2, shiftKey: false });" +
            "canvas.dispatchEvent(new CustomEvent('action', { bubbles: true, cancelable: true, detail: { action: 'scale', oldRatio, ratio: requestedRatio, scale: (requestedRatio / oldRatio) - 1, relatedEvent } }));",
            Convert.ToDouble(ratio));
    }

    private void DispatchNativeWheelZoomOutsideSelection()
    {
        _fixture.ExecuteScript<object>(
            "const canvas = document.querySelector('cropper-canvas');" +
            "if (!canvas) return;" +
            "const rect = canvas.getBoundingClientRect();" +
            "canvas.dispatchEvent(new WheelEvent('wheel', { bubbles: true, cancelable: true, deltaY: -120, clientX: rect.left + 8, clientY: rect.top + 8, pageX: rect.left + 8, pageY: rect.top + 8 }));");
    }

    private string GetSelectionFaceStyle()
    {
        return _fixture.ExecuteScript<string>(
            "const selection = document.querySelector('cropper-selection');" +
            "if (!selection) return '';" +
            "const styles = getComputedStyle(selection);" +
            "return `${styles.clipPath};${styles.borderRadius}`;");
    }

    private string GetZoomRatioSettingValue(string testId)
    {
        return GetSettingsInputValue(testId);
    }

    private decimal GetZoomRatioSettingDecimalValue(string testId)
    {
        string value = GetZoomRatioSettingValue(testId);

        return decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out decimal result)
            ? result
            : decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out result)
            ? result
            : decimal.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out result)
            ? result
            : 0;
    }

    private decimal GetSelectionAspectRatio()
    {
        return _page.Cropper.GetSelectionAspectRatio();
    }

    private decimal GetCurrentAspectRatioSettingDecimalValue()
    {
        string value = GetSettingsInputValue("aspect-current-ratio");

        return decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.CurrentCulture, out decimal result)
            ? result
            : decimal.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out result)
            ? result
            : decimal.TryParse(value.Replace(',', '.'), System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out result)
            ? result
            : 0;
    }

    private void ChangeSelectionZone(decimal x, decimal y, decimal width, decimal height)
    {
        ChangeSelectionZone(x, y, width, height, dispatchActionEvents: false);
    }

    private void ChangeSelectionZone(decimal x, decimal y, decimal width, decimal height, bool dispatchActionEvents)
    {
        _fixture.ExecuteScript<object>(
            "const canvas = document.querySelector('cropper-canvas');" +
            "const selection = document.querySelector('cropper-selection');" +
            "if (!selection) return;" +
            "const rect = canvas?.getBoundingClientRect();" +
            "const relatedEvent = rect ? new MouseEvent('pointermove', { bubbles: true, cancelable: true, shiftKey: false, clientX: rect.left + Number(arguments[0]), clientY: rect.top + Number(arguments[1]) }) : null;" +
            "const actionDetail = { action: 'select', relatedEvent, startX: relatedEvent?.clientX ?? 0, startY: relatedEvent?.clientY ?? 0, endX: (relatedEvent?.clientX ?? 0) + Number(arguments[2]), endY: (relatedEvent?.clientY ?? 0) + Number(arguments[3]) };" +
            "if (arguments[4] && canvas) canvas.dispatchEvent(new CustomEvent('actionstart', { bubbles: true, cancelable: true, detail: actionDetail }));" +
            "selection.$change(arguments[0], arguments[1], arguments[2], arguments[3], selection.aspectRatio, true);" +
            "if (arguments[4] && canvas) canvas.dispatchEvent(new CustomEvent('actionmove', { bubbles: true, cancelable: true, detail: actionDetail }));" +
            "selection.dispatchEvent(new CustomEvent('change', { bubbles: true, cancelable: true, detail: { x: arguments[0], y: arguments[1], width: arguments[2], height: arguments[3] } }));" +
            "selection.dispatchEvent(new CustomEvent('change', { cancelable: true, detail: { x: arguments[0], y: arguments[1], width: arguments[2], height: arguments[3] } }));" +
            "if (arguments[4] && canvas) canvas.dispatchEvent(new CustomEvent('actionend', { bubbles: true, cancelable: true, detail: actionDetail }));",
            Convert.ToDouble(x),
            Convert.ToDouble(y),
            Convert.ToDouble(width),
            Convert.ToDouble(height),
            dispatchActionEvents);
    }

    private void WaitForSelectionDimension(string dimension, int expectedValue)
    {
        _page.Cropper.WaitForSelectionDimension(dimension, expectedValue);
    }

    private decimal GetSelectionDimension(string dimension)
    {
        return _page.Cropper.GetSelectionDimension(dimension);
    }

    private decimal GetSelectionWidth(int selectionIndex)
    {
        return Convert.ToDecimal(_fixture.ExecuteScript<object>(
            "const selection = document.querySelectorAll('.img-container cropper-selection')[arguments[0]];" +
            "return selection?.width ?? 0;",
            selectionIndex));
    }

    private decimal GetSelectionRatio(int selectionIndex)
    {
        return Convert.ToDecimal(_fixture.ExecuteScript<object>(
            "const selection = document.querySelectorAll('.img-container cropper-selection')[arguments[0]];" +
            "return selection && selection.height ? selection.width / selection.height : 0;",
            selectionIndex));
    }

    private void ActivateSelectionByIndex(int selectionIndex)
    {
        _fixture.ExecuteScript<object>(
            "const canvas = document.querySelector('cropper-canvas');" +
            "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
            "const selection = selections[arguments[0]];" +
            "if (!selection) return;" +
            "const detail = { action: 'select', x: selection.x, y: selection.y, width: selection.width, height: selection.height };" +
            "selections.forEach(currentSelection => currentSelection.active = currentSelection === selection);" +
            "canvas?.dispatchEvent(new CustomEvent('actionstart', { bubbles: true, cancelable: true, detail }));" +
            "selection.dispatchEvent(new CustomEvent('change', { bubbles: true, cancelable: true, detail: { x: selection.x, y: selection.y, width: selection.width, height: selection.height } }));",
            selectionIndex);
    }

    private string GetSelectionSelector(int selectionIndex)
    {
        return _fixture.ExecuteScript<string>(
            "const selection = document.querySelectorAll('.img-container cropper-selection')[arguments[0]];" +
            "return selection ? `#${selection.id}` : '';",
            selectionIndex);
    }

    private string GetLivePreviewSelectionSelector()
    {
        return _fixture.ExecuteScript<string>(
            "return document.querySelector('.img-preview')?.querySelector('cropper-viewer')?.getAttribute('selection') ?? '';");
    }

    private string GetLivePreviewSelectionBounds()
    {
        return _fixture.ExecuteScript<string>(
            "const viewer = document.querySelector('.img-preview')?.querySelector('cropper-viewer');" +
            "const selection = viewer?.$selection;" +
            "return selection ? `${Math.round(selection.x)},${Math.round(selection.y)},${Math.round(selection.width)},${Math.round(selection.height)}` : '';");
    }

    private string GetSelectionCardViewerPreviewData(string previewClass)
    {
        return _fixture.ExecuteScript<string>(
            "const previews = Array.from(document.querySelectorAll('[data-testid=\"selection-card-preview\"]'));" +
            "const preview = previews.find(currentPreview => currentPreview.querySelector('.selection-card-viewer')?.classList.contains(arguments[0]));" +
            "const selection = preview?.querySelector('cropper-viewer')?.$selection;" +
            "return selection ? `${Math.round(selection.x)},${Math.round(selection.y)},${Math.round(selection.width)},${Math.round(selection.height)}` : '';",
            previewClass);
    }

    private bool SelectionCardViewerCanvasesMatchSelections(int expectedCount)
    {
        return _fixture.ExecuteAsyncScript<bool>(
            "const done = arguments[arguments.length - 1];" +
            "const expectedCount = Number(arguments[0]);" +
            "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection')).slice(0, expectedCount);" +
            "const viewers = Array.from(document.querySelectorAll('[data-testid=\"selection-card-preview\"]')).slice(0, expectedCount).map(preview => preview.querySelector('cropper-viewer'));" +
            "Promise.all(viewers.map(async (viewer, index) => {" +
            "  const viewerSelection = viewer?.$selection;" +
            "  const selection = selections[index];" +
            "  if (!viewerSelection || !selection || viewerSelection !== selection) return false;" +
            "  const viewerCanvas = await viewerSelection.$toCanvas();" +
            "  const selectionCanvas = await selection.$toCanvas();" +
            "  return viewerCanvas.width === selectionCanvas.width" +
            "    && viewerCanvas.height === selectionCanvas.height" +
            "    && viewerCanvas.toDataURL() === selectionCanvas.toDataURL();" +
            "})).then(results => done(selections.length === expectedCount && viewers.length === expectedCount && results.every(Boolean))).catch(() => done(false));",
            expectedCount);
    }

    private bool SelectionCardViewerCanvasMatchesSelection(string previewClass, int expectedWidth, int expectedHeight)
    {
        return _fixture.ExecuteAsyncScript<bool>(
            "const done = arguments[arguments.length - 1];" +
            "const previewClass = arguments[0];" +
            "const expectedWidth = Number(arguments[1]);" +
            "const expectedHeight = Number(arguments[2]);" +
            "const preview = Array.from(document.querySelectorAll('[data-testid=\"selection-card-preview\"]')).find(currentPreview => currentPreview.querySelector('.selection-card-viewer')?.classList.contains(previewClass));" +
            "const viewer = preview?.querySelector('cropper-viewer');" +
            "const selection = viewer?.$selection;" +
            "const target = viewer?.getAttribute('selection') ? document.querySelector(viewer.getAttribute('selection')) : null;" +
            "if (!selection || !target || selection !== target) { done(false); return; }" +
            "Promise.all([selection.$toCanvas(), target.$toCanvas()])" +
            "  .then(([viewerCanvas, selectionCanvas]) => done(viewerCanvas.width === expectedWidth" +
            "    && viewerCanvas.height === expectedHeight" +
            "    && selectionCanvas.width === expectedWidth" +
            "    && selectionCanvas.height === expectedHeight" +
            "    && viewerCanvas.toDataURL() === selectionCanvas.toDataURL()))" +
            "  .catch(() => done(false));",
            previewClass,
            expectedWidth,
            expectedHeight);
    }

    private IReadOnlyCollection<object> GetImageTransform()
    {
        return _page.Cropper.GetImageTransform();
    }

    private void WaitForTransformChange(IReadOnlyCollection<object> previousTransform)
    {
        _page.Cropper.WaitForTransformChange(previousTransform);
    }

    private static bool TransformEquals(IReadOnlyCollection<object> left, IReadOnlyCollection<object> right)
    {
        decimal[] leftValues = left.Select(Convert.ToDecimal).ToArray();
        decimal[] rightValues = right.Select(Convert.ToDecimal).ToArray();

        return leftValues.Length == rightValues.Length
            && leftValues.Zip(rightValues).All(pair => Math.Abs(pair.First - pair.Second) <= 0.02m);
    }

    private static bool TransformTranslationIsStable(IReadOnlyCollection<object> left, IReadOnlyCollection<object> right, decimal tolerance)
    {
        decimal[] leftValues = left.Select(Convert.ToDecimal).ToArray();
        decimal[] rightValues = right.Select(Convert.ToDecimal).ToArray();

        return leftValues.Length >= 6
            && rightValues.Length >= 6
            && Math.Abs(leftValues[4] - rightValues[4]) <= tolerance
            && Math.Abs(leftValues[5] - rightValues[5]) <= tolerance;
    }

    private void WaitForSelectionAvailable()
    {
        _page.Cropper.WaitForSelectionAvailable();
    }

    private void RemoveSelectionByIndex(int selectionIndex)
    {
        _fixture.ExecuteScript<object>(
            "const selection = document.querySelectorAll('.img-container cropper-selection')[arguments[0]];" +
            "if (!selection) return;" +
            "const selections = Array.from(document.querySelectorAll('.img-container cropper-selection'));" +
            "if (selections.length === 1) { selection.$clear(); return; }" +
            "selection.remove();",
            selectionIndex);
    }

    private void WaitForPreviewImages()
    {
        _page.Cropper.WaitForPreviewImages();
    }

    private bool WaitForSelectionCleared()
    {
        _fixture.Wait.Until(_ =>
        {
            return _fixture.ExecuteScript<bool>(
                "const selection = document.querySelector('cropper-selection');" +
                "if (!selection) return false;" +
                "return selection.hidden === true || (Math.round(selection.width ?? 0) === 0 && Math.round(selection.height ?? 0) === 0);");
        });

        return true;
    }

    private bool WaitForSelectionVisible()
    {
        return _page.Cropper.WaitForSelectionVisible();
    }

    private void WaitForDataPreviewValue(string label)
    {
        _page.DataPreview.WaitForValue(label);
    }

    private bool WaitForDataPreviewValue(string label, string expectedValue)
    {
        return _page.DataPreview.WaitForValue(label, expectedValue);
    }

    private void WaitForDataPreviewValueChange(string label, string previousValue)
    {
        _page.DataPreview.WaitForValueChange(label, previousValue);
    }

    private string GetDataPreviewValue(string label)
    {
        return _page.DataPreview.GetValue(label);
    }

    private void WaitForCardInputValue(string cardTitle, string label)
    {
        _page.WaitForCardInputValue(cardTitle, label);
    }

    private string GetCardInputValue(string cardTitle, string label)
    {
        return _page.GetCardInputValue(cardTitle, label);
    }

    private void ClickAndWaitForCroppedCanvasDialog(string buttonText)
    {
        ClickButtonByText(buttonText);

        _page.Dialog.WaitForCroppedCanvasDialog();
    }

    private void ClickAndWaitForCroppedCanvasDialogByExactText(string buttonText)
    {
        ClickButtonByExactText(buttonText);

        _page.Dialog.WaitForCroppedCanvasDialog();
    }

    private void CloseDialog()
    {
        _page.Dialog.Close();
    }

    private void AssertNoBrowserConsoleErrorContaining(string text)
    {
        IReadOnlyCollection<LogEntry> logs = _fixture.Driver.Manage().Logs.GetLog(LogType.Browser);

        Assert.DoesNotContain(logs, log => log.Level == LogLevel.Severe && log.Message.Contains(text, StringComparison.OrdinalIgnoreCase));
    }
}
