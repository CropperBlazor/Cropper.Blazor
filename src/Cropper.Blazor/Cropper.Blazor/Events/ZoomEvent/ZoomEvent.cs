using System;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Events.ZoomEvent
{
    /// <summary>
    /// Provides the metadata of a Zoom Event.
    /// </summary>
    public class ZoomEvent : IDisposable
    {
        /// <summary>
        /// The old (current) ratio of the canvas.
        /// </summary>
        [JsonPropertyName("oldRatio")]
        public decimal OldRatio { get; set; }

        /// <summary>
        /// The new (next) ratio of the canvas (canvasData.width / canvasData.naturalWidth).
        /// </summary>
        [JsonPropertyName("ratio")]
        public decimal Ratio { get; set; }

        /// <summary>
        /// Represents a wheel, pointermove, touchmove, mousemove original event.
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
