namespace Cropper.Blazor.IntegrationTests.Pages;

internal sealed class DataPreviewBlock
{
    private readonly SeleniumTestFixture _fixture;

    public DataPreviewBlock(SeleniumTestFixture fixture)
    {
        _fixture = fixture;
    }

    public void WaitForValue(string label)
    {
        _fixture.Wait.Until(_ => !string.IsNullOrWhiteSpace(GetValue(label)) && GetValue(label) != "0");
    }

    public bool WaitForValue(string label, string expectedValue)
    {
        _fixture.Wait.Until(_ => GetValue(label) == expectedValue);

        return true;
    }

    public void WaitForValueChange(string label, string previousValue)
    {
        _fixture.Wait.Until(_ =>
        {
            string currentValue = GetValue(label);
            return !string.IsNullOrWhiteSpace(currentValue) && currentValue != previousValue;
        });
    }

    public string GetValue(string label)
    {
        return _fixture.ExecuteScript<string>(
            "const labels = Array.from(document.querySelectorAll('.docs-data label, .docs-data .mud-input-label'));" +
            "const label = labels.find(x => x.textContent.trim().startsWith(arguments[0]));" +
            "const root = label?.closest('.mud-input-control');" +
            "return root?.querySelector('input')?.value ?? '';",
            label);
    }
}
