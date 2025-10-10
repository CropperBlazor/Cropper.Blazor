using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.ModuleOptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseJsInteropService
    {
        private readonly NavigationManager _navigationManager;
        private readonly ICropperJsInteropOptions _cropperJsInteropOptions;

        /// <summary>
        /// 
        /// </summary>
        protected readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// 
        /// </summary>
        protected IJSObjectReference? Module = null;

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
        /// <param name="cropperJsInteropOptions">The <see cref="ICropperJsInteropOptions"/>.</param>
        public BaseJsInteropService(
            IJSRuntime jsRuntime,
            NavigationManager navigationManager,
            ICropperJsInteropOptions cropperJsInteropOptions)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _cropperJsInteropOptions = cropperJsInteropOptions;
        }

        /// <summary>
        /// Load JavaScript object into .NET.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        public async Task LoadModuleAsync(CancellationToken cancellationToken = default)
        {
            string globalPathToCropperModule = GetGlobalPathToCropperModule();

            Module = await _jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", cancellationToken, globalPathToCropperModule);
        }

        /// <summary>
        /// Finds path to the cropper module.
        /// </summary>
        /// <returns>The path to the cropper module.</returns>
        private string GetGlobalPathToCropperModule()
        {
            if (_cropperJsInteropOptions.IsActiveGlobalPath)
            {
                return _cropperJsInteropOptions.GlobalPathToCropperModule;
            }
            else
            {
                Uri baseUri = new(_navigationManager.BaseUri);
                string hostName = baseUri.GetHostName();

                return Path.Combine(hostName, _cropperJsInteropOptions.DefaultInternalPathToCropperModule);
            }
        }
    }
}
