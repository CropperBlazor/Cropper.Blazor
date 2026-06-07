using OpenQA.Selenium;

namespace Cropper.Blazor.IntegrationTests.Pages;

internal sealed class CropperBlock
{
    private readonly SeleniumTestFixture _fixture;

    public CropperBlock(SeleniumTestFixture fixture)
    {
        _fixture = fixture;
    }

    public void ScrollToCropper()
    {
        _fixture.ExecuteScript<object>("document.querySelector('cropper-canvas')?.scrollIntoView({ block: 'center', inline: 'center' });");
    }

    public void ScrollToCustomElementsPlayground()
    {
        _fixture.ExecuteScript<object>("document.querySelector('[data-testid=\"v2-custom-elements-panel\"]')?.scrollIntoView({ block: 'center', inline: 'center' });");
    }

    public void ScrollIntoView(IWebElement element)
    {
        _fixture.ExecuteScript<object>("arguments[0].scrollIntoView({ block: 'center', inline: 'center' });", element);
    }

    public void ClickElement(IWebElement element)
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

    public T GetProperty<T>(string selector, string propertyName)
    {
        return _fixture.ExecuteScript<T>(
            "const element = document.querySelector(arguments[0]); return element ? element[arguments[1]] : null;",
            selector,
            propertyName);
    }

    public string GetAttribute(string selector, string attributeName)
    {
        return _fixture.ExecuteScript<string>(
            "const element = document.querySelector(arguments[0]); return element?.getAttribute(arguments[1]) ?? '';",
            selector,
            attributeName);
    }

    public decimal GetImageZoomRatio()
    {
        object value = _fixture.ExecuteScript<object>(
            "const image = document.querySelector('cropper-image');" +
            "if (!image) return 0;" +
            "const data = image.$getTransform();" +
            "return Math.abs(data[0] || 0);");

        return Convert.ToDecimal(value);
    }

    public decimal GetSelectionAspectRatio()
    {
        object value = _fixture.ExecuteScript<object>(
            "const selection = document.querySelector('cropper-selection');" +
            "if (!selection || !selection.height) return 0;" +
            "return selection.width / selection.height;");

        return Convert.ToDecimal(value);
    }

    public decimal GetSelectionDimension(string dimension)
    {
        object value = _fixture.ExecuteScript<object>(
            "const selections = Array.from(document.querySelectorAll('cropper-selection'));" +
            "const selection = selections.find(item => item.active) ?? selections[0];" +
            "if (!selection) return 0;" +
            "return arguments[0] === 'width' ? selection.width : selection.height;",
            dimension);

        return Convert.ToDecimal(value);
    }

    public IReadOnlyCollection<object> GetImageTransform()
    {
        return _fixture.ExecuteScript<IReadOnlyCollection<object>>(
            "return Array.from(document.querySelector('cropper-image').$getTransform());");
    }

    public void WaitForSelectionDimension(string dimension, int expectedValue)
    {
        _fixture.Wait.Until(_ =>
        {
            decimal value = GetSelectionDimension(dimension);
            return Math.Abs(value - expectedValue) <= 2;
        });
    }

    public void WaitForTransformChange(IReadOnlyCollection<object> previousTransform)
    {
        string previous = string.Join(",", previousTransform);

        _fixture.Wait.Until(_ =>
        {
            IReadOnlyCollection<object> current = GetImageTransform();
            return string.Join(",", current) != previous;
        });
    }

    public void WaitForSelectionAvailable()
    {
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>("const selection = document.querySelector('cropper-selection'); return !!selection && selection.isConnected;"));
    }

    public bool WaitForSelectionVisible()
    {
        _fixture.Wait.Until(_ =>
        {
            return _fixture.ExecuteScript<bool>(
                "const selection = document.querySelector('cropper-selection');" +
                "if (!selection) return false;" +
                "return selection.hidden !== true && Math.round(selection.width ?? 0) > 0 && Math.round(selection.height ?? 0) > 0;");
        });

        return true;
    }

    public void WaitForPreviewImages()
    {
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
            "const selectors = ['.preview-lg', '.preview-md', '.preview-sm', '.preview-xs'];" +
            "return selectors.every(selector => {" +
            "const preview = document.querySelector(`.img-preview${selector}`);" +
            "const viewer = preview?.querySelector('cropper-viewer');" +
            "return !!viewer && preview.clientWidth > 0 && preview.clientHeight > 0 && viewer.clientWidth > 0 && viewer.clientHeight > 0;" +
            "});"));
    }
}
