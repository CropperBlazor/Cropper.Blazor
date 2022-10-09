using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.ZoomEvent
{
    public class ZoomEvent
    {
        [JsonPropertyName("oldRatio")]
        public decimal OldRatio { get; set; }

        [JsonPropertyName("ratio")]
        public decimal Ratio { get; set; }
    }
}
