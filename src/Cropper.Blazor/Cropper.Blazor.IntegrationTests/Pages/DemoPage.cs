using OpenQA.Selenium;

namespace Cropper.Blazor.IntegrationTests.Pages;

internal sealed class DemoPage
{
    private readonly SeleniumTestFixture _fixture;

    public DemoPage(SeleniumTestFixture fixture)
    {
        _fixture = fixture;
        Cropper = new CropperBlock(_fixture);
        Settings = new SettingsBlock(_fixture);
        DataPreview = new DataPreviewBlock(_fixture);
        Dialog = new DialogBlock(_fixture, Cropper);
    }

    public CropperBlock Cropper { get; }

    public SettingsBlock Settings { get; }

    public DataPreviewBlock DataPreview { get; }

    public DialogBlock Dialog { get; }

    public void Open(string route)
    {
        _fixture.NavigateTo(route);
        _fixture.Wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState")?.Equals("complete") == true);
        WaitForNetworkIdle();
        _fixture.WaitForStartupElement(By.CssSelector("cropper-canvas"));
        _fixture.WaitForStartupElement(By.CssSelector("cropper-selection"));
    }

    public void OpenV2()
    {
        Open("/v2");
    }

    public void ClickButtonByTitle(string title)
    {
        IWebElement button = _fixture.WaitForClickable(By.CssSelector($"button[title='{title}']"));
        Cropper.ScrollIntoView(button);
        Cropper.ClickElement(button);
    }

    public void ClickActionButtonByTitle(string title)
    {
        IWebElement button = _fixture.WaitForClickable(By.XPath($"//button[@title='{title}' and not(@aria-pressed)]"));
        Cropper.ScrollIntoView(button);
        Cropper.ClickElement(button);
    }

    public void ClickButtonByText(string buttonText)
    {
        IWebElement button = _fixture.WaitForClickable(By.XPath($"//button[contains(normalize-space(), '{buttonText}')]"));
        Cropper.ScrollIntoView(button);
        Cropper.ClickElement(button);
    }

    public void ClickButtonByExactText(string buttonText)
    {
        IWebElement button = _fixture.WaitForClickable(By.XPath($"//button[normalize-space()='{buttonText}']"));
        Cropper.ScrollIntoView(button);
        Cropper.ClickElement(button);
    }

    public void ClickTooltipButton(string tooltipText)
    {
        IWebElement button = GetFaceButton(tooltipText);

        Cropper.ScrollIntoView(button);
        Cropper.ClickElement(button);
    }

    public void ClickCardButton(string cardTitle, string buttonText)
    {
        IWebElement button = _fixture.WaitForClickable(By.XPath($"//*[contains(concat(' ', normalize-space(@class), ' '), ' mud-card ')][.//*[normalize-space()='{cardTitle}']]//button[normalize-space()='{buttonText}']"));
        Cropper.ScrollIntoView(button);
        Cropper.ClickElement(button);
    }

    public string GetCardInputValue(string cardTitle, string label)
    {
        return _fixture.ExecuteScript<string>(
            "const cards = Array.from(document.querySelectorAll('.mud-card'));" +
            "const card = cards.find(card => Array.from(card.querySelectorAll('.mud-card-header, .mud-typography')).some(x => x.textContent.trim() === arguments[0]));" +
            "const labels = Array.from(card?.querySelectorAll('label, .mud-input-label') ?? []);" +
            "const fieldLabel = labels.find(x => x.textContent.trim().startsWith(arguments[1]));" +
            "const root = fieldLabel?.closest('.mud-input-control');" +
            "return root?.querySelector('input')?.value ?? '';",
            cardTitle,
            label);
    }

    public void WaitForCardInputValue(string cardTitle, string label)
    {
        _fixture.Wait.Until(_ =>
        {
            string value = GetCardInputValue(cardTitle, label);
            return !string.IsNullOrWhiteSpace(value) && value != "0" && value != "0.0000";
        });
    }

    private IWebElement GetFaceButton(string tooltipText)
    {
        string testId = tooltipText switch
        {
            "Default (Rectangle)" => "cropper-face-default",
            "Close" => "cropper-face-close",
            "Pentagon" => "cropper-face-pentagon",
            "Circle" => "cropper-face-circle",
            "Arrow" => "cropper-face-arrow",
            _ => throw new ArgumentException($"Unsupported tooltip text: {tooltipText}", nameof(tooltipText))
        };

        return _fixture.WaitForClickable(By.CssSelector($"[data-testid='{testId}']"));
    }

    private void WaitForNetworkIdle()
    {
        _fixture.Wait.Until(driver =>
        {
            var js = (IJavaScriptExecutor)driver;

            var script = """
            const pendingFetches = window.__pendingFetches || 0;

            return pendingFetches === 0;
            """;

            return js.ExecuteScript(script) is true;
        });
    }
}
