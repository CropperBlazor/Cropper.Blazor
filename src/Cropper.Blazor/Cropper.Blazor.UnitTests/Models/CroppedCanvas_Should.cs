using Bogus;
using Cropper.Blazor.Models;
using Microsoft.JSInterop;
using Moq;
using Xunit;

namespace Cropper.Blazor.UnitTests.Models
{
    public class CroppedCanvas_Should
    {
        private readonly Mock<IJSObjectReference> _mockIJSObjectReference;
        private readonly CroppedCanvas _croppedCanvas;

        public CroppedCanvas_Should()
        {
            _mockIJSObjectReference = new Mock<IJSObjectReference>();
            _croppedCanvas = new Faker<CroppedCanvas>()
                .CustomInstantiator(f => new CroppedCanvas(_mockIJSObjectReference.Object));
        }

        [Fact]
        public void Verify_Dispose()
        {
            // act
            _croppedCanvas.Dispose();

            // assert
            _mockIJSObjectReference.Verify(c => c.DisposeAsync(), Times.Once());
        }
    }
}
