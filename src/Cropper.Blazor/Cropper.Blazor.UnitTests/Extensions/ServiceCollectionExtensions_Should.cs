using Cropper.Blazor.Extensions;
using Cropper.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Extensions
{
    public class ServiceCollectionExtensions_Should
    {
        private readonly ServiceCollectionMock _serviceCollectionMock;

        public ServiceCollectionExtensions_Should()
        {
            Mock<IServiceCollection> serviceCollection = new Mock<IServiceCollection>();
            serviceCollection.Object.AddCropper();
            _serviceCollectionMock = new ServiceCollectionMock(serviceCollection);
        }

        [Fact]
        public void Verify_Cropper_Service_Is_Registered()
        {
            // assert
            _serviceCollectionMock.ContainsTransientService<ICropperJsInterop, CropperJsInterop>();
        }
    }
}
