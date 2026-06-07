using Bogus;
using Xunit;
using Cropper.Blazor.Events;
using Event = Cropper.Blazor.Events.CropMoveEvent.CropMoveEvent;

namespace Cropper.Blazor.UnitTests.Events.CropMoveEvent
{
    public class CropMoveEvent_Should
    {
        [Fact]
        public void Create_WithOriginalEventMetadata()
        {
            // arrange
            OriginalEvent originalEvent = new()
            {
                Type = "pointermove",
                Buttons = 1,
                PageX = 56,
                PageY = 78
            };

            // act
            Event @event = new Faker<Event>()
                .RuleFor(x => x.OriginalEvent, originalEvent);

            // assert
            Assert.Same(originalEvent, @event.OriginalEvent);
        }
    }
}
