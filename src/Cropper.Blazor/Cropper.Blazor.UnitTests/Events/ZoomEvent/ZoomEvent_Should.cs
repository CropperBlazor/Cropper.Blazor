using Bogus;
using Microsoft.JSInterop;
using Moq;
using Xunit;
using Event = Cropper.Blazor.Events.ZoomEvent.ZoomEvent;

namespace Cropper.Blazor.UnitTests.Events.ZoomEvent
{
    public class ZoomEvent_Should
    {
        private readonly Mock<IJSObjectReference> _mockIJSObjectReference;
        private readonly Event _event;

        public ZoomEvent_Should()
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
