using Cropper.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Cropper.Blazor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCropper(this IServiceCollection services)
        {
            // We use the custom method "TryAddTransient<ICropperJsInterop, CropperJsInterop>"
            // as same provided below for .NET 6 or greater because this method throws
            // a NullReferenceException if no such dependency currently exists
            // in the IServiceCollection for .NET 5
            #if NET5_0
                TryAddTransient<ICropperJsInterop, CropperJsInterop>(services);
            #elif NET6_0_OR_GREATER
                services.TryAddTransient<ICropperJsInterop, CropperJsInterop>();
            #endif

            return services;
        }

        #if NET5_0

        /// <summary>
        /// Adds the specified <typeparamref name="TService"/> as a <see cref="ServiceLifetime.Transient"/> service
        /// implementation type specified in <typeparamref name="TImplementation"/>
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
        private static void TryAddTransient<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(this IServiceCollection collection)
            where TService : class
            where TImplementation : class, TService
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            TryAddTransient(collection, typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Adds the specified <paramref name="service"/> as a <see cref="ServiceLifetime.Transient"/> service
        /// with the <paramref name="implementationType"/> implementation
        /// to the <paramref name="collection"/> if the service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
        /// <param name="service">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        private static void TryAddTransient(
            this IServiceCollection collection,
            Type service,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (implementationType == null)
            {
                throw new ArgumentNullException(nameof(implementationType));
            }

            var descriptor = ServiceDescriptor.Transient(service, implementationType);
            TryAdd(collection, descriptor);
        }

        /// <summary>
        /// Adds the specified <paramref name="descriptor"/> to the <paramref name="collection"/> if the
        /// service type hasn't already been registered.
        /// </summary>
        /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
        /// <param name="descriptor">The <see cref="ServiceDescriptor"/> to add.</param>
        private static void TryAdd(
            this IServiceCollection collection,
            ServiceDescriptor descriptor)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            int count = collection.Count;
            for (int i = 0; i < count; i++)
            {
                if (collection[i].ServiceType == descriptor.ServiceType)
                {
                    // Already added
                    return;
                }
            }

            collection.Add(descriptor);
        }

        #endif
    }
}
