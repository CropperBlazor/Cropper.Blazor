using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Cropper.Blazor.Client.Extensions
{
    public static class StringExtensions
    {
        public static string ToCompressedEncodedUrl(this string code)
        {
            string urlEncodedBase64compressedCode;
            byte[] bytes;

            using MemoryStream uncompressed = new(Encoding.UTF8.GetBytes(code));
            using MemoryStream compressed = new();
            using (DeflateStream compressor = new(compressed, CompressionMode.Compress))
            {
                uncompressed.CopyTo(compressor);
                compressor.Close();
                bytes = compressed.ToArray();
                urlEncodedBase64compressedCode = WebEncoders.Base64UrlEncode(bytes);
            }

            return urlEncodedBase64compressedCode;
        }
    }
}
