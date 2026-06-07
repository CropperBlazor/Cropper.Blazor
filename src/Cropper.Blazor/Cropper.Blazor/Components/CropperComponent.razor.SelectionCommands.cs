using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cropper.Blazor.Models;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent
    {
        /// <summary>
        /// Reset the cropper selection to its initial state.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ResetSelection(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ResetSelectionAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Center the cropper selection within the canvas.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void CenterSelection(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.CenterSelectionAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Clear the cropper selection.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ClearSelection(CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ClearSelectionAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Move the cropper selection to an absolute point.
        /// </summary>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void MoveSelectionTo(decimal x, decimal y, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.MoveSelectionToAsync(CropperComponentId, x, y, cancellationToken);
        }

        /// <summary>
        /// Change the cropper selection position and size.
        /// </summary>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="width">The selection width.</param>
        /// <param name="height">The selection height.</param>
        /// <param name="aspectRatio">The optional selection aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ChangeSelection(
            decimal x,
            decimal y,
            decimal width,
            decimal height,
            decimal? aspectRatio = null,
            CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ChangeSelectionAsync(CropperComponentId, x, y, width, height, aspectRatio, cancellationToken);
        }

        /// <summary>
        /// Change a Cropper.js v2 selection by zero-based selection index.
        /// </summary>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="width">The selection width.</param>
        /// <param name="height">The selection height.</param>
        /// <param name="aspectRatio">The optional selection aspect ratio.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void ChangeSelectionByIndex(int selectionIndex, decimal x, decimal y, decimal width, decimal height, decimal? aspectRatio = null, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.ChangeSelectionByIndexAsync(CropperComponentId, selectionIndex, x, y, width, height, aspectRatio, cancellationToken);
        }

        /// <summary>
        /// Create a new Cropper.js v2 selection when multiple selection mode is enabled.
        /// </summary>
        /// <param name="x">The x-axis coordinate.</param>
        /// <param name="y">The y-axis coordinate.</param>
        /// <param name="width">The selection width.</param>
        /// <param name="height">The selection height.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void CreateSelection(decimal x, decimal y, decimal width, decimal height, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.CreateSelectionAsync(CropperComponentId, x, y, width, height, cancellationToken);
        }

        /// <summary>
        /// Remove a Cropper.js v2 selection by zero-based selection index.
        /// </summary>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void RemoveSelectionByIndex(int selectionIndex, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.RemoveSelectionByIndexAsync(CropperComponentId, selectionIndex, cancellationToken);
        }

        /// <summary>
        /// Set a Cropper.js v2 selection figure by zero-based selection index.
        /// </summary>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="shape">The selection figure.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        public void SetSelectionShapeByIndex(int selectionIndex, string shape, CancellationToken cancellationToken = default)
        {
            CropperJsIntertop!.SetSelectionShapeByIndexAsync(CropperComponentId, selectionIndex, shape, cancellationToken);
        }

        /// <summary>
        /// Get the number of Cropper.js v2 selection elements.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> containing selection count.</returns>
        public ValueTask<int> GetSelectionCountAsync(CancellationToken cancellationToken = default)
        {
            return CropperJsIntertop!.GetSelectionCountAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Get the current Cropper.js v2 selection elements as a Blazor collection.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> containing the current selections.</returns>
        public ValueTask<IReadOnlyList<CropperSelectionData>> GetSelectionsDataAsync(CancellationToken cancellationToken = default)
        {
            return CropperJsIntertop!.GetSelectionsDataAsync(CropperComponentId, cancellationToken);
        }

        /// <summary>
        /// Get a canvas element reference from a Cropper.js v2 selection by zero-based index.
        /// </summary>
        /// <param name="selectionIndex">The zero-based selection index.</param>
        /// <param name="getCroppedCanvasOptions">The <see cref="GetCroppedCanvasOptions"/> used to get a selection canvas.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> containing a JavaScript object reference to the selection canvas element.</returns>
        public ValueTask<IJSObjectReference?> GetSelectionCanvasReferenceByIndexAsync(
            int selectionIndex,
            GetCroppedCanvasOptions getCroppedCanvasOptions,
            CancellationToken cancellationToken = default)
        {
            return CropperJsIntertop!.GetSelectionCanvasReferenceByIndexAsync(
                CropperComponentId,
                selectionIndex,
                getCroppedCanvasOptions,
                cancellationToken);
        }
    }
}
