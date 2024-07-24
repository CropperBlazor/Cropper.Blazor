using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum CropperComponentType
    {
        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "image")]
        Image,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "canvas")]
        Canvas
    }
}
