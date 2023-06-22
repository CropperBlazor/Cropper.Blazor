using System;
using System.Collections.Generic;
using Cropper.Blazor.Extensions;
using FluentAssertions;
using Xunit;

namespace Cropper.Blazor.UnitTests.Extensions
{
    public class UriExtensions_Should
    {
        [Theory, MemberData(nameof(TestData_ToHostName))]
        public void ToHostName(Uri uri, string expectedHostName)
        {
            // act
            string hostName = uri.GetHostName();

            // assert
            hostName.Should().Be(expectedHostName);
        }

        public static IEnumerable<object[]> TestData_ToHostName()
        {
            yield return WrapArgs(new Uri("http://localhost"), "http:localhost");

            yield return WrapArgs(new Uri("https://localhost"), "https:localhost");

            yield return WrapArgs(new Uri("http://localhost:5000"), "http:localhost:5000");

            yield return WrapArgs(new Uri("https://localhost:5001"), "https:localhost:5001");

            yield return WrapArgs(new Uri("http://localhost/"), "http:localhost");

            yield return WrapArgs(new Uri("https://localhost/"), "https:localhost");

            yield return WrapArgs(new Uri("http://localhost:5000/"), "http:localhost:5000");

            yield return WrapArgs(new Uri("https://localhost:5001/"), "https:localhost:5001");

            yield return WrapArgs(new Uri("http://localhost/testpath"), "http://localhost");

            yield return WrapArgs(new Uri("https://localhost/testpath"), "https://localhost");

            yield return WrapArgs(new Uri("http://localhost:5000/testpath"), "http://localhost:5000");

            yield return WrapArgs(new Uri("https://localhost:5001/testpath"), "https://localhost:5001");

            yield return WrapArgs(new Uri("http://localhost/testpath"), "http://localhost");

            yield return WrapArgs(new Uri("https://localhost/testpath"), "https://localhost");

            yield return WrapArgs(new Uri("http://localhost:5000/testpath"), "http://localhost:5000");

            yield return WrapArgs(new Uri("https://localhost:5001/testpath"), "https://localhost:5001");

            yield return WrapArgs(new Uri("http://localhost/testpath?name=123"), "http://localhost");

            yield return WrapArgs(new Uri("https://localhost/testpath?name=123"), "https://localhost");

            yield return WrapArgs(new Uri("http://localhost:5000/testpath?name=123"), "http://localhost:5000");

            yield return WrapArgs(new Uri("https://localhost:5001/testpath?name=123"), "https://localhost:5001");

            static object[] WrapArgs(
                Uri uri,
                string expectedHostName)
                => new object[]
                {
                    uri,
                    expectedHostName,
                };
        }
    }
}
