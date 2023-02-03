using System.Runtime.Serialization;

namespace Cropper.Blazor.Events.ZoomEvent
{
    /// <summary>
    /// Provides the metadata of a Original Event.
    /// </summary>
    public enum OriginalEvent
    {
        /// <summary>
        /// Wheel Event.
        /// </summary>
        [EnumMember(Value = nameof(WheelEvent))]
        WheelEvent,

        /// <summary>
        /// Pointer Event.
        /// </summary>
        [EnumMember(Value = nameof(PointerEvent))]
        PointerEvent,

        /// <summary>
        /// Touch Event.
        /// </summary>
        [EnumMember(Value = nameof(TouchEvent))]
        TouchEvent,

        /// <summary>
        /// Mouse Event.
        /// </summary>
        [EnumMember(Value = nameof(MouseEvent))]
        MouseEvent
    }
}
