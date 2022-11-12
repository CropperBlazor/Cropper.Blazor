using Bogus;
using Bunit;
using Cropper.Blazor.Base;
using Cropper.Blazor.Extensions;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Moq;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public class CropperJsInterop_Should : IDisposable
    {
        private readonly Faker _faker;
        private readonly TestContext _testContext;
        private readonly ICropperJsInterop _cropperJsInterop;

        public CropperJsInterop_Should()
        {
            _faker = new Faker();

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
            VerifyLoadCropperModule();

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
                .Setup<object>("cropper.clear")
                .SetResult(new());

            // assert
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

            // act
            CropBoxData cropBoxData = await _cropperJsInterop.GetCropBoxDataAsync();

            // assert
            expectedCropBoxData.Should().BeEquivalentTo(cropBoxData);
        }

        [Fact]
        public async Task Verify_GetCroppedCanvasAsync()
        {
            // arrange
            object expectedCroppedCanvas = new Faker<object>();
            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>();

            _testContext.JSInterop
                .Setup<object>("cropper.getCroppedCanvas", getCroppedCanvasOptions)
                .SetResult(expectedCroppedCanvas);

            // assert
            VerifyLoadCropperModule();

            // act
            object croppedCanvas = await _cropperJsInterop.GetCroppedCanvasAsync(getCroppedCanvasOptions);

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            CancellationToken cancellationToken = new CancellationToken();
            Mock<IBrowserFile> mockImageFile = new Mock<IBrowserFile>();
            string expectedText = _faker.Random.Word();

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(expectedText)))
            {

                mockImageFile
                    .Setup(m => m.OpenReadStream(maxAllowedSize, cancellationToken))
                    .Returns(stream);

                Stream jsImageStream = mockImageFile.Object.OpenReadStream(maxAllowedSize, cancellationToken);
                using (DotNetStreamReference dotnetImageStream = new DotNetStreamReference(jsImageStream))
                {
                    _testContext.JSInterop
                        .Setup<string>("cropper.getImageUsingStreaming",
                                       jSRuntimeInvocation => jSRuntimeInvocation.Arguments.Count == 1 && VerifyStreamArgument(jSRuntimeInvocation))
                        .SetResult(expectedImageData);

                    bool VerifyStreamArgument(JSRuntimeInvocation jSRuntimeInvocation)
                    {
                        DotNetStreamReference? streamReference = (DotNetStreamReference?)jSRuntimeInvocation.Arguments.First();
                        string textStream = Encoding.UTF8.GetString(((MemoryStream)streamReference!.Stream).ToArray());

                        return expectedText == textStream;
                    }
                }
            }

            // assert
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

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
            VerifyLoadCropperModule();

            // act
            await _cropperJsInterop.ZoomToAsync(ratio, pivotX, pivotY);
        }

        [Fact]
        public async Task Verify_DisposeAsync()
        {
            // arrange
            CropperJsInterop cropperJsInterop = new CropperJsInterop(_testContext.JSInterop.JSRuntime);

            // assert
            VerifyLoadCropperModule();

            // act
            await cropperJsInterop.LoadModuleAsync();
            await cropperJsInterop.DisposeAsync();
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
