using Bogus;
using Xunit;
using Cropper.Blazor.Events;
using Event = Cropper.Blazor.Events.ZoomEvent.ZoomEvent;

namespace Cropper.Blazor.UnitTests.Events.ZoomEvent
{
    public class ZoomEvent_Should
    {
        [Fact]
        public void Create_WithOriginalEventMetadata()
        {
            // arrange
            OriginalEvent originalEvent = new()
            {
                Type = "wheel",
                DeltaY = -120,
                ClientX = 34,
                ClientY = 56
            };

            // act
            Event @event = new Faker<Event>()
                .RuleFor(x => x.OriginalEvent, originalEvent);

            // assert
            Assert.Same(originalEvent, @event.OriginalEvent);
        }
    }
}
