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

namespace Cropper.Blazor.Client.Pages
{
    public partial class CropperDemo
    {
        private Options options;
        private decimal? x;
        private decimal? y;
        private decimal? height;
        private decimal? width;
        private decimal? rotate;
        private decimal? scaleX;
        private decimal? scaleY;

        protected override void OnInitialized()
        {
            options = new Options()
            {
                Preview = ".img-preview",
                AspectRatio = (decimal)16 / 9,
                //DragMode =  DragMode.Crop.ToString()
            };
        }

        public void OnCropEndEvent(CropEndEvent cropEndEvent)
        {
            Console.WriteLine($"CropEndEvent, {cropEndEvent.ActionEvent}");
        }

        public void OnCropStartEvent(CropStartEvent cropStartEvent)
        {
            Console.WriteLine($"CropStartEvent, {cropStartEvent.ActionEvent}");
        }

        public void OnZoomEvent(ZoomEvent zoomEvent)
        {
            Console.WriteLine($"ZoomEvent, OldRatio: {zoomEvent.OldRatio}, Ratio: {zoomEvent.Ratio}");
        }

        public void OnCropMoveEvent(CropMoveEvent cropMoveEvent)
        {
            Console.WriteLine($"CropMoveEvent, {cropMoveEvent.ActionEvent}");
        }

        public void OnCropReadyEvent(CropReadyEvent cropReadyEvent)
        {
            Console.WriteLine("Cropper Is Ready");
        }

        public void OnLoadImageEvent()
        {
            Console.WriteLine("Image Is loaded");
        }

        public void OnCropEvent(CropEvent cropEvent)
        {
            x = cropEvent.X;
            y = cropEvent.Y;
            width = cropEvent.Width;
            height = cropEvent.Height;
            rotate = cropEvent.Rotate;
            scaleX = cropEvent.ScaleX;
            scaleY = cropEvent.ScaleY;
            Console.WriteLine($"CropEvent, X: {cropEvent.X}, Y: {cropEvent.Y}, " +
                $"Height: {cropEvent.Height}, Width: {cropEvent.Width}, " +
                $"ScaleX: {cropEvent.ScaleX}, ScaleY: {cropEvent.ScaleY}, Rotate: {cropEvent.Rotate}");
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
