namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains cropper box options
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
