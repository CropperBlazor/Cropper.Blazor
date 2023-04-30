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

            using (var uncompressed = new MemoryStream(Encoding.UTF8.GetBytes(code)))
            using (var compressed = new MemoryStream())
            using (var compressor = new DeflateStream(compressed, CompressionMode.Compress))
            {
                uncompressed.CopyTo(compressor);
                compressor.Close();
                bytes = compressed.ToArray();
                urlEncodedBase64compressedCode = WebEncoders.Base64UrlEncode(bytes);

                return urlEncodedBase64compressedCode;
            }
        }
    }
}
