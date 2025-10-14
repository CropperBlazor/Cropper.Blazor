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
        /// Try load JavaScript object into .NET when module empty.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        Task TryLoadModuleAsync(CancellationToken cancellationToken = default);
    }
}
