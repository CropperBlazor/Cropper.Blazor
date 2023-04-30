using System;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropEndEvent
{
    /// <summary>
    /// Provides the metadata of a Crop End Event.
    /// </summary>
    public class CropEndEvent : IDisposable
    {
        /// <summary>
        /// Event actions.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Represents a pointerup, pointercancel, touchend, touchcancel, mouseup original event.
        /// </summary>
        [JsonPropertyName("originalEvent")]
        public IJSObjectReference? OriginalEvent { get; set; }

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        public void Dispose()
        {
            OriginalEvent?.DisposeAsync();
        }
    }
}
