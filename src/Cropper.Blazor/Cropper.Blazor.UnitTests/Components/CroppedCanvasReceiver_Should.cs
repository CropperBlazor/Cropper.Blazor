using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cropper.Blazor.Components;
using Cropper.Blazor.Models;
using FluentAssertions;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Components
{
    public class CroppedCanvasReceiver_Should
    {
        [Theory]
        [InlineData(nameof(CroppedCanvasReceiver.ReceiveCanvasReference))]
        public void Verify_Method_To_Be_Invokable_From_JS<T>(string methodName)
        {
            // act
            MethodInfo? methodInfo = typeof(CroppedCanvasReceiver)
                .GetMethod(methodName);

            JSInvokableAttribute? attribute = methodInfo!
                .GetCustomAttribute<JSInvokableAttribute>();

            // assert
            attribute.Should().NotBeNull();
        }

        [Fact]
        public async Task ReceiveCanvasReference_Should_Set_CroppedCanvas_Property()
        {
            // arrange
            Mock<IJSObjectReference> mockJsRef = new Mock<IJSObjectReference>();
            TaskCompletionSource<CroppedCanvas> tcs = new TaskCompletionSource<CroppedCanvas>();
            CancellationToken cancellationToken = new CancellationToken();

            var receiver = new CroppedCanvasReceiver(
                (canvas, token) =>
                {
                    tcs.TrySetResult(canvas);
                    cancellationToken.Should().BeEquivalentTo(token);

                    return Task.CompletedTask;
                },
                cancellationToken);

            // act
            receiver.ReceiveCanvasReference(mockJsRef.Object);

            // assert
            var completedCroppedCanvasTask = await tcs.Task;

            receiver.CroppedCanvas!.JSRuntimeObjectRef.Should().BeSameAs(mockJsRef.Object);
            completedCroppedCanvasTask.Should().BeSameAs(receiver.CroppedCanvas);
        }

        [Fact]
        public void ReceiveCanvasReference_Should_Pass_Filled_JSObjectReference_To_Callback()
        {
            // arrange
            Mock<IJSObjectReference> mockJsRef = new Mock<IJSObjectReference>();
            CroppedCanvas? receivedCanvas = null;

            var receiver = new CroppedCanvasReceiver(
                (canvas, _) =>
                {
                    receivedCanvas = canvas;

                    return Task.CompletedTask;
                },
                CancellationToken.None);

            // act
            receiver.ReceiveCanvasReference(mockJsRef.Object);

            // assert
            receivedCanvas.Should().NotBeNull();
            receivedCanvas!.JSRuntimeObjectRef.Should().BeSameAs(mockJsRef.Object);
            receiver.CroppedCanvas.JSRuntimeObjectRef.Should().BeSameAs(mockJsRef.Object);
        }
    }
}
