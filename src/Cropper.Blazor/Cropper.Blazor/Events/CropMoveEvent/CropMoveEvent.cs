using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropMoveEvent
{
    public class CropMoveEvent
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }
    }
}
