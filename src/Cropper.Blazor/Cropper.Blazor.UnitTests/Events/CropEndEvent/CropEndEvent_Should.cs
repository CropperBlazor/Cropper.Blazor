using Bogus;
using Xunit;
using Cropper.Blazor.Events;
using Event = Cropper.Blazor.Events.CropEndEvent.CropEndEvent;

namespace Cropper.Blazor.UnitTests.Events.CropEndEvent
{
    public class CropEndEvent_Should
    {
        [Fact]
        public void Create_WithOriginalEventMetadata()
        {
            // arrange
            OriginalEvent originalEvent = new()
            {
                Type = "pointerup",
                Button = 0,
                ClientX = 90,
                ClientY = 12
            };

            // act
            Event @event = new Faker<Event>()
                .RuleFor(x => x.OriginalEvent, originalEvent);

            // assert
            Assert.Same(originalEvent, @event.OriginalEvent);
        }
    }
}
