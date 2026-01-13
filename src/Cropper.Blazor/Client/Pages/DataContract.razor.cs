using Cropper.Blazor.Client.Components.Docs;
using Cropper.Blazor.Components;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Pages
{
    public partial class DataContract
    {
        [Parameter]
        public string Name { get; set; }

        public Type? ComponentType { get; set; }

        private bool IsContract = true;
        private bool? IsComponentContract = null;

        protected override void OnParametersSet()
        {
            ComponentType = ApiLink.GetTypeFromComponentLink(Name);

            if (ComponentType == typeof(CropperComponent))
            {
                IsContract = false;
                IsComponentContract = false;
            }
            else if (ComponentType == typeof(ImageReceiver) || ComponentType == typeof(CroppedCanvasReceiver))
            {
                IsContract = false;
                IsComponentContract = true;
            }

            StateHasChanged();
        }
    }
}
