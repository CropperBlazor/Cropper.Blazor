using System;
using System.Text.RegularExpressions;

namespace Cropper.Blazor.Extensions
{
    /// <summary>
    /// Class for decoding data URLs.
    /// </summary>
    public static class DataUrlDecoder
    {
        // lang=regex
        private const string DataUrlPattern = @"data:(?<type>.+?),(?<data>.+)";
        private static readonly Regex DataUrlRegex = new(DataUrlPattern, RegexOptions.Compiled);

        /// <summary>
        /// Decodes a data url into a Base64 image data and outs the media type.
        /// </summary>
        /// <param name="dataUrl">The data url to be decoded (e.g. data:image/png;base64,SGVsbG8gd29ybGQ=).</param>
        /// <returns>A Base64 image data and outs the media type.</returns>
        public static (string base64ImageData, string mediaType) Decode(this string dataUrl)
        {
            Match match = DataUrlRegex.Match(dataUrl);

            if (!match.Success)
            {
                throw new ArgumentException($"Could not parse '{dataUrl}' as '\"data:(?<type>.+?),(?<data>.+)\"' data URL pattern.");
            }

            string mediaType = match.Groups["type"].Value;
            string base64ImageData = match.Groups["data"].Value;

            return (base64ImageData, mediaType);
        }
    }
}
