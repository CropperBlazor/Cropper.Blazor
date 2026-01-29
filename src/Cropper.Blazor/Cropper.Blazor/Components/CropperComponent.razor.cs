using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// A Blazor component that provides image and canvas cropping functionality
    /// via JavaScript interop, wrapping the underlying Cropper.js behavior.
    /// </summary>
    public partial class CropperComponent : ICropperComponentBase, IAsyncDisposable, IDisposable
    {
        [Inject] ICropperJsInterop CropperJsIntertop { get; set; } = null!;

        /// <summary>
        /// Gets a reference to the img HTML element rendered by the component.
        /// </summary>
        private ElementReference? ImageReference;

        /// <summary>
        /// Gets a reference to the canvas HTML element rendered by the component.
        /// </summary>
        private ElementReference? CanvasReference;

        /// <summary>
        /// The unique identifier of the cropper component.
        /// </summary>
        private Guid CropperComponentId;

        /// <summary>
        /// The options for cropping. Check out the available <see cref="Models.Options"/>.
        /// </summary>
        [Parameter]
        public Options Options { get; set; } = new Options();

        /// <summary>
        /// Specifies the path to the image.
        /// </summary>
        [Parameter]
        public string Src { get; set; } = null!;

        /// <summary>
        /// Specifies the target element for cropping, the default value is <see cref="CropperComponentType.Image"/>.
        /// In addition, for <see cref="CropperComponentType.Canvas"/> type requires manual uploading of images into canvas HTMl element, including error handling.
        /// </summary>
        [Parameter]
        public CropperComponentType CropperComponentType { get; set; } = CropperComponentType.Image;

        /// <summary>
        /// Specifies the path to the image when loading from src fails.
        /// </summary>
        [Parameter]
        public string ErrorLoadImageSrc { get; set; } = null!;

        /// <summary>
        /// User class names for error image, separated by space.
        /// </summary>
        [Parameter]
        public string ErrorLoadImageClass { get; set; } = null!;

        /// <summary>
        /// Returns the state of image loading.
        /// </summary>
        [Parameter]
        public bool IsErrorLoadImage { get; set; }

        /// <summary>
        /// Content is shown instead of the default error image.
        /// </summary>
        [Parameter]
        public RenderFragment? ErrorLoadImageContent { get; set; }

        /// <summary>
        /// Responsible for allowing the initialization of the cropper after a successful image download, the default is always allowed (true).
        /// In addition, it should be used to disable re-initialization (replace image) of cropper after successful image load when set to false.
        /// </summary>
        [Parameter]
        public bool IsAvailableInitCropper { get; set; } = true;

        /// <summary>
        /// User class names, separated by space.
        /// </summary>
        [Parameter]
        public string Class { get; set; } = null!;

        /// <summary> 
        /// Captures all additional attributes passed to the component that do not match declared [Parameter] properties.
        /// These attributes can be applied ("splatted") onto a rendered HTML element using the Razor `@attributes` directive.
        /// You can pass standard Blazor event handlers (like `@onclick`, `@oninput`, etc.) in this dictionary as well.
        /// The supported DOM events are defined in <see cref="Microsoft.AspNetCore.Components.Web.EventHandlers"/> via <see cref="EventHandlerAttribute"/>.
        /// The dictionary key should match the event name (e.g., `onclick`, `oninput`) or any valid HTML attribute.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = null!;

        private bool IsRendered = false;

        /// <summary>
        /// Method invoked after each time the component has been rendered. Note that the component does
        /// not automatically re-render after the completion of any returned <see cref="Task"/>, because
        /// that would cause an infinite render loop.
        /// </summary>
        /// <param name="firstRender">
        /// Set to <c>true</c> if this is the first time <see cref="OnAfterRenderAsync(bool)"/> has been invoked
        /// on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        /// <remarks>
        /// The <see cref="OnAfterRenderAsync(bool)"/> lifecycle methods
        /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
        /// Use the <paramref name="firstRender"/> parameter to ensure that initialization work is only performed
        /// once.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsRendered = true;

                await CropperJsIntertop!.TryLoadModuleAsync();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Called when initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            CropperComponentId = Guid.NewGuid();
        }

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            if (!IsRendered && CropperJsIntertop.IsBlazorServer)
            {
                return;
            }

            ElementReference? cropperElementReference = GetCropperElementReference();

            if (cropperElementReference.HasValue)
            {
                await DestroyAsync();
            }
        }

        /// <summary>
        /// Called to dispose this instance and internal services.
        /// </summary>
        public void Dispose()
        {
            if (!IsRendered && CropperJsIntertop.IsBlazorServer)
            {
                return;
            }

            ElementReference? cropperElementReference = GetCropperElementReference();

            if (cropperElementReference.HasValue)
            {
                Destroy();
            }
        }
    }
}
