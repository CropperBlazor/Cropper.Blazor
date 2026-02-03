using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bunit;
using Cropper.Blazor.Components;
using Cropper.Blazor.Services;
using Cropper.Blazor.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

#if NET8_0_OR_GREATER
using TestContext = Bunit.BunitContext;
#endif

namespace Cropper.Blazor.UnitTests.Components
{
    public class CropperComponent_Dispose_Should : IDisposable
    {
        private readonly TestContext _testContext;
        private readonly Mock<ICropperJsInterop> _mockCropperJsInterop;

        public CropperComponent_Dispose_Should()
        {
            _testContext = new Faker<TestContext>()
                .Generate();

            _mockCropperJsInterop = new Mock<ICropperJsInterop>();

            _testContext.Services.AddSingleton(_mockCropperJsInterop.Object);
        }

        [Fact]
        public void Should_Dispose_CropperComponent_After_Render()
        {
            // arrange
            CancellationToken cancellationToken = new();

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .GetIRenderedComponent<CropperComponent>();

            cropperComponent.Instance.Dispose();

            // assert
            Guid cropperComponentId = (Guid)cropperComponent.Instance
                .GetInstanceField("CropperComponentId");

            _mockCropperJsInterop.Verify(c => c.TryLoadModuleAsync(cancellationToken), Times.Once());
            _mockCropperJsInterop.Verify(c => c.DisposeAsync(), Times.Never());
            _mockCropperJsInterop.Verify(c => c.DestroyAsync(cropperComponentId, cancellationToken), Times.Once());
            _mockCropperJsInterop.VerifyNoOtherCalls();
        }

        [Fact]
        public void Should_Dispose_CropperComponent_When_BlazorServer_And_Prerender()
        {
            // arrange
            CancellationToken cancellationToken = new();

            _mockCropperJsInterop
                .Setup(c => c.IsBlazorServer)
                .Returns(true);

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .GetIRenderedComponent<CropperComponent>();

            FieldInfo? isRenderedField = cropperComponent
                .Instance
                .GetType()
                .GetField("IsRendered", BindingFlags.NonPublic | BindingFlags.Instance);
            isRenderedField!.SetValue(cropperComponent.Instance, false);

            cropperComponent.Instance.Dispose();

            // assert
            Guid cropperComponentId = (Guid)cropperComponent.Instance
                .GetInstanceField("CropperComponentId");

            _mockCropperJsInterop.Verify(c => c.IsBlazorServer, Times.Once());
            _mockCropperJsInterop.Verify(c => c.TryLoadModuleAsync(cancellationToken), Times.Once());
            _mockCropperJsInterop.Verify(c => c.DisposeAsync(), Times.Never());
            _mockCropperJsInterop.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Should_DisposeAsync_CropperComponent_After_Render()
        {
            // arrange
            CancellationToken cancellationToken = new();

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .GetIRenderedComponent<CropperComponent>();

            await cropperComponent.Instance.DisposeAsync();

            // assert
            Guid cropperComponentId = (Guid)cropperComponent.Instance
                .GetInstanceField("CropperComponentId");

            _mockCropperJsInterop.Verify(c => c.TryLoadModuleAsync(cancellationToken), Times.Once());
            _mockCropperJsInterop.Verify(c => c.DisposeAsync(), Times.Never());
            _mockCropperJsInterop.Verify(c => c.DestroyAsync(cropperComponentId, cancellationToken), Times.Once());
            _mockCropperJsInterop.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Should_DisposeAsync_CropperComponent_When_BlazorServer_And_Prerender()
        {
            // arrange
            CancellationToken cancellationToken = new();

            _mockCropperJsInterop
                .Setup(c => c.IsBlazorServer)
                .Returns(true);

            // act
            IRenderedComponent<CropperComponent> cropperComponent = _testContext
                .GetIRenderedComponent<CropperComponent>();

            FieldInfo? isRenderedField = cropperComponent
                .Instance
                .GetType()
                .GetField("IsRendered", BindingFlags.NonPublic | BindingFlags.Instance);
            isRenderedField!.SetValue(cropperComponent.Instance, false);

            await cropperComponent.Instance.DisposeAsync();

            // assert
            Guid cropperComponentId = (Guid)cropperComponent.Instance
                .GetInstanceField("CropperComponentId");

            _mockCropperJsInterop.Verify(c => c.IsBlazorServer, Times.Once());
            _mockCropperJsInterop.Verify(c => c.TryLoadModuleAsync(cancellationToken), Times.Once());
            _mockCropperJsInterop.Verify(c => c.DisposeAsync(), Times.Never());
            _mockCropperJsInterop.VerifyNoOtherCalls();
        }

        public void Dispose()
        {
            _testContext.DisposeTestContext();
            GC.SuppressFinalize(this);
        }
    }
}
