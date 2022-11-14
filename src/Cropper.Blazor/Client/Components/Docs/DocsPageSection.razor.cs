using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components.Docs;

public partial class DocsPageSection
{
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UserAttributes { get; set; } = new Dictionary<string, object>();
    
    bool _renderImmediately = false;
}
