using System;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Models;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// Represents a receiver for cropped canvas reference streamed from JavaScript to .NET.
    /// </summary>
    public class CroppedCanvasReceiver
    {
        private readonly Func<CroppedCanvas, CancellationToken, Task> _onReceive;
        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Provides the metadata of Cropped Canvas.
        /// </summary>
        public CroppedCanvas CroppedCanvas { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CroppedCanvasReceiver"/> class,
        /// which handles the reception of a <see cref="CroppedCanvas"/> asynchronously.
        /// </summary>
        /// <param name="onReceive">A callback function invoked when a <see cref="CroppedCanvas"/> is received.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        public CroppedCanvasReceiver(
            Func<CroppedCanvas, CancellationToken, Task> onReceive,
            CancellationToken cancellationToken)
        {
            _onReceive = onReceive;
            _cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Receives a cropped canvas reference from JavaScript.
        /// </summary>
        /// <param name="jsRuntimeObjectRef">The cropped canvas reference.</param>
        [JSInvokable("ReceiveCanvasReference")]
        public void ReceiveCanvasReference(IJSObjectReference jsRuntimeObjectRef)
        {
            CroppedCanvas = new CroppedCanvas(jsRuntimeObjectRef);

            _onReceive(CroppedCanvas, _cancellationToken);
        }
    }
}
