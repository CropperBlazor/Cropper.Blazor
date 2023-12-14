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

        private const string PageURLs = "/api/CanvasData,/api/ContainerData,/api/CropBoxData,/api/CropEndEvent,/api/CropMoveEvent,/api/CropReadyEvent,/api/CropStartEvent,/api/CropperData,/api/DragMode,/api/GetCroppedCanvasOptions,/api/ImageData,/api/JSEventData,/api/Options,/api/SetCanvasDataOptions,/api/SetCropBoxDataOptions,/api/SetDataOptions,/api/ViewMode,/api/ZoomEvent";
    }
}
