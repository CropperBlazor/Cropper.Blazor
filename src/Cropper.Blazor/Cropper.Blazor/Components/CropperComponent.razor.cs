using Cropper.Blazor.Base;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Components
{
    public partial class CropperComponent : ICropperComponentBase
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; }

        private ElementReference imageReference;

        [Parameter]
        public Options Options { get; set; }

        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public Action? OnLoadImageEvent { get; set; }

        [Parameter]
        public Action<CropReadyEvent>? OnReadyEvent { get; set; }

        [Parameter]
        public Action<CropEvent>? OnCropEvent { get; set; }

        [Parameter]
        public Action<CropStartEvent>? OnCropStartEvent { get; set; }

        [Parameter]
        public Action<CropEndEvent>? OnCropEndEvent { get; set; }

        [Parameter]
        public Action<CropMoveEvent>? OnCropMoveEvent { get; set; }

        [Parameter]
        public Action<ZoomEvent>? OnZoomEvent { get; set; }

        protected override void OnInitialized()
        {

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CropperJsIntertop.LoadAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void OnLoadImage(ProgressEventArgs progressEventArgs)
        {
            ICropperComponentBase cropperComponentBase = this;
            CropperJsIntertop.InitCropper(imageReference, Options, DotNetObjectReference.Create(cropperComponentBase));
            OnLoadImageEvent?.Invoke();
        }

        [JSInvokable]
        public void CropperIsCroped(CropEvent cropEvent)
        {
            OnCropEvent?.Invoke(cropEvent);
        }

        [JSInvokable]
        public void CropperIsEnded(CropEndEvent cropEndEvent)
        {
            OnCropEndEvent?.Invoke(cropEndEvent);
        }

        [JSInvokable]
        public void CropperIsMoved(CropMoveEvent cropMoveEvent)
        {
            OnCropMoveEvent?.Invoke(cropMoveEvent);
        }

        [JSInvokable]
        public void CropperIsStarted(CropStartEvent cropStartEvent)
        {
            OnCropStartEvent?.Invoke(cropStartEvent);
        }

        [JSInvokable]
        public void CropperIsZoomed(ZoomEvent zoomEvent)
        {
            OnZoomEvent?.Invoke(zoomEvent);
        }

        [JSInvokable]
        public void IsReady(CropReadyEvent cropReadyEvent)
        {
            OnReadyEvent?.Invoke(cropReadyEvent);
        }
    }
}
