namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains options for setting crop box data options.
    /// </summary>
    public class SetCropBoxDataOptions
    {
        /// <summary>
        /// The new offset left of the crop box.
        /// </summary>
        public decimal? Left { get; set; }

        /// <summary>
        /// The new offset top of the crop box.
        /// </summary>
        public decimal? Top { get; set; }

        /// <summary>
        /// The new width of the crop box.
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// The new height of the crop box.
        /// </summary>
        public decimal? Height { get; set; }
    }
}
