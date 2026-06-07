using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropEndEvent
{
    /// <summary>
    /// Provides the metadata of a Crop End Event.
    /// </summary>
    public class CropEndEvent
    {
        /// <summary>
        /// Event actions.
        /// </summary>
        [JsonConverter(typeof(ActionEventJsonConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Represents a pointerup, pointercancel, touchend, touchcancel, mouseup original event.
        /// </summary>
        [JsonPropertyName("originalEvent")]
        public OriginalEvent? OriginalEvent { get; set; }
    }
}
