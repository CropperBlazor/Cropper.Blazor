using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.ModuleOptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// Provides JavaScript interop functionality for working with image URLs in Blazor.
    /// <br/>
    /// Supports creating object URLs from streamed image files and revoking them when no longer needed.
    /// </summary>
    public class UrlImageInterop : BaseJsInterop, IUrlImageInterop
    {
        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
        /// <param name="cropperJsInteropOptions">The <see cref="ICropperJsInteropOptions"/>.</param>
        public UrlImageInterop(
            IJSRuntime jsRuntime,
            NavigationManager navigationManager,
            ICropperJsInteropOptions cropperJsInteropOptions) : base(jsRuntime, navigationManager, cropperJsInteropOptions)
        {

        }

        /// <summary>
        /// Used to get an image from a stream.
        /// <br/>
        /// Converts JavaScript stream to .NET stream uses <see cref="DotNetStreamReference"/> and then creates a URL blob reference.
        /// </summary>
        /// <param name="imageFile">The <see cref="IBrowserFile"/> to convert to a new image file.</param>
        /// <param name="maxAllowedSize">The maximum number of bytes that can be supplied by the Stream. Defaults to 500 KB.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{String}"/> representing URL blob reference asynchronous operation.</returns>
        public async ValueTask<string> GetImageUsingStreamingAsync(
            IBrowserFile imageFile,
            long maxAllowedSize = 512000L,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            var jsImageStream = imageFile.OpenReadStream(maxAllowedSize, cancellationToken);
            var dotnetImageStream = new DotNetStreamReference(jsImageStream);

            return await _jsRuntime.InvokeAsync<string>(
                "cropperUrlImageHelper.getImageUsingStreaming",
                cancellationToken,
                dotnetImageStream);
        }

        /// <summary>
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RevokeObjectUrlAsync(
            string url,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime.InvokeVoidAsync(
                "cropperUrlImageHelper.revokeObjectUrl",
                cancellationToken,
                url);
        }
    }
}
