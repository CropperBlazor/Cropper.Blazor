using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components.Docs;

public partial class DocsPageSection
{
    [CascadingParameter] public DocsPageSection? ParentSection { get; protected set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UserAttributes { get; set; } = new Dictionary<string, object>();

    public int Level { get; private set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Level = (ParentSection?.Level ?? -1) + 1;
    }
}
