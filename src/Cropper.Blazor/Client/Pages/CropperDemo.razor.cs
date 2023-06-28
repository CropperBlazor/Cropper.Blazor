using System.Reflection;
using System.Text.Json;
using Cropper.Blazor.Client.Components;
using Cropper.Blazor.Client.Enums;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.Client.Pages
{
    public partial class CropperDemo : IDisposable
    {
        [Inject] IBreakpointService BreakpointListener { get; set; } = null!;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        public CropperComponent? CropperComponent = null!;
        private CropperDataPreview? CropperDataPreview = null!;
        private GetSetCropperData? GetSetCropperData = null!;
        private Options Options = null!;
        private CropperFace CropperFace = CropperFace.Default;
        private decimal? ScaleXValue;
        private decimal? ScaleYValue;
        private decimal AspectRatio = 1.7777777777777777m;
        private bool IsEnableAspectRatioSettings;

        private string Src = "https://fengyuanchen.github.io/cropperjs/v2/picture.jpg";
        private bool IsErrorLoadImage { get; set; } = false;
        private bool IsAvaibleInitCropper { get; set; } = true;
        private readonly string _errorLoadImageSrc = "not-found-image.jpg";
        private Breakpoint Start;
        private Guid SubscriptionId;

        public Dictionary<string, object> InputAttributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "loading", "lazy" },
                { "test-Attribute", "123-test" }
            };

        protected override void OnInitialized()
        {
            Options = new Options()
            {
                Preview = ".img-preview",
                AspectRatio = (decimal)16 / 9,
                ViewMode = ViewMode.Vm0
            };
        }

        public async void OnCropEvent(JSEventData<CropEvent> cropJSEvent)
        {
            if (cropJSEvent?.Detail is not null)
            {
                ScaleXValue = cropJSEvent.Detail.ScaleX;
                ScaleYValue = cropJSEvent.Detail.ScaleY;

                decimal width = Math.Round(cropJSEvent.Detail.Width ?? 0);
                decimal height = Math.Round(cropJSEvent.Detail.Height ?? 0);

                if (width < GetSetCropperData!.CroppedDimensionsSettings.MinimumWidth
                    || height < GetSetCropperData!.CroppedDimensionsSettings.MinimumHeight
                    || width > GetSetCropperData!.CroppedDimensionsSettings.MaximumWidth
                    || height > GetSetCropperData!.CroppedDimensionsSettings.MaximumHeight
                  )
                {
                    CropperComponent!.SetData(new SetDataOptions
                    {
                        Width = Math.Max(
                            GetSetCropperData!.CroppedDimensionsSettings.MinimumWidth ?? 0M,
                            Math.Min(GetSetCropperData!.CroppedDimensionsSettings.MaximumWidth ?? 0M, width)),
                        Height = Math.Max(
                            GetSetCropperData!.CroppedDimensionsSettings.MinimumHeight ?? 0M,
                            Math.Min(GetSetCropperData!.CroppedDimensionsSettings.MaximumHeight ?? 0M, height)),

                    });
                }
                else
                {
                    await InvokeAsync(() =>
                    {
                        //JSRuntime!.InvokeVoidAsync("console.log", $"CropJSEvent {JsonSerializer.Serialize(cropJSEvent)}");
                        CropperDataPreview?.OnCropEvent(cropJSEvent.Detail);
                    });
                }
            }
        }

        public async void OnCropEndEvent(JSEventData<CropEndEvent> cropEndJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndEvent, {JsonSerializer.Serialize(cropEndJSEvent)}");

            //if (cropEndJSEvent?.Detail?.OriginalEvent is not null)
            //{
            //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
            //        "jsObject.getInstanceProperty",
            //        cropEndJSEvent.Detail.OriginalEvent, "clientX");

            //    await JSRuntime!.InvokeVoidAsync("console.log", $"CropEndJSEvent OriginalEvent clientX: {clientX}");
            //}
        }

        public async void OnCropStartEvent(JSEventData<CropStartEvent> cropStartJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartEvent, {JsonSerializer.Serialize(cropStartJSEvent)}");

            //if (cropStartJSEvent?.Detail?.OriginalEvent is not null)
            //{
            //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
            //        "jsObject.getInstanceProperty",
            //        cropStartJSEvent.Detail.OriginalEvent, "clientX");

            //    await JSRuntime!.InvokeVoidAsync("console.log", $"CropStartJSEvent OriginalEvent clientX: {clientX}");
            //}
        }

        public async void OnZoomEvent(JSEventData<ZoomEvent> zoomJSEvent)
        {
            if (zoomJSEvent.Detail is not null)
            {
                await InvokeAsync(() =>
                {
                    //JSRuntime!.InvokeVoidAsync("console.log", $"ZoomEvent {JsonSerializer.Serialize(zoomJSEvent)}");
                    GetSetCropperData!.OnZoomEvent(zoomJSEvent.Detail);
                });

                //if (zoomJSEvent.Detail.OriginalEvent is not null)
                //{
                //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
                //        "jsObject.getInstanceProperty",
                //        zoomJSEvent.Detail.OriginalEvent, "clientX");

                //    await JSRuntime!.InvokeVoidAsync("console.log", $"ZoomJSEvent clientX: {clientX}");
                //}
            }
        }

        public async void OnCropMoveEvent(JSEventData<CropMoveEvent> cropMoveJSEvent)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropMoveEvent, {JsonSerializer.Serialize(cropMoveJSEvent)}");

            //if (cropMoveJSEvent?.Detail?.OriginalEvent is not null)
            //{
            //    decimal clientX = await JSRuntime!.InvokeAsync<decimal>(
            //        "jsObject.getInstanceProperty",
            //        cropMoveJSEvent.Detail.OriginalEvent, "clientX");

            //    await JSRuntime!.InvokeVoidAsync("console.log", $"CropMoveJSEvent OriginalEvent clientX: {clientX}");
            //}
            CropBoxData cropBoxData = await CropperComponent!.GetCropBoxDataAsync();

            if (cropBoxData.Height != 0)
            {
                decimal aspectRatio = cropBoxData.Width / cropBoxData.Height;

                AspectRatio = aspectRatio;
                GetSetCropperData!.AspectRatioSettings.SetUpAspectRatio(aspectRatio);

                if (GetSetCropperData?.AspectRatioSettings?.MinAspectRatio is not null
                    || GetSetCropperData?.AspectRatioSettings?.MaxAspectRatio is not null)
                {
                    if (aspectRatio < GetSetCropperData!.AspectRatioSettings!.MinAspectRatio)
                    {
                        CropperComponent!.SetCropBoxData(new SetCropBoxDataOptions
                        {
                            Width = cropBoxData.Height * GetSetCropperData!.AspectRatioSettings.MinAspectRatio
                        });
                    }
                    else if (aspectRatio > GetSetCropperData!.AspectRatioSettings!.MaxAspectRatio)
                    {
                        CropperComponent!.SetCropBoxData(new SetCropBoxDataOptions
                        {
                            Width = cropBoxData.Height * GetSetCropperData!.AspectRatioSettings.MaxAspectRatio
                        });
                    }
                }
            }
            else
            {
                AspectRatio = 0;
                GetSetCropperData!.AspectRatioSettings.SetUpAspectRatio(0);
            }
        }

        public async void OnCropReadyEvent(JSEventData<CropReadyEvent> jSEventData)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropReadyJSEvent, {JsonSerializer.Serialize(jSEventData)}");
        }

        public async void OnLoadImageEvent()
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Image Is loaded");
        }

        public void OnErrorLoadImageEvent(ErrorEventArgs errorEventArgs)
        {
            IsErrorLoadImage = true;
            Destroy();
            StateHasChanged();
        }

        private void SetMoveDragMode()
        {
            CropperComponent?.SetDragMode(DragMode.Move);
        }

        private void SetCropDragMode()
        {
            CropperComponent?.SetDragMode(DragMode.Crop);
        }

        private void Zoom(decimal ratio)
        {
            CropperComponent?.Zoom(ratio);
        }

        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperComponent?.ZoomTo(ratio, pivotX, pivotY);
        }

        private void Move(decimal offsetX, decimal? offsetY)
        {
            CropperComponent?.Move(offsetX, offsetY);
        }

        public void MoveTo(decimal x, decimal? y)
        {
            CropperComponent?.MoveTo(x, y);
        }

        private void Rotate(decimal degree)
        {
            CropperComponent?.Rotate(degree);
        }

        private void ScaleX(decimal? scaleX)
        {
            CropperComponent?.ScaleX(scaleX ?? 0);
        }

        private void ScaleY(decimal? scaleY)
        {
            CropperComponent?.ScaleY(scaleY ?? 0);
        }

        public void Scale(decimal? scaleX, decimal? scaleY)
        {
            CropperComponent?.Scale(scaleX ?? 0, scaleY ?? 0);
        }

        private void Crop()
        {
            CropperComponent?.Crop();
        }

        private void Clear()
        {
            CropperComponent?.Clear();
        }

        private void Enable()
        {
            CropperComponent?.Enable();
        }

        private void Disable()
        {
            CropperComponent?.Disable();
        }

        private void Destroy()
        {
            CropperComponent?.Destroy();
            CropperComponent?.RevokeObjectUrlAsync(Src);
        }

        public void SetAspectRatio(decimal aspectRatio, bool isEnableAspectRatioSettings = false)
        {
            IsEnableAspectRatioSettings = isEnableAspectRatioSettings;

            if (aspectRatio != 0)
            {
                AspectRatio = aspectRatio;
            }

            CropperComponent?.SetAspectRatio(aspectRatio);
        }

        public void SetFreeAspectRatio() => SetAspectRatio(0, true);

        public void SetViewMode(ViewMode viewMode)
        {
            Options.ViewMode = viewMode;
            CropperComponent?.Destroy();
            CropperComponent?.InitCropper();
        }

        private void Reset()
        {
            CropperComponent?.Reset();
        }

        public async void GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            string croppedCanvasDataURL = await CropperComponent!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async void GetCroppedCanvasData(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            CroppedCanvas croppedCanvas = await CropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            string croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL");

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async void GetCroppedCanvasDataByPolygonFilter(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            CroppedCanvas croppedCanvas = await CropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            string croppedCanvasDataURL;

            if (CropperFace == CropperFace.Default)
            {
                croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL");
            }
            else if (CropperFace == CropperFace.Circle)
            {
                croppedCanvasDataURL = await JSRuntime!.InvokeAsync<string>("window.addClipPathEllipse", croppedCanvas!.JSRuntimeObjectRef);
            }
            else
            {
                IEnumerable<int> croppedPathToCanvasCropper = GetCroppedPathToCanvasCropper();

                croppedCanvasDataURL = await JSRuntime!.InvokeAsync<string>("window.addClipPathPolygon", croppedCanvas!.JSRuntimeObjectRef, croppedPathToCanvasCropper);
            }

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async Task InputFileChangeAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;

            if (imageFile != null)
            {
                string oldSrc = Src;

                Src = await CropperComponent!.GetImageUsingStreamingAsync(imageFile, imageFile.Size);

                IsAvaibleInitCropper = true;
                IsErrorLoadImage = false;

                CropperComponent?.Destroy();
                CropperComponent?.RevokeObjectUrlAsync(oldSrc);
            }
        }

        public async Task ReplaceImageAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;

            if (imageFile != null)
            {
                string oldSrc = Src;
                string src = await CropperComponent!.GetImageUsingStreamingAsync(imageFile, imageFile.Size);

                if (IsErrorLoadImage)
                {
                    IsAvaibleInitCropper = true;
                    IsErrorLoadImage = false;
                }
                else
                {
                    IsAvaibleInitCropper = false;
                }

                CropperComponent?.ReplaceAsync(src);
                CropperComponent?.RevokeObjectUrlAsync(oldSrc);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var subscriptionResult = await BreakpointListener.SubscribeAsync((breakpoint) =>
                {
                    InvokeAsync(StateHasChanged);
                });

                Start = subscriptionResult.Breakpoint;
                SubscriptionId = subscriptionResult.SubscriptionId;

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperComponent?.SetCropBoxData(cropBoxDataOptions);
        }

        public void SetData(SetDataOptions setDataOptions)
        {
            CropperComponent?.SetData(setDataOptions);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperComponent?.SetCanvasData(setCanvasDataOptions);
        }

        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await CropperComponent!.GetCropBoxDataAsync();
        }

        public async ValueTask<CropperData> GetDataAsync(bool rounded)
        {
            return await CropperComponent!.GetDataAsync(rounded);
        }

        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await CropperComponent!.GetContainerDataAsync();
        }

        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await CropperComponent!.GetImageDataAsync();
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await CropperComponent!.GetCanvasDataAsync();
        }

        public void OptionsChecked(string property, bool? newValue)
        {
            Type type = Options.GetType();
            PropertyInfo? propertyInfo = type!.GetProperty(property);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(Options, newValue, null);
                CropperComponent?.Destroy();
                CropperComponent?.InitCropper();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetCropperFace(CropperFace cropperFace)
        {
            CropperFace = cropperFace;
        }

        public string GetClassNameCropper() =>
            "img-container" + CropperFace switch
            {
                CropperFace.Default => string.Empty,
                CropperFace.Close => " cropper-face-close",
                CropperFace.Pentagon => " cropper-face-pentagon",
                CropperFace.Circle => " cropper-face-circle",
                CropperFace.Arrow => " cropper-face-arrow",
                _ => string.Empty,
            };

        public IEnumerable<int> GetCroppedPathToCanvasCropper() =>
            CropperFace switch
            {
                // That enumerable is equivalent css like that (the same for another paths) - clip-path: polygon(20% 0%, 0% 20%, 30% 50%, 0% 80%, 20% 100%, 50% 70%, 80% 100%, 100% 80%, 70% 50%, 100% 20%, 80% 0%, 50% 30%);
                CropperFace.Close => new List<int> { 20, 0, 0, 20, 30, 50, 0, 80, 20, 100, 50, 70, 80, 100, 100, 80, 70, 50, 100, 20, 80, 0, 50, 30 },
                CropperFace.Pentagon => new List<int> { 50, 0, 100, 38, 82, 100, 18, 100, 0, 38 },
                CropperFace.Arrow => new List<int> { 40, 0, 40, 40, 100, 40, 100, 60, 40, 60, 40, 100, 0, 50 },
                _ => throw new InvalidOperationException()
            };

        private void OpenCroppedCanvasDialog(string croppedCanvasDataURL)
        {
            DialogParameters parameters = new()
            {
                { "Src", croppedCanvasDataURL }
            };

            DialogOptions options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                DisableBackdropClick = true
            };

            _dialogService.Show<Shared.CroppedCanvasDialog>("CroppedCanvasDialog", parameters, options);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Destroy();
                CropperComponent?.DisposeAsync();
                JSRuntime!.InvokeVoidAsync("console.log", "Cropper Demo component is destroyed");
            }
        }
    }
}
