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
        public CropperComponent? CropperComponent = null!;
        private readonly string _errorLoadImageSrc = "not-found-image.jpg";
        private CropperDataPreview? CropperDataPreview = null!;
        private CropperFace CropperFace = CropperFace.Default;
        private ElementReference ElementReferencePreviewLg;
        private ElementReference ElementReferencePreviewMd;
        private ElementReference ElementReferencePreviewSm;
        private ElementReference ElementReferencePreviewXs;
        private GetSetCropperData? GetSetCropperData = null!;
        private bool IsFreeAspectRatioEnabled;
        private Options Options = null!;
        private decimal? ScaleXValue;
        private decimal? ScaleYValue;
        private string Src = "https://fengyuanchen.github.io/cropperjs/v2/picture.jpg";
        private Breakpoint Start;

        public Dictionary<string, object> InputAttributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "loading", "lazy" },
                { "test-Attribute", "123-test" },
                { "alt", "Cropper.Blazor demo image" }
            };

        [Inject] private IBrowserViewportService BrowserViewportService { get; set; } = null!;

        private bool IsAvailableInitCropper { get; set; } = true;
        //private decimal AspectRatio = 1.7777777777777777m;
        private bool IsErrorLoadImage { get; set; } = false;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask<CanvasData> GetCanvasDataAsync()
        {
            return await CropperComponent!.GetCanvasDataAsync();
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

        public async ValueTask<ContainerData> GetContainerDataAsync()
        {
            return await CropperComponent!.GetContainerDataAsync();
        }

        public async ValueTask<CropBoxData> GetCropBoxDataAsync()
        {
            return await CropperComponent!.GetCropBoxDataAsync();
        }

        public async void GetCroppedCanvasData(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            CroppedCanvas croppedCanvas = await CropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            string croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL", "image/png", 1);

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async void GetCroppedCanvasDataByPolygonFilter(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            CroppedCanvas croppedCanvas = await CropperComponent!.GetCroppedCanvasAsync(getCroppedCanvasOptions);
            string croppedCanvasDataURL;

            if (CropperFace == CropperFace.Default)
            {
                croppedCanvasDataURL = await croppedCanvas!.JSRuntimeObjectRef.InvokeAsync<string>("toDataURL", "image/png", 1);
            }
            else if (CropperFace == CropperFace.Circle)
            {
                croppedCanvasDataURL = await JSRuntime!.InvokeAsync<string>("window.getEllipseImage", croppedCanvas!.JSRuntimeObjectRef);
            }
            else
            {
                IEnumerable<int> croppedPathToCanvasCropper = GetCroppedPathToCanvasCropper();

                croppedCanvasDataURL = await JSRuntime!.InvokeAsync<string>("window.getPolygonImage", croppedCanvas!.JSRuntimeObjectRef, croppedPathToCanvasCropper);
            }

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public async void GetCroppedCanvasDataURL(GetCroppedCanvasOptions getCroppedCanvasOptions)
        {
            string croppedCanvasDataURL = await CropperComponent!.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);

            OpenCroppedCanvasDialog(croppedCanvasDataURL);
        }

        public IEnumerable<int> GetCroppedPathToCanvasCropper() =>
            CropperFace switch
            {
                // That enumerable is equivalent css like that (the same for another paths) - clip-path: polygon(20% 0%, 0% 20%, 30% 50%, 0% 80%, 20% 100%, 50% 70%, 80% 100%, 100% 80%, 70% 50%, 100% 20%, 80% 0%, 50% 30%);
                CropperFace.Close => [20, 0, 0, 20, 30, 50, 0, 80, 20, 100, 50, 70, 80, 100, 100, 80, 70, 50, 100, 20, 80, 0, 50, 30],
                CropperFace.Pentagon => [50, 0, 100, 38, 82, 100, 18, 100, 0, 38],
                CropperFace.Arrow => [40, 0, 40, 40, 100, 40, 100, 60, 40, 60, 40, 100, 0, 50],
                _ => throw new InvalidOperationException()
            };

        public async ValueTask<CropperData> GetDataAsync(bool rounded)
        {
            return await CropperComponent!.GetDataAsync(rounded);
        }

        public async ValueTask<ImageData> GetImageDataAsync()
        {
            return await CropperComponent!.GetImageDataAsync();
        }

        public async Task InputFileChangeAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;

            if (imageFile != null)
            {
                string oldSrc = Src;

                Src = await CropperComponent!.GetImageUsingStreamingAsync(imageFile, imageFile.Size);

                IsAvailableInitCropper = true;
                IsErrorLoadImage = false;

                CropperComponent?.Destroy();
                CropperComponent?.RevokeObjectUrlAsync(oldSrc);
            }
        }

        public void MoveTo(decimal x, decimal? y)
        {
            CropperComponent?.MoveTo(x, y);
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
                    decimal nWidth = Math.Max(GetSetCropperData!.CroppedDimensionsSettings.MinimumWidth ?? 0M,
                            Math.Min(GetSetCropperData!.CroppedDimensionsSettings.MaximumWidth ?? 0M, width));
                    decimal nHeight = Math.Max(GetSetCropperData!.CroppedDimensionsSettings.MinimumHeight ?? 0M,
                        Math.Min(GetSetCropperData!.CroppedDimensionsSettings.MaximumHeight ?? 0M, height));

                    if (!IsFreeAspectRatioEnabled)
                    {
                        if (nWidth == 0)
                        {
                            nWidth = nHeight * Options.AspectRatio ?? 0M;
                        }
                        else if (nHeight == 0)
                        {
                            nHeight = nWidth * Options.AspectRatio ?? 0M;
                        }
                    }

                    CropperComponent!.SetData(new SetDataOptions
                    {
                        Width = nWidth,
                        Height = nHeight
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

                Options.AspectRatio = aspectRatio;
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
                Options.AspectRatio = 0;
                GetSetCropperData!.AspectRatioSettings.SetUpAspectRatio(0);
            }
        }

        public async void OnCropReadyEvent(JSEventData<CropReadyEvent> jSEventData)
        {
            await JSRuntime!.InvokeVoidAsync("console.log", $"CropReadyJSEvent, {JsonSerializer.Serialize(jSEventData)}");

            await InvokeAsync(async () =>
            {
                ImageData imageData = await CropperComponent!.GetImageDataAsync();
                decimal initZoomRatio = imageData.Width / imageData.NaturalWidth;

                GetSetCropperData!.SetRatio(initZoomRatio);
            });
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

        public void OnErrorLoadImageEvent(ErrorEventArgs errorEventArgs)
        {
            IsErrorLoadImage = true;
            Destroy();
            StateHasChanged();
        }

        public async void OnLoadImageEvent()
        {
            await JSRuntime!.InvokeVoidAsync("console.log", "Image Is loaded");
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

        public async Task ReplaceImageAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            var imageFile = inputFileChangeEventArgs.File;

            if (imageFile != null)
            {
                string oldSrc = Src;
                string newSrc = await CropperComponent!.GetImageUsingStreamingAsync(imageFile, imageFile.Size);

                if (IsErrorLoadImage)
                {
                    IsAvailableInitCropper = true;
                    IsErrorLoadImage = false;
                }
                else
                {
                    IsAvailableInitCropper = false;
                }

                await Task.WhenAll(
                    CropperComponent!.ReplaceAsync(newSrc, false).AsTask(),
                    CropperComponent!.RevokeObjectUrlAsync(oldSrc).AsTask())
                    .ContinueWith(x =>
                    {
                        Src = newSrc;
                    });
            }
        }

        public void Scale(decimal? scaleX, decimal? scaleY)
        {
            CropperComponent?.Scale(scaleX ?? 0, scaleY ?? 0);
        }

        public void SetAspectRatio(decimal aspectRatio)
        {
            Options.AspectRatio = aspectRatio;
            IsFreeAspectRatioEnabled = aspectRatio == 0m;
            CropperComponent?.SetAspectRatio(aspectRatio);
        }

        public void SetCanvasData(SetCanvasDataOptions setCanvasDataOptions)
        {
            CropperComponent?.SetCanvasData(setCanvasDataOptions);
        }

        public void SetCropBoxData(SetCropBoxDataOptions cropBoxDataOptions)
        {
            CropperComponent?.SetCropBoxData(cropBoxDataOptions);
        }

        public void SetCropperFace(CropperFace cropperFace)
        {
            CropperFace = cropperFace;
        }

        public void SetData(SetDataOptions setDataOptions)
        {
            CropperComponent?.SetData(setDataOptions);
        }

        public void SetViewMode(ViewMode viewMode)
        {
            Options.ViewMode = viewMode;
            CropperComponent?.Destroy();
            CropperComponent?.InitCropper();
        }

        public void ZoomTo(decimal ratio, decimal pivotX, decimal pivotY)
        {
            CropperComponent?.ZoomTo(ratio, pivotX, pivotY);
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

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                Options.Preview = new ElementReference[]
                {
                    ElementReferencePreviewXs,
                    ElementReferencePreviewSm,
                    ElementReferencePreviewMd,
                    ElementReferencePreviewLg
                };
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await BrowserViewportService.SubscribeAsync(this, fireImmediately: true);

                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        public async ValueTask DisposeAsync() => await BrowserViewportService.UnsubscribeAsync(this);

        Guid IBrowserViewportObserver.Id { get; } = Guid.NewGuid();

        ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
        {
            ReportRate = 250,
            NotifyOnBreakpointOnly = true
        };

        Task IBrowserViewportObserver.NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
        {
            if (browserViewportEventArgs.IsImmediate)
            {
                Start = browserViewportEventArgs.Breakpoint;
            }

            return InvokeAsync(StateHasChanged);
        }

        protected override void OnInitialized()
        {
            Options = new Options()
            {
                //Preview = ".img-preview",
                AspectRatio = 1.7777777777777777m,
                ViewMode = ViewMode.Vm0
            };
        }
        private void Clear()
        {
            CropperComponent?.Clear();
        }

        private void Crop()
        {
            CropperComponent?.Crop();
        }

        private void Destroy()
        {
            CropperComponent?.Destroy();
            CropperComponent?.RevokeObjectUrlAsync(Src);
        }

        private void Disable()
        {
            CropperComponent?.Disable();
        }

        private void Enable()
        {
            CropperComponent?.Enable();
        }

        private void Move(decimal offsetX, decimal? offsetY)
        {
            CropperComponent?.Move(offsetX, offsetY);
        }

        private void OpenCroppedCanvasDialog(string croppedCanvasDataURL)
        {
            DialogParameters parameters = new()
            {
                { "Src", croppedCanvasDataURL }
            };

            DialogOptions options = new()
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                BackdropClick = false
            };

            _dialogService.Show<Shared.CroppedCanvasDialog>("CroppedCanvasDialog", parameters, options);
        }

        private void Reset()
        {
            CropperComponent?.Reset();
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

        private void SetCropDragMode()
        {
            CropperComponent?.SetDragMode(DragMode.Crop);
        }

        private void SetMoveDragMode()
        {
            CropperComponent?.SetDragMode(DragMode.Move);
        }
        private void Zoom(decimal ratio)
        {
            CropperComponent?.Zoom(ratio);
        }
    }
}
