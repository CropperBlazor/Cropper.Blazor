using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropStartEvent
{
    public class CropStartEvent
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }
    }
}
