using Cropper.Blazor.Client.Components.Docs;
using Cropper.Blazor.Client.Pages.Replace.Examples;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Pages.Replace
{
    public partial class CropperReplacePage
    {
        private SectionContent ActiveReplaceActive = null!;

        private RenderFragment ReplaceActiveRenderFragment => builder =>
        {
            if (ActiveReplaceActive.ActiveCode == nameof(BasicReplaceImageWithNewSizeExample))
            {
                builder.OpenComponent<BasicReplaceImageWithNewSizeExample>(1);
                builder.CloseComponent();
            }
            else if (ActiveReplaceActive.ActiveCode == nameof(BasicReplaceImageWithSameSizeExample))
            {
                builder.OpenComponent<BasicReplaceImageWithSameSizeExample>(1);
                builder.CloseComponent();
            }
            else if (ActiveReplaceActive.ActiveCode == nameof(BasicInputReplaceImageWithNewSizeExample))
            {
                builder.OpenComponent<BasicInputReplaceImageWithNewSizeExample>(1);
                builder.CloseComponent();
            }
            else if (ActiveReplaceActive.ActiveCode == nameof(BasicInputReplaceImageWithSameSizeExample))
            {
                builder.OpenComponent<BasicInputReplaceImageWithSameSizeExample>(1);
                builder.CloseComponent();
            }
            else
            {
                throw new InvalidOperationException();
            }
        };
    }
}
