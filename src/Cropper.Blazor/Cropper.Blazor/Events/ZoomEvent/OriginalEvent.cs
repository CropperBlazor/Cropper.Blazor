using System.Runtime.Serialization;

namespace Cropper.Blazor.Events.ZoomEvent
{
    public enum OriginalEvent
    {
        [EnumMember(Value = nameof(WheelEvent))]
        WheelEvent,
        [EnumMember(Value = nameof(PointerEvent))]
        PointerEvent,
        [EnumMember(Value = nameof(TouchEvent))]
        TouchEvent,
        [EnumMember(Value = nameof(MouseEvent))]
        MouseEvent
    }
}
