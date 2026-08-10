using OpenQA.Selenium;
using Xunit;

namespace Cropper.Blazor.IntegrationTests;

public sealed class DocsPageTests : IClassFixture<SeleniumTestFixture>
{
    private readonly SeleniumTestFixture _fixture;

    public DocsPageTests(SeleniumTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory]
    [ClassData(typeof(ExamplePageRoutes))]
    public void ExamplePage_RendersInteractiveContent(string route)
    {
        RunWithDiagnostics($"ExamplePage_{route.Replace('/', '_')}", () =>
        {
            _fixture.NavigateTo(route);

            _fixture.WaitForElement(By.CssSelector(".docs-page-content, .docs-content, main, .mud-main-content"));
            AssertNoBlazorError();

            IReadOnlyCollection<IWebElement> runnableExamples = _fixture.Driver.FindElements(By.CssSelector(".img-container, cropper-canvas, cropper-image, .docs-example, .docs-source-code"));
            Assert.NotEmpty(runnableExamples);

            IReadOnlyCollection<IWebElement> codeBlocks = _fixture.Driver.FindElements(By.CssSelector("pre, code, .docs-code"));
            Assert.NotEmpty(codeBlocks);
        });
    }

    [Theory]
    [InlineData("/v1/examples/preview#add-preview-option")]
    [InlineData("/v1/examples/dimensions#basic-dimensions-settings")]
    [InlineData("/v1/examples/zooming#zoomable")]
    [InlineData("/v1/examples/aspectratio#basic-setup")]
    public void V1ExamplePage_InitializesCropperElements(string route)
    {
        RunWithDiagnostics($"V1ExamplePage_InitializesCropperElements_{route.Replace('/', '_').Replace('#', '_')}", () =>
        {
            _fixture.NavigateTo(route);

            _fixture.WaitForElement(By.CssSelector("cropper-canvas"));
            AssertNoBlazorError();

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const canvas = document.querySelector('cropper-canvas');" +
                "const image = document.querySelector('cropper-image');" +
                "const selection = document.querySelector('cropper-selection');" +
                "return !!canvas && !!image && !!selection && canvas.clientHeight > 0 && image.getBoundingClientRect().height > 0 && selection.width > 0 && selection.height > 0;"));
        });
    }

    [Theory]
    [InlineData("Preview from String selector", "preview-example-string", 1)]
    [InlineData("Preview from ElementReference selector", "preview-example-element-reference", 1)]
    [InlineData("Preview from multiple ElementReference selector", "preview-example-multiple-element-reference", 2)]
    public void V1PreviewExample_RendersPreviewViewerForEachSelectorMode(string tabTitle, string previewTestId, int expectedPreviewCount)
    {
        RunWithDiagnostics($"V1PreviewExample_RendersPreviewViewerForEachSelectorMode_{previewTestId}", () =>
        {
            _fixture.NavigateTo("/v1/examples/preview#add-preview-option");

            IWebElement tab = _fixture.WaitForClickable(By.XPath($"//button[contains(normalize-space(), '{tabTitle}')]"));
            tab.Click();

            _fixture.WaitForElement(By.CssSelector($"[data-testid='{previewTestId}']"));
            AssertNoBlazorError();

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const previews = Array.from(document.querySelectorAll(`[data-testid='${arguments[0]}']`));" +
                "return previews.length === arguments[1] && previews.every(preview => {" +
                "const rect = preview.getBoundingClientRect();" +
                "const viewer = preview.querySelector('cropper-viewer');" +
                "const viewerRect = viewer?.getBoundingClientRect();" +
                "return rect.width > 0 && rect.height > 0 && preview.childElementCount > 0 && !!viewer && viewer.shadowRoot?.childElementCount > 0 && viewerRect.width > 0 && viewerRect.height > 0;" +
                "});",
                previewTestId,
                expectedPreviewCount));
        });
    }

    [Fact]
    public void V1DimensionsExample_ClampsCropBoxToMinMaxNaturalDimensions()
    {
        RunWithDiagnostics(nameof(V1DimensionsExample_ClampsCropBoxToMinMaxNaturalDimensions), () =>
        {
            _fixture.NavigateTo("/v1/examples/dimensions#min-and-max-dimensions");

            _fixture.WaitForElement(By.CssSelector("[data-testid='dimensions-min-max-section'] cropper-selection"));
            AssertNoBlazorError();

            _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
                "const section = document.querySelector('[data-testid=\"dimensions-min-max-section\"]');" +
                "const canvas = section?.querySelector('cropper-canvas');" +
                "const image = section?.querySelector('cropper-image');" +
                "const selection = section?.querySelector('cropper-selection');" +
                "return !!canvas && !!image && !!selection && canvas.clientHeight > 0 && image.getBoundingClientRect().height > 0 && selection.width >= 320 && selection.width <= 640 && selection.height >= 160 && selection.height <= 320;"));

            _fixture.ExecuteScript<object>(
                "const section = document.querySelector('[data-testid=\"dimensions-min-max-section\"]');" +
                "const selection = section?.querySelector('cropper-selection');" +
                "const canvas = section?.querySelector('cropper-canvas');" +
                "selection?.$change(selection.x, selection.y, 120, 80, selection.aspectRatio, true);" +
                "canvas?.dispatchEvent(new CustomEvent('change', { bubbles: true, cancelable: true, composed: true, detail: { x: selection.x, y: selection.y, width: 120, height: 80 } }));");
            WaitForDimensionsExampleSelectionSize(320, 160);

            _fixture.ExecuteScript<object>(
                "const section = document.querySelector('[data-testid=\"dimensions-min-max-section\"]');" +
                "const selection = section?.querySelector('cropper-selection');" +
                "const canvas = section?.querySelector('cropper-canvas');" +
                "selection?.$change(selection.x, selection.y, 900, 500, selection.aspectRatio, true);" +
                "canvas?.dispatchEvent(new CustomEvent('change', { bubbles: true, cancelable: true, composed: true, detail: { x: selection.x, y: selection.y, width: 900, height: 500 } }));");
            WaitForDimensionsExampleSelectionSize(480, 320);
        });
    }

    [Theory]
    [ClassData(typeof(ApiPageRoutes))]
    public void ApiPage_RendersContractMembers(string route, string expectedText)
    {
        RunWithDiagnostics($"ApiPage_{route.Replace('/', '_')}", () =>
        {
            _fixture.NavigateTo(route);

            _fixture.WaitForElement(By.XPath($"//*[contains(normalize-space(), '{expectedText}')]"));
            AssertNoBlazorError();

            IReadOnlyCollection<IWebElement> memberSections = _fixture.Driver.FindElements(By.XPath("//*[normalize-space()='Properties' or normalize-space()='Methods' or normalize-space()='EventCallbacks' or normalize-space()='Description']"));
            Assert.NotEmpty(memberSections);
        });
    }

    private void RunWithDiagnostics(string testName, Action test)
    {
        try
        {
            test();
        }
        catch
        {
            string artifactsDirectory = _fixture.CaptureDiagnostics(testName);
            _fixture.PauseForInspection($"Test '{testName}' failed. Diagnostics captured in: {artifactsDirectory}");
            throw;
        }
    }

    private void AssertNoBlazorError()
    {
        bool hasError = _fixture.ExecuteScript<bool>("return document.querySelector('#blazor-error-ui')?.style.display === 'block';");
        Assert.False(hasError);
    }

    private void WaitForDimensionsExampleSelectionSize(int expectedWidth, int expectedHeight)
    {
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
            "const section = document.querySelector('[data-testid=\"dimensions-min-max-section\"]');" +
            "const selection = section?.querySelector('cropper-selection');" +
            "const widthInput = section?.querySelector('[data-testid=\"dimensions-current-width\"]');" +
            "const heightInput = section?.querySelector('[data-testid=\"dimensions-current-height\"]');" +
            "const width = Math.round(selection?.width ?? 0);" +
            "const height = Math.round(selection?.height ?? 0);" +
            "return Math.abs(width - arguments[0]) <= 2 && Math.abs(height - arguments[1]) <= 2 && !!widthInput?.value && !!heightInput?.value;",
            expectedWidth,
            expectedHeight));
    }
}
