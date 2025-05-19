using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Cropper.Blazor.Exceptions;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// Represents a receiver for image data streamed from JavaScript to .NET, 
    /// supporting chunked image transfer, error handling, and final image assembly.
    /// </summary>
    public class ImageReceiver
    {
        private readonly Channel<byte[]> _channel = Channel.CreateUnbounded<byte[]>();
        private ImageProcessingException? imageProcessingException;

        /// <summary>
        /// Gets the exception that occurred during image processing.
        /// </summary>
        public ImageProcessingException? ImageProcessingException { get => imageProcessingException; }

        /// <summary>
        /// Handles an image processing error invoked from JavaScript.
        /// Sets the exception and completes the image transfer.
        /// </summary>
        /// <param name="errorMessage">The error message from JavaScript.</param>
        [JSInvokable("HandleImageProcessingError")]
        public void HandleImageProcessingError(string errorMessage)
        {
            imageProcessingException = new(errorMessage);
            CompleteImageTransfer();
        }

        /// <summary>
        /// Receives a chunk of image data from JavaScript.
        /// </summary>
        /// <param name="chunk">The image data chunk.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        [JSInvokable("ReceiveImageChunk")]
        public async ValueTask ReceiveImageChunkAsync(byte[] chunk)
        {
            await _channel.Writer.WriteAsync(chunk);
        }

        /// <summary>
        /// Marks the end of the image data transfer from JavaScript.
        /// </summary>
        [JSInvokable("CompleteImageTransfer")]
        public void CompleteImageTransfer()
        {
            _channel.Writer.Complete();
        }

        /// <summary>
        /// Asynchronously streams the received image data chunks.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>An async enumerable of byte arrays representing the image chunks.</returns>
        public async IAsyncEnumerable<byte[]> GetImageStreamAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var chunk in _channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return chunk;
            }
        }

        /// <summary>
        /// Asynchronously reads the image chunks into a memory stream.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <exception cref="ImageProcessingException">
        /// Thrown if an error occurs during image processing.
        /// </exception>
        /// <returns>A <see cref="Task{MemoryStream}"/> representing asynchronous operation on stream image data.</returns>
        public async Task<MemoryStream> GetImageChunkStreamAsync(CancellationToken cancellationToken = default)
        {
            MemoryStream memoryStream = new MemoryStream();

            await foreach (var chunk in GetImageStreamAsync(cancellationToken))
            {
                await memoryStream.WriteAsync(chunk, 0, chunk.Length, cancellationToken);
            }

            if (ImageProcessingException != null)
            {
                await memoryStream.DisposeAsync();

                throw ImageProcessingException;
            }

            return memoryStream;
        }
    }
}
