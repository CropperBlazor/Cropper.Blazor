using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    public partial class CropperJsInterop
    {
        /// <summary>
        /// Initializes cropper. 
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="image">Reference to img html-DOM</param>
        /// <param name="options">Cropper options</param>
        /// <param name="cropperComponentBase">Reference to base cropper component. Default equal to 'this' object.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask InitCropperAsync(
            [NotNull] Guid cropperComponentId,
            [NotNull] ElementReference image,
            [NotNull] Options options,
            [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.initCropper",
                cancellationToken,
                cropperComponentId,
                image,
                options,
                cropperComponentBase);
        }

        /// <summary>
        /// Clear the crop box.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ClearAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.clear",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Get the current Cropper.js v2 selection elements for a cropper component.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> containing the current selections.</returns>
        public async ValueTask<IReadOnlyList<CropperSelectionData>> GetSelectionsDataAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            return await _jsRuntime!.InvokeAsync<IReadOnlyList<CropperSelectionData>>(
                "cropper.getSelectionsData",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Show the crop box manually.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask CropAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.crop",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Destroy the cropper and remove the instance from the image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DestroyAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.destroy",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Disable (freeze) the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisableAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.disable",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Enable (unfreeze) the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask EnableAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.enable",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Move the canvas with relative offsets.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="offsetX">The relative offset distance on the x-axis.</param>
        /// <param name="offsetY">The relative offset distance on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveAsync(
            [NotNull] Guid cropperComponentId,
            decimal offsetX,
            decimal? offsetY,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.move",
                cancellationToken,
                cropperComponentId,
                offsetX,
                offsetY);
        }

        /// <summary>
        /// Move the canvas to an absolute point.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveToAsync(
            [NotNull] Guid cropperComponentId,
            decimal x,
            decimal? y,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.moveTo",
                cancellationToken,
                cropperComponentId,
                x,
                y);
        }

        /// <summary>
        /// Center the image within the cropper canvas.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="size">The center sizing mode. Supported values are <c>contain</c> and <c>cover</c>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask CenterAsync(
            [NotNull] Guid cropperComponentId,
            string size,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.center",
                cancellationToken,
                cropperComponentId,
                size);
        }

        /// <summary>
        /// Reset the cropper selection to its initial state.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ResetSelectionAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.resetSelection",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Center the cropper selection within the canvas.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask CenterSelectionAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.centerSelection",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Clear the cropper selection.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ClearSelectionAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.clearSelection",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Move the cropper selection to an absolute point.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask MoveSelectionToAsync(
            [NotNull] Guid cropperComponentId,
            decimal x,
            decimal y,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.moveSelectionTo",
                cancellationToken,
                cropperComponentId,
                x,
                y);
        }

        /// <summary>
        /// Change the cropper selection position and size.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="width">The selection width.</param>
        /// <param name="height">The selection height.</param>
        /// <param name="aspectRatio">The optional selection aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ChangeSelectionAsync(
            [NotNull] Guid cropperComponentId,
            decimal x,
            decimal y,
            decimal width,
            decimal height,
            decimal? aspectRatio = null,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.changeSelection",
                cancellationToken,
                cropperComponentId,
                x,
                y,
                width,
                height,
                aspectRatio);
        }

        /// <summary>
        /// Create a new Cropper.js v2 selection when multiple selection mode is enabled.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="width">The selection width.</param>
        /// <param name="height">The selection height.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask CreateSelectionAsync(
            [NotNull] Guid cropperComponentId,
            decimal x,
            decimal y,
            decimal width,
            decimal height,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.createSelection",
                cancellationToken,
                cropperComponentId,
                x,
                y,
                width,
                height);
        }

        /// <summary>
        /// Remove a cropper selection by zero-based selection index.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RemoveSelectionByIndexAsync(
            [NotNull] Guid cropperComponentId,
            int selectionIndex,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.removeSelectionByIndex",
                cancellationToken,
                cropperComponentId,
                selectionIndex);
        }

        /// <summary>
        /// Change a cropper selection by zero-based selection index.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="width">The selection width.</param>
        /// <param name="height">The selection height.</param>
        /// <param name="aspectRatio">The optional selection aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ChangeSelectionByIndexAsync(
            [NotNull] Guid cropperComponentId,
            int selectionIndex,
            decimal x,
            decimal y,
            decimal width,
            decimal height,
            decimal? aspectRatio = null,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.changeSelectionByIndex",
                cancellationToken,
                cropperComponentId,
                selectionIndex,
                x,
                y,
                width,
                height,
                aspectRatio);
        }

        /// <summary>
        /// Set a cropper selection figure by zero-based selection index.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="shape">The selection figure.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetSelectionShapeByIndexAsync(
            [NotNull] Guid cropperComponentId,
            int selectionIndex,
            string shape,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setSelectionShapeByIndex",
                cancellationToken,
                cropperComponentId,
                selectionIndex,
                shape);
        }

        /// <summary>
        /// Get the number of Cropper.js v2 selection elements for a cropper component.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> containing selection count.</returns>
        public async ValueTask<int> GetSelectionCountAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            return await _jsRuntime!.InvokeAsync<int>(
                "cropper.getSelectionCount",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Replace the image's src and rebuild the cropper.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="url">The new URL.</param>
        /// <param name="hasSameSize">If the new image has the same size as the old one, then it will not rebuild the cropper and only update the URLs of all related images. This can be used for applying filters.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ReplaceAsync(
            [NotNull] Guid cropperComponentId,
            string url,
            bool hasSameSize,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.replace",
                cancellationToken,
                cropperComponentId,
                url,
                hasSameSize);
        }

        /// <summary>
        /// Reset the image and crop box to their initial states.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ResetAsync(
            [NotNull] Guid cropperComponentId,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.reset",
                cancellationToken,
                cropperComponentId);
        }

        /// <summary>
        /// Rotate the canvas with a relative degree.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RotateAsync(
            [NotNull] Guid cropperComponentId,
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.rotate",
                cancellationToken,
                cropperComponentId,
                degree);
        }

        /// <summary>
        /// Rotate the canvas to an absolute degree.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="degree">The rotate degree.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask RotateToAsync(
            [NotNull] Guid cropperComponentId,
            decimal degree,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.rotateTo",
                cancellationToken,
                cropperComponentId,
                degree);
        }

        /// <summary>
        /// Scale the image.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleAsync(
            [NotNull] Guid cropperComponentId,
            decimal scaleX,
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.scale",
                cancellationToken,
                cropperComponentId,
                scaleX,
                scaleY);
        }

        /// <summary>
        /// Scale the image on the x-axis.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleX">The scale ratio on the x-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleXAsync(
            [NotNull] Guid cropperComponentId,
            decimal scaleX,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.scaleX",
                cancellationToken,
                cropperComponentId,
                scaleX);
        }

        /// <summary>
        /// Scale the image on the y-axis.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="scaleY">The scale ratio on the y-axis.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ScaleYAsync(
            [NotNull] Guid cropperComponentId,
            decimal scaleY,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.scaleY",
                cancellationToken,
                cropperComponentId,
                scaleY);
        }

        /// <summary>
        /// Change the aspect ratio of the crop box.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="aspectRatio">The new aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetAspectRatioAsync(
            [NotNull] Guid cropperComponentId,
            decimal aspectRatio,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setAspectRatio",
                cancellationToken,
                cropperComponentId,
                aspectRatio);
        }

        /// <summary>
        /// Set the canvas position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="setCanvasDataOptions">The new canvas data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetCanvasDataAsync(
            [NotNull] Guid cropperComponentId,
            SetCanvasDataOptions setCanvasDataOptions,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setCanvasData",
                cancellationToken,
                cropperComponentId,
                setCanvasDataOptions);
        }

        /// <summary>
        /// Set the crop box position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="cropBoxDataOptions">The new crop box data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetCropBoxDataAsync(
            [NotNull] Guid cropperComponentId,
            SetCropBoxDataOptions cropBoxDataOptions,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setCropBoxData",
                cancellationToken,
                cropperComponentId,
                cropBoxDataOptions);
        }

        /// <summary>
        /// Set the cropped area position and size with new data.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="setDataOptions">The new data.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDataAsync(
            [NotNull] Guid cropperComponentId,
            SetDataOptions setDataOptions,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setData",
                cancellationToken,
                cropperComponentId,
                setDataOptions);
        }

        /// <summary>
        /// Change the drag mode.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="dragMode">The new drag mode.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDragModeAsync(
            [NotNull] Guid cropperComponentId,
            DragMode dragMode,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setDragMode",
                cancellationToken,
                cropperComponentId,
                dragMode.ToEnumString());
        }

        /// <summary>
        /// Zoom the canvas with a relative ratio.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ZoomAsync(
            [NotNull] Guid cropperComponentId,
            decimal ratio,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.zoom",
                cancellationToken,
                cropperComponentId,
                ratio);
        }

        /// <summary>
        /// Zoom the canvas to an absolute ratio.
        /// </summary>
        /// <param name="cropperComponentId">The identifier of the cropper component.</param>
        /// <param name="ratio">The target ratio.</param>
        /// <param name="pivotX">The zoom pivot point X coordinate.</param>
        /// <param name="pivotY">The zoom pivot point Y coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask ZoomToAsync(
            [NotNull] Guid cropperComponentId,
            decimal ratio,
            decimal pivotX,
            decimal pivotY,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.zoomTo",
                cancellationToken,
                cropperComponentId,
                ratio,
                pivotX,
                pivotY);
        }

        /// <summary>
        /// Get the no conflict cropper class.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask NoConflictAsync(CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.noConflict",
                cancellationToken);
        }

        /// <summary>
        /// Change the default options.
        /// </summary>
        /// <param name="options">The new default options.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask SetDefaultsAsync(
            [NotNull] Options options,
            CancellationToken cancellationToken = default)
        {
            await TryLoadModuleAsync(cancellationToken);

            await _jsRuntime!.InvokeVoidAsync(
                "cropper.setDefaults",
                cancellationToken,
                options);
        }
    }
}
