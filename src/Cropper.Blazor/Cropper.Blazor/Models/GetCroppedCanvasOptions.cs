using System.ComponentModel.DataAnnotations;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of a Get Cropped Canvas Options.
    /// </summary>
    public class GetCroppedCanvasOptions
    {
        /// <summary>
        /// The destination width of the output canvas.
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// The destination height of the output canvas.
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// The minimum destination width of the output canvas, the default value is 0.
        /// </summary>
        public decimal? MinWidth { get; set; }

        /// <summary>
        /// The minimum destination height of the output canvas, the default value is 0.
        /// </summary>
        public decimal? MinHeight { get; set; }

        /// <summary>
        /// The maximum destination width of the output canvas, the default value is Infinity.
        /// </summary>
        public decimal? MaxWidth { get; set; }

        /// <summary>
        /// The maximum destination height of the output canvas, the default value is Infinity.
        /// </summary>
        public decimal? MaxHeight { get; set; }

        /// <summary>
        /// A color to fill any alpha values in the output canvas, the default value is the transparent.
        /// </summary>
        public string? FillColor { get; set; } = "transparent";

        /// <summary>
        /// Set to change if images are smoothed (true, default) or not (false).
        /// </summary>
        public bool? ImageSmoothingEnabled { get; set; }

        /// <summary>
        /// Set the quality of image smoothing, one of "low" (default), "medium", or "high".
        /// </summary>
        [EnumDataType(typeof(ImageSmoothingQuality))]
        public string? ImageSmoothingQuality { get; set; }

        /// <summary>
        /// Set true to use rounded values (the cropped area position and size data), the default value is false.
        /// </summary>
        public bool? Rounded { get; set; }
    }
}