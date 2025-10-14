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
    /// Provides a base implementation for JavaScript interop services used in Cropper.Blazor components.
    /// <br/>
    /// Handles loading of JS modules, managing interop references, and resolving paths to Cropper-related scripts.
    /// </summary>
    public abstract class BaseJsInterop : IBaseJsInterop
    {
        private readonly NavigationManager _navigationManager;
        private readonly ICropperJsInteropOptions _cropperJsInteropOptions;

        /// <summary>
        /// JavaScript runtime interface for invoking JS functions from .NET.
        /// </summary>
        protected readonly IJSRuntime _jsRuntime;

        /// <summary>
        /// Reference to the loaded JavaScript module.
        /// </summary>
        protected IJSObjectReference? Module = null;

        /// <summary>
        /// Implementation of the constructor.
        /// </summary>
        /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
        /// <param name="navigationManager">The <see cref="NavigationManager"/>.</param>
        /// <param name="cropperJsInteropOptions">The <see cref="ICropperJsInteropOptions"/>.</param>
        public BaseJsInterop(
            IJSRuntime jsRuntime,
            NavigationManager navigationManager,
            ICropperJsInteropOptions cropperJsInteropOptions)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _cropperJsInteropOptions = cropperJsInteropOptions;
        }

        /// <summary>
        /// Try load JavaScript object into .NET when module empty.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        public async Task TryLoadModuleAsync(CancellationToken cancellationToken = default)
        {
            if (Module is null)
            {
                await LoadModuleAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Load JavaScript object into .NET.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.
        /// </param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        private async Task LoadModuleAsync(CancellationToken cancellationToken = default)
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

        /// <summary>
        /// Called to dispose js module.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (Module is not null)
            {
                await Module.DisposeAsync();
            }

            Module = null;
        }

        /// <summary>
        /// Called to dispose this instance.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing any asynchronous operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }
    }
}
