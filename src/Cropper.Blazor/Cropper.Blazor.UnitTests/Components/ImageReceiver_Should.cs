using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Cropper.Blazor.Components;
using Cropper.Blazor.Exceptions;
using FluentAssertions;
using Microsoft.JSInterop;
using Xunit;

namespace Cropper.Blazor.UnitTests.Components
{
    public class ImageReceiver_Should
    {
        [Theory]
        [InlineData(nameof(ImageReceiver.CompleteImageTransfer))]
        [InlineData(nameof(ImageReceiver.HandleImageProcessingError))]
        [InlineData(nameof(ImageReceiver.ReceiveImageChunkAsync))]
        public void Verify_Method_To_Be_Invokable_From_JS<T>(string methodName)
        {
            // act
            MethodInfo? methodInfo = typeof(ImageReceiver)
                .GetMethod(methodName);

            JSInvokableAttribute attribute = methodInfo!
                .GetCustomAttribute<JSInvokableAttribute>();

            // assert
            attribute.Should().NotBeNull();
        }

        [Fact]
        public async Task ReceiveImageChunkAsync_Should_Write_Chunks_To_Stream()
        {
            // arrange
            var receiver = new ImageReceiver();
            var testChunk1 = new byte[] { 1, 2, 3 };
            var testChunk2 = new byte[] { 4, 5 };

            // act
            await receiver.ReceiveImageChunkAsync(testChunk1);
            await receiver.ReceiveImageChunkAsync(testChunk2);
            receiver.CompleteImageTransfer();

            var resultStream = await receiver.GetImageChunkStreamAsync();

            // assert
            resultStream.ToArray().Should().BeEquivalentTo(testChunk1.Concat(testChunk2));
        }

        [Fact]
        public async Task HandleImageProcessingError_Should_Throw_ImageProcessingException()
        {
            // arrange
            var receiver = new ImageReceiver();
            string errorMessage = "Simulated JS error";

            // act
            receiver.HandleImageProcessingError(errorMessage);

            // assert
            var ex = await Assert.ThrowsAsync<ImageProcessingException>(() =>
                receiver.GetImageChunkStreamAsync());

            ex.Message.Should().BeEquivalentTo(errorMessage);
        }

        [Fact]
        public async Task CompleteImageTransfer_Should_Complete_Stream()
        {
            // arrange
            var receiver = new ImageReceiver();
            var chunk = new byte[] { 9, 9, 9 };

            // act
            await receiver.ReceiveImageChunkAsync(chunk);
            receiver.CompleteImageTransfer();

            var chunks = new List<byte[]>();
            await foreach (var receivedChunk in receiver.GetImageStreamAsync())
            {
                chunks.Add(receivedChunk);
            }

            // assert
            chunks
                .Should()
                .ContainSingle()
                .Which
                .Should()
                .BeEquivalentTo(chunk);
        }
    }
}
