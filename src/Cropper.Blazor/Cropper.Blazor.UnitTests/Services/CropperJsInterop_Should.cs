using FluentAssertions;
using Xunit;

namespace Cropper.Blazor.UnitTests.Services
{
    public class CropperJsInterop_Should
    {
        public CropperJsInterop_Should()
        {

        }

        [Fact]
        public async Task Verify_LoadAsync()
        {
            1.Should().Be(2);
        }
    }
}
