using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Base
{
    /// <summary>
    /// Provides the metadata of a ICropperComponentBase.
    /// </summary>
    public interface ICropperComponentBase
    {
        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box changes.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   When the autoCrop option is set to the true, a crop event will be triggered before the ready event.
        ///  </para>
        ///  <para>
        ///   When the data option is set, another crop event will be triggered before the ready event
        ///  </para>
        /// </remarks>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task CropperIsCropedAsync(IJSObjectReference jSObjectReference);

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box stops changing.
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task CropperIsEndedAsync(IJSObjectReference jSObjectReference);

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box is changing.
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task CropperIsMovedAsync(IJSObjectReference jSObjectReference);

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box starts to change.
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task CropperIsStartedAsync(IJSObjectReference jSObjectReference);

        /// <summary>
        /// This event fires when a cropper instance starts to zoom in or zoom out its canvas (image wrapper).
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task CropperIsZoomedAsync(IJSObjectReference jSObjectReference);

        /// <summary>
        /// This event fires when the target image has been loaded and the cropper instance is ready for operating.
        /// </summary>
        /// <param name="jSObjectReference">The <see cref="IJSObjectReference"/>.</param>
        void IsReady(IJSObjectReference jSObjectReference);
    }
}
