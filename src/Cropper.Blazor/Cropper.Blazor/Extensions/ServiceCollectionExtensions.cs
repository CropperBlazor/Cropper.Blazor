﻿using System;
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
        /// Adds a see <see cref="CropperJsInterop"/> as a Scoped instance.
        /// </summary>
        /// <param name="services">Continues the <see cref="IServiceCollection"/> chain.</param>
        /// <param name="cropperJsInteropOptions">Continues the <see cref="CropperJsInteropOptions"/> chain.
        /// When option is default (null) then uses internal path with default cropper JavaScript interop options.</param>
        /// <returns>Continues the <see cref="IServiceCollection"/> chain.</returns>
        public static IServiceCollection AddCropper(this IServiceCollection services, CropperJsInteropOptions? cropperJsInteropOptions = null)
        {
            CropperJsInteropOptions? options = cropperJsInteropOptions ?? new CropperJsInteropOptions();
            Func<IServiceProvider, CropperJsInteropOptions> funcServiceProvider = (IServiceProvider serviceProvider) => options;

            services.AddSingleton<ICropperJsInteropOptions, CropperJsInteropOptions>(funcServiceProvider);
            services.TryAddScoped<ICropperJsInterop, CropperJsInterop>();

            return services;
        }
    }
}
