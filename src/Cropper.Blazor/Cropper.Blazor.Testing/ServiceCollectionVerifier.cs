using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Cropper.Blazor.Testing
{
    public sealed class ServiceCollectionVerifier
    {
        private readonly Mock<IServiceCollection> _serviceCollectionMock;

        public ServiceCollectionVerifier(Mock<IServiceCollection> serviceCollectionMock)
        {
            _serviceCollectionMock = serviceCollectionMock;
        }

        public void TryContainsSingletonService<TService, TInstance>()
        {
            TryIsRegistered<TService, TInstance>(ServiceLifetime.Singleton);
        }

        public void ContainsSingletonService<TService, TInstance>()
        {
            IsRegistered<TService, TInstance>(ServiceLifetime.Singleton);
        }

        public void TryContainsTransientService<TService, TInstance>()
        {
            TryIsRegistered<TService, TInstance>(ServiceLifetime.Transient);
        }

        public void TryContainsScopedService<TService, TInstance>()
        {
            TryIsRegistered<TService, TInstance>(ServiceLifetime.Scoped);
        }

        private void TryIsRegistered<TService, TInstance>(ServiceLifetime lifetime)
        {
            _serviceCollectionMock
                .Verify(serviceCollection => serviceCollection.Add(
                    It.Is<ServiceDescriptor>(serviceDescriptor => serviceDescriptor.TryIs<TService, TInstance>(lifetime))));
        }

        private void IsRegistered<TService, TInstance>(ServiceLifetime lifetime)
        {
            _serviceCollectionMock
                .Verify(serviceCollection => serviceCollection.Add(
                    It.Is<ServiceDescriptor>(serviceDescriptor => serviceDescriptor.Is<TService, TInstance>(lifetime))));
        }
    }
}
