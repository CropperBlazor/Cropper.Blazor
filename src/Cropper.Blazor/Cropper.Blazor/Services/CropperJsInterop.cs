using Cropper.Blazor.ModuleOptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// This service listens to cropper js events and allows you to manage calls from cropper.
    /// </summary>
    public partial class CropperJsInterop : BaseJsInterop, ICropperJsInterop
    {
        private readonly IUrlImageInterop _urlImageInterop;

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
        /// <param name="cropperJsInteropOptions">The <see cref="ICropperJsInteropOptions"/>.</param>
        /// <param name="urlImageInterop">The <see cref="IUrlImageInterop"/>.</param>
        public CropperJsInterop(
            IJSRuntime jsRuntime,
            NavigationManager navigationManager,
            ICropperJsInteropOptions cropperJsInteropOptions,
            IUrlImageInterop urlImageInterop) : base(jsRuntime, navigationManager, cropperJsInteropOptions)
        {
            _urlImageInterop = urlImageInterop;
        }
    }
}
