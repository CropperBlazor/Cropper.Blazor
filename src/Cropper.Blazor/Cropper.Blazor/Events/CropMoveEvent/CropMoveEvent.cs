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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }
    }
}
