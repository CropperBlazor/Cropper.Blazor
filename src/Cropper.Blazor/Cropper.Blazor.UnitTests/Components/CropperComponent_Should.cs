using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bogus;
using Bunit;
using Cropper.Blazor.Base;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
using Cropper.Blazor.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;
using ErrorEventArgs = Microsoft.AspNetCore.Components.Web.ErrorEventArgs;

namespace Cropper.Blazor.UnitTests.Components
{
    public class CropperComponent_Should : IDisposable
    {
        private readonly TestContext _testContext;
        private readonly Mock<ICropperJsInterop> _mockCropperJsInterop;

        public CropperComponent_Should()
        {
            _testContext = new Faker<TestContext>()
                .Generate();

            _mockCropperJsInterop = new Mock<ICropperJsInterop>();

            _testContext.Services.AddSingleton(_mockCropperJsInterop.Object);
        }

        [Theory]
        [InlineData(-100)]
        [InlineData(-1)]
        [InlineData(-0.99)]
        [InlineData(1.11)]
        [InlineData(111)]
        public async Task Throw_Exception_Because_Of_Invalid_NumberAsync(float numberImageQuality)
        {
            // arrange
            Faker faker = new();
            CancellationToken cancellationToken = new();

            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>()
                .Generate();
            string imageFormatType = faker.Random.Word();

            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .RenderComponent<CropperComponent>();

            await cropperComponent.InvokeAsync(async () =>
            {
                // act
                Func<Task> func = async () => await cropperComponent.Instance.GetCroppedCanvasDataURLAsync(
                    getCroppedCanvasOptions,
                    imageFormatType,
                    numberImageQuality,
                    cancellationToken);

                // assert
                await func
                    .Should()
                    .ThrowAsync<ArgumentException>()
                    .WithMessage($"The given number should be between 0 and 1 for indication the image quality, but found {numberImageQuality}. (Parameter 'number')");

                _mockCropperJsInterop.Verify(c => c.GetCroppedCanvasDataURLAsync(
                    It.IsAny<GetCroppedCanvasOptions>(),
                    It.IsAny<string>(),
                    It.IsAny<float>(),
                    It.IsAny<CancellationToken>()), Times.Never());
            });
        }

