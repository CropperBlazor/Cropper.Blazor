using Cropper.Blazor.ModuleOptions;
using Cropper.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Cropper.Blazor.Extensions
{
    /// <summary>
    /// Service collection extensions class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a see <see cref="CropperJsInterop"/> as a Singleton instance.
        /// </summary>
        /// <param name="services">Continues the <see cref="IServiceCollection"/> chain.</param>
        /// <param name="cropperJsInteropOptions">Continues the <see cref="CropperJsInteropOptions"/> chain.
        /// When option is default (null) then uses internal path with default cropper JavaScript interop options.</param>
        /// <returns>Continues the <see cref="IServiceCollection"/> chain.</returns>
        public static IServiceCollection AddCropper(this IServiceCollection services, CropperJsInteropOptions? cropperJsInteropOptions = null)
        {
            services.AddSingleton<ICropperJsInteropOptions, CropperJsInteropOptions>(services => cropperJsInteropOptions ?? new CropperJsInteropOptions());

            services.TryAddSingleton<ICropperJsInterop, CropperJsInterop>();

            return services;
        }
    }
}
