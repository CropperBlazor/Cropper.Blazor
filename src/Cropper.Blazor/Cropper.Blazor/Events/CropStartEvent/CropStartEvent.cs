using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropStartEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Start Event
    /// </summary>
    public class CropStartEvent
    {
        /// <summary>
        /// Event actions
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }
    }
}
