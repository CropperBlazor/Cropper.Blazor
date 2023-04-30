using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bunit;
using Bunit.TestDoubles;
using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public class CropperJsInterop_Should : IDisposable
    {
        private readonly Faker _faker;
        private readonly TestContext _testContext;
        private readonly ICropperJsInterop _cropperJsInterop;
        private string DefaultPathToCropperModule => Path.Combine("http:localhost", CropperJsInterop.PathToCropperModule);

        public CropperJsInterop_Should()
        {
            _faker = new Faker();

            _testContext = new Faker<TestContext>()
                .Generate();

            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            _cropperJsInterop = new Faker<ICropperJsInterop>()
                .CustomInstantiator(f => new CropperJsInterop(_testContext.JSInterop.JSRuntime, fakeNavigationManager))
                .Generate();
        }

        [Theory, MemberData(nameof(TestData_LoadCropperModule))]
        public async Task Verify_LoadCropperModuleAsync(
            string pathToCropperModule,
            string expectedPathToCropperModule)
        {
            // arrange
            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            fakeNavigationManager.NavigateTo(pathToCropperModule);

            // assert
            VerifyLoadCropperModule(expectedPathToCropperModule);

            // act
            await _cropperJsInterop.LoadModuleAsync();
        }

        [Fact]
        public async Task Verify_InitCropperAsync()
        {
            // arrange
            ElementReference elementReference = new(Guid.NewGuid().ToString());
            Options options = new Faker<Options>().Generate();
            ICropperComponentBase cropperComponentBase = new Mock<ICropperComponentBase>().Object;
            DotNetObjectReference<ICropperComponentBase> refToCropperComponentBase = new Faker<DotNetObjectReference<ICropperComponentBase>>()
                .CustomInstantiator(f => DotNetObjectReference.Create(cropperComponentBase));

            object[] expectedInitCropperMethodArguments = new object[]
            {
                elementReference,
                options,
                refToCropperComponentBase
            };

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            _testContext.JSInterop
                .SetupVoid("cropper.initCropper", expectedInitCropperMethodArguments)
                .SetVoidResult();

            // act
            await _cropperJsInterop.InitCropperAsync(elementReference, options, refToCropperComponentBase);
        }

        [Fact]
        public async Task Verify_ClearAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.clear")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ClearAsync();
        }

        [Fact]
        public async Task Verify_CropAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.crop")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.CropAsync();
        }

        [Fact]
        public async Task Verify_DestroyAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.destroy")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.DestroyAsync();
        }

        [Fact]
        public async Task Verify_DisableAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.disable")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.DisableAsync();
        }

        [Fact]
        public async Task Verify_EnableAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.enable")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.EnableAsync();
        }

        [Fact]
        public async Task Verify_GetCanvasDataAsync()
        {
            // arrange
            CanvasData expectedCanvasData = new Faker<CanvasData>();

            _testContext.JSInterop
                .Setup<CanvasData>("cropper.getCanvasData")
                .SetResult(expectedCanvasData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CanvasData canvasData = await _cropperJsInterop.GetCanvasDataAsync();

            // assert
            expectedCanvasData.Should().BeEquivalentTo(canvasData);
        }

        [Fact]
        public async Task Verify_GetContainerDataAsync()
        {
            // arrange
            ContainerData expectedContainerData = new Faker<ContainerData>();

            _testContext.JSInterop
                .Setup<ContainerData>("cropper.getContainerData")
                .SetResult(expectedContainerData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            ContainerData containerData = await _cropperJsInterop.GetContainerDataAsync();

            // assert
            expectedContainerData.Should().BeEquivalentTo(containerData);
        }

        [Fact]
        public async Task Verify_GetCropBoxDataAsync()
        {
            // arrange
            CropBoxData expectedCropBoxData = new Faker<CropBoxData>();

            _testContext.JSInterop
                .Setup<CropBoxData>("cropper.getCropBoxData")
                .SetResult(expectedCropBoxData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CropBoxData cropBoxData = await _cropperJsInterop.GetCropBoxDataAsync();

            // assert
            expectedCropBoxData.Should().BeEquivalentTo(cropBoxData);
        }

        [Fact]
        public async Task Verify_GetCroppedCanvasAsync()
        {
            // arrange
            Mock<IJSObjectReference> mockIJSObjectReference = new();

            CroppedCanvas expectedCroppedCanvas = new Faker<CroppedCanvas>()
                .CustomInstantiator(c => new CroppedCanvas(mockIJSObjectReference.Object));
            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>();

            _testContext.JSInterop
                .SetupModule("cropper.getCroppedCanvas", invocation => invocation.Arguments?[0] is GetCroppedCanvasOptions argumentGetCroppedCanvasOptions
                       && argumentGetCroppedCanvasOptions.Equals(getCroppedCanvasOptions));

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CroppedCanvas croppedCanvas = await _cropperJsInterop.GetCroppedCanvasAsync(getCroppedCanvasOptions);

            // assert
            expectedCroppedCanvas.Should().BeEquivalentTo(croppedCanvas);
        }

        [Fact]
        public async Task Verify_GetCroppedCanvasDataURLAsync()
        {
            // arrange
            string expectedCroppedCanvasURL = _faker.Random.Word();
            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>();

            _testContext.JSInterop
                .Setup<string>("cropper.getCroppedCanvasDataURL", getCroppedCanvasOptions)
                .SetResult(expectedCroppedCanvasURL);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            string croppedCanvasURL = await _cropperJsInterop.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);

            // assert
            expectedCroppedCanvasURL.Should().BeEquivalentTo(croppedCanvasURL);
        }

        [Fact]
        public async Task Verify_GetDataAsync()
        {
            // arrange
            CropperData expectedCropperData = new Faker<CropperData>();
            bool rounded = _faker.Random.Bool();

            _testContext.JSInterop
                .Setup<CropperData>("cropper.getData", rounded)
                .SetResult(expectedCropperData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CropperData cropperData = await _cropperJsInterop.GetDataAsync(rounded);

            // assert
            expectedCropperData.Should().BeEquivalentTo(cropperData);
        }

        [Fact]
        public async Task Verify_GetImageDataAsync()
        {
            // arrange
            ImageData expectedImageData = new Faker<ImageData>();

            _testContext.JSInterop
                .Setup<ImageData>("cropper.getImageData")
                .SetResult(expectedImageData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            ImageData imageData = await _cropperJsInterop.GetImageDataAsync();

            // assert
            expectedImageData.Should().BeEquivalentTo(imageData);
        }

        [Fact]
        public async Task Verify_GetImageUsingStreamingAsync()
        {
            // arrange
            string expectedImageData = _faker.Random.Word();
            long maxAllowedSize = _faker.Random.Long();
            CancellationToken cancellationToken = new();
            Mock<IBrowserFile> mockImageFile = new();
            string expectedText = _faker.Random.Word();

            using (MemoryStream stream = new(Encoding.UTF8.GetBytes(expectedText)))
            {

                mockImageFile
                    .Setup(m => m.OpenReadStream(maxAllowedSize, cancellationToken))
                    .Returns(stream);

                Stream jsImageStream = mockImageFile.Object.OpenReadStream(maxAllowedSize, cancellationToken);
                using DotNetStreamReference dotnetImageStream = new(jsImageStream);
                _testContext.JSInterop
                    .Setup<string>("cropper.getImageUsingStreaming",
                                   jSRuntimeInvocation => jSRuntimeInvocation.Arguments.Count == 1 && VerifyStreamArgument(jSRuntimeInvocation))
                    .SetResult(expectedImageData);

                bool VerifyStreamArgument(JSRuntimeInvocation jSRuntimeInvocation)
                {
                    DotNetStreamReference? streamReference = (DotNetStreamReference?)jSRuntimeInvocation.Arguments[0];
                    string textStream = Encoding.UTF8.GetString(((MemoryStream)streamReference!.Stream).ToArray());

                    return expectedText == textStream;
                }
            }

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            string imageData = await _cropperJsInterop.GetImageUsingStreamingAsync(mockImageFile.Object, maxAllowedSize);

            // assert
            expectedImageData.Should().BeEquivalentTo(imageData);
        }

        [Fact]
        public async Task Verify_MoveAsync()
        {
            // arrange
            decimal offsetX = _faker.Random.Decimal();
            decimal? offsetY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.move", offsetX, offsetY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.MoveAsync(offsetX, offsetY);
        }

        [Fact]
        public async Task Verify_MoveToAsync()
        {
            // arrange
            decimal x = _faker.Random.Decimal();
            decimal? y = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.moveTo", x, y)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.MoveToAsync(x, y);
        }

        [Fact]
        public async Task Verify_NoConflictAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.noConflict")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.NoConflictAsync();
        }

        [Fact]
        public async Task Verify_ReplaceAsync()
        {
            // arrange
            string url = _faker.Random.String();
            bool onlyColorChanged = _faker.Random.Bool();

            _testContext.JSInterop
                .SetupVoid("cropper.replace", url, onlyColorChanged)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ReplaceAsync(url, onlyColorChanged);
        }

        [Fact]
        public async Task Verify_ResetAsync()
        {
            // arrange
            _testContext.JSInterop
                .SetupVoid("cropper.reset")
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ResetAsync();
        }

        [Fact]
        public async Task Verify_RevokeObjectUrlAsync()
        {
            // arrange
            string url = _faker.Random.Word();

            _testContext.JSInterop
                .SetupVoid("cropper.revokeObjectUrl", url)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.RevokeObjectUrlAsync(url);
        }

        [Fact]
        public async Task Verify_RotateAsync()
        {
            // arrange
            decimal degree = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.rotate", degree)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.RotateAsync(degree);
        }

        [Fact]
        public async Task Verify_RotateToAsync()
        {
            // arrange
            decimal degree = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.rotateTo", degree)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.RotateToAsync(degree);
        }

        [Fact]
        public async Task Verify_ScaleAsync()
        {
            // arrange
            decimal scaleX = _faker.Random.Decimal();
            decimal scaleY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.scale", scaleX, scaleY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ScaleAsync(scaleX, scaleY);
        }

        [Fact]
        public async Task Verify_ScaleXAsync()
        {
            // arrange
            decimal scaleX = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.scaleX", scaleX)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ScaleXAsync(scaleX);
        }

        [Fact]
        public async Task Verify_ScaleYAsync()
        {
            // arrange
            decimal scaleY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.scaleY", scaleY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ScaleYAsync(scaleY);
        }

        [Fact]
        public async Task Verify_SetAspectRatioAsync()
        {
            // arrange
            decimal aspectRatio = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.setAspectRatio", aspectRatio)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetAspectRatioAsync(aspectRatio);
        }

        [Fact]
        public async Task Verify_SetCanvasDataAsync()
        {
            // arrange
            SetCanvasDataOptions setCanvasDataOptions = new Faker<SetCanvasDataOptions>();

            _testContext.JSInterop
                .SetupVoid("cropper.setCanvasData", setCanvasDataOptions)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetCanvasDataAsync(setCanvasDataOptions);
        }

        [Fact]
        public async Task Verify_SetCropBoxDataAsync()
        {
            // arrange
            SetCropBoxDataOptions setCropBoxDataOptions = new Faker<SetCropBoxDataOptions>();

            _testContext.JSInterop
                .SetupVoid("cropper.setCropBoxData", setCropBoxDataOptions)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetCropBoxDataAsync(setCropBoxDataOptions);
        }

        [Fact]
        public async Task Verify_SetDataAsync()
        {
            // arrange
            SetDataOptions setDataOptions = new Faker<SetDataOptions>();

            _testContext.JSInterop
                .SetupVoid("cropper.setData", setDataOptions)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetDataAsync(setDataOptions);
        }

        [Fact]
        public async Task Verify_SetDefaultsAsync()
        {
            // arrange
            Options options = new Faker<Options>();

            _testContext.JSInterop
                .SetupVoid("cropper.setDefaults", options)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetDefaultsAsync(options);
        }

        [Fact]
        public async Task Verify_SetDragModeAsync()
        {
            // arrange
            DragMode dragMode = _faker.Random.Enum<DragMode>();

            _testContext.JSInterop
                .SetupVoid("cropper.setDragMode", dragMode.ToEnumString())
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetDragModeAsync(dragMode);
        }

        [Fact]
        public async Task Verify_ZoomAsync()
        {
            // arrange
            decimal ratio = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.zoom", ratio)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ZoomAsync(ratio);
        }

        [Fact]
        public async Task Verify_ZoomToAsync()
        {
            // arrange
            decimal ratio = _faker.Random.Decimal();
            decimal pivotX = _faker.Random.Decimal();
            decimal pivotY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.zoomTo", ratio, pivotX, pivotY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ZoomToAsync(ratio, pivotX, pivotY);
        }

        [Fact]
        public async Task Verify_DisposeAsync()
        {
            // arrange
            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            CropperJsInterop cropperJsInterop = new(_testContext.JSInterop.JSRuntime, fakeNavigationManager);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await cropperJsInterop.LoadModuleAsync();
            await cropperJsInterop.DisposeAsync();
        }

        public static IEnumerable<object[]> TestData_LoadCropperModule()
        {
            yield return WrapArgs("http://localhost", Path.Combine("http:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://localhost/", Path.Combine("http:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://localhost/testPath", Path.Combine("http:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://localhost/testPath/", Path.Combine("http:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost", Path.Combine("https:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost/", Path.Combine("https:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost/testPath", Path.Combine("https:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost/testPath/", Path.Combine("https:localhost", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost:5001", Path.Combine("https:localhost:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost:5001/", Path.Combine("https:localhost:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost:5001/testPath", Path.Combine("https:localhost:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://localhost:5001/testPath/", Path.Combine("https:localhost:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io", Path.Combine("http:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io/", Path.Combine("http:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io/testPath", Path.Combine("http:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("http://cropperblazor.github.io/testPath/", Path.Combine("http:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io", Path.Combine("https:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io/", Path.Combine("https:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io/testPath", Path.Combine("https:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io/testPath/", Path.Combine("https:cropperblazor.github.io", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001", Path.Combine("https:cropperblazor.github.io:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001/", Path.Combine("https:cropperblazor.github.io:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001/testPath", Path.Combine("https:cropperblazor.github.io:5001", CropperJsInterop.PathToCropperModule));
            yield return WrapArgs("https://cropperblazor.github.io:5001/testPath/", Path.Combine("https:cropperblazor.github.io:5001", CropperJsInterop.PathToCropperModule));

            static object[] WrapArgs(
                string pathToCropperModule,
                string expectedPathToCropperModule)
                => new object[]
                {
                    pathToCropperModule,
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
