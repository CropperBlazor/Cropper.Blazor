using System.Text.RegularExpressions;
using System;

namespace Cropper.Blazor.Extensions
{
    /// <summary>
    /// Class for decoding data urls.
    /// </summary>
    public static class DataUrlDecoder
    {
        // lang=regex
        private const string DataUrlPattern = @"data:(?<type>.+?),(?<data>.+)";
        private static readonly Regex DataUrlRegex = new (DataUrlPattern, RegexOptions.Compiled);
        /// <summary>
        /// Decodes a data url into a byte array and outs the <paramref name="mediaType"/>
        /// </summary>
        /// <param name="dataUrl">The data url to be decoded</param>
        /// <returns>A DecodedDataUrl contained the media type and decoded url.</returns>
        public static (byte[] ImageData, string MediaType) Decode(string dataUrl)
        {
            var match = DataUrlRegex.Match(dataUrl);
            var mediaType = match.Groups["type"].Value;
            var base64DataStr = match.Groups["data"].Value;
            var base64Data = Convert.FromBase64String(base64DataStr);
            return (base64Data, mediaType);
        }
    }
}
