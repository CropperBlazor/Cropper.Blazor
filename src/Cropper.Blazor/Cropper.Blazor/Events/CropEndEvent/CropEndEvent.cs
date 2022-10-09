using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropEndEvent
{
    public class CropEndEvent
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }
    }
}
