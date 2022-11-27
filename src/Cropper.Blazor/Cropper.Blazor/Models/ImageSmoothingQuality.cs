using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Provides the metadata of a Image Smoothing Quality
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
