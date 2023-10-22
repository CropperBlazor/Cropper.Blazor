using System.Collections.Generic;
using Bogus;
using Microsoft.JSInterop;
using Moq;
using Xunit;
using Event = Cropper.Blazor.Events.CropStartEvent.CropStartEvent;

namespace Cropper.Blazor.UnitTests.Events.CropStartEvent
{
    public class CropStartEvent_Should
    {
        private static Mock<IJSObjectReference> _mockIJSObjectReference = null!;

        [Theory, MemberData(nameof(TestData_Verify_Dispose))]
        public void Verify_Dispose(IJSObjectReference? jSObjectReference, Times times)
        {
            // arrange
            Event @event = new Faker<Event>()
                .RuleFor(x => x.OriginalEvent, jSObjectReference);

            // act
            @event.Dispose();

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
