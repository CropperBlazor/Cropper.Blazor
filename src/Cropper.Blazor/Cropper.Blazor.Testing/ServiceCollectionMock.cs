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

        public void ContainsSingletonService<TService, TInstance>()
        {
            _serviceCollectionVerifier.ContainsSingletonService<TService, TInstance>();
        }

        public void ContainsTransientService<TService, TInstance>()
        {
            _serviceCollectionVerifier.ContainsTransientService<TService, TInstance>();
        }

        public void ContainsScopedService<TService, TInstance>()
        {
            _serviceCollectionVerifier.ContainsTransientService<TService, TInstance>();
        }
    }
}
