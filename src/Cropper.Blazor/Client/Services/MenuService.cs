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
                new DocsLink {Title = "Crop Box Dimensions", Href = "examples/dimensions"},
                new DocsLink {Title = "View Modes", Href = "examples/viewmodes"},
                new DocsLink {Title = "Aspect Ratio", Href = "examples/aspectratio"},
                new DocsLink {Title = "Zooming", Href = "examples/zooming"}
            };


        public IEnumerable<DocsLink> Api => _api ??= new List<DocsLink>
            {
                new DocsLink {Title = "CropperComponent", Href = "api#eventcallbacks"},
                new DocsLink {Title = "Options", Href = "contract/Options"},
                new DocsLink {Title = "GetCroppedCanvasOptions", Href = "contract/GetCroppedCanvasOptions"},
                new DocsLink {Title = "SetCropBoxDataOptions", Href = "contract/SetCropBoxDataOptions"},
                new DocsLink {Title = "SetCanvasDataOptions", Href = "contract/SetCanvasDataOptions"},
                new DocsLink {Title = "SetDataOptions", Href = "contract/SetDataOptions"},
                new DocsLink {Title = "DragMode", Href = "contract/DragMode"},
                new DocsLink {Title = "CropEndEvent", Href = "contract/CropEndEvent"},
                new DocsLink {Title = "CropMoveEvent", Href = "contract/CropMoveEvent"},
                new DocsLink {Title = "CropStartEvent", Href = "contract/CropStartEvent"},
                new DocsLink {Title = "CropReadyEvent", Href = "contract/CropReadyEvent"},
                new DocsLink {Title = "ZoomEvent", Href = "contract/ZoomEvent"},
                new DocsLink {Title = "CropperData", Href = "contract/CropperData"},
                new DocsLink {Title = "ImageData", Href = "contract/ImageData"},
                new DocsLink {Title = "ContainerData", Href = "contract/ContainerData"},
                new DocsLink {Title = "CanvasData", Href = "contract/CanvasData"},
                new DocsLink {Title = "CropBoxData", Href = "contract/CropBoxData"},
                new DocsLink {Title = "JSEventData", Href = "contract/JSEventData"},
                new DocsLink {Title = "ViewMode", Href = "contract/ViewMode"}
            };
    }
}
