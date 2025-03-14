using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageReceiver
    {
        private readonly Channel<byte[]> _channel = Channel.CreateUnbounded<byte[]>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        [JSInvokable("ReceiveImageChunk")]
        public async ValueTask ReceiveImageChunkAsync(byte[] chunk)
        {
            await _channel.Writer.WriteAsync(chunk);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [JSInvokable("CompleteImageTransfer")]
        public void CompleteImageTransfer()
        {
            _channel.Writer.Complete();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<byte[]> GetImageStreamAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var chunk in _channel.Reader.ReadAllAsync(cancellationToken))
            {
                yield return chunk;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MemoryStream> GetFullImageAsync(CancellationToken cancellationToken = default)
        {
            MemoryStream memoryStream = new MemoryStream();

            await foreach (var chunk in GetImageStreamAsync(cancellationToken))
            {
                await memoryStream.WriteAsync(chunk, 0, chunk.Length, cancellationToken);
            }

            return memoryStream;
        }
    }
}
