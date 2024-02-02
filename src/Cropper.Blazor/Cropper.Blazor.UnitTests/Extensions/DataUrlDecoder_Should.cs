using System;
using System.Collections.Generic;
using Cropper.Blazor.Extensions;
using FluentAssertions;
using Xunit;

namespace Cropper.Blazor.UnitTests.Extensions
{
    public class DataUrlDecoder_Should
    {
        [Theory, MemberData(nameof(TestData_Decode))]
        public void DecodeDataUrlIntoByteArrayAndMediaType(
            string dataUrl,
            string expectedBase64ImageData,
            string expectedMediaType)
        {
            // act
            var (imageData, mediaType) = dataUrl.Decode();

            // assert
            imageData.Should().BeEquivalentTo(expectedBase64ImageData);
            mediaType.Should().BeEquivalentTo(expectedMediaType);
        }

        [Theory, MemberData(nameof(TestData_Throw_ArgumentException_When_Decode))]
        public void Throw_ArgumentException_When_Decode(
            string dataUrl,
            string expectedMessage)
        {
            // arrange
            Action act = () => dataUrl.Decode();

            // act & assert
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(expectedMessage);
        }

        public static IEnumerable<object[]> TestData_Decode()
        {
            yield return WrapArgs(
                "data:text/plain;base64,SGVsbG8gd29ybGQ=",
                "SGVsbG8gd29ybGQ=",
                "text/plain;base64");

            yield return WrapArgs(
                "data:image/png;base64,SGVsbG8gd29ybGQ=",
                "SGVsbG8gd29ybGQ=",
                "image/png;base64");

            static object[] WrapArgs(
                string dataUrl,
                string expectedBase64ImageData,
                string expectedMediaType)
                => new object[]
                {
                    dataUrl,
                    expectedBase64ImageData,
                    expectedMediaType,
                };
        }

        public static IEnumerable<object[]> TestData_Throw_ArgumentException_When_Decode()
        {
            yield return WrapArgs(
                "text/plain;base64,SGVsbG8gd29ybGQ=",
                $"Could not parse 'text/plain;base64,SGVsbG8gd29ybGQ=' as '\"data:(?<type>.+?),(?<data>.+)\"' data URL pattern.");

            yield return WrapArgs(
                ":image/png;base64,SGVsbG8gd29ybGQ=",
                $"Could not parse ':image/png;base64,SGVsbG8gd29ybGQ=' as '\"data:(?<type>.+?),(?<data>.+)\"' data URL pattern.");

            yield return WrapArgs(
                "data:image/png;base64.SGVsbG8gd29ybGQ=",
                $"Could not parse 'data:image/png;base64.SGVsbG8gd29ybGQ=' as '\"data:(?<type>.+?),(?<data>.+)\"' data URL pattern.");

            yield return WrapArgs(
                "data:image/png;base64SGVsbG8gd29ybGQ=",
                $"Could not parse 'data:image/png;base64SGVsbG8gd29ybGQ=' as '\"data:(?<type>.+?),(?<data>.+)\"' data URL pattern.");

            static object[] WrapArgs(
                string dataUrl,
                string expectedMessage)
                => new object[]
                {
                    dataUrl,
                    expectedMessage
                };
        }
    }
}
