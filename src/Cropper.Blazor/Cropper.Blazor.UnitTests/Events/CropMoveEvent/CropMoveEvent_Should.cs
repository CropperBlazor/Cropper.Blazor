using Bogus;
using Microsoft.JSInterop;
using Moq;
using Xunit;
using Event = Cropper.Blazor.Events.CropMoveEvent.CropMoveEvent;

namespace Cropper.Blazor.UnitTests.Events.CropMoveEvent
{
    public class CropMoveEvent_Should
    {
        private readonly Mock<IJSObjectReference> _mockIJSObjectReference;
        private readonly Event _event;

        public CropMoveEvent_Should()
        {
            _mockIJSObjectReference = new Mock<IJSObjectReference>();
            _event = new Faker<Event>()
                .RuleFor(x => x.OriginalEvent, _mockIJSObjectReference.Object);
        }

        [Fact]
        public void Verify_Dispose()
        {
            // act
            _event.Dispose();

            // assert
            _mockIJSObjectReference.Verify(c => c.DisposeAsync(), Times.Once());
        }
    }
}
