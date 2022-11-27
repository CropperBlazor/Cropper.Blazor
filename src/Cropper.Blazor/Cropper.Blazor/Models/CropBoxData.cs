namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of a Crop Box Data
    /// </summary>
    public class CropBoxData
    {
        /// <summary>
        /// The offset left of the crop box
        /// </summary>
        public decimal Left { get; set; }

        /// <summary>
        /// The offset top of the crop box
        /// </summary>
        public decimal Top { get; set; }

        /// <summary>
        /// The width of the crop box
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// The height of the crop box
        /// </summary>
        public decimal Height { get; set; }
    }
}
