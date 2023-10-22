using System.Collections.Generic;
using Bogus;
using Cropper.Blazor.Models;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Models
{
    public class CroppedCanvas_Should
    {
        private static Mock<IJSObjectReference> _mockIJSObjectReference = null!;

        [Theory, MemberData(nameof(TestData_Verify_Dispose))]
        public void Verify_Dispose(IJSObjectReference? jSObjectReference, Times times)
        {
            // arrange
            CroppedCanvas croppedCanvas = new Faker<CroppedCanvas>()
                .CustomInstantiator(f => new CroppedCanvas(jSObjectReference));

            // act
            croppedCanvas.Dispose();

            // assert
            _mockIJSObjectReference.Verify(c => c.DisposeAsync(), times);
        }

        public static IEnumerable<object[]> TestData_Verify_Dispose()
        {
            yield return WrapArgs(null, Times.Never());

            _mockIJSObjectReference = new Mock<IJSObjectReference>();
            yield return WrapArgs(_mockIJSObjectReference.Object, Times.Once());

            static object[] WrapArgs(
                IJSObjectReference? jSObjectReference,
                Times times)
                => new object[]
                {
                    jSObjectReference,
                    times
                };
        }
    }
}
