using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Services
{
    public interface IMenuService
    {
        //Menu sections
        IEnumerable<DocsLink> DocsLinkExamples { get; }
        IEnumerable<DocsLink> DocsLinkApi { get; }
        IEnumerable<DocsLink> GetDocsLinkExamples(string routePrefix);
        IEnumerable<DocsLink> GetDocsLinkApi(string routePrefix);
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
            new() {Title = "Cropper.js API", Href = "examples/cropperjs-api"},
            new() {Title = "Rebuild cropper", Href = "examples/rebuild"}
        };

        public IEnumerable<DocsLink> DocsLinkApi => Api ??= new List<DocsLink>
        {
            new() {Title = "CropperComponent", Href = "api"},
            new() {Title = "CroppedCanvasReceiver", Href = "api/CroppedCanvasReceiver"},
            new() {Title = "ImageReceiver", Href = "api/ImageReceiver"},
            new() {Group = "Data", Title = "DragMode", Href = "api/DragMode"},
            new() {Group = "Data", Title = "CropperComponentType", Href = "api/CropperComponentType"},
            new() {Group = "Data", Title = "CropperData", Href = "api/CropperData"},
            new() {Group = "Data", Title = "ImageData", Href = "api/ImageData"},
            new() {Group = "Data", Title = "ContainerData", Href = "api/ContainerData"},
            new() {Group = "Data", Title = "CanvasData", Href = "api/CanvasData"},
            new() {Group = "Data", Title = "CropBoxData", Href = "api/CropBoxData"},
            new() {Group = "Data", Title = "JSEventData", Href = "api/JSEventData"},
            new() {Group = "Data", Title = "CroppedCanvas", Href = "api/CroppedCanvas"},
            new() {Group = "Data", Title = "ActionEvent", Href = "api/ActionEvent"},
            new() {Group = "Data", Title = "ImageSmoothingQuality", Href = "api/ImageSmoothingQuality"},
            new() {Group = "Options", Title = "Options", Href = "api/Options"},
            new() {Group = "Options", Title = "GetCroppedCanvasOptions", Href = "api/GetCroppedCanvasOptions"},
            new() {Group = "Options", Title = "SetCropBoxDataOptions", Href = "api/SetCropBoxDataOptions"},
            new() {Group = "Options", Title = "SetCanvasDataOptions", Href = "api/SetCanvasDataOptions"},
            new() {Group = "Options", Title = "SetDataOptions", Href = "api/SetDataOptions"},
            new() {Group = "Events", Title = "CropEndEvent", Href = "api/CropEndEvent"},
            new() {Group = "Events", Title = "CropMoveEvent", Href = "api/CropMoveEvent"},
            new() {Group = "Events", Title = "CropStartEvent", Href = "api/CropStartEvent"},
            new() {Group = "Events", Title = "CropReadyEvent", Href = "api/CropReadyEvent"},
            new() {Group = "Events", Title = "ZoomEvent", Href = "api/ZoomEvent"},
            new() {Group = "Events", Title = "CropEvent", Href = "api/CropEvent"},
            new() {Group = "Exceptions", Title = "ImageProcessingException", Href = "api/ImageProcessingException"},
            new() {Group = "Helpers", Title = "IUrlImageInterop", Href = "api/IUrlImageInterop"},
        };

        public IEnumerable<DocsLink> GetDocsLinkExamples(string routePrefix)
        {
            return DocsLinkExamples.Select(link => new DocsLink { Group = link.Group, Title = link.Title, Href = PrefixHref(routePrefix, link.Href) });
        }

        public IEnumerable<DocsLink> GetDocsLinkApi(string routePrefix)
        {
            return DocsLinkApi.Select(link => new DocsLink { Group = link.Group, Title = link.Title, Href = PrefixHref(routePrefix, link.Href) });
        }

        private static string PrefixHref(string routePrefix, string href)
        {
            string normalizedPrefix = routePrefix.TrimEnd('/');
            string normalizedHref = href.StartsWith('/') ? href : $"/{href}";

            if (normalizedHref.StartsWith("/v1", StringComparison.OrdinalIgnoreCase)
                || normalizedHref.StartsWith("/v2", StringComparison.OrdinalIgnoreCase)
                || normalizedHref.StartsWith("/releases", StringComparison.OrdinalIgnoreCase))
            {
                return normalizedHref.TrimStart('/');
            }

            return $"{normalizedPrefix}{normalizedHref}".TrimStart('/');
        }
    }
}
