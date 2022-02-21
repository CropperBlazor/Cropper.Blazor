using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
