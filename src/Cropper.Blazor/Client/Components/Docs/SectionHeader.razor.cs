using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Utilities;

namespace Cropper.Blazor.Client.Components.Docs;

public partial class SectionHeader
{
    protected string Classname =>
        new CssBuilder("docs-section-header")
            .AddClass(Class)
            .Build();

    [CascadingParameter] private DocsPage DocsPage { get; set; }

    [CascadingParameter] private DocsPageSection Section { get; set; }

    [Parameter] public string Class { get; set; }

    [Parameter] public string Title { get; set; }
    [Parameter] public string TitleClass { get; set; }
    [Parameter] public string TitleId { get; set; }

    [Parameter] public bool HideTitle { get; set; }

    [Parameter] public RenderFragment SubTitle { get; set; }

    [Parameter] public RenderFragment Description { get; set; }

    public ElementReference SectionReference;

    public DocsSectionLink SectionInfo { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (DocsPage == null || string.IsNullOrWhiteSpace(Title))
        {
            return;
        }

        var parentTitle = DocsPage.GetParentTitle(Section) ?? string.Empty;
        if (string.IsNullOrEmpty(parentTitle) == false)
        {
            parentTitle += '-';
        }

        var id = (parentTitle + Title).Replace(" ", "-").ToLowerInvariant();

        SectionInfo = new DocsSectionLink { Id = id, Title = Title, };
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender == true && DocsPage != null && !string.IsNullOrWhiteSpace(Title))
        {
            DocsPage.AddSection(SectionInfo, Section);
        }
    }

    private string GetSectionId() => SectionInfo?.Id ?? Guid.NewGuid().ToString();

    private Typo GetTitleTypo()
    {
        if (Section.Level >= 1)
        {
            return Typo.h6;
        }
        else
        {
            return Typo.h5;
        }
    }
}
