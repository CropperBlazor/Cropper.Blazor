using Cropper.Blazor.Extensions;

namespace Cropper.Blazor.Models
{
    /// <summary>
    /// A class that represents a decoded data url.
    /// </summary>
    public class DecodedDataUrl
    {
        /// <summary>
        /// The image data of the data url converted from Base64.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// The media type defined in the data url. (e.g. image/png;base64).
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Decodes a data url into a DecodedDataUrl object.
        /// </summary>
        /// <param name="dataUrl">Represents the data URL.</param>
        public DecodedDataUrl(string dataUrl)
        {
            (ImageData, MediaType) = DataUrlDecoder.Decode(dataUrl);
        }
    }
}
