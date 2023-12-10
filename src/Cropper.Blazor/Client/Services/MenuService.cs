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
            new DocsLink {Title = "ViewMode", Href = "contract/ViewMode"},
            new DocsLink {Title = "DragMode", Href = "contract/DragMode"},
            new DocsLink {Group = "Options", Title = "Options", Href = "contract/Options"},
            new DocsLink {Group = "Options", Title = "GetCroppedCanvasOptions", Href = "contract/GetCroppedCanvasOptions"},
            new DocsLink {Group = "Options", Title = "SetCropBoxDataOptions", Href = "contract/SetCropBoxDataOptions"},
            new DocsLink {Group = "Options", Title = "SetCanvasDataOptions", Href = "contract/SetCanvasDataOptions"},
            new DocsLink {Group = "Options", Title = "SetDataOptions", Href = "contract/SetDataOptions"},
            new DocsLink {Group = "Event", Title = "CropEndEvent", Href = "contract/CropEndEvent"},
            new DocsLink {Group = "Event", Title = "CropMoveEvent", Href = "contract/CropMoveEvent"},
            new DocsLink {Group = "Event", Title = "CropStartEvent", Href = "contract/CropStartEvent"},
            new DocsLink {Group = "Event", Title = "CropReadyEvent", Href = "contract/CropReadyEvent"},
            new DocsLink {Group = "Event", Title = "ZoomEvent", Href = "contract/ZoomEvent"},
            new DocsLink {Group = "Data", Title = "CropperData", Href = "contract/CropperData"},
            new DocsLink {Group = "Data", Title = "ImageData", Href = "contract/ImageData"},
            new DocsLink {Group = "Data", Title = "ContainerData", Href = "contract/ContainerData"},
            new DocsLink {Group = "Data", Title = "CanvasData", Href = "contract/CanvasData"},
            new DocsLink {Group = "Data", Title = "CropBoxData", Href = "contract/CropBoxData"},
            new DocsLink {Group = "Data", Title = "JSEventData", Href = "contract/JSEventData"}
        };
    }
}
