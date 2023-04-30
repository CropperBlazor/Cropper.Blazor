using Bogus;
using Microsoft.JSInterop;
using Moq;
using Xunit;
using Event = Cropper.Blazor.Events.CropEndEvent.CropEndEvent;

namespace Cropper.Blazor.UnitTests.Events.CropEndEvent
{
    public class CropEndEvent_Should
    {
        private readonly Mock<IJSObjectReference> _mockIJSObjectReference;
        private readonly Event _event;

        public CropEndEvent_Should()
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
