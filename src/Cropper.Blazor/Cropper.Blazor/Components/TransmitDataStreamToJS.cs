#if NET5_0
namespace Cropper.Blazor.Components
{
    using Cropper.Blazor.DotNet5;
    using Microsoft.JSInterop;
    using System;
    using System.Buffers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <Summary>
    /// A stream that pulls each chunk on demand using JavaScript interop. This implementation is used for
    /// WebAssembly and WebView applications.
    /// </Summary>
    internal static class TransmitDataStreamToJS
    {
        private static long _nextObjectReferenceId;

        internal static async Task TransmitStreamAsync(IJSRuntime runtime, long streamId, DotNetStreamReference dotNetStreamReference)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(32 * 1024);

            try
            {
                int bytesRead;
                while ((bytesRead = await dotNetStreamReference.Stream.ReadAsync(buffer)) > 0)
                {
                    await runtime.InvokeVoidAsync("Blazor._internal.receiveDotNetDataStream", streamId, buffer, bytesRead, null);
                }

                // Notify client that the stream has completed
                await runtime.InvokeVoidAsync("Blazor._internal.receiveDotNetDataStream", streamId, Array.Empty<byte>(), 0, null);
            }
            catch (Exception ex)
            {
                try
                {
                    // Attempt to notify the client of the error.
                    await runtime.InvokeVoidAsync("Blazor._internal.receiveDotNetDataStream", streamId, Array.Empty<byte>(), 0, ex.Message);
                }
                catch
                {
                    // JS Interop encountered an issue, unable to send error message to JS.
                }

                throw;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer, clearArray: true);

                if (!dotNetStreamReference.LeaveOpen)
                {
                    dotNetStreamReference.Stream?.Dispose();
                }
            }
        }

        /// <summary>
        /// Transmits the stream data from .NET to JS. Subclasses should override this method and provide
        /// an implementation that transports the data to JS and calls DotNet.jsCallDispatcher.supplyDotNetStream.
        /// </summary>
        /// <param name="streamId">An identifier for the stream.</param>
        /// <param name="dotNetStreamReference">Reference to the .NET stream along with whether the stream should be left open.</param>
        internal static Task TransmitStreamAsync(long streamId, DotNetStreamReference dotNetStreamReference)
        {
            if (!dotNetStreamReference.LeaveOpen)
            {
                dotNetStreamReference.Stream.Dispose();
            }

            throw new NotSupportedException("The current JS runtime does not support sending streams from .NET to JS.");
        }

        internal static long BeginTransmittingStream(DotNetStreamReference dotNetStreamReference)
        {
            // It's fine to share the ID sequence
            var streamId = Interlocked.Increment(ref _nextObjectReferenceId);

            // Fire and forget sending the stream so the client can proceed to
            // reading the stream.
            _ = TransmitStreamAsync(streamId, dotNetStreamReference);

            return streamId;
        }
    }
}
#endif
