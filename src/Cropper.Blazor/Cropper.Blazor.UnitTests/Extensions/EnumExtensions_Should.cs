using System.Collections.Generic;
using System.Runtime.Serialization;
using Cropper.Blazor.Extensions;
using FluentAssertions;
using Xunit;
using static Cropper.Blazor.UnitTests.Extensions.EnumExtensions_Should.TestData;

namespace Cropper.Blazor.UnitTests.Extensions
{
    public class EnumExtensions_Should
    {
        public static class TestData
        {
            public const string TestEnum_First_Name = "FirstNameTest";
            public const string TestEnum_Second_Name = "SecondNameTest";

            public enum TestEnum
            {
                DefaultValue,

                [EnumMember(Value = TestEnum_First_Name)]
                FirstValue,

                [EnumMember(Value = TestEnum_Second_Name)]
                SecondValue
            }
        }

        [Theory, MemberData(nameof(TestData_ToEnumString))]
        public void ToEnumString(TestEnum enum-value, string expectedString)
        {
            // act
            var actualValue = enum-value.ToEnumString();

            // assert
            actualValue.Should().Be(expectedString);
        }

        public static IEnumerable<object[]> TestData_ToEnumString()
        {
            yield return WrapArgs(TestEnum.DefaultValue, null);
            yield return WrapArgs(TestEnum.FirstValue, TestEnum_First_Name);
            yield return WrapArgs(TestEnum.SecondValue, TestEnum_Second_Name);

            static object[] WrapArgs(
                TestEnum enum-value,
                string? expectedString)
                => new object[]
                {
                    enum-value,
                    expectedString!
                };
        }
    }
}
