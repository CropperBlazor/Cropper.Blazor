using Bogus;
using Xunit;
using Cropper.Blazor.Events;
using Event = Cropper.Blazor.Events.CropStartEvent.CropStartEvent;

namespace Cropper.Blazor.UnitTests.Events.CropStartEvent
{
    public class CropStartEvent_Should
    {
        [Fact]
        public void Create_WithOriginalEventMetadata()
        {
            // arrange
            OriginalEvent originalEvent = new()
            {
                Type = "pointerdown",
                ClientX = 12,
                ClientY = 34,
                ShiftKey = false
            };

            // act
            Event @event = new Faker<Event>()
                .RuleFor(x => x.OriginalEvent, originalEvent);

            // assert
            Assert.Same(originalEvent, @event.OriginalEvent);
        }
    }
}
