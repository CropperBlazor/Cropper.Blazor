using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Cropper.Blazor.UnitTests
{
    internal sealed class ServiceCollectionVerifier
    {
        private readonly Mock<IServiceCollection> _serviceCollectionMock;

        public ServiceCollectionVerifier(Mock<IServiceCollection> serviceCollectionMock)
        {
            _serviceCollectionMock = serviceCollectionMock;
        }

        public void ContainsSingletonService<TService, TInstance>()
        {
            IsRegistered<TService, TInstance>(ServiceLifetime.Singleton);
        }

        public void ContainsTransientService<TService, TInstance>()
        {
            IsRegistered<TService, TInstance>(ServiceLifetime.Transient);
        }

        public void ContainsScopedService<TService, TInstance>()
        {
            IsRegistered<TService, TInstance>(ServiceLifetime.Scoped);
        }

        private void IsRegistered<TService, TInstance>(ServiceLifetime lifetime)
        {
            _serviceCollectionMock
                .Verify(serviceCollection => serviceCollection.Add(
                    It.Is<ServiceDescriptor>(serviceDescriptor => serviceDescriptor.Is<TService, TInstance>(lifetime))));

        }
    }
}
