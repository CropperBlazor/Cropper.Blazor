using System.Collections.Generic;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.ModuleOptions;
using Cropper.Blazor.Services;
using Cropper.Blazor.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Extensions
{
    public class ServiceCollectionExtensions_Should
    {
        private ServiceCollectionMock ServiceCollectionMock = null!;
        private readonly Mock<IServiceCollection> ServiceCollection = new();

        [Theory, MemberData(nameof(TestData_AddCropper_Service))]
        public void Verify_Cropper_Service_Is_Registered(CropperJsInteropOptions? cropperJsInteropOptions)
        {
            // act
            ServiceCollection.Object.AddCropper(cropperJsInteropOptions);

            // assert
            ServiceCollectionMock = new(ServiceCollection);
            ServiceCollectionMock.ContainsSingletonService<ICropperJsInteropOptions, CropperJsInteropOptions>();
            ServiceCollectionMock.TryContainsTransientService<ICropperJsInterop, CropperJsInterop>();
        }

        public static IEnumerable<object[]> TestData_AddCropper_Service()
        {
            yield return WrapArgs(null);

            yield return WrapArgs(new CropperJsInteropOptions());

            static object[] WrapArgs(
                CropperJsInteropOptions? cropperJsInteropOptions)
                => new object[]
                {
                    cropperJsInteropOptions!
                };
        }
    }
}
