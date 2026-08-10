using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Defines how the Cropper.js v2 image element is initially centered in the canvas.
    /// </summary>
    [JsonConverter(typeof(EnumMemberJsonConverter<CropperImageInitialCenterSize>))]
    public enum CropperImageInitialCenterSize
    {
        /// <summary>
        /// Fit the whole image inside the canvas.
        /// </summary>
        [EnumMember(Value = "contain")]
        Contain,

        /// <summary>
        /// Cover the canvas with the image.
        /// </summary>
        [EnumMember(Value = "cover")]
        Cover
    }
}
