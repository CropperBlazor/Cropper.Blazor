namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of a Set Canvas Data Options
    /// </summary>
    public class SetCanvasDataOptions
    {
        /// <summary>
        /// The new offset left of the canvas
        /// </summary>
        public decimal? Left { get; set; }

        /// <summary>
        /// The new offset top of the canvas
        /// </summary>
        public decimal? Top { get; set; }

        /// <summary>
        /// The new width of the canvas
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// The new height of the canvas
        /// </summary>
        public decimal? Height { get; set; }
    }
}
