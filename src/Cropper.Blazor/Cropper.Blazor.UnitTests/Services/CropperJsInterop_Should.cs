using Bogus;
using Bunit;
using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Xunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public class CropperJsInterop_Should : IDisposable
    {
        private readonly TestContext _testContext;
        private readonly ICropperJsInterop _cropperJsInterop;

        public CropperJsInterop_Should()
        {
            _testContext = new Faker<TestContext>()
                .Generate();

            _cropperJsInterop = new Faker<ICropperJsInterop>()
                .CustomInstantiator(f => new CropperJsInterop(_testContext.JSInterop.JSRuntime))
                .Generate();
        }

        [Fact]
        public async Task Verify_LoadCropperModuleAsync()
        {
            // assert
            VerifyLoadCropperModule();

            // act
            await _cropperJsInterop.LoadModuleAsync();
        }

        [Fact]
        public async Task Verify_InitCropperAsync()
        {
            // arrange
            ElementReference elementReference = new ElementReference(Guid.NewGuid().ToString());
            Options options = new Faker<Options>().Generate();
            ICropperComponentBase cropperComponentBase = new Faker<CropperComponentBase>().Generate();
            DotNetObjectReference<ICropperComponentBase> refToCropperComponentBase = new Faker<DotNetObjectReference<ICropperComponentBase>>()
                .CustomInstantiator(f => DotNetObjectReference.Create(cropperComponentBase));

            object[] expectedInitCropperMethodArguments = new object[]
            {
                elementReference,
                options,
                refToCropperComponentBase
            };

            // assert
            VerifyLoadCropperModule();

            _testContext.JSInterop
                .SetupVoid("cropper.initCropper", expectedInitCropperMethodArguments)
                .SetVoidResult();

            // act
            await _cropperJsInterop.InitCropperAsync(elementReference, options, refToCropperComponentBase);
        }

        private void VerifyLoadCropperModule()
        {
            _testContext.JSInterop
                .SetupModule(CropperJsInterop.PathToCropperModule);
        }

        public void Dispose()
        {
            _testContext.Dispose();
        }
    }
}
