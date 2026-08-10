using OpenQA.Selenium;

namespace Cropper.Blazor.IntegrationTests.Pages;

internal sealed class DialogBlock
{
    private readonly SeleniumTestFixture _fixture;
    private readonly CropperBlock _cropper;

    public DialogBlock(SeleniumTestFixture fixture, CropperBlock cropper)
    {
        _fixture = fixture;
        _cropper = cropper;
    }

    public void WaitForCroppedCanvasDialog()
    {
        _fixture.WaitForElement(By.XPath("//*[contains(normalize-space(), 'Cropped canvas data')]"));
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("const src = document.querySelector('.cropped-canvas-dialog img')?.getAttribute('src') ?? ''; return src.startsWith('data:image') && src.length > 32;"));
    }

    public string GetCroppedCanvasDialogImageSource()
    {
        WaitForCroppedCanvasDialog();

        return _fixture.ExecuteScript<string>("return document.querySelector('.cropped-canvas-dialog img')?.getAttribute('src') ?? '';");
    }

    public (long Width, long Height) GetCroppedCanvasDialogImageSize()
    {
        WaitForCroppedCanvasDialog();

        long[] size = _fixture.ExecuteScript<IReadOnlyCollection<object>>(
            "const image = document.querySelector('.cropped-canvas-dialog img');" +
            "return [image?.naturalWidth ?? 0, image?.naturalHeight ?? 0];")
            .Select(Convert.ToInt64)
            .ToArray();

        return (size[0], size[1]);
    }

    public void Close()
    {
        long dialogCount = _fixture.ExecuteScript<long>("return document.querySelectorAll('.mud-dialog-container .cropped-canvas-dialog').length;");
        IWebElement closeButton = _fixture.WaitForClickable(By.CssSelector(".mud-dialog button.mud-button-close[aria-label='Close']"));

        _cropper.ScrollIntoView(closeButton);
        _cropper.ClickElement(closeButton);

        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
            "const count = document.querySelectorAll('.mud-dialog-container .cropped-canvas-dialog').length;" +
            "if (count < arguments[0]) return true;" +
            "const button = document.querySelector('.mud-dialog button.mud-button-close[aria-label=\"Close\"]');" +
            "if (button) button.dispatchEvent(new MouseEvent('click', { bubbles: true, cancelable: true, view: window }));" +
            "document.activeElement?.dispatchEvent(new KeyboardEvent('keydown', { key: 'Escape', code: 'Escape', bubbles: true }));" +
            "return document.querySelectorAll('.mud-dialog-container .cropped-canvas-dialog').length < arguments[0];",
            dialogCount));
    }
}
