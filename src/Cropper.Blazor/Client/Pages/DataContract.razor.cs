using Cropper.Blazor.Client.Components.Docs;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Pages
{
    public partial class DataContract
    {
        [Parameter]
        public string Name { get; set; }

        public Type ComponentType { get; set; }

        protected override void OnParametersSet()
        {
            ComponentType = ApiLink.GetTypeFromComponentLink(Name);
            StateHasChanged();
        }
    }
}
