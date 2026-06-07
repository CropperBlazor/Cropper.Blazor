using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Contains the current state of a Cropper.js selection element.
    /// </summary>
    public class CropperSelectionData
    {
        /// <summary>
        /// Gets or sets the zero-based selection index.
        /// </summary>
        [JsonPropertyName("index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets whether the selection is active.
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the x-axis coordinate.
        /// </summary>
        [JsonPropertyName("x")]
        public decimal X { get; set; }

        /// <summary>
        /// Gets or sets the y-axis coordinate.
        /// </summary>
        [JsonPropertyName("y")]
        public decimal Y { get; set; }

        /// <summary>
        /// Gets or sets the selection width.
        /// </summary>
        [JsonPropertyName("width")]
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets the selection height.
        /// </summary>
        [JsonPropertyName("height")]
        public decimal Height { get; set; }

        /// <summary>
        /// Gets or sets the selection aspect ratio.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("aspectRatio")]
        public decimal? AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the custom selection figure.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("shape")]
        public string? Shape { get; set; }
    }
}
