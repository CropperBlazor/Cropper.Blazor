using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Bunit;
using Bunit.TestDoubles;
using Cropper.Blazor.ModuleOptions;
using Cropper.Blazor.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public class UrlImageInterop_Should : BaseJsInteropService_Should, IDisposable
    {
        private readonly Faker _faker;
        private readonly IUrlImageInterop _urlImageInterop;
        private const string PathToCropperModule = "_content/Cropper.Blazor/cropperJsInterop.min.js";
        private static string DefaultPathToCropperModule => Path.Combine("http:localhost", PathToCropperModule);

        public UrlImageInterop_Should() : base()
        {
            _faker = new Faker();

            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            _urlImageInterop = new Faker<IUrlImageInterop>()
                .CustomInstantiator(f => new UrlImageInterop(_testContext.JSInterop.JSRuntime, fakeNavigationManager, new CropperJsInteropOptions()))
                .Generate();
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
                    .Setup<string>("cropperUrlImageHelper.getImageUsingStreaming",
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
            string imageData = await _urlImageInterop.GetImageUsingStreamingAsync(mockImageFile.Object, maxAllowedSize);

            // assert
            expectedImageData.Should().BeEquivalentTo(imageData);
        }

        [Fact]
        public async Task Verify_RevokeObjectUrlAsync()
        {
            // arrange
            string url = _faker.Random.Word();

            _testContext.JSInterop
                .SetupVoid("cropperUrlImageHelper.revokeObjectUrl", url)
                .SetVoidResult();

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await _urlImageInterop.RevokeObjectUrlAsync(url);
        }

        [Fact]
        public async Task Verify_DisposeAsync()
        {
            // arrange
            FakeNavigationManager fakeNavigationManager = _testContext.Services.GetRequiredService<FakeNavigationManager>();
            UrlImageInterop urlImageInterop = new(_testContext.JSInterop.JSRuntime, fakeNavigationManager, new CropperJsInteropOptions());

            // assert
            VerifyLoadCropperModule(DefaultPathToCropperModule);

            // act
            await urlImageInterop.LoadModuleAsync();
            await urlImageInterop.DisposeAsync();
        }

        public void Dispose()
        {
            _testContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
