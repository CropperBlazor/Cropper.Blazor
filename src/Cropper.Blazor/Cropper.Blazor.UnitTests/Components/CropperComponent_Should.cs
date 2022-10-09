using AngleSharp.Dom;
using Bogus;
using Bunit;
using Cropper.Blazor.Base;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events.CropEndEvent;
using Cropper.Blazor.Events.CropEvent;
using Cropper.Blazor.Events.CropMoveEvent;
using Cropper.Blazor.Events.CropReadyEvent;
using Cropper.Blazor.Events.CropStartEvent;
using Cropper.Blazor.Events.ZoomEvent;
using Cropper.Blazor.Models;
using Cropper.Blazor.Services;
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

        [Fact]
        public async Task Should_Render_CropperComponent_SuccessfulAsync()
        {
            // arrange
            string errorLoadImageClass = "cropper-error-load";
            string imageClass = "cropper";
            string lazyAttributeValue = "lazy";
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
            CancellationToken cancellationToken = new CancellationToken();
            ZoomEvent zoomEvent = new Faker<ZoomEvent>()
                .Generate();
            CropStartEvent cropStartEvent = new Faker<CropStartEvent>()
                .Generate();
            CropMoveEvent cropMoveEvent = new Faker<CropMoveEvent>()
                .Generate();
            CropEndEvent cropEndEvent = new Faker<CropEndEvent>()
                .Generate();
            CropEvent cropEvent = new Faker<CropEvent>()
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
            object expectedCroppedCanvas = new Faker<object>()
                .Generate();
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
            CropReadyEvent cropReadyEvent = new Faker<CropReadyEvent>()
                .Generate();

            Faker faker = new Faker();
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

            Action? onLoadImageHandler = () =>
            {
                countCallsOnLoadImageHandler++;
            };
            Action<ErrorEventArgs>? onErrorLoadImageHandler = (ErrorEventArgs e) =>
            {
                countCallsOnErrorLoadImageHandler++;
                errorEventArgs.Should().BeEquivalentTo(e);
            };
            Action<CropEvent>? onCropEventHandler = (CropEvent c) =>
            {
                countCallsOnCropEventHandler++;
                cropEvent.Should().BeEquivalentTo(c);
            };
            Action<CropEndEvent>? onCropEndEventHandler = (CropEndEvent c) =>
            {
                countCallsOnCropEndEventHandler++;
                cropEndEvent.Should().BeEquivalentTo(c);
            };
            Action<CropMoveEvent>? onCropMoveEventHandler = (CropMoveEvent c) =>
            {
                countCallsOnCropMoveEventHandler++;
                cropMoveEvent.Should().BeEquivalentTo(c);
            };
            Action<CropStartEvent>? onCropStartEventHandler = (CropStartEvent c) =>
            {
                countCallsOnCropStartEventHandler++;
                cropStartEvent.Should().BeEquivalentTo(c);
            };
            Action<ZoomEvent>? onZoomEventHandler = (ZoomEvent z) =>
            {
                countCallsOnZoomEventHandler++;
                zoomEvent.Should().BeEquivalentTo(z);
            };
            Action<CropReadyEvent>? onCropReadyEventHandler = (CropReadyEvent C) =>
            {
                countCallsOnCropReadyEventHandler++;
                cropReadyEvent.Should().BeEquivalentTo(C);
            };

            ComponentParameter imageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Class),
                imageClass);
            ComponentParameter errorLoadImageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageClass),
                errorLoadImageClass);
            ComponentParameter loadingParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Loading),
                lazyAttributeValue);
            ComponentParameter errorLoadImageSrcParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageSrc),
                errorLoadImageSrcAttributeValue);
            ComponentParameter srcParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Src),
                imageSrcAttributeValue);
            ComponentParameter isErrorLoadImageParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.IsErrorLoadImage),
                false);
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
                .Setup(c => c.GetCanvasDataAsync())
                .ReturnsAsync(expectedCanvasData);

            _mockCropperJsInterop
                .Setup(c => c.GetContainerDataAsync())
                .ReturnsAsync(expectedContainerData);

            _mockCropperJsInterop
                .Setup(c => c.GetCropBoxDataAsync())
                .ReturnsAsync(expectedCropBoxData);

            _mockCropperJsInterop
                .Setup(c => c.GetCroppedCanvasAsync(getCroppedCanvasOptions))
                .ReturnsAsync(expectedCroppedCanvas);

            _mockCropperJsInterop
                .Setup(c => c.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions))
                .ReturnsAsync(expectedCroppedCanvasDataURL);

            _mockCropperJsInterop
                .Setup(c => c.GetDataAsync(isRounded))
                .ReturnsAsync(expectedCropperData);

            _mockCropperJsInterop
                .Setup(c => c.GetImageDataAsync())
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
                .GetInstanceField("imageReference");

            _mockCropperJsInterop.Verify(c => c.LoadModuleAsync(), Times.Once());
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
                It.IsAny<DotNetObjectReference<ICropperComponentBase>>()), Times.Once());

            countCallsOnErrorLoadImageHandler.Should().Be(0);
            expectedElement.TriggerEvent("onerror", errorEventArgs);
            countCallsOnErrorLoadImageHandler.Should().Be(1);

            await cropperComponent.InvokeAsync(async () =>
            {
                cropperComponent.Instance.Clear();
                _mockCropperJsInterop.Verify(c => c.ClearAsync(), Times.Once());

                cropperComponent.Instance.Crop();
                _mockCropperJsInterop.Verify(c => c.CropAsync(), Times.Once());

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
                _mockCropperJsInterop.Verify(c => c.DestroyAsync(), Times.Once());

                cropperComponent.Instance.Disable();
                _mockCropperJsInterop.Verify(c => c.DisableAsync(), Times.Once());

                cropperComponent.Instance.Enable();
                _mockCropperJsInterop.Verify(c => c.EnableAsync(), Times.Once());

                CanvasData canvasData = await cropperComponent.Instance.GetCanvasDataAsync();
                expectedCanvasData.Should().BeEquivalentTo(canvasData);
                _mockCropperJsInterop.Verify(c => c.GetCanvasDataAsync(), Times.Once());

                ContainerData containerData = await cropperComponent.Instance.GetContainerDataAsync();
                expectedContainerData.Should().BeEquivalentTo(containerData);
                _mockCropperJsInterop.Verify(c => c.GetContainerDataAsync(), Times.Once());

                CropBoxData cropBoxData = await cropperComponent.Instance.GetCropBoxDataAsync();
                expectedCropBoxData.Should().BeEquivalentTo(cropBoxData);
                _mockCropperJsInterop.Verify(c => c.GetCropBoxDataAsync(), Times.Once());

                object croppedCanvas = await cropperComponent.Instance.GetCroppedCanvasAsync(getCroppedCanvasOptions);
                expectedCroppedCanvas.Should().BeEquivalentTo(croppedCanvas);
                _mockCropperJsInterop.Verify(c => c.GetCroppedCanvasAsync(getCroppedCanvasOptions), Times.Once());

                string croppedCanvasDataURL = await cropperComponent.Instance.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions);
                expectedCroppedCanvasDataURL.Should().BeEquivalentTo(croppedCanvasDataURL);
                _mockCropperJsInterop.Verify(c => c.GetCroppedCanvasDataURLAsync(getCroppedCanvasOptions), Times.Once());

                CropperData cropperData = await cropperComponent.Instance.GetDataAsync(isRounded);
                expectedCropperData.Should().BeEquivalentTo(cropperData);
                _mockCropperJsInterop.Verify(c => c.GetDataAsync(isRounded), Times.Once());

                ImageData imageData = await cropperComponent.Instance.GetImageDataAsync();
                expectedImageData.Should().Be(imageData);
                _mockCropperJsInterop.Verify(c => c.GetImageDataAsync(), Times.Once());

                string image = await cropperComponent.Instance.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken);
                expectedImage.Should().Be(image);
                _mockCropperJsInterop.Verify(c => c.GetImageUsingStreamingAsync(imageFile, maxAllowedSize, cancellationToken), Times.Once());

                cropperComponent.Instance.IsReady(cropReadyEvent);
                countCallsOnCropReadyEventHandler.Should().Be(1);

                cropperComponent.Instance.Move(offsetX, offsetY);
                _mockCropperJsInterop.Verify(c => c.MoveAsync(offsetX, offsetY), Times.Once());

                cropperComponent.Instance.MoveTo(x, y);
                _mockCropperJsInterop.Verify(c => c.MoveToAsync(x, y), Times.Once());

                cropperComponent.Instance.OnErrorLoadImage(errorEventArgs);
                countCallsOnErrorLoadImageHandler.Should().Be(2);

                cropperComponent.Instance.Reset();
                _mockCropperJsInterop.Verify(c => c.ResetAsync(), Times.Once());

                await cropperComponent.Instance.RevokeObjectUrlAsync(url);
                _mockCropperJsInterop.Verify(c => c.RevokeObjectUrlAsync(url), Times.Once());

                cropperComponent.Instance.Rotate(degree);
                _mockCropperJsInterop.Verify(c => c.RotateAsync(degree), Times.Once());

                cropperComponent.Instance.Scale(scaleX, scaleY);
                _mockCropperJsInterop.Verify(c => c.ScaleAsync(scaleX, scaleY), Times.Once());

                cropperComponent.Instance.ScaleX(scaleX);
                _mockCropperJsInterop.Verify(c => c.ScaleXAsync(scaleX), Times.Once());

                cropperComponent.Instance.ScaleY(scaleY);
                _mockCropperJsInterop.Verify(c => c.ScaleYAsync(scaleY), Times.Once());

                cropperComponent.Instance.SetAspectRatio(aspectRatio);
                _mockCropperJsInterop.Verify(c => c.SetAspectRatioAsync(aspectRatio), Times.Once());

                cropperComponent.Instance.SetCanvasData(setCanvasDataOptions);
                _mockCropperJsInterop.Verify(c => c.SetCanvasDataAsync(setCanvasDataOptions), Times.Once());

                cropperComponent.Instance.SetCropBoxData(setCropBoxDataOptions);
                _mockCropperJsInterop.Verify(c => c.SetCropBoxDataAsync(setCropBoxDataOptions), Times.Once());

                cropperComponent.Instance.SetData(setDataOptions);
                _mockCropperJsInterop.Verify(c => c.SetDataAsync(setDataOptions), Times.Once());

                cropperComponent.Instance.SetDragMode(dragMode);
                _mockCropperJsInterop.Verify(c => c.SetDragModeAsync(dragMode), Times.Once());

                cropperComponent.Instance.Zoom(ratio);
                _mockCropperJsInterop.Verify(c => c.ZoomAsync(ratio), Times.Once());

                cropperComponent.Instance.ZoomTo(ratio, pivotX, pivotY);
                _mockCropperJsInterop.Verify(c => c.ZoomToAsync(ratio, pivotX, pivotY), Times.Once());
            });
        }

        [Fact]
        public void Should_Render_CropperComponent_With_ErrorLoadImage_Parameter()
        {
            // arrange
            string errorLoadImageClass = "cropper-error-load";
            string lazyAttributeValue = "lazy";
            string errorLoadImageSrcAttributeValue = "https://.../not-found-image.jpg";
            ComponentParameter errorLoadImageClassParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.ErrorLoadImageClass),
                errorLoadImageClass);
            ComponentParameter loadingParameter = ComponentParameter.CreateParameter(
                nameof(CropperComponent.Loading),
                lazyAttributeValue);
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
                .GetInstanceField("imageReference");

            elementReference.Id.Should().BeNullOrEmpty();
            expectedElement.ClassName.Should().Be(errorLoadImageClass);
            expectedElement.GetAttribute("loading").Should().Be(lazyAttributeValue);
            expectedElement.GetAttribute("src").Should().Be(errorLoadImageSrcAttributeValue);
            expectedElement.GetAttribute("blazor:elementreference").Should().BeNullOrEmpty();
        }

        public void Dispose()
        {
            _testContext.Dispose();
            _testContext.DisposeComponents();
        }
    }
}
