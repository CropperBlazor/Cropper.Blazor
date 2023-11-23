using Cropper.Blazor.Client.Components.Docs;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Pages
{
    public partial class DataContract
    {
        [Parameter]
        public string Name { get; set; }

        public Type? ComponentType { get; set; }

        protected override void OnParametersSet()
        {
            ComponentType = ApiLink.GetTypeFromComponentLink(Name);
            StateHasChanged();
        }

        private const string PageURLs = "/contract/CanvasData,/contract/ContainerData,/contract/CropBoxData,/contract/CropEndEvent,/contract/CropMoveEvent,/contract/CropReadyEvent,/contract/CropStartEvent,/contract/CropperData,/contract/DragMode,/contract/GetCroppedCanvasOptions,/contract/ImageData,/contract/JSEventData,/contract/Options,/contract/SetCanvasDataOptions,/contract/SetCropBoxDataOptions,/contract/SetDataOptions,/contract/ViewMode,/contract/ZoomEvent";
    }
}
