using System;
using Cropper.Blazor.Events;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// The cropper component.
    /// </summary>
    public partial class CropperComponent
    {
        /// <summary>
        /// A shortcut to the load image event.
        /// </summary>
        [Parameter]
        public Action? OnLoadImageEvent { get; set; }

        /// <summary>
        /// A shortcut to the ready event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropReadyEvent>>? OnReadyEvent { get; set; }

        /// <summary>
        /// A shortcut to the crop event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropEvent>>? OnCropEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropstart event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropStartEvent>>? OnCropStartEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropend event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropEndEvent>>? OnCropEndEvent { get; set; }

        /// <summary>
        /// A shortcut to the cropmove event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<CropMoveEvent>>? OnCropMoveEvent { get; set; }

        /// <summary>
        /// A shortcut to the zoom event.
        /// </summary>
        [Parameter]
        public Action<JSEventData<ZoomEvent>>? OnZoomEvent { get; set; }

        /// <summary>
        /// A shortcut to the image loading error event.
        /// </summary>
        [Parameter]
        public Action<ErrorEventArgs>? OnErrorLoadImageEvent { get; set; }

        /// <summary>
        /// This event is fired when an image is loaded or called manually.
        /// </summary>
        /// <param name="progressEventArgs">
        ///     <para>If successful, outputs a <see cref="ProgressEventArgs"/> which is </para>
        ///     <para>generated from the data.</para>
        /// </param>
        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            if (IsAvailableInitCropper)
            {
                InitCropper();
            }
            else
            {
                OnLoadImageEvent?.Invoke();
            }
        }

        /// <summary>
        /// This event occurs if an error occurred while loading the image.
        /// </summary>
        /// <param name="errorEventArgs">The <see cref="ErrorEventArgs"/> used to send if it's not in a good state.</param>
        public void OnErrorLoadImage(ErrorEventArgs errorEventArgs)
        {
            OnErrorLoadImageEvent?.Invoke(errorEventArgs);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box changes.
        /// </summary>
        /// <param name="jSEventData">
        /// The <see cref="JSEventData{CropEvent}"/> containing data of the underlying
        /// JavaScript <c>crop</c> event.
        /// </param>
        [JSInvokable("CropperIsCroped")]
        public void CropperIsCroped(JSEventData<CropEvent> jSEventData)
        {
            OnCropEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box stops changing.
        /// </summary>
        /// <param name="jSEventData">
        /// The <see cref="JSEventData{CropEndEvent}"/> containing data of the underlying
        /// JavaScript <c>cropend</c> event.
        /// </param>
        [JSInvokable("CropperIsEnded")]
        public void CropperIsEnded(JSEventData<CropEndEvent> jSEventData)
        {
            OnCropEndEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box is changing.
        /// </summary>
        /// <param name="jSEventData">
        /// The <see cref="JSEventData{CropMoveEvent}"/> containing data of the underlying
        /// JavaScript <c>cropmove</c> event.
        /// </param>
        [JSInvokable("CropperIsMoved")]
        public void CropperIsMoved(JSEventData<CropMoveEvent> jSEventData)
        {
            OnCropMoveEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the canvas (image wrapper) or the crop box starts to change.
        /// </summary>
        /// <param name="jSEventData">
        /// The <see cref="JSEventData{CropStartEvent}"/> containing data of the underlying
        /// JavaScript <c>cropstart</c> event.
        /// </param>
        [JSInvokable("CropperIsStarted")]
        public void CropperIsStarted(JSEventData<CropStartEvent> jSEventData)
        {
            OnCropStartEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when a cropper instance starts to zoom in or zoom out its canvas (image wrapper).
        /// </summary>
        /// <param name="jSEventData">
        /// The <see cref="JSEventData{ZoomEvent}"/> containing data of the underlying
        /// JavaScript <c>zoom</c> event.
        /// </param>
        [JSInvokable("CropperIsZoomed")]
        public void CropperIsZoomed(JSEventData<ZoomEvent> jSEventData)
        {
            OnZoomEvent?.Invoke(jSEventData);
        }

        /// <summary>
        /// This event fires when the target image has been loaded and the cropper instance is ready for operating.
        /// </summary>
        /// <param name="jSEventData">
        /// The <see cref="JSEventData{CropReadyEvent}"/> containing the data of the
        /// underlying JavaScript <c>ready</c> event.
        /// </param>
        [JSInvokable]
        public void IsReady(JSEventData<CropReadyEvent> jSEventData)
        {
            OnReadyEvent?.Invoke(jSEventData);
        }
    }
}
