using OpenQA.Selenium;

namespace Cropper.Blazor.IntegrationTests.Pages;

internal sealed class SettingsBlock
{
    private readonly SeleniumTestFixture _fixture;
    private readonly CropperBlock _cropper;

    public SettingsBlock(SeleniumTestFixture fixture)
    {
        _fixture = fixture;
        _cropper = new CropperBlock(fixture);
    }

    public void ClickSwitch(string testId)
    {
        _fixture.Wait.Until(_ => _fixture.ExecuteScript<bool>(
            "const marker = document.querySelector(`[data-testid='${arguments[0]}']`);" +
            "return !!marker;",
            testId));

        _fixture.ExecuteScript<object>(
            "const marker = document.querySelector(`[data-testid='${arguments[0]}']`);" +
            "const root = marker.closest('.mud-switch') ?? marker;" +
            "root.scrollIntoView({ block: 'center', inline: 'center' });" +
            "const target = root.querySelector('input') ?? root.querySelector('button') ?? root;" +
            "target.click();",
            testId);
    }

    public void SetInput(string testId, string value)
    {
        IWebElement input = _fixture.WaitForClickable(By.CssSelector($"input[data-testid='{testId}']"));
        _cropper.ScrollIntoView(input);
        input.Click();
        input.SendKeys(Keys.Control + "a");
        input.SendKeys(Keys.Backspace);
        input.SendKeys(value);
        input.SendKeys(Keys.Tab);
        _fixture.Wait.Until(_ => GetInputValue(testId) == value);
    }

    public void ClearInput(string testId)
    {
        IWebElement input = _fixture.WaitForClickable(By.CssSelector($"input[data-testid='{testId}']"));
        _cropper.ScrollIntoView(input);
        input.Click();
        input.SendKeys(Keys.Control + "a");
        input.SendKeys(Keys.Backspace);
        input.SendKeys(Keys.Tab);
        _fixture.Wait.Until(_ => string.IsNullOrWhiteSpace(GetInputValue(testId)));
    }

    public void SetColorInput(string testId, string value)
    {
        IWebElement input = _fixture.WaitForClickable(By.CssSelector($"input[data-testid='{testId}']"));
        _cropper.ScrollIntoView(input);
        _fixture.ExecuteScript<object>(
            "arguments[0].value = arguments[1];" +
            "arguments[0].dispatchEvent(new Event('input', { bubbles: true }));" +
            "arguments[0].dispatchEvent(new Event('change', { bubbles: true }));",
            input,
            value);
        input.SendKeys(Keys.Tab);
    }

    public string GetInputValue(string testId)
    {
        return _fixture.ExecuteScript<string>(
            "return document.querySelector(`input[data-testid='${arguments[0]}']`)?.value ?? '';",
            testId);
    }
}
