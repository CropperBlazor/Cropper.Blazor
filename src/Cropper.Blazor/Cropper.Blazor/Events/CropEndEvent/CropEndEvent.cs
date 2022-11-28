using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropEndEvent
{
    /// <summary>
    /// Provides the metadata of a Crop End Event
    /// </summary>
    public class CropEndEvent
    {
        /// <summary>
        /// Action Event
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }
    }
}
