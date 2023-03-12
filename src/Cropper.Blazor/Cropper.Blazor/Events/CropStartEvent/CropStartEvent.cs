using System;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropStartEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Start Event.
    /// </summary>
    public class CropStartEvent : IDisposable
    {
        /// <summary>
        /// Event actions.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Represents a pointerdown, touchstart, and mousedown original event.
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
