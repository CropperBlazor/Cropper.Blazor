using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropMoveEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Move Event.
    /// </summary>
    public class CropMoveEvent
    {
        /// <summary>
        /// Event actions.
        /// </summary>
        [JsonConverter(typeof(ActionEventJsonConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Represents a pointermove, touchmove, mousemove original event.
        /// </summary>
        [JsonPropertyName("originalEvent")]
        public OriginalEvent? OriginalEvent { get; set; }
    }
}
