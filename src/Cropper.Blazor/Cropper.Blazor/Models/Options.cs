using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using Cropper.Blazor.Extensions;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains cropper options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        public Options()
        {
            // Define the view mode of the cropper
            ViewMode = 0; // 0, 1, 2, 3

            // Define the dragging mode of the cropper
            DragMode =  Models.DragMode.Crop.ToEnumString()!; // 'crop', 'move' or 'none'

            // Define the initial aspect ratio of the crop box
            InitialAspectRatio = null;

            // Define the aspect ratio of the crop box
            AspectRatio = null;

            // An object with the previous cropping result data
            SetDataOptions = null;

            // A selector for adding extra containers to preview
            Preview = null;

            // Re-render the cropper when resize the window
            Responsive = true;

            // Restore the cropped area after resize the window
            Restore = true;

            // Check if the current image is a cross-origin image
            CheckCrossOrigin = true;

            // Check the current image's Exif Orientation information
            CheckOrientation = true;

            // Show the black modal
            Modal = true;

            // Show the dashed lines for guiding
            Guides = true;

            // Show the center indicator for guiding
            Center= true;

            // Show the white modal to highlight the crop box
            Highlight = true;

            // Show the grid background
            Background = true;

            // Enable to crop the image automatically when initialize
            AutoCrop = true;

            // Define the percentage of automatic cropping area when initializes
            AutoCropArea = 0.8m;

            // Enable to move the image
            Movable = true;

            // Enable to rotate the image
            Rotatable = true;

            // Enable to scale the image
            Scalable = true;

            // Enable to zoom the image
            Zoomable = true;

            // Enable to zoom the image by dragging touch
            ZoomOnTouch = true;

            // Enable to zoom the image by wheeling mouse
            ZoomOnWheel = true;

            // Define zoom ratio when zoom the image by wheeling mouse
            WheelZoomRatio = 0.1m;

            // Enable to move the crop box
            CropBoxMovable = true;

            // Enable to resize the crop box
            CropBoxResizable = true;

            // Toggle drag mode between "crop" and "move" when click twice on the cropper
            ToggleDragModeOnDblclick = true;

            // Size limitation
            MinCanvasWidth = 0;
            MinCanvasHeight = 0;
            MinCropBoxWidth = 0;
            MinCropBoxHeight = 0;
            MinContainerWidth = 200;
            MinContainerHeight = 100;
        }

        private object? preview = null;

        /// <summary>
        /// Define the fixed aspect ratio of the crop box.
        /// </summary>
        /// <remarks>
        /// By default, the crop box has a free ratio.
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("aspectRatio")]
        public decimal? AspectRatio { get; set; }

        /// <summary>
        /// Add extra elements (containers) for preview. 
        /// An element or an array of elements or a list <see cref="ElementReference"/> or a valid string selector for <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Document/querySelectorAll">Document.querySelectorAll</seealso>.
        /// </summary>
        /// <remarks>
        /// Default: null.
        /// <br/>
        /// Notes:
        /// <br/>
        /// The maximum width is the initial width of the preview container.
        /// <br/>
        /// The maximum height is the initial height of the preview container.
        /// <br/>
        /// If you set an aspectRatio option, be sure to set the same aspect ratio to the preview container.
        /// <br/>
        /// If the preview does not display correctly, set the overflow: hidden style to the preview container.
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("preview")]
        public object? Preview
        {
            get => preview;
            set
            {
                if (value is ElementReference elementReference)
                {
                    if (elementReference.Equals(default(ElementReference)))
                    {
                        throw new ArgumentException("'Preview' must not contain an empty Reference element.");
                    }

                    preview = value;
                }
                else if (value is string classValue)
                {
                    if (!string.IsNullOrWhiteSpace(classValue))
                    {
                        preview = value;
                    }
                    else
                    {
                        throw new ArgumentException("'Preview' should be not an empty or include white spaces in the string.");
                    }
                }
                else if (value is IEnumerable<ElementReference> listElementReferences)
                {
                    if (listElementReferences.Any(elementReference => elementReference.Equals(default(ElementReference))))
                    {
                        throw new ArgumentException("'Preview' must not contain an empty Reference element in the ElementReference collection.");
                    }

                    if (listElementReferences.Any())
                    {
                        preview = value;
                    }
                    else
                    {
                        throw new ArgumentException("'Preview' should be not an empty collection of ElementReference.");
                    }
                }
                else if (value is null)
                {
                    preview = value;
                }
                else
                {
                    throw new ArgumentException($"'Preview' is only available for string, ElementReference, IEnumerable<ElementReference> types, but found '{value!.GetType()}' type.");
                }
            }
        }

        /// <summary>
        /// Enable to crop the image automatically when initialized.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("autoCrop")]
        public bool? AutoCrop { get; set; }

        /// <summary>
        /// It should be a number between 0 and 1. Define the automatic cropping area size (percentage). 
        /// </summary>
        /// <remarks>
        /// Default: 0.8 (80% of the image)
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("autoCropArea")]
        public decimal? AutoCropArea { get; set; }

        /// <summary>
        /// Show the grid background of the container.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("background")]
        public bool? Background { get; set; }

        /// <summary>
        /// Show the center indicator above the crop box.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("center")]
        public bool? Center { get; set; }

        /// <summary>
        /// Check if the current image is a cross-origin image.
        /// For more information see official <seealso href="https://github.com/fengyuanchen/cropperjs#checkcrossorigin">crooper.js documentation</seealso>.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("checkCrossOrigin")]
        public bool? CheckCrossOrigin { get; set; }

        /// <summary>
        /// Check the current image's Exif Orientation information.
        /// </summary>
        /// <remarks>
        /// <b>Note that only a JPEG image may contain Exif Orientation information.</b>
        /// <br/>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("checkOrientation")]
        public bool? CheckOrientation { get; set; }

        /// <summary>
        /// Enable to move the crop box by dragging.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("cropBoxMovable")]
        public bool? CropBoxMovable { get; set; }

        /// <summary>
        /// Enable to resize the crop box by dragging.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("cropBoxResizable")]
        public bool? CropBoxResizable { get; set; }

        /// <summary>
        /// Change the cropped area position and size with new data (based on the original image).
        /// </summary>
        /// <remarks>
        ///  <para>
        ///  Note: This method only available when the value of the <b><i>viewMode</i></b> option is greater than or equal to <b><i>1</i></b>.
        ///  </para>
        ///  <para>
        ///  Default: null
        ///  </para>
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("data")]
        public SetDataOptions? SetDataOptions { get; set; } = null;

        /// <summary>
        /// Define the dragging mode of the cropper.
        /// </summary>
        /// <remarks>
        /// Default: 'crop'
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("dragMode")]
        [EnumDataType(typeof(DragMode))]
        public string DragMode { get; set; } = null!;

        /// <summary>
        /// Show the dashed lines above the crop box.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("guides")]
        public bool? Guides { get; set; }

        /// <summary>
        /// Show the white modal above the crop box (highlight the crop box).
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("highlight")]
        public bool? Highlight { get; set; }

        /// <summary>
        /// Define the initial aspect ratio of the crop box. By default, it is the same as the aspect ratio of the canvas (image wrapper).
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   Only available when the <b><i>aspectRatio</i></b> option is set to <b><i>NaN</i></b>.
        ///  </para>
        ///  <para>
        ///   Default: NaN
        ///  </para>
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("initialAspectRatio")]
        public decimal? InitialAspectRatio { get; set; }

        /// <summary>
        /// The minimum height of the canvas (image wrapper).
        /// </summary>
        /// <remarks>
        /// Default: 0
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCanvasHeight")]
        public decimal? MinCanvasHeight { get; set; }

        /// <summary>
        /// The minimum width of the canvas (image wrapper).
        /// </summary>
        /// <remarks>
        /// Default: 0
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCanvasWidth")]
        public decimal? MinCanvasWidth { get; set; }

        /// <summary>
        /// The minimum height of the container.
        /// </summary>
        /// <remarks>
        /// Default: 100
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minContainerHeight")]
        public decimal? MinContainerHeight { get; set; }

        /// <summary>
        /// The minimum width of the container.
        /// </summary>
        /// <remarks>
        /// Default: 200
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minContainerWidth")]
        public decimal? MinContainerWidth { get; set; }

        /// <summary>
        /// The minimum height of the crop box.
        /// <br/>
        /// This size is relative to the page, not the image.
        /// </summary>
        /// <remarks>
        /// Default: 0
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCropBoxHeight")]
        public decimal? MinCropBoxHeight { get; set; }

        /// <summary>
        /// The minimum width of the crop box.
        /// <br/>
        /// This size is relative to the page, not the image.
        /// Default: 0
        /// </summary>
        /// <remarks>
        /// Default: 0
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("minCropBoxWidth")]
        public decimal? MinCropBoxWidth { get; set; }

        /// <summary>
        /// Show the black modal above the image and under the crop box.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("modal")]
        public bool? Modal { get; set; }

        /// <summary>
        /// Enable to move the image.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("movable")]
        public bool? Movable { get; set; }

        /// <summary>
        /// Re-render the cropper when resizing the window.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("responsive")]
        public bool? Responsive { get; set; }

        /// <summary>
        /// Restore the cropped area after resizing the window.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("restore")]
        public bool? Restore { get; set; }

        /// <summary>
        /// Enable to rotate the image.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("rotatable")]
        public bool? Rotatable { get; set; }

        /// <summary>
        /// Enable to scale the image.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("scalable")]
        public bool? Scalable { get; set; }

        /// <summary>
        /// Enable to toggle drag mode between "crop" and "move" when clicking twice on the cropper.
        /// <br/>
        /// Requires <seealso href="https://developer.mozilla.org/en-US/docs/Web/API/Element/dblclick_event">dblclick</seealso> event support.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("toggleDragModeOnDblclick")]
        public bool? ToggleDragModeOnDblclick { get; set; }

        /// <summary>
        /// Define the view mode of the cropper.
        /// <br/>
        /// If you set viewMode to 0, the crop box can extend outside the canvas, while a value of 1, 2, or 3 will restrict the crop box to the size of the canvas.
        /// ViewMode of 2 or 3 will additionally restrict the canvas to the container.
        /// There is no difference between 2 and 3 when the proportions of the canvas and the container are the same.
        /// </summary>
        /// <remarks>
        /// Default: 0
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("viewMode")]
        public ViewMode? ViewMode { get; set; }

        /// <summary>
        /// Define zoom ratio when zooming the image by mouse wheeling.
        /// </summary>
        /// <remarks>
        /// Default: 0.1
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("wheelZoomRatio")]
        public decimal? WheelZoomRatio { get; set; }

        /// <summary>
        /// Enable to zoom the image by dragging touch.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomOnTouch")]
        public bool? ZoomOnTouch { get; set; }

        /// <summary>
        /// Enable to zoom the image by mouse wheeling.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomOnWheel")]
        public bool? ZoomOnWheel { get; set; }

        /// <summary>
        /// Enable to zoom the image.
        /// </summary>
        /// <remarks>
        /// Default: true
        /// </remarks>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomable")]
        public bool? Zoomable { get; set; }

        /// <summary>
        /// A Correlation ID is a unique identifier that is added to the very first interaction (incoming request)
        /// to identify the context and is passed to all components that are involved in the transaction flow
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [JsonPropertyName("correlationId")]
        public string? CorrelationId { get; set; } = "Cropper.Blazor";
    }
}
