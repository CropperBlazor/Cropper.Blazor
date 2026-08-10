using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains options for the Cropper.js v2 <c>cropper-canvas</c> element.
    /// </summary>
    public class CanvasElementOptions
    {
        /// <summary>
        /// Hide the canvas element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Show the canvas background.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("background")]
        public bool? Background { get; set; }

        /// <summary>
        /// Disable canvas interactions.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("disabled")]
        public bool? Disabled { get; set; }

        /// <summary>
        /// Set the canvas scale step.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("scaleStep")]
        public decimal? ScaleStep { get; set; }

        /// <summary>
        /// Set the canvas theme color.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("themeColor")]
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Enable the default slot for the canvas element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for the Cropper.js v2 <c>cropper-image</c> element.
    /// </summary>
    public class ImageElementOptions
    {
        /// <summary>
        /// Hide the image element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Enable image rotation.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("rotatable")]
        public bool? Rotatable { get; set; }

        /// <summary>
        /// Enable image scaling.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("scalable")]
        public bool? Scalable { get; set; }

        /// <summary>
        /// Enable image skewing.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("skewable")]
        public bool? Skewable { get; set; }

        /// <summary>
        /// Enable image translation.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("translatable")]
        public bool? Translatable { get; set; }

        /// <summary>
        /// Set how the image is initially centered in the canvas.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("initialCenterSize")]
        public CropperImageInitialCenterSize? InitialCenterSize { get; set; }

        /// <summary>
        /// Set the image alternative text.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("alt")]
        public string? Alt { get; set; }

        /// <summary>
        /// Enable the default slot for the image element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for the Cropper.js v2 <c>cropper-shade</c> element.
    /// </summary>
    public class ShadeElementOptions
    {
        /// <summary>
        /// Hide the shade element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Set the shade theme color.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("themeColor")]
        public string? ThemeColor { get; set; }

        /// <summary>
        /// The x-axis coordinate of the shade.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("x")]
        public decimal? X { get; set; }

        /// <summary>
        /// The y-axis coordinate of the shade.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("y")]
        public decimal? Y { get; set; }

        /// <summary>
        /// The shade width.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("width")]
        public decimal? Width { get; set; }

        /// <summary>
        /// The shade height.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("height")]
        public decimal? Height { get; set; }

        /// <summary>
        /// Enable the default slot for the shade element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for a Cropper.js v2 <c>cropper-handle</c> element.
    /// </summary>
    public class HandleElementOptions
    {
        /// <summary>
        /// Hide the handle element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Set the Cropper.js action performed by the handle.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("action")]
        public CropperAction? Action { get; set; }

        /// <summary>
        /// Render the handle as a plain handle.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("plain")]
        public bool? Plain { get; set; }

        /// <summary>
        /// Set the handle theme color.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("themeColor")]
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Enable the default slot for the handle element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for the Cropper.js v2 <c>cropper-selection</c> element.
    /// </summary>
    public class SelectionElementOptions
    {
        /// <summary>
        /// Hide the selection element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// The x-axis coordinate of the selection.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("x")]
        public decimal? X { get; set; }

        /// <summary>
        /// The y-axis coordinate of the selection.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("y")]
        public decimal? Y { get; set; }

        /// <summary>
        /// The selection width.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("width")]
        public decimal? Width { get; set; }

        /// <summary>
        /// The selection height.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("height")]
        public decimal? Height { get; set; }

        /// <summary>
        /// The selection aspect ratio.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("aspectRatio")]
        public decimal? AspectRatio { get; set; }

        /// <summary>
        /// The initial selection aspect ratio.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("initialAspectRatio")]
        public decimal? InitialAspectRatio { get; set; }

        /// <summary>
        /// The initial selection coverage.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("initialCoverage")]
        public decimal? InitialCoverage { get; set; }

        /// <summary>
        /// Enable dynamic selection updates.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("dynamic")]
        public bool? Dynamic { get; set; }

        /// <summary>
        /// Enable selection movement.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("movable")]
        public bool? Movable { get; set; }

        /// <summary>
        /// Enable selection resizing.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("resizable")]
        public bool? Resizable { get; set; }

        /// <summary>
        /// Enable zooming from the selection.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("zoomable")]
        public bool? Zoomable { get; set; }

        /// <summary>
        /// Enable multiple selections.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("multiple")]
        public bool? Multiple { get; set; }

        /// <summary>
        /// Enable keyboard interactions for the selection.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("keyboard")]
        public bool? Keyboard { get; set; }

        /// <summary>
        /// Show the selection outline.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("outlined")]
        public bool? Outlined { get; set; }

        /// <summary>
        /// Enable precise selection changes.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("precise")]
        public bool? Precise { get; set; }

        /// <summary>
        /// Enable the default slot for the selection element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for the Cropper.js v2 <c>cropper-grid</c> element.
    /// </summary>
    public class GridElementOptions
    {
        /// <summary>
        /// Hide the grid element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Set the grid row count.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("rows")]
        public decimal? Rows { get; set; }

        /// <summary>
        /// Set the grid column count.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("columns")]
        public decimal? Columns { get; set; }

        /// <summary>
        /// Show grid borders.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("bordered")]
        public bool? Bordered { get; set; }

        /// <summary>
        /// Cover the grid element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("covered")]
        public bool? Covered { get; set; }

        /// <summary>
        /// Set the grid theme color.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("themeColor")]
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Enable the default slot for the grid element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for the Cropper.js v2 <c>cropper-crosshair</c> element.
    /// </summary>
    public class CrosshairElementOptions
    {
        /// <summary>
        /// Hide the crosshair element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Center the crosshair element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("centered")]
        public bool? Centered { get; set; }

        /// <summary>
        /// Set the crosshair theme color.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("themeColor")]
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Enable the default slot for the crosshair element.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }

    /// <summary>
    /// Contains options for the Cropper.js v2 resize handle elements.
    /// </summary>
    public class ResizeHandleElementOptions
    {
        /// <summary>
        /// Hide the resize handle elements.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("hidden")]
        public bool? Hidden { get; set; }

        /// <summary>
        /// Set the resize handle theme color.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("themeColor")]
        public string? ThemeColor { get; set; }

        /// <summary>
        /// Enable the default slot for the resize handle elements.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("slottable")]
        public bool? Slottable { get; set; }
    }
}
