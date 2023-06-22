using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Cropper.Blazor.Testing
{
    public sealed class ServiceCollectionMock
    {
        private readonly ServiceCollectionVerifier _serviceCollectionVerifier;

        public ServiceCollectionMock(Mock<IServiceCollection> serviceCollectionMock)
        {
            _serviceCollectionVerifier = new ServiceCollectionVerifier(serviceCollectionMock);
        }

        public void TryContainsSingletonService<TService, TInstance>()
        {
            _serviceCollectionVerifier.TryContainsSingletonService<TService, TInstance>();
        }

        public void ContainsSingletonService<TService, TInstance>()
        {
            _serviceCollectionVerifier.ContainsSingletonService<TService, TInstance>();
        }

        public void TryContainsTransientService<TService, TInstance>()
        {
            _serviceCollectionVerifier.TryContainsTransientService<TService, TInstance>();
        }

        public void TryContainsScopedService<TService, TInstance>()
        {
            _serviceCollectionVerifier.TryContainsScopedService<TService, TInstance>();
        }
    }
}
