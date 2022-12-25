using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains cropper options
    /// </summary>
    public class CropperData
    {
        /// <summary>
        /// The offset left of the cropped area
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("x")]
        public decimal? X { get; set; }

        /// <summary>
        /// The offset top of the cropped area
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("y")]
        public decimal? Y { get; set; }

        /// <summary>
        /// The width of the cropped area
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("width")]
        public decimal? Width { get; set; }

        /// <summary>
        /// The height of the cropped area
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("height")]
        public decimal? Height { get; set; }

        /// <summary>
        /// The rotated degrees of the image
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("rotate")]
        public decimal? Rotate { get; set; }

        /// <summary>
        /// The scaling factor to apply on the abscissa of the image
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("scaleX")]
        public decimal? ScaleX { get; set; }

        /// <summary>
        /// The scaling factor to apply on the ordinate of the image
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("scaleY")]
        public decimal? ScaleY { get; set; }
    }
}
