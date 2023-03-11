using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events
{
    /// <summary>
    /// Provides the metadata of a Crop JavaScript Event Data.
    /// </summary>
    public class JSEventData<Event>
    {
        /// <summary>
        /// Represents a Boolean value indicating whether the event is trusted or not.
        /// </summary>
        [JsonPropertyName("isTrusted")]
        public bool IsTrusted { get; set; }

        /// <summary>
        /// Details about an event.
        /// </summary>
        [JsonPropertyName("detail")]
        public Event? Detail { get; set; }

        /// <summary>
        /// Data structure that defines the data contained in an event.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        /// <summary>
        /// Indicates which phase of the event flow is currently being evaluated.
        /// </summary>
        [JsonPropertyName("eventPhase")]
        public int? EventPhase { get; set; }

        /// <summary>
        /// Indicates whether the event bubbles up through the DOM tree or not.
        /// </summary>
        [JsonPropertyName("bubbles")]
        public bool? Bubbles { get; set; }

        /// <summary>
        /// Indicates whether the event can be canceled, and therefore prevented as if the event never happened.
        /// </summary>
        [JsonPropertyName("cancelable")]
        public bool? Cancelable { get; set; }

        /// <summary>
        /// Indicating whether or not the call to Event.preventDefault() canceled the event.
        /// </summary>
        [JsonPropertyName("defaultPrevented")]
        public bool? DefaultPrevented { get; set; }

        /// <summary>
        /// Indicates whether or not the event will propagate across the shadow DOM boundary into the standard DOM.
        /// </summary>
        [JsonPropertyName("composed")]
        public bool? Composed { get; set; }

        /// <summary>
        /// Number of milliseconds from the document was finished loading until the specific event was created.
        /// </summary>
        [JsonPropertyName("timeStamp")]
        public double? TimeStamp { get; set; }

        /// <summary>
        /// Indicates whether the default action for this event has been prevented or not.
        /// </summary>
        [JsonPropertyName("returnValue")]
        public bool? ReturnValue { get; set; }

        /// <summary>
        /// Prevents the event-flow from bubbling up to parent elements.
        /// </summary>
        [JsonPropertyName("cancelBubble")]
        public bool? CancelBubble { get; set; }
    }
}
