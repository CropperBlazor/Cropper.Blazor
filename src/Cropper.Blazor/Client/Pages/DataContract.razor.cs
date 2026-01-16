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

        private bool HasName => !string.IsNullOrWhiteSpace(Name);

        protected override void OnParametersSet()
        {
            // RESET state derived from parameters
            IsContract = true;
            IsComponentContract = null;

            if (HasName)
            {
                ComponentType = ApiLink.GetTypeFromComponentLink(Name);
            }
            else
            {
                ComponentType = typeof(CropperComponent);
            }

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
            base.OnParametersSet();
        }
    }
}
