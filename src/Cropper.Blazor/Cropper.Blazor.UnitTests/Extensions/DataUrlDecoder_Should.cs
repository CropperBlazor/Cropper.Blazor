using System.Collections.Generic;
using System.Text;
using Cropper.Blazor.Extensions;
using FluentAssertions;
using Xunit;

namespace Cropper.Blazor.UnitTests.Extensions
{
    public class DataUrlDecoder_Should
    {
        [Theory, MemberData(nameof(TestData_Decode))]
        public void DecodeDataUrlIntoByteArrayAndMediaType(string dataUrl, byte[] expectedImageData, string expectedMediaType)
        {
            // act
            var (imageData, mediaType) = DataUrlDecoder.Decode(dataUrl);

            // assert
            imageData.Should().Equal(expectedImageData);
            mediaType.Should().Be(expectedMediaType);
        }

        public static IEnumerable<object[]> TestData_Decode()
        {
            // The base64 string 'SGVsbG8gd29ybGQ=' corresponds to 'Hello world'
            yield return WrapArgs("data:text/plain;base64,SGVsbG8gd29ybGQ=", Encoding.UTF8.GetBytes("Hello world"), "text/plain;base64");

            static object[] WrapArgs(
                string dataUrl,
                byte[] expectedImageData,
                string expectedMediaType)
                => new object[]
                {
                    dataUrl,
                    expectedImageData,
                    expectedMediaType,
                };
        }
    }
}
