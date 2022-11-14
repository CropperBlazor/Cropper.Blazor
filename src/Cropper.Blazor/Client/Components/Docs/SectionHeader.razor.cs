using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace Cropper.Blazor.Client.Components.Docs;

public partial class SectionHeader
{
    protected string Classname =>
        new CssBuilder("docs-section-header")
            .AddClass(Class)
            .Build();

    [Parameter] public string Class { get; set; }

    [Parameter] public string Title { get; set; }

    [Parameter] public bool HideTitle { get; set; }

    [Parameter] public RenderFragment SubTitle { get; set; }

    [Parameter] public RenderFragment Description { get; set; }
}
