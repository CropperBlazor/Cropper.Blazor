using System;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.CropMoveEvent
{
    /// <summary>
    /// Provides the metadata of a Crop Move Event.
    /// </summary>
    public class CropMoveEvent : IDisposable
    {
        /// <summary>
        /// Event actions.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("action")]
        public ActionEvent ActionEvent { get; set; }

        /// <summary>
        /// Represents a pointermove, touchmove, mousemove original event.
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
