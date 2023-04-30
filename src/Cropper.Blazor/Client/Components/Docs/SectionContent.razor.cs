using System.Text.RegularExpressions;
using Cropper.Blazor.Client.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;

namespace Cropper.Blazor.Client.Components.Docs;

public partial class SectionContent
{
    [Inject] protected IJsApiService? JsApiService { get; set; }

    protected string Classname =>
        new CssBuilder("docs-section-content")
            .AddClass($"outlined", Outlined && ChildContent != null)
            .AddClass($"darken", DarkenBackground)
            .AddClass("show-code", HasCode && ShowCode)
            .AddClass(Class)
            .Build();
    protected string ToolbarClassname =>
        new CssBuilder("docs-section-content-toolbar")
            .AddClass($"outlined", Outlined && ChildContent != null)
            .AddClass("darken", ChildContent == null && Codes != null)
            .Build();

    protected string InnerClassname =>
        new CssBuilder("docs-section-content-inner")
            .AddClass($"relative d-flex flex-grow-1 flex-wrap justify-center align-center", !Block)
            .AddClass($"d-block mx-auto", Block)
            .AddClass($"mud-width-full", Block && FullWidth)
            .AddClass("pa-8", !HasCode)
            .AddClass("px-8 pb-8 pt-2", HasCode)
            .Build();

    protected string SourceClassname =>
        new CssBuilder("docs-section-source")
            .AddClass($"outlined", Outlined && ChildContent != null)
            .AddClass("show-code", HasCode && ShowCode)
            .Build();

    [Parameter] public string Class { get; set; }
    [Parameter] public bool DarkenBackground { get; set; }
    [Parameter] public bool Outlined { get; set; } = true;
    [Parameter] public bool ShowCode { get; set; } = true;
    [Parameter] public bool Block { get; set; }
    [Parameter] public bool FullWidth { get; set; }
    [Parameter] public string Code { get; set; }
    [Parameter] public string HighLight { get; set; }
    [Parameter] public IEnumerable<CodeFile> Codes { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    private bool HasCode;
    private string ActiveCode;

    protected override void OnParametersSet()
    {
        if (Codes != null)
        {
            HasCode = true;
            ActiveCode = Codes.FirstOrDefault().code;
        }
        else if (!String.IsNullOrWhiteSpace(Code))
        {
            HasCode = true;
            ActiveCode = Code;
        }
    }

    public void OnShowCode()
    {
        ShowCode = !ShowCode;
    }

    public void SetActiveCode(string value)
    {
        ActiveCode = value;
    }

    private string GetActiveCode(string value)
    {
        if (value == ActiveCode)
        {
            return "file-button active";
        }
        else
        {
            return "file-button";
        }
    }

    private async Task CopyTextToClipboard()
    {
        await JsApiService.CopyToClipboardAsync(Snippets.GetCode(string.IsNullOrWhiteSpace(Code) ? ActiveCode : Code));
    }

    RenderFragment CodeComponent(string code) => builder =>
    {
        try
        {
            var key = typeof(SectionContent).Assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains($".{code}Code.html"));
            using (var stream = typeof(SectionContent).Assembly.GetManifestResourceStream(key))
            using (var reader = new StreamReader(stream))
            {
                var read = reader.ReadToEnd();

                if (!string.IsNullOrEmpty(HighLight))
                {
                    if (HighLight.Contains(","))
                    {
                        var highlights = HighLight.Split(",");

                        foreach (var value in highlights)
                        {
                            read = Regex.Replace(read, $"{value}(?=\\s|\")", $"<mark>$&</mark>");
                        }
                    }
                    else
                    {
                        read = Regex.Replace(read, $"{HighLight}(?=\\s|\")", $"<mark>$&</mark>");
                    }
                }

                builder.AddMarkupContent(0, read);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    };
}
