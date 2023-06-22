using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Bunit;
using Bunit.TestDoubles;
using Cropper.Blazor.ModuleOptions;
using Cropper.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public class LoadCropperModule_Should : IDisposable
    {
        private readonly TestContext _testContext;
        private ICropperJsInterop _cropperJsInterop;
        private const string PathToCropperModule = "_content/Cropper.Blazor/cropperJsInterop.min.js";
        private static string DefaultPathToCropperModule => Path.Combine("http:localhost", PathToCropperModule);

        public LoadCropperModule_Should()
        {
            _testContext = new Faker<TestContext>()
                .Generate();
        }

        [Theory, MemberData(nameof(TestData_LoadCropperModule))]
        public async Task Verify_LoadCropperModuleAsync(
            string pathToCropperModule,
            CropperJsInteropOptions cropperJsInteropOptions,
            string expectedPathToCropperModule)
        {
            // arrange
            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();

            _cropperJsInterop = new Faker<ICropperJsInterop>()
                .CustomInstantiator(f => new CropperJsInterop(_testContext.JSInterop.JSRuntime, fakeNavigationManager, cropperJsInteropOptions))
                .Generate();

            fakeNavigationManager.NavigateTo(pathToCropperModule);

            // assert
            VerifyLoadCropperModule(expectedPathToCropperModule);

            // act
            await _cropperJsInterop.LoadModuleAsync();
        }

        public static IEnumerable<object[]> TestData_LoadCropperModule()
        {
            CropperJsInteropOptions cropperJsInteropOptions = new();

            yield return WrapArgs("http://localhost", cropperJsInteropOptions, Path.Combine("http:localhost", PathToCropperModule));
            yield return WrapArgs("http://localhost/", cropperJsInteropOptions, Path.Combine("http:localhost", PathToCropperModule));
            yield return WrapArgs("http://localhost/testPath", cropperJsInteropOptions, Path.Combine("http:localhost", PathToCropperModule));
            yield return WrapArgs("http://localhost/testPath/", cropperJsInteropOptions, Path.Combine("http:localhost", PathToCropperModule));
            yield return WrapArgs("https://localhost", cropperJsInteropOptions, Path.Combine("https:localhost", PathToCropperModule));
            yield return WrapArgs("https://localhost/", cropperJsInteropOptions, Path.Combine("https:localhost", PathToCropperModule));
            yield return WrapArgs("https://localhost/testPath", cropperJsInteropOptions, Path.Combine("https:localhost", PathToCropperModule));
            yield return WrapArgs("https://localhost/testPath/", cropperJsInteropOptions, Path.Combine("https:localhost", PathToCropperModule));
            yield return WrapArgs("https://localhost:5001", cropperJsInteropOptions, Path.Combine("https:localhost:5001", PathToCropperModule));
            yield return WrapArgs("https://localhost:5001/", cropperJsInteropOptions, Path.Combine("https:localhost:5001", PathToCropperModule));
            yield return WrapArgs("https://localhost:5001/testPath", cropperJsInteropOptions, Path.Combine("https:localhost:5001", PathToCropperModule));
            yield return WrapArgs("https://localhost:5001/testPath/", cropperJsInteropOptions, Path.Combine("https:localhost:5001", PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io", cropperJsInteropOptions, Path.Combine("http:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io/", cropperJsInteropOptions, Path.Combine("http:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io/testPath", cropperJsInteropOptions, Path.Combine("http:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io/testPath/", cropperJsInteropOptions, Path.Combine("http:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io/", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io/testPath", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io/testPath/", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io:5001", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001/", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io:5001", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001/testPath", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io:5001", PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001/testPath/", cropperJsInteropOptions, Path.Combine("https:cropperblazor.github.io:5001", PathToCropperModule));

            cropperJsInteropOptions = new();
            cropperJsInteropOptions.IsActiveGlobalPath = true;
            cropperJsInteropOptions.GlobalPathToCropperModule = "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js";

            yield return WrapArgs("http://localhost", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://localhost/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://localhost/testPath", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://localhost/testPath/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost/testPath", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost/testPath/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost:5001", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost:5001/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost:5001/testPath", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://localhost:5001/testPath/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://cropperblazor.github.io", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://cropperblazor.github.io/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://cropperblazor.github.io/testPath", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("http://cropperblazor.github.io/testPath/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io/testPath", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io/testPath/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io:5001", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io:5001/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io:5001/testPath", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");
            yield return WrapArgs("https://cropperblazor.github.io:5001/testPath/", cropperJsInteropOptions, "https://cropperblazor.github.io:5001/testPath/_content/Cropper.Blazor/cropperJsInterop.min.js");

            static object[] WrapArgs(
                string pathToCropperModule,
                CropperJsInteropOptions cropperJsInteropOptions,
                string expectedPathToCropperModule)
                => new object[]
                {
                    pathToCropperModule,
                    cropperJsInteropOptions,
                    expectedPathToCropperModule
                };
        }

        private void VerifyLoadCropperModule(
            string pathToCropperModule)
        {
            _testContext.JSInterop
                .SetupModule(pathToCropperModule);
        }

        public void Dispose()
        {
            _testContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
