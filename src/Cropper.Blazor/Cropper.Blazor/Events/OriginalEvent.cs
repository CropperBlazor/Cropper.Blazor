using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events
{
    /// <summary>
    /// Provides serializable metadata for the original browser event that triggered a Cropper event.
    /// </summary>
    public sealed class OriginalEvent
    {
        /// <summary>
        /// The event type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// The mouse button changed by the event.
        /// </summary>
        [JsonPropertyName("button")]
        public int? Button { get; set; }

        /// <summary>
        /// The mouse buttons pressed during the event.
        /// </summary>
        [JsonPropertyName("buttons")]
        public int? Buttons { get; set; }

        /// <summary>
        /// The horizontal coordinate within the viewport.
        /// </summary>
        [JsonPropertyName("clientX")]
        public decimal? ClientX { get; set; }

        /// <summary>
        /// The vertical coordinate within the viewport.
        /// </summary>
        [JsonPropertyName("clientY")]
        public decimal? ClientY { get; set; }

        /// <summary>
        /// The horizontal coordinate within the page.
        /// </summary>
        [JsonPropertyName("pageX")]
        public decimal? PageX { get; set; }

        /// <summary>
        /// The vertical coordinate within the page.
        /// </summary>
        [JsonPropertyName("pageY")]
        public decimal? PageY { get; set; }

        /// <summary>
        /// The horizontal wheel delta.
        /// </summary>
        [JsonPropertyName("deltaX")]
        public decimal? DeltaX { get; set; }

        /// <summary>
        /// The vertical wheel delta.
        /// </summary>
        [JsonPropertyName("deltaY")]
        public decimal? DeltaY { get; set; }

        /// <summary>
        /// The pointer device type.
        /// </summary>
        [JsonPropertyName("pointerType")]
        public string? PointerType { get; set; }

        /// <summary>
        /// Indicates whether the Shift key was pressed.
        /// </summary>
        [JsonPropertyName("shiftKey")]
        public bool? ShiftKey { get; set; }
    }
}
