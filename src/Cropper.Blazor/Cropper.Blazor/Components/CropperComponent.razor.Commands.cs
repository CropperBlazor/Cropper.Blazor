using System;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
        /// <summary>
        /// Initializes cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void InitCropper(CancellationToken cancellationToken = default)
        {
            ICropperComponentBase cropperComponentBase = this;
            ElementReference? cropperElementReference = GetCropperElementReference();

            if (cropperElementReference.HasValue)
            {
                cropperComponentBaseReference?.Dispose();
                cropperComponentBaseReference = DotNetObjectReference.Create(cropperComponentBase);

                _ = InitializeCropperAsync(
                    CropperComponentId,
                    cropperElementReference.Value,
                    GetEffectiveOptions(),
                    cropperComponentBaseReference,
                    cancellationToken);

                OnLoadImageEvent?.Invoke();
            }
        }

        private async Task InitializeCropperAsync(
            Guid cropperComponentId,
            ElementReference cropperElementReference,
            Options options,
            DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken)
        {
            await CropperJsIntertop!.InitCropperAsync(
                cropperComponentId,
                cropperElementReference,
                options,
                cropperComponentBase,
                cancellationToken);

            if (CropperState is not null)
            {
                await CropperState.NotifyCropperInitializedAsync(this);
            }
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Crop(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.CropAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Clear(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ClearAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Enable(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.EnableAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Disable(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.DisableAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Reset the image and crop box to its initial states.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Reset(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ResetAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void Destroy(CancellationToken cancellationToken = default)
        {
            DestroyAsync(cancellationToken);
        }

        private async ValueTask DestroyAsync(CancellationToken cancellationToken = default)
        {
            await CropperJsIntertop!.DestroyAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Replace the image's src and rebuild the cropper.
        /// </summary>
        /// <param name="url">The new URL.</param>
        /// <param name="hasSameSize">If the new image has the same size as the old one, then it will not rebuild the cropper and only update the URLs of all related images. This can be used for applying filters.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ReplaceAsync(
            string url,
            bool hasSameSize = true,
            CancellationToken cancellationToken = default)
        {
            Src = url;
            await CropperJsIntertop!.ReplaceAsync(CropperComponentId, url, hasSameSize, cancellationToken);
        }

        /// <summary>
        /// Call this method when you've finished using an object URL to let the browser know not to keep the reference to the file any longer.
        /// <br/>
        /// Obsolete — use <see cref="Services.IUrlImageInterop.RevokeObjectUrlAsync(string, CancellationToken)"/>
        /// for centralized JS interop handling.
        /// </summary>
        /// <param name="url">A string representing an object URL.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        [Obsolete("⚠️ This method will be removed in future versions. Use IUrlImageInterop.RevokeObjectUrlAsync instead for proper JS interop management.")]
        public async ValueTask RevokeObjectUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            await CropperJsIntertop!.RevokeObjectUrlAsync(url, cancellationToken);
        }
    }
}
