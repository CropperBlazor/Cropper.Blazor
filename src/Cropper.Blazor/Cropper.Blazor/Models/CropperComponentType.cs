using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Enumeration of cropper component types.
    /// </summary>
    public enum CropperComponentType
    {
        /// <summary>
        /// Specifies the target image element for cropping.
        /// </summary>
        [EnumMember(Value = "image")]
        Image,

        /// <summary>
        /// Specifies the target canvas element for cropping.
        /// </summary>
        [EnumMember(Value = "canvas")]
        Canvas
    }
}
