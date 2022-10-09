using System.Runtime.Serialization;

namespace Cropper.Blazor.Models
{
    public enum ImageSmoothingQuality
    {
        [EnumMember(Value = "low")]
        Low,
        [EnumMember(Value = "medium")]
        Medium,
        [EnumMember(Value = "high")]
        High,
    }
}
