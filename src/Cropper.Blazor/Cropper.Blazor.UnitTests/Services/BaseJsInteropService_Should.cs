using System.Threading;
using System.Threading.Tasks;
using System;
using Bogus;
using Cropper.Blazor.ModuleOptions;
using Cropper.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Xunit;
using FluentAssertions;
using Cropper.Blazor.Testing;

#if NET8_0_OR_GREATER
using TestContext = Bunit.BunitContext;
#else
using Bunit;
#endif

namespace Cropper.Blazor.UnitTests.Services
{
    public class BaseJsInteropService_Should : IDisposable
    {
        protected readonly TestContext _testContext;

        public BaseJsInteropService_Should()
        {
            _testContext = new Faker<TestContext>()
                .Generate();
        }

        [Fact]
        public void IsBlazorServer_Should_Return_True_For_RemoteJSRuntime()
        {
            // Arrange
            RemoteJSRuntime jsRuntime = new RemoteJSRuntime();
            NavigationManager navigation = _testContext.Services.GetRequiredService<NavigationManager>();
            ICropperJsInteropOptions options = new Faker<CropperJsInteropOptions>().Generate();
            TestBaseJsInterop service = new TestBaseJsInterop(jsRuntime, navigation, options);

            // Act
            bool result = service.IsBlazorServer;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsBlazorServer_Should_Return_False_For_OtherJSRuntime()
        {
            // Arrange
            NavigationManager navigation = _testContext.Services.GetRequiredService<NavigationManager>();
            ICropperJsInteropOptions options = new Faker<CropperJsInteropOptions>().Generate();
            TestBaseJsInterop service = new TestBaseJsInterop(_testContext.JSInterop.JSRuntime, navigation, options);

            // Act
            bool result = service.IsBlazorServer;

            // Assert
            result.Should().BeFalse();
        }

        public void Dispose()
        {
            _testContext.DisposeTestContext();
            GC.SuppressFinalize(this);
        }

        private class TestBaseJsInterop : BaseJsInterop
        {
            public TestBaseJsInterop(
                IJSRuntime jsRuntime,
                NavigationManager navigationManager,
                ICropperJsInteropOptions cropperJsInteropOptions) : base(jsRuntime, navigationManager, cropperJsInteropOptions)
            {

            }
        }

        private class RemoteJSRuntime : IJSRuntime
        {
            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
                throw new NotImplementedException();

            public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
                throw new NotImplementedException();
        }
    }
}
