using System;
using System.Collections.Generic;
using System.Text.Json;
using Cropper.Blazor.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using Xunit;

namespace Cropper.Blazor.UnitTests.Models
{
    public class Options_Should
    {
        private readonly Options _options;
        private readonly InternalJSRuntime _internalJSRuntime;

        public class InternalJSRuntime : JSRuntime
        {
            protected override void BeginInvokeJS(long taskId, string identifier, string? argsJson, JSCallResultType resultType, long targetInstanceId)
            {
                throw new System.NotImplementedException();
            }

            protected override void EndInvokeDotNet(DotNetInvocationInfo invocationInfo, in DotNetInvocationResult invocationResult)
            {
                throw new System.NotImplementedException();
            }

            public JsonSerializerOptions GetJsonSerializerOptions => JsonSerializerOptions;
        }

        public class CustomElementReferenceContext : ElementReferenceContext
        {

        }

        public Options_Should()
        {
            _options = new();
            _internalJSRuntime = new();
        }

        [Theory, MemberData(nameof(TestData_Setup_Options_Preview))]
        public void Verify_Setup_Options_Preview(object? preview, string expectedObject)
        {
            // arrange
            _options.Preview = preview;

            // act
            string resultObj = JsonSerializer.Serialize(
                _options,
                _internalJSRuntime.GetJsonSerializerOptions);

            // assert
            _options.Preview.Should().BeEquivalentTo(preview);

            resultObj.Should().BeEquivalentTo(expectedObject);
        }

        [Theory, MemberData(nameof(TestData_Throw_ArgumentException_When_Setup_Options_Preview))]
        public void Throw_ArgumentException_When_Setup_Options_Preview(object? preview, string expectedMessage)
        {
            // arrange
            Action act = () => _options.Preview = preview;

            // act & assert
            act
                .Should()
                .Throw<ArgumentException>()
                .WithMessage(expectedMessage);
        }

        public static IEnumerable<object[]> TestData_Setup_Options_Preview()
        {
            yield return WrapArgs(
                null,
                "{\"correlationId\":\"Cropper.Blazor\"}");

            yield return WrapArgs(
                ".testClass",
                "{\"preview\":\".testClass\",\"correlationId\":\"Cropper.Blazor\"}");

            yield return WrapArgs(
                new ElementReference("ElementReferenceId"),
                "{\"preview\":{\"id\":\"ElementReferenceId\",\"context\":null},\"correlationId\":\"Cropper.Blazor\"}");

            yield return WrapArgs(
                new ElementReference("ElementReferenceId", new CustomElementReferenceContext()),
                "{\"preview\":{\"id\":\"ElementReferenceId\",\"context\":{}},\"correlationId\":\"Cropper.Blazor\"}");

            yield return WrapArgs(
                new ElementReference[]
                {
                    new ElementReference("ElementReferenceId"),
                    new ElementReference("ElementReferenceId", new CustomElementReferenceContext())
                },
                "{\"preview\":[{\"id\":\"ElementReferenceId\",\"context\":null},{\"id\":\"ElementReferenceId\",\"context\":{}}],\"correlationId\":\"Cropper.Blazor\"}");

            static object[] WrapArgs(
                object? preview,
                string expectedObject)
                    => new object[]
                    {
                        preview!,
                        expectedObject
                    };
        }

        public static IEnumerable<object[]> TestData_Throw_ArgumentException_When_Setup_Options_Preview()
        {
            yield return WrapArgs(
                Array.Empty<ElementReference>(),
                "'Preview' should be not an empty collection of ElementReference.");

            yield return WrapArgs(
                new ElementReference(),
                "'Preview' must not contain an empty Reference element.");

            yield return WrapArgs(
                new ElementReference[]
                {
                    new ElementReference(),
                    new ElementReference("ElementReferenceId"),
                    new ElementReference("ElementReferenceId", new CustomElementReferenceContext())
                },
                "'Preview' must not contain an empty Reference element in the ElementReference collection.");

            yield return WrapArgs(
                "",
                "'Preview' should be not an empty or include white spaces in the string.");

            yield return WrapArgs(
                " ",
                "'Preview' should be not an empty or include white spaces in the string.");

            yield return WrapArgs(
                new object(),
                "'Preview' is only available for string, ElementReference, IEnumerable<ElementReference> types, but found 'System.Object' type.");

            yield return WrapArgs(
                new CustomElementReferenceContext(),
                "'Preview' is only available for string, ElementReference, IEnumerable<ElementReference> types, but found 'Cropper.Blazor.UnitTests.Models.Options_Should+CustomElementReferenceContext' type.");

            static object[] WrapArgs(
                object? preview,
                string expectedMessage)
                    => new object[]
                    {
                        preview!,
                        expectedMessage
                    };
        }
    }
}
