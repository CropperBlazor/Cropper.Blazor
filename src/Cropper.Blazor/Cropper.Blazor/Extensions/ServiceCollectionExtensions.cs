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
        /// Adds a CropperJsInterop as a Transient instance.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>Continues the <see cref="IServiceCollection"/> chain.</returns>
        public static IServiceCollection AddCropper(this IServiceCollection services)
        {
            services.TryAddTransient<ICropperJsInterop, CropperJsInterop>();
            return services;
        }
    }
}
