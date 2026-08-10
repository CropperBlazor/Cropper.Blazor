using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// Provides shared synchronization state between a cropper component and cropper viewers.
    /// </summary>
    public sealed class CropperState
    {
        internal CropperComponent? CropperComponent { get; private set; }

        internal int Version { get; private set; }

        internal event Func<CropperComponent, ValueTask>? CropperInitialized;

        /// <summary>
        /// Notifies viewers that the cropper component is initialized.
        /// </summary>
        /// <param name="cropperComponent">The initialized cropper component.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask NotifyCropperInitializedAsync(CropperComponent cropperComponent)
        {
            CropperComponent = cropperComponent;
            Version++;

            if (CropperInitialized is null)
            {
                return;
            }

            foreach (Func<CropperComponent, ValueTask> handler in CropperInitialized.GetInvocationList().Cast<Func<CropperComponent, ValueTask>>())
            {
                await handler(cropperComponent);
            }
        }
    }
}