        [Fact]
        public async Task Should_Render_CropperComponent_SuccessfulAsync()
        {
            // arrange
            string errorLoadImageClass = "cropper-error-load";
            string imageClass = "cropper";
            string lazyAttributeValue = "lazy";
            Dictionary<string, object> inputAttributes = new()
            {
                { "loading", lazyAttributeValue }
            };
            string imageSrcAttributeValue = "https://.../image.jpg";
            string errorLoadImageSrcAttributeValue = "https://.../not-found-image.jpg";
            int countCallsOnLoadImageHandler = 0;
            int countCallsOnErrorLoadImageHandler = 0;
            int countCallsOnCropEventHandler = 0;
            int countCallsOnCropEndEventHandler = 0;
            int countCallsOnCropMoveEventHandler = 0;
            int countCallsOnCropStartEventHandler = 0;
            int countCallsOnZoomEventHandler = 0;
            int countCallsOnCropReadyEventHandler = 0;

            IBrowserFile imageFile = new Mock<IBrowserFile>().Object;
            CancellationToken cancellationToken = new();
            JSEventData<CropReadyEvent> cropReadyEvent = new Faker<JSEventData<CropReadyEvent>>()
                .Generate();
            JSEventData<ZoomEvent> zoomEvent = new Faker<JSEventData<ZoomEvent>>()
                .Generate();
            JSEventData<CropStartEvent> cropStartEvent = new Faker<JSEventData<CropStartEvent>>()
                .Generate();
            JSEventData<CropMoveEvent> cropMoveEvent = new Faker<JSEventData<CropMoveEvent>>()
                .Generate();
            JSEventData<CropEndEvent> cropEndEvent = new Faker<JSEventData<CropEndEvent>>()
                .Generate();
            JSEventData<CropEvent> cropEvent = new Faker<JSEventData<CropEvent>>()
                .Generate();
            ProgressEventArgs progressEventArgs = new Faker<ProgressEventArgs>()
                .Generate();
            ErrorEventArgs errorEventArgs = new Faker<ErrorEventArgs>()
                .Generate();
            Options options = new Faker<Options>()
                .Generate();
            CanvasData expectedCanvasData = new Faker<CanvasData>()
                .Generate();
            ContainerData expectedContainerData = new Faker<ContainerData>()
                .Generate();
            CropBoxData expectedCropBoxData = new Faker<CropBoxData>()
                .Generate();
            GetCroppedCanvasOptions getCroppedCanvasOptions = new Faker<GetCroppedCanvasOptions>()
                .Generate();
            Mock<IJSObjectReference> mockIJSObjectReference = new();
            CroppedCanvas expectedCroppedCanvas = new Faker<CroppedCanvas>()
                .CustomInstantiator(c => new CroppedCanvas(mockIJSObjectReference.Object));
            CropperData expectedCropperData = new Faker<CropperData>()
                .Generate();
            ImageData expectedImageData = new Faker<ImageData>()
                .Generate();
            SetCanvasDataOptions setCanvasDataOptions = new Faker<SetCanvasDataOptions>()
                .Generate();
            SetCropBoxDataOptions setCropBoxDataOptions = new Faker<SetCropBoxDataOptions>()
                .Generate();
            SetDataOptions setDataOptions = new Faker<SetDataOptions>()
                .Generate();

            Faker faker = new();
            string expectedCroppedCanvasDataURL = faker.Random.Word();
            bool isRounded = faker.Random.Bool();
            decimal degree = faker.Random.Decimal();
            long maxAllowedSize = faker.Random.Long();
            string expectedImage = faker.Random.Word();
            decimal offsetX = faker.Random.Decimal();
            decimal? offsetY = faker.Random.Decimal();
            decimal x = faker.Random.Decimal();
            decimal? y = faker.Random.Decimal();
            string url = faker.Random.Word();
            decimal scaleX = faker.Random.Decimal();
            decimal scaleY = faker.Random.Decimal();
            decimal aspectRatio = faker.Random.Decimal();
            DragMode dragMode = faker.Random.Enum<DragMode>();
            decimal ratio = faker.Random.Decimal();
            decimal pivotX = faker.Random.Decimal();
            decimal pivotY = faker.Random.Decimal();
            string newUrlImage = faker.Random.Word();
            bool hasSameSize = faker.Random.Bool();
            string imageFormatType = faker.Random.Word();
            float numberImageQuality = faker.Random.Float(0, 1);

            Action? onLoadImageHandler = () =>
            {
                countCallsOnLoadImageHandler++;
            };
            Action<ErrorEventArgs>? onErrorLoadImageHandler = (ErrorEventArgs e) =>
            {
                countCallsOnErrorLoadImageHandler++;
                errorEventArgs.Should().BeEquivalentTo(e);
            };
            Action<JSEventData<CropEvent>>? onCropEventHandler = (JSEventData<CropEvent> c) =>
            {
                countCallsOnCropEventHandler++;
                cropEvent.Should().BeEquivalentTo(c);
            };
            Action<JSEventData<CropEndEvent>>? onCropEndEventHandler = (JSEventData<CropEndEvent> c) =>
            {
                countCallsOnCropEndEventHandler++;
                cropEndEvent.Should().BeEquivalentTo(c);
            };
            Action<JSEventData<CropMoveEvent>>? onCropMoveEventHandler = (JSEventData<CropMoveEvent> c) =>
            {
                countCallsOnCropMoveEventHandler++;
                cropMoveEvent.Should().BeEquivalentTo(c);
            };
            Action<JSEventData<CropStartEvent>>? onCropStartEventHandler = (JSEventData<CropStartEvent> c) =>
            {
                countCallsOnCropStartEventHandler++;
                cropStartEvent.Should().BeEquivalentTo(c);
            };
            Action<JSEventData<ZoomEvent>>? onZoomEventHandler = (JSEventData<ZoomEvent> z) =>
            {
                countCallsOnZoomEventHandler++;
                zoomEvent.Should().BeEquivalentTo(z);
            };
            Action<JSEventData<CropReadyEvent>>? onCropReadyEventHandler = (JSEventData<CropReadyEvent> c) =>
            {
                countCallsOnCropReadyEventHandler++;
                cropReadyEvent.Should().BeEquivalentTo(c);
            };

            ComponentParameter imageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Class),
                imageClass);
            ComponentParameter errorLoadImageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageClass),
                errorLoadImageClass);
            ComponentParameter loadingParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.InputAttributes),
                inputAttributes);
            ComponentParameter errorLoadImageSrcParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageSrc),
                errorLoadImageSrcAttributeValue);
            ComponentParameter srcParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Src),
                imageSrcAttributeValue);
            ComponentParameter isErrorLoadImageParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.IsErrorLoadImage),
                false);
            ComponentParameter isAvaibleInitCropperParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.IsAvaibleInitCropper),
                true);
            ComponentParameter onLoadImageParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnLoadImageEvent),
                onLoadImageHandler);
            ComponentParameter onErrorLoadImageParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnErrorLoadImageEvent),
                onErrorLoadImageHandler);
            ComponentParameter optionsParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Options),
                options);
            ComponentParameter onCropEventParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnCropEvent),
                onCropEventHandler);
            ComponentParameter onCropEndEventParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnCropEndEvent),
                onCropEndEventHandler);
            ComponentParameter onCropMoveEventParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnCropMoveEvent),
                onCropMoveEventHandler);
            ComponentParameter onCropStartEventParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnCropStartEvent),
                onCropStartEventHandler);
            ComponentParameter onZoomEventParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnZoomEvent),
                onZoomEventHandler);
            ComponentParameter onReadyEventParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.OnReadyEvent),
                onCropReadyEventHandler);

            _mockCropperJsInterop
                .Setup(c => c.GetCanvasDataAsync(cancellationToken))
                .ReturnsAsync(expectedCanvasData);

            _mockCropperJsInterop
                .Setup(c => c.GetContainerDataAsync(cancellationToken))
                .ReturnsAsync(expectedContainerData);

            _mockCropperJsInterop
                .Setup(c => c.GetCropBoxDataAsync(cancellationToken))
                .ReturnsAsync(expectedCropBoxData);

            _mockCropperJsInterop
                .Setup(c => c.GetCroppedCanvasAsync(getCroppedCanvasOptions, cancellationToken))
                .ReturnsAsync(expectedCroppedCanvas);

            _mockCropperJsInterop
                .Setup(c => c.GetCroppedCanvasDataURLAsync(
                    getCroppedCanvasOptions,
                    imageFormatType,
                    numberImageQuality,
                    cancellationToken))
                .ReturnsAsync(expectedCroppedCanvasDataURL);

            _mockCropperJsInterop
                .Setup(c => c.GetDataAsync(isRounded, cancellationToken))
                .ReturnsAsync(expectedCropperData);

            _mockCropperJsInterop
                .Setup(c => c.GetImageDataAsync(cancellationToken))
                .ReturnsAsync(expectedImageData);

            _mockCropperJsInterop
                .Setup(c => c.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken))
                .ReturnsAsync(expectedImage);

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .RenderComponent<CropperComponent>(
                    errorLoadImageClassParameter,
                    loadingParameter,
                    errorLoadImageSrcParameter,
                    isErrorLoadImageParameter,
                    isAvaibleInitCropperParameter,
                    srcParameter,
                    imageClassParameter,
                    onLoadImageParameter,
                    onErrorLoadImageParameter,
                    optionsParameter,
                    onCropEventParameter,
                    onCropEndEventParameter,
                    onCropMoveEventParameter,
                    onCropStartEventParameter,
                    onZoomEventParameter,
                    onReadyEventParameter);

            // assert
            IElement expectedElement = cropperComponent.Find($"img.{imageClass}");
            ElementReference elementReference = (ElementReference)cropperComponent.Instance
                .GetInstanceField("ImageReference");

            _mockCropperJsInterop.Verify(c => c.LoadModuleAsync(cancellationToken), Times.Once());
            elementReference.Id.Should().NotBeNullOrEmpty();
            expectedElement.ClassName.Should().Be(imageClass);
            expectedElement.GetAttribute("loading").Should().Be(lazyAttributeValue);
            expectedElement.GetAttribute("src").Should().Be(imageSrcAttributeValue);
            expectedElement.GetAttribute("blazor:elementreference").Should().Be(elementReference.Id);

            countCallsOnLoadImageHandler.Should().Be(0);
            expectedElement.TriggerEvent("onload", progressEventArgs);
            countCallsOnLoadImageHandler.Should().Be(1);
            _mockCropperJsInterop.Verify(c => c.InitCropperAsync(
                elementReference,
                options,
                It.IsAny<DotNetObjectReference<ICropperComponentBase>>(),
                cancellationToken), Times.Once());

            countCallsOnErrorLoadImageHandler.Should().Be(0);
            expectedElement.TriggerEvent("onerror", errorEventArgs);
            countCallsOnErrorLoadImageHandler.Should().Be(1);

            await cropperComponent.InvokeAsync(async () =>
            {
                cropperComponent.Instance.Clear();
                _mockCropperJsInterop.Verify(c => c.ClearAsync(cancellationToken), Times.Once());

                cropperComponent.Instance.Crop();
                _mockCropperJsInterop.Verify(c => c.CropAsync(cancellationToken), Times.Once());

                cropperComponent.Instance.CropperIsCroped(cropEvent);
                countCallsOnCropEventHandler.Should().Be(1);

                cropperComponent.Instance.CropperIsEnded(cropEndEvent);
                countCallsOnCropEndEventHandler.Should().Be(1);

                cropperComponent.Instance.CropperIsMoved(cropMoveEvent);
                countCallsOnCropMoveEventHandler.Should().Be(1);

                cropperComponent.Instance.CropperIsStarted(cropStartEvent);
                countCallsOnCropStartEventHandler.Should().Be(1);

                cropperComponent.Instance.CropperIsZoomed(zoomEvent);
                countCallsOnZoomEventHandler.Should().Be(1);

                cropperComponent.Instance.Destroy();
                _mockCropperJsInterop.Verify(c => c.DestroyAsync(cancellationToken), Times.Once());

                cropperComponent.Instance.Disable();
                _mockCropperJsInterop.Verify(c => c.DisableAsync(cancellationToken), Times.Once());

                cropperComponent.Instance.Enable();
                _mockCropperJsInterop.Verify(c => c.EnableAsync(cancellationToken), Times.Once());

                CanvasData canvasData = await cropperComponent.Instance.GetCanvasDataAsync();
                expectedCanvasData.Should().BeEquivalentTo(canvasData);
                _mockCropperJsInterop.Verify(c => c.GetCanvasDataAsync(cancellationToken), Times.Once());

                ContainerData containerData = await cropperComponent.Instance.GetContainerDataAsync();
                expectedContainerData.Should().BeEquivalentTo(containerData);
                _mockCropperJsInterop.Verify(c => c.GetContainerDataAsync(cancellationToken), Times.Once());

                CropBoxData cropBoxData = await cropperComponent.Instance.GetCropBoxDataAsync();
                expectedCropBoxData.Should().BeEquivalentTo(cropBoxData);
                _mockCropperJsInterop.Verify(c => c.GetCropBoxDataAsync(cancellationToken), Times.Once());

                object croppedCanvas = await cropperComponent.Instance.GetCroppedCanvasAsync(getCroppedCanvasOptions);
                expectedCroppedCanvas.Should().BeEquivalentTo(croppedCanvas);
                _mockCropperJsInterop.Verify(c => c.GetCroppedCanvasAsync(getCroppedCanvasOptions, cancellationToken), Times.Once());

                string croppedCanvasDataURL = await cropperComponent.Instance.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions, imageFormatType, numberImageQuality);
                expectedCroppedCanvasDataURL.Should().BeEquivalentTo(croppedCanvasDataURL);
                _mockCropperJsInterop.Verify(c => c.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions, imageFormatType, numberImageQuality, cancellationToken), Times.Once());

                CropperData cropperData = await cropperComponent.Instance.GetDataAsync(isRounded);
                expectedCropperData.Should().BeEquivalentTo(cropperData);
                _mockCropperJsInterop.Verify(c => c.GetDataAsync(isRounded, cancellationToken), Times.Once());

                ImageData imageData = await cropperComponent.Instance.GetImageDataAsync();
                expectedImageData.Should().Be(imageData);
                _mockCropperJsInterop.Verify(c => c.GetImageDataAsync(cancellationToken), Times.Once());

                await cropperComponent.Instance.ReplaceAsync(newUrlImage, hasSameSize);
                _mockCropperJsInterop.Verify(c => c.ReplaceAsync(newUrlImage, hasSameSize, cancellationToken), Times.Once());

                string image = await cropperComponent.Instance.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
                expectedImage.Should().Be(image);
                _mockCropperJsInterop.Verify(c => c.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken), Times.Once());

                cropperComponent.Instance.IsReady(cropReadyEvent);
                countCallsOnCropReadyEventHandler.Should().Be(1);

                cropperComponent.Instance.Move(offsetX, offsetY);
                _mockCropperJsInterop.Verify(c => c.MoveAsync(offsetX, offsetY, cancellationToken), Times.Once());

                cropperComponent.Instance.MoveTo(x, y);
                _mockCropperJsInterop.Verify(c => c.MoveToAsync(x, y, cancellationToken), Times.Once());

                cropperComponent.Instance.OnErrorLoadImage(errorEventArgs);
                countCallsOnErrorLoadImageHandler.Should().Be(2);

                cropperComponent.Instance.Reset();
                _mockCropperJsInterop.Verify(c => c.ResetAsync(cancellationToken), Times.Once());

                await cropperComponent.Instance.RevokeObjectUrlAsync(url);
                _mockCropperJsInterop.Verify(c => c.RevokeObjectUrlAsync(url, cancellationToken), Times.Once());

                cropperComponent.Instance.Rotate(degree);
                _mockCropperJsInterop.Verify(c => c.RotateAsync(degree, cancellationToken), Times.Once());

                cropperComponent.Instance.Scale(scaleX, scaleY);
                _mockCropperJsInterop.Verify(c => c.ScaleAsync(scaleX, scaleY, cancellationToken), Times.Once());

                cropperComponent.Instance.ScaleX(scaleX);
                _mockCropperJsInterop.Verify(c => c.ScaleXAsync(scaleX, cancellationToken), Times.Once());

                cropperComponent.Instance.ScaleY(scaleY);
                _mockCropperJsInterop.Verify(c => c.ScaleYAsync(scaleY, cancellationToken), Times.Once());

                cropperComponent.Instance.SetAspectRatio(aspectRatio);
                _mockCropperJsInterop.Verify(c => c.SetAspectRatioAsync(aspectRatio, cancellationToken), Times.Once());

                cropperComponent.Instance.SetCanvasData(setCanvasDataOptions);
                _mockCropperJsInterop.Verify(c => c.SetCanvasDataAsync(setCanvasDataOptions, cancellationToken), Times.Once());

                cropperComponent.Instance.SetCropBoxData(setCropBoxDataOptions);
                _mockCropperJsInterop.Verify(c => c.SetCropBoxDataAsync(setCropBoxDataOptions, cancellationToken), Times.Once());

                cropperComponent.Instance.SetData(setDataOptions);
                _mockCropperJsInterop.Verify(c => c.SetDataAsync(setDataOptions, cancellationToken), Times.Once());

                cropperComponent.Instance.SetDragMode(dragMode);
                _mockCropperJsInterop.Verify(c => c.SetDragModeAsync(dragMode, cancellationToken), Times.Once());

                cropperComponent.Instance.Zoom(ratio);
                _mockCropperJsInterop.Verify(c => c.ZoomAsync(ratio, cancellationToken), Times.Once());

                cropperComponent.Instance.ZoomTo(ratio, pivotX, pivotY);
                _mockCropperJsInterop.Verify(c => c.ZoomToAsync(ratio, pivotX, pivotY, cancellationToken), Times.Once());

                await cropperComponent.Instance.DisposeAsync();
                _mockCropperJsInterop.Verify(c => c.DisposeAsync(), Times.Once());
            });
        }

        [Fact]
        public void Should_Render_CropperComponent_With_ErrorLoadImage_Parameter()
        {
            // arrange
            string errorLoadImageClass = "cropper-error-load";
            string lazyAttributeValue = "lazy";
            Dictionary<string, object> inputAttributes = new()
            {
                { "loading", lazyAttributeValue },
                { "Attribute_TEST", "TEST_VALUE" },
                { "src", "new_src" }
            };
            string errorLoadImageSrcAttributeValue = "https://cropper/not-found-image.jpg";

            ComponentParameter errorLoadImageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageClass),
                errorLoadImageClass);
            ComponentParameter loadingParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.InputAttributes),
                inputAttributes);
            ComponentParameter errorLoadImageSrcParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageSrc),
                errorLoadImageSrcAttributeValue);
            ComponentParameter isErrorLoadImage = ComponentParameter.CreateParameter(
                nameof(CropperComponent.IsErrorLoadImage),
                true);

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .RenderComponent<CropperComponent>(
                    errorLoadImageClassParameter,
                    loadingParameter,
                    errorLoadImageSrcParameter,
                    isErrorLoadImage);

            // assert
            IElement expectedElement = cropperComponent.Find($"img.{errorLoadImageClass}");
            ElementReference elementReference = (ElementReference)cropperComponent.Instance
                .GetInstanceField("ImageReference");

            elementReference.Id.Should().BeNullOrEmpty();
            expectedElement.ClassName.Should().Be(errorLoadImageClass);
            expectedElement.GetAttribute("loading").Should().Be(lazyAttributeValue);
            expectedElement.GetAttribute("src").Should().Be(errorLoadImageSrcAttributeValue);
            expectedElement.GetAttribute("Attribute_TEST").Should().Be("TEST_VALUE");
            expectedElement.GetAttribute("blazor:elementreference").Should().BeNullOrEmpty();

            _mockCropperJsInterop.Verify(c => c.InitCropperAsync(
                It.IsAny<ElementReference>(),
                It.IsAny<Options>(),
                It.IsAny<DotNetObjectReference<ICropperComponentBase>>(),
                It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public void Should_Not_Render_CropperComponent_With_IsNotAvaibleInitCropper_Parameter()
        {
            // arrange
            string errorLoadImageClass = "cropper-error-load";
            string lazyAttributeValue = "lazy";
            Dictionary<string, object> inputAttributes = new()
            {
                { "loading", lazyAttributeValue },
                { "Attribute_TEST", "TEST_VALUE" },
                { "src", "new_src" }
            };
            string errorLoadImageSrcAttributeValue = "https://cropper/not-found-image.jpg";

            ComponentParameter errorLoadImageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageClass),
                errorLoadImageClass);
            ComponentParameter loadingParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.InputAttributes),
                inputAttributes);
            ComponentParameter errorLoadImageSrcParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageSrc),
                errorLoadImageSrcAttributeValue);
            ComponentParameter isErrorLoadImage = ComponentParameter.CreateParameter(
                nameof(CropperComponent.IsErrorLoadImage),
                true);
            ComponentParameter isAvaibleInitCropperParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.IsAvaibleInitCropper),
                false);

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .RenderComponent<CropperComponent>(
                    errorLoadImageClassParameter,
                    loadingParameter,
                    errorLoadImageSrcParameter,
                    isErrorLoadImage,
                    isAvaibleInitCropperParameter);

            // assert
            IElement expectedElement = cropperComponent.Find($"img.{errorLoadImageClass}");
            ElementReference elementReference = (ElementReference)cropperComponent.Instance
                .GetInstanceField("ImageReference");

            elementReference.Id.Should().BeNullOrEmpty();
            expectedElement.ClassName.Should().Be(errorLoadImageClass);
            expectedElement.GetAttribute("loading").Should().Be(lazyAttributeValue);
            expectedElement.GetAttribute("src").Should().Be(errorLoadImageSrcAttributeValue);
            expectedElement.GetAttribute("Attribute_TEST").Should().Be("TEST_VALUE");
            expectedElement.GetAttribute("blazor:elementreference").Should().BeNullOrEmpty();

            _mockCropperJsInterop.Verify(c => c.InitCropperAsync(
                It.IsAny<ElementReference>(),
                It.IsAny<Options>(),
                It.IsAny<DotNetObjectReference<ICropperComponentBase>>(),
                It.IsAny<CancellationToken>()), Times.Never());
        }

        [Theory]
        [InlineData(nameof(CropperComponent.CropperIsCroped))]
        [InlineData(nameof(CropperComponent.CropperIsEnded))]
        [InlineData(nameof(CropperComponent.CropperIsMoved))]
        [InlineData(nameof(CropperComponent.CropperIsStarted))]
        [InlineData(nameof(CropperComponent.CropperIsZoomed))]
        [InlineData(nameof(CropperComponent.IsReady))]
        public void Verify_Method_To_Be_Invokable_From_JS<T>(string methodName)
        {
            // act
            MethodInfo? methodInfo = typeof(CropperComponent)
                .GetMethod(methodName);

            JSInvokableAttribute attribute = methodInfo!
                .GetCustomAttribute<JSInvokableAttribute>();

            // assert
            attribute.Should().NotBeNull();
        }

        public void Dispose()
        {
            _testContext.DisposeComponents();
            _testContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
