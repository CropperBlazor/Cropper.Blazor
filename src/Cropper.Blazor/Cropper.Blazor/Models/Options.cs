using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of a Options
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Define the fixed aspect ratio of the crop box.
        /// By default, the crop box has a free ratio.
        /// Default: NaN
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("aspectRatio")]
        public decimal? AspectRatio { get; set; }

        /// <summary>
        /// Add extra elements (containers) for preview. 
        /// An element or an array of elements or a node list object or a valid selector for <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Document/querySelectorAll">Document.querySelectorAll</seealso>.
        /// Default: ''
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("preview")]
        public string Preview { get; set; } = string.Empty;

        /// <summary>
        /// Enable to crop the image automatically when initialized.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("autoCrop")]
        public bool? AutoCrop { get; set; }

        /// <summary>
        /// It should be a number between 0 and 1. Define the automatic cropping area size (percentage). 
        /// Default: 0.8 (80% of the image)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("autoCropArea")]
        public decimal? AutoCropArea { get; set; }

        /// <summary>
        /// Show the grid background of the container.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("background")]
        public bool? Background { get; set; }

        /// <summary>
        /// Show the center indicator above the crop box.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("center")]
        public bool? Center { get; set; }

        /// <summary>
        /// Check if the current image is a cross-origin image.
        /// If so, a crossOrigin attribute will be added to the cloned image element, and a timestamp parameter will be added to the src attribute to reload the source image to avoid browser cache error.
        /// Adding a crossOrigin attribute to the image element will stop adding a timestamp to the image URL and stop reloading the image. But the request (XMLHttpRequest) to read the image data for orientation checking will require a timestamp to bust the cache to avoid browser cache error.You can set the checkOrientation option to false to cancel this request.
        /// If the value of the image's crossOrigin attribute is "use-credentials", then the withCredentials attribute will set to true when read the image data by XMLHttpRequest.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("checkCrossOrigin")]
        public bool? CheckCrossOrigin { get; set; }

        /// <summary>
        /// Check the current image's Exif Orientation information.
        /// Note that only a JPEG image may contain Exif Orientation information. 
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("checkOrientation")]
        public bool? CheckOrientation { get; set; }

        /// <summary>
        /// Enable to move the crop box by dragging.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("cropBoxMovable")]
        public bool? CropBoxMovable { get; set; }

        /// <summary>
        /// Enable to resize the crop box by dragging.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("cropBoxResizable")]
        public bool? CropBoxResizable { get; set; }

        /// <summary>
        /// Change the cropped area position and size with new data (based on the original image).
        /// This option only available when the value of the viewMode option is greater than or equal to 1
        /// Only available when the autoCrop option had set to the true.
        /// Default: null
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("data")]
        public SetDataOptions? SetDataOptions { get; set; } = null;

        /// <summary>
        /// Define the dragging mode of the cropper.
        /// Default: 'crop'
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("dragMode")]
        [EnumDataType(typeof(DragMode))]
        public string DragMode { get; set; } = null!;

        /// <summary>
        /// Show the dashed lines above the crop box.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("guides")]
        public bool? Guides { get; set; }

        /// <summary>
        /// Show the white modal above the crop box (highlight the crop box).
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("highlight")]
        public bool? Highlight { get; set; }

        /// <summary>
        /// Define the initial aspect ratio of the crop box. By default, it is the same as the aspect ratio of the canvas (image wrapper).
        /// Only available when the aspectRatio option is set to NaN.
        /// Default: NaN
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("initialAspectRatio")]
        public decimal? InitialAspectRatio { get; set; }

        /// <summary>
        /// The minimum height of the canvas (image wrapper).
        /// Default: 0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCanvasHeight")]
        public decimal? MinCanvasHeight { get; set; }

        /// <summary>
        /// The minimum width of the canvas (image wrapper).
        /// Default: 0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCanvasWidth")]
        public decimal? MinCanvasWidth { get; set; }

        /// <summary>
        /// The minimum height of the container.
        /// Default: 100
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minContainerHeight")]
        public decimal? MinContainerHeight { get; set; }

        /// <summary>
        /// The minimum width of the container.
        /// Default: 200
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minContainerWidth")]
        public decimal? MinContainerWidth { get; set; }

        /// <summary>
        /// The minimum height of the crop box.
        /// This size is relative to the page, not the image.
        /// Default: 0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCropBoxHeight")]
        public decimal? MinCropBoxHeight { get; set; }

        /// <summary>
        /// The minimum width of the crop box.
        /// This size is relative to the page, not the image.
        /// Default: 0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCropBoxWidth")]
        public decimal? MinCropBoxWidth { get; set; }

        /// <summary>
        /// Show the black modal above the image and under the crop box.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("modal")]
        public bool? Modal { get; set; }

        /// <summary>
        /// Enable to move the image.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("movable")]
        public bool? Movable { get; set; }

        /// <summary>
        /// Re-render the cropper when resizing the window.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("responsive")]
        public bool? Responsive { get; set; }

        /// <summary>
        /// Restore the cropped area after resizing the window.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("restore")]
        public bool? Restore { get; set; }

        /// <summary>
        /// Enable to rotate the image.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("rotatable")]
        public bool? Rotatable { get; set; }

        /// <summary>
        /// Enable to scale the image.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("scalable")]
        public bool? Scalable { get; set; }

        /// <summary>
        /// Enable to toggle drag mode between "crop" and "move" when clicking twice on the cropper.
        /// Requires <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Element/dblclick_event">dblclick</seealso> event support.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("toggleDragModeOnDblclick")]
        public bool? ToggleDragModeOnDblclick { get; set; }

        /// <summary>
        /// Define the view mode of the cropper.
        /// If you set viewMode to 0, the crop box can extend outside the canvas, while a value of 1, 2, or 3 will restrict the crop box to the size of the canvas.
        /// ViewMode of 2 or 3 will additionally restrict the canvas to the container.
        /// There is no difference between 2 and 3 when the proportions of the canvas and the container are the same.
        /// Default: 0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("viewMode")]
        public ViewMode? ViewMode { get; set; }

        /// <summary>
        /// Define zoom ratio when zooming the image by mouse wheeling.
        /// Default: 0.1
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("wheelZoomRatio")]
        public decimal? WheelZoomRatio { get; set; }

        /// <summary>
        /// Enable to zoom the image by dragging touch.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomOnTouch")]
        public bool? ZoomOnTouch { get; set; }

        /// <summary>
        /// Enable to zoom the image by mouse wheeling.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomOnWheel")]
        public bool? ZoomOnWheel { get; set; }

        /// <summary>
        /// Enable to zoom the image.
        /// Default: true
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomable")]
        public bool? Zoomable { get; set; }
    }
}
