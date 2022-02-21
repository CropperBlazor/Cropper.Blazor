using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    public enum DragMode
    {
        [EnumMember(Value = "crop")]
        Crop,
        [EnumMember(Value = "move")]
        Move,
        [EnumMember(Value = "none")]
        None
    }
}
