using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.ZoomEvent
{
    /// <summary>
    /// Provides the metadata of a Zoom Event
    /// </summary>
    public class ZoomEvent
    {
        /// <summary>
        /// The old (current) ratio of the canvas
        /// </summary>
        [JsonPropertyName("oldRatio")]
        public decimal OldRatio { get; set; }

        /// <summary>
        /// The new (next) ratio of the canvas (canvasData.width / canvasData.naturalWidth)
        /// </summary>
        [JsonPropertyName("ratio")]
        public decimal Ratio { get; set; }
    }
}
