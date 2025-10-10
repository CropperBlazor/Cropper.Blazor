using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    public partial class CropperJsInterop
    {

        /// <summary>
        /// Used to get an image from a stream.
        /// <br/>
        /// Converts JavaScript stream to .NET stream uses <see cref="DotNetStreamReference"/> and then creates a URL blob reference.
        /// <br/>
        /// Obsolete — use <see cref="IUrlImageInterop.GetImageUsingStreamingAsync(IBrowserFile, long, CancellationToken)"/> 
        /// for centralized JS interop handling.
        /// </summary>
        /// <param name="imageFile">The <see cref="IBrowserFile"/> to convert to a new image file.</param>
        /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream. Defaults to 500 KB.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL blob reference asynchronous operation.</returns>
        [Obsolete("Use IUrlImageInterop.GetImageUsingStreamingAsync instead for centralized JS interop handling.")]
        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            return await _urlImageInterop
                .GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
        }

        /// <summary>
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// <br/>
        /// Obsolete — use <see cref="IUrlImageInterop.RevokeObjectUrlAsync(string, CancellationToken)"/> 
        /// for centralized JS interop handling.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        [Obsolete("Use IUrlImageInterop.RevokeObjectUrlAsync instead for centralized JS interop handling.")]
        public async ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default)
        {
            await _urlImageInterop
                .RevokeObjectUrlAsync(url, cancellationToken);
        }
    }
}
