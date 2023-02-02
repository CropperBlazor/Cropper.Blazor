using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Enumeration of drag modes.
    /// </summary>
    public enum DragMode
    {
        /// <summary>
        /// Create a new crop box.
        /// </summary>
        [EnumMember(Value = "crop")]
        Crop,

        /// <summary>
        /// Move the canvas.
        /// </summary>
        [EnumMember(Value = "move")]
        Move,

        /// <summary>
        /// Do nothing.
        /// </summary>
        [EnumMember(Value = "none")]
        None
    }
}
