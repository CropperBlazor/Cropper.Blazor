namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains cropper canvas options
    /// </summary>
    public class CanvasData
    {
        /// <summary>
        /// The offset left of the canvas
        /// </summary>
        public decimal Left { get; set; }

        /// <summary>
        /// The offset top of the canvas
        /// </summary>
        public decimal Top { get; set; }

        /// <summary>
        /// The width of the canvas
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// The height of the canvas
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// The natural width of the canvas (read only)
        /// </summary>
        public decimal NaturalWidth { get; set; }

        /// <summary>
        /// The natural height of the canvas (read only)
        /// </summary>
        public decimal NaturalHeight { get; set; }
    }
}
