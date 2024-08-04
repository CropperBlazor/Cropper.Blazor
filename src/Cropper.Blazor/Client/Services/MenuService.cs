using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Services
{
    public interface IMenuService
    {
        //Menu sections
        IEnumerable<DocsLink> DocsLinkExamples { get; }
        IEnumerable<DocsLink> DocsLinkApi { get; }
    }

    /// <summary>
    /// The aim of this class is to add new items to NavMenu
    /// </summary>
    public class MenuService : IMenuService
    {
        private IEnumerable<DocsLink>? Examples;
        private IEnumerable<DocsLink>? Api;

        /// <summary>
        /// Examples menu links
        /// </summary>
        public IEnumerable<DocsLink> DocsLinkExamples => Examples ??= new List<DocsLink>
        {
            new() {Title = "Basic Usage", Href = "examples/cropperusage"},
            new() {Title = "View Modes", Href = "examples/viewmodes"},
            new() {Title = "Preview", Href = "examples/preview"},
            new() {Title = "Crop Box Dimensions", Href = "examples/dimensions"},
            new() {Title = "Aspect Ratio", Href = "examples/aspectratio"},
            new() {Title = "Zooming", Href = "examples/zooming"},
            new() {Title = "Cropping", Href = "examples/cropping"},
            new() {Title = "Replacing", Href = "examples/replacing"},
            new() {Title = "Rebuild cropper", Href = "examples/rebuild"}
        };

        public IEnumerable<DocsLink> DocsLinkApi => Api ??= new List<DocsLink>
        {
            new() {Title = "CropperComponent", Href = "api"},
            new() {Title = "ViewMode", Href = "api/ViewMode"},
            new() {Title = "DragMode", Href = "api/DragMode"},
            new() {Title = "CropperComponentType", Href = "api/CropperComponentType"},
            new() {Group = "Options", Title = "Options", Href = "api/Options"},
            new() {Group = "Options", Title = "GetCroppedCanvasOptions", Href = "api/GetCroppedCanvasOptions"},
            new() {Group = "Options", Title = "SetCropBoxDataOptions", Href = "api/SetCropBoxDataOptions"},
            new() {Group = "Options", Title = "SetCanvasDataOptions", Href = "api/SetCanvasDataOptions"},
            new() {Group = "Options", Title = "SetDataOptions", Href = "api/SetDataOptions"},
            new() {Group = "Event", Title = "CropEndEvent", Href = "api/CropEndEvent"},
            new() {Group = "Event", Title = "CropMoveEvent", Href = "api/CropMoveEvent"},
            new() {Group = "Event", Title = "CropStartEvent", Href = "api/CropStartEvent"},
            new() {Group = "Event", Title = "CropReadyEvent", Href = "api/CropReadyEvent"},
            new() {Group = "Event", Title = "ZoomEvent", Href = "api/ZoomEvent"},
            new() {Group = "Data", Title = "CropperData", Href = "api/CropperData"},
            new() {Group = "Data", Title = "ImageData", Href = "api/ImageData"},
            new() {Group = "Data", Title = "ContainerData", Href = "api/ContainerData"},
            new() {Group = "Data", Title = "CanvasData", Href = "api/CanvasData"},
            new() {Group = "Data", Title = "CropBoxData", Href = "api/CropBoxData"},
            new() {Group = "Data", Title = "JSEventData", Href = "api/JSEventData"}
        };
    }
}
