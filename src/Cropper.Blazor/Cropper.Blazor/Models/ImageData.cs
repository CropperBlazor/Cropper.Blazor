namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains image options.
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// The offset left of the image.
        /// </summary>
        public decimal Left { get; set; }

        /// <summary>
        /// The offset top of the image.
        /// </summary>
        public decimal Top { get; set; }

        /// <summary>
        /// The width of the image.
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// The height of the image.
        /// </summary>
        public decimal Height { get; set; }

        /// <summary>
        /// The rotated degrees of the image if it is rotated.
        /// </summary>
        public decimal Rotate { get; set; }

        /// <summary>
        /// The scaling factor to apply on the abscissa of the image if scaled.
        /// </summary>
        public decimal ScaleX { get; set; }

        /// <summary>
        /// The scaling factor to apply on the ordinate of the image if scaled.
        /// </summary>
        public decimal ScaleY { get; set; }

        /// <summary>
        /// The natural width of the image.
        /// </summary>
        public decimal NaturalWidth { get; set; }

        /// <summary>
        /// The natural height of the image.
        /// </summary>
        public decimal NaturalHeight { get; set; }

        /// <summary>
        /// The aspect ratio of the image.
        /// </summary>
        public decimal AspectRatio { get; set; }
    }
}
