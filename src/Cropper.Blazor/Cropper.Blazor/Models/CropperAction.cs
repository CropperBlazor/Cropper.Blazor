using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Defines Cropper.js v2 action values used by cropper handles and action events.
    /// </summary>
    [JsonConverter(typeof(EnumMemberJsonConverter<CropperAction>))]
    public enum CropperAction
    {
        /// <summary>
        /// No action.
        /// </summary>
        [EnumMember(Value = "none")]
        None,

        /// <summary>
        /// Create a new selection.
        /// </summary>
        [EnumMember(Value = "select")]
        Select,

        /// <summary>
        /// Move the image or selection.
        /// </summary>
        [EnumMember(Value = "move")]
        Move,

        /// <summary>
        /// Scale the image or selection.
        /// </summary>
        [EnumMember(Value = "scale")]
        Scale,

        /// <summary>
        /// Rotate the image.
        /// </summary>
        [EnumMember(Value = "rotate")]
        Rotate,

        /// <summary>
        /// Transform the image.
        /// </summary>
        [EnumMember(Value = "transform")]
        Transform,

        /// <summary>
        /// Resize the north side of the selection.
        /// </summary>
        [EnumMember(Value = "n-resize")]
        ResizeNorth,

        /// <summary>
        /// Resize the east side of the selection.
        /// </summary>
        [EnumMember(Value = "e-resize")]
        ResizeEast,

        /// <summary>
        /// Resize the south side of the selection.
        /// </summary>
        [EnumMember(Value = "s-resize")]
        ResizeSouth,

        /// <summary>
        /// Resize the west side of the selection.
        /// </summary>
        [EnumMember(Value = "w-resize")]
        ResizeWest,

        /// <summary>
        /// Resize the northeast corner of the selection.
        /// </summary>
        [EnumMember(Value = "ne-resize")]
        ResizeNorthEast,

        /// <summary>
        /// Resize the northwest corner of the selection.
        /// </summary>
        [EnumMember(Value = "nw-resize")]
        ResizeNorthWest,

        /// <summary>
        /// Resize the southeast corner of the selection.
        /// </summary>
        [EnumMember(Value = "se-resize")]
        ResizeSouthEast,

        /// <summary>
        /// Resize the southwest corner of the selection.
        /// </summary>
        [EnumMember(Value = "sw-resize")]
        ResizeSouthWest
    }
}
