using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Pages
{
    public partial class CropperDemo : ICropperComponentBase
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; }
        private ElementReference imageReference;
        private Options options;

        protected override void OnInitialized()
        {
            options = new Options()
            {
                Preview = ".img-preview",
                AspectRatio = 16f / 9
            };
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
            CropperJsIntertop.InitCropper(imageReference, options, DotNetObjectReference.Create(cropperComponentBase));
        }

        [JSInvokable]
        public void CropperIsCroped(EventArgs eventArgs)
        {
            Console.WriteLine("CropperIsCroped");
        }

        [JSInvokable]
        public void CropperIsEnded(EventArgs eventArgs)
        {
            Console.WriteLine("CropperIsEnded");
        }

        [JSInvokable]
        public void CropperIsMoved(EventArgs eventArgs)
        {
            Console.WriteLine("CropperIsMoved");
        }

        [JSInvokable]
        public void CropperIsStarted(EventArgs eventArgs)
        {
            Console.WriteLine("CropperIsStarted");
        }

        [JSInvokable]
        public void CropperIsZoomed(EventArgs eventArgs)
        {
            Console.WriteLine("CropperIsZoomed");
        }

        [JSInvokable]
        public void IsReady(EventArgs eventArgs)
        {
            Console.WriteLine("IsReady");
        }
    }
}
