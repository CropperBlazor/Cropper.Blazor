using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events.CropEvent
{
    /// <summary>
    /// 
    /// </summary>
    public class CropJSEventData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("isTrusted")]
        public bool IsTrusted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("detail")]
        public CropEvent? CropEvent { get; set; }
    }
}
