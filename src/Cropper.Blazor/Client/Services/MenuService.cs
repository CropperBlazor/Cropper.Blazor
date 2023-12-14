using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Services
{
    public interface IMenuService
    {
        //Menu sections
        IEnumerable<DocsLink> Examples { get; }
        IEnumerable<DocsLink> Api { get; }
    }

    /// <summary>
    /// The aim of this class is to add new items to NavMenu
    /// </summary>
    public class MenuService : IMenuService
    {
        private IEnumerable<DocsLink> _examples;
        private IEnumerable<DocsLink> _api;

        /// <summary>
        /// Examples menu links
        /// </summary>
        public IEnumerable<DocsLink> Examples => _examples ??= new List<DocsLink>
        {
            new DocsLink {Title = "Basic Usage", Href = "examples/cropperusage"},
            new DocsLink {Title = "View Modes", Href = "examples/viewmodes"},
            new DocsLink {Title = "Preview", Href = "examples/preview"},
            new DocsLink {Title = "Crop Box Dimensions", Href = "examples/dimensions"},
            new DocsLink {Title = "Aspect Ratio", Href = "examples/aspectratio"},
            new DocsLink {Title = "Zooming", Href = "examples/zooming"},
            new DocsLink {Title = "Cropping", Href = "examples/cropping"},
            new DocsLink {Title = "Replacing", Href = "examples/replacing"},
            new DocsLink {Title = "Rebuild cropper", Href = "examples/rebuild"}
        };

        public IEnumerable<DocsLink> Api => _api ??= new List<DocsLink>
        {
            new DocsLink {Title = "CropperComponent", Href = "api"},
            new DocsLink {Title = "ViewMode", Href = "api/ViewMode"},
            new DocsLink {Title = "DragMode", Href = "api/DragMode"},
            new DocsLink {Group = "Options", Title = "Options", Href = "api/Options"},
            new DocsLink {Group = "Options", Title = "GetCroppedCanvasOptions", Href = "api/GetCroppedCanvasOptions"},
            new DocsLink {Group = "Options", Title = "SetCropBoxDataOptions", Href = "api/SetCropBoxDataOptions"},
            new DocsLink {Group = "Options", Title = "SetCanvasDataOptions", Href = "api/SetCanvasDataOptions"},
            new DocsLink {Group = "Options", Title = "SetDataOptions", Href = "api/SetDataOptions"},
            new DocsLink {Group = "Event", Title = "CropEndEvent", Href = "api/CropEndEvent"},
            new DocsLink {Group = "Event", Title = "CropMoveEvent", Href = "api/CropMoveEvent"},
            new DocsLink {Group = "Event", Title = "CropStartEvent", Href = "api/CropStartEvent"},
            new DocsLink {Group = "Event", Title = "CropReadyEvent", Href = "api/CropReadyEvent"},
            new DocsLink {Group = "Event", Title = "ZoomEvent", Href = "api/ZoomEvent"},
            new DocsLink {Group = "Data", Title = "CropperData", Href = "api/CropperData"},
            new DocsLink {Group = "Data", Title = "ImageData", Href = "api/ImageData"},
            new DocsLink {Group = "Data", Title = "ContainerData", Href = "api/ContainerData"},
            new DocsLink {Group = "Data", Title = "CanvasData", Href = "api/CanvasData"},
            new DocsLink {Group = "Data", Title = "CropBoxData", Href = "api/CropBoxData"},
            new DocsLink {Group = "Data", Title = "JSEventData", Href = "api/JSEventData"}
        };
    }
}
