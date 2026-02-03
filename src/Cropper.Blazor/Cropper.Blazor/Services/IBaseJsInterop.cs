using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cropper.Blazor.Services
{
    /// <summary>
    /// Provides the metadata of a IBaseJsInterop.
    /// </summary>
    public interface IBaseJsInterop : IAsyncDisposable
    {
        /// <summary>
        /// Determines whether the application is running in a Blazor Server environment
        /// by checking if the JavaScript runtime type is RemoteJSRuntime.
        /// </summary>
        /// <returns>
        /// True if running on Blazor Server; otherwise, false.
        /// </returns>
        bool IsBlazorServer { get; }

        /// <summary>
        /// Try load JavaScript object into .NET when module empty.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task TryLoadModuleAsync(CancellationToken cancellationToken = default);
    }
}
