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
        }

        [JSInvokable]
        public void CropperIsCroped(CropEvent cropEvent)
        {
            Console.WriteLine($"CropEvent, X: {cropEvent.X}, Y: {cropEvent.Y}, " +
                $"Height: {cropEvent.Height}, Width: {cropEvent.Width}, " +
                $"ScaleX: {cropEvent.ScaleX}, ScaleY: {cropEvent.ScaleY}, Rotate: {cropEvent.Rotate}");
        }

        [JSInvokable]
        public void CropperIsEnded(CropEndEvent cropEndEvent)
        {
            Console.WriteLine($"CropEndEvent, {cropEndEvent.ActionEvent}");
        }

        [JSInvokable]
        public void CropperIsMoved(CropMoveEvent cropMoveEvent)
        {
            Console.WriteLine($"CropMoveEvent, {cropMoveEvent.ActionEvent}");
        }

        [JSInvokable]
        public void CropperIsStarted(CropStartEvent cropStartEvent)
        {
            Console.WriteLine($"CropStartEvent, {cropStartEvent.ActionEvent}");
        }

        [JSInvokable]
        public void CropperIsZoomed(ZoomEvent zoomEvent)
        {
            Console.WriteLine($"ZoomEvent, OldRatio: {zoomEvent.OldRatio}, Ratio: {zoomEvent.Ratio}");
        }

        [JSInvokable]
        public void IsReady(CropReadyEvent cropReadyEvent)
        {
            Console.WriteLine("IsReady");
        }
    }
}
