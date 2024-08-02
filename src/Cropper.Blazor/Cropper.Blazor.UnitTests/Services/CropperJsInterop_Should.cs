using System;
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
using Cropper.Blazor.ModuleOptions;
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
        private const string PathToCropperModule = "_content/Cropper.Blazor/cropperJsInterop.min.js";
        private static string DefaultPathToCropperModule => Path.Combine("http:localhost", PathToCropperModule);

        public CropperJsInterop_Should()
        {
            _faker = new Faker();

            _testContext = new Faker<TestContext>()
                .Generate();

            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            _cropperJsInterop = new Faker<ICropperJsInterop>()
                .CustomInstantiator(f => new CropperJsInterop(_testContext.JSInterop.JSRuntime, fakeNavigationManager, new CropperJsInteropOptions()))
                .Generate();
        }

        [Fact]
        public async Task Verify_InitCropperAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            ElementReference elementReference = new(Guid.NewGuid().ToString());
            Options options = new Faker<Options>().Generate();
            ICropperComponentBase cropperComponentBase = new Mock<ICropperComponentBase>().Object;
            DotNetObjectReference<ICropperComponentBase> refToCropperComponentBase = new Faker<DotNetObjectReference<ICropperComponentBase>>()
                .CustomInstantiator(f => DotNetObjectReference.Create(cropperComponentBase));

            object[] expectedInitCropperMethodArguments = new object[]
            {
                cropperComponentId,
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
            await _cropperJsInterop.InitCropperAsync(cropperComponentId, elementReference, options, refToCropperComponentBase);
        }

        [Fact]
        public async Task Verify_ClearAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();

            _testContext.JSInterop
                .SetupVoid("cropper.clear", cropperComponentId)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ClearAsync(cropperComponentId);
        }

        [Fact]
        public async Task Verify_CropAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();

            _testContext.JSInterop
                .SetupVoid("cropper.crop", cropperComponentId)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.CropAsync(cropperComponentId);
        }

        [Fact]
        public async Task Verify_DestroyAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();

            _testContext.JSInterop
                .SetupVoid("cropper.destroy", cropperComponentId)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.DestroyAsync(cropperComponentId);
        }

        [Fact]
        public async Task Verify_DisableAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();

            _testContext.JSInterop
                .SetupVoid("cropper.disable", cropperComponentId)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.DisableAsync(cropperComponentId);
        }

        [Fact]
        public async Task Verify_EnableAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();

            _testContext.JSInterop
                .SetupVoid("cropper.enable", cropperComponentId)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.EnableAsync(cropperComponentId);
        }

        [Fact]
        public async Task Verify_GetCanvasDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            CanvasData expectedCanvasData = new Faker<CanvasData>();

            _testContext.JSInterop
                .Setup<CanvasData>("cropper.getCanvasData", cropperComponentId)
                .SetResult(expectedCanvasData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CanvasData canvasData = await _cropperJsInterop.GetCanvasDataAsync(cropperComponentId);

            // assert
            expectedCanvasData.Should().BeEquivalentTo(canvasData);
        }

        [Fact]
        public async Task Verify_GetContainerDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            ContainerData expectedContainerData = new Faker<ContainerData>();

            _testContext.JSInterop
                .Setup<ContainerData>("cropper.getContainerData", cropperComponentId)
                .SetResult(expectedContainerData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            ContainerData containerData = await _cropperJsInterop.GetContainerDataAsync(cropperComponentId);

            // assert
            expectedContainerData.Should().BeEquivalentTo(containerData);
        }

        [Fact]
        public async Task Verify_GetCropBoxDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            CropBoxData expectedCropBoxData = new Faker<CropBoxData>();

            _testContext.JSInterop
                .Setup<CropBoxData>("cropper.getCropBoxData", cropperComponentId)
                .SetResult(expectedCropBoxData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CropBoxData cropBoxData = await _cropperJsInterop.GetCropBoxDataAsync(cropperComponentId);

            // assert
            expectedCropBoxData.Should().BeEquivalentTo(cropBoxData);
        }

        [Fact]
        public async Task Verify_GetCroppedCanvasAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            Mock<IJSObjectReference> mockIJSObjectReference = new();

            CroppedCanvas expectedCroppedCanvas = new Faker<CroppedCanvas>()
                .CustomInstantiator(c => new CroppedCanvas(mockIJSObjectReference.Object));
            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>();

            _testContext.JSInterop
                .SetupModule("cropper.getCroppedCanvas", invocation => invocation.Arguments?[0] is Guid guid
                    && guid.Equals(cropperComponentId)
                    && invocation.Arguments?[1] is GetCroppedCanvasOptions argumentGetCroppedCanvasOptions
                    && argumentGetCroppedCanvasOptions.Equals(getCroppedCanvasOptions));

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CroppedCanvas croppedCanvas = await _cropperJsInterop.GetCroppedCanvasAsync(cropperComponentId, getCroppedCanvasOptions);

            // assert
            expectedCroppedCanvas.Should().BeEquivalentTo(croppedCanvas);
        }

        [Fact]
        public async Task Verify_GetCroppedCanvasDataURL_By_TypeAndNumber_Async()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            string expectedCroppedCanvasURL = _faker.Random.Word();
            string type = _faker.Random.Word();
            float number = _faker.Random.Float();
            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>();

            _testContext.JSInterop
                .Setup<string>("cropper.getCroppedCanvasDataURL", cropperComponentId, getCroppedCanvasOptions, type, number)
                .SetResult(expectedCroppedCanvasURL);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            string croppedCanvasURL = await _cropperJsInterop.GetCroppedCanvasDataURLAsync(cropperComponentId, getCroppedCanvasOptions, type, number);

            // assert
            expectedCroppedCanvasURL.Should().BeEquivalentTo(croppedCanvasURL);
        }

        [Fact]
        public async Task Verify_GetDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            CropperData expectedCropperData = new Faker<CropperData>();
            bool rounded = _faker.Random.Bool();

            _testContext.JSInterop
                .Setup<CropperData>("cropper.getData", cropperComponentId, rounded)
                .SetResult(expectedCropperData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            CropperData cropperData = await _cropperJsInterop.GetDataAsync(cropperComponentId, rounded);

            // assert
            expectedCropperData.Should().BeEquivalentTo(cropperData);
        }

        [Fact]
        public async Task Verify_GetImageDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            ImageData expectedImageData = new Faker<ImageData>();

            _testContext.JSInterop
                .Setup<ImageData>("cropper.getImageData", cropperComponentId)
                .SetResult(expectedImageData);

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            ImageData imageData = await _cropperJsInterop.GetImageDataAsync(cropperComponentId);

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
            Guid cropperComponentId = Guid.NewGuid();
            decimal offsetX = _faker.Random.Decimal();
            decimal? offsetY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.move", cropperComponentId, offsetX, offsetY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.MoveAsync(cropperComponentId, offsetX, offsetY);
        }

        [Fact]
        public async Task Verify_MoveToAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal x = _faker.Random.Decimal();
            decimal? y = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.moveTo", cropperComponentId, x, y)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.MoveToAsync(cropperComponentId, x, y);
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
            Guid cropperComponentId = Guid.NewGuid();
            string url = _faker.Random.String();
            bool onlyColorChanged = _faker.Random.Bool();

            _testContext.JSInterop
                .SetupVoid("cropper.replace", cropperComponentId, url, onlyColorChanged)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ReplaceAsync(cropperComponentId, url, onlyColorChanged);
        }

        [Fact]
        public async Task Verify_ResetAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            _testContext.JSInterop
                .SetupVoid("cropper.reset", cropperComponentId)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ResetAsync(cropperComponentId);
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
            Guid cropperComponentId = Guid.NewGuid();
            decimal degree = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.rotate", cropperComponentId, degree)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.RotateAsync(cropperComponentId, degree);
        }

        [Fact]
        public async Task Verify_RotateToAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal degree = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.rotateTo", cropperComponentId, degree)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.RotateToAsync(cropperComponentId, degree);
        }

        [Fact]
        public async Task Verify_ScaleAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal scaleX = _faker.Random.Decimal();
            decimal scaleY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.scale", cropperComponentId, scaleX, scaleY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ScaleAsync(cropperComponentId, scaleX, scaleY);
        }

        [Fact]
        public async Task Verify_ScaleXAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal scaleX = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.scaleX", cropperComponentId, scaleX)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ScaleXAsync(cropperComponentId, scaleX);
        }

        [Fact]
        public async Task Verify_ScaleYAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal scaleY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.scaleY", cropperComponentId, scaleY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ScaleYAsync(cropperComponentId, scaleY);
        }

        [Fact]
        public async Task Verify_SetAspectRatioAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal aspectRatio = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.setAspectRatio", cropperComponentId, aspectRatio)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetAspectRatioAsync(cropperComponentId, aspectRatio);
        }

        [Fact]
        public async Task Verify_SetCanvasDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            SetCanvasDataOptions setCanvasDataOptions = new Faker<SetCanvasDataOptions>();

            _testContext.JSInterop
                .SetupVoid("cropper.setCanvasData", cropperComponentId, setCanvasDataOptions)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetCanvasDataAsync(cropperComponentId, setCanvasDataOptions);
        }

        [Fact]
        public async Task Verify_SetCropBoxDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            SetCropBoxDataOptions setCropBoxDataOptions = new Faker<SetCropBoxDataOptions>();

            _testContext.JSInterop
                .SetupVoid("cropper.setCropBoxData", cropperComponentId, setCropBoxDataOptions)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetCropBoxDataAsync(cropperComponentId, setCropBoxDataOptions);
        }

        [Fact]
        public async Task Verify_SetDataAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            SetDataOptions setDataOptions = new Faker<SetDataOptions>();

            _testContext.JSInterop
                .SetupVoid("cropper.setData", cropperComponentId, setDataOptions)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetDataAsync(cropperComponentId, setDataOptions);
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
            Guid cropperComponentId = Guid.NewGuid();
            DragMode dragMode = _faker.Random.Enum<DragMode>();

            _testContext.JSInterop
                .SetupVoid("cropper.setDragMode", cropperComponentId, dragMode.ToEnumString())
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.SetDragModeAsync(cropperComponentId, dragMode);
        }

        [Fact]
        public async Task Verify_ZoomAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal ratio = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.zoom", cropperComponentId, ratio)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ZoomAsync(cropperComponentId, ratio);
        }

        [Fact]
        public async Task Verify_ZoomToAsync()
        {
            // arrange
            Guid cropperComponentId = Guid.NewGuid();
            decimal ratio = _faker.Random.Decimal();
            decimal pivotX = _faker.Random.Decimal();
            decimal pivotY = _faker.Random.Decimal();

            _testContext.JSInterop
                .SetupVoid("cropper.zoomTo", cropperComponentId, ratio, pivotX, pivotY)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _cropperJsInterop.ZoomToAsync(cropperComponentId, ratio, pivotX, pivotY);
        }

        [Fact]
        public async Task Verify_DisposeAsync()
        {
            // arrange
            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            CropperJsInterop cropperJsInterop = new(_testContext.JSInterop.JSRuntime, fakeNavigationManager, new CropperJsInteropOptions());

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await cropperJsInterop.LoadModuleAsync();
            await cropperJsInterop.DisposeAsync();
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
