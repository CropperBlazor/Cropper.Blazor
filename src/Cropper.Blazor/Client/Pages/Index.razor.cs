using Cropper.Blazor.Client.Components.Docs;
using Cropper.Blazor.Client.Pages.Home.Uses.Examples;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Pages
{
    public partial class Index
    {
        SectionContent ActivePreviewActive = null!;

        RenderFragment PreviewActiveRenderFragment => builder =>
        {
            if (ActivePreviewActive.ActiveCode == nameof(UsesPreviewFromStringSelectorComponentsExample))
            {
                builder.OpenComponent<UsesPreviewFromStringSelectorComponentsExample>(1);
                builder.CloseComponent();
            }
            else if (ActivePreviewActive.ActiveCode == nameof(UsesPreviewFromElementReferenceSelectorComponentsExample))
            {
                builder.OpenComponent<UsesPreviewFromElementReferenceSelectorComponentsExample>(1);
                builder.CloseComponent();
            }
            else if (ActivePreviewActive.ActiveCode == nameof(UsesPreviewFromMultipleElementReferenceSelectorComponentsExample))
            {
                builder.OpenComponent<UsesPreviewFromMultipleElementReferenceSelectorComponentsExample>(1);
                builder.CloseComponent();
            }
            else
            {
                throw new InvalidOperationException();
            }
        };
    }
}
