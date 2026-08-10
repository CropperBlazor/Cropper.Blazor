using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropStartEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Start Event.
    /// </summary>
    public class CropStartEvent
    {
        /// <summary>
        /// Event actions.
        /// </summary>
        [JsonConverter(typeof(ActionEventJsonConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Represents a pointerdown, touchstart, mousedown original event.
        /// </summary>
        [JsonPropertyName("originalEvent")]
        public OriginalEvent? OriginalEvent { get; set; }
    }
}
