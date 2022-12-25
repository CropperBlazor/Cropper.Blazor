using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Enumeration of image smoothing quality options
    /// </summary>
    public enum ImageSmoothingQuality
    {
        /// <summary>
        /// Low quality
        /// </summary>
        [EnumMember(Value = "low")]
        Low,

        /// <summary>
        /// Medium quality
        /// </summary>
        [EnumMember(Value = "medium")]
        Medium,

        /// <summary>
        /// High quality
        /// </summary>
        [EnumMember(Value = "high")]
        High,
    }
}
