using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
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
                throw new NotImplementedException();
            }

            protected override void EndInvokeDotNet(DotNetInvocationInfo invocationInfo, in DotNetInvocationResult invocationResult)
            {
                throw new NotImplementedException();
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

            JsonNode.DeepEquals(JsonNode.Parse(resultObj), JsonNode.Parse(expectedObject))
                .Should()
                .BeTrue();
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

        [Fact]
        public void Verify_Serialize_V2_Element_Options()
        {
            // arrange
            _options.CanvasOptions = new CanvasElementOptions
            {
                Hidden = true,
                ThemeColor = "#ff0000"
            };
            _options.ImageOptions = new ImageElementOptions
            {
                Hidden = true,
                InitialCenterSize = CropperImageInitialCenterSize.Cover,
                Alt = "Updated v2 alt"
            };
            _options.GridOptions = new GridElementOptions
            {
                Hidden = true,
                Rows = 5,
                Columns = 4,
                Bordered = false,
                Covered = false,
                ThemeColor = "#00ff00"
            };
            _options.CrosshairOptions = new CrosshairElementOptions
            {
                Hidden = true,
                Centered = false,
                ThemeColor = "#0000ff"
            };
            _options.MoveHandleOptions = new HandleElementOptions
            {
                Hidden = true,
                ThemeColor = "#ff00ff"
            };
            _options.ResizeHandleOptions = new ResizeHandleElementOptions
            {
                Hidden = true,
                ThemeColor = "#00ffff"
            };

            // act
            string resultObj = JsonSerializer.Serialize(
                _options,
                _internalJSRuntime.GetJsonSerializerOptions);

            // assert
            resultObj.Should().Contain("\"canvasOptions\":{");
            resultObj.Should().Contain("\"hidden\":true");
            resultObj.Should().Contain("\"themeColor\":\"#ff0000\"");
            resultObj.Should().Contain("\"imageOptions\":{");
            resultObj.Should().Contain("\"initialCenterSize\":\"cover\"");
            resultObj.Should().Contain("\"alt\":\"Updated v2 alt\"");
            resultObj.Should().Contain("\"gridOptions\":{");
            resultObj.Should().Contain("\"rows\":5");
            resultObj.Should().Contain("\"columns\":4");
            resultObj.Should().Contain("\"bordered\":false");
            resultObj.Should().Contain("\"covered\":false");
            resultObj.Should().Contain("\"crosshairOptions\":{");
            resultObj.Should().Contain("\"centered\":false");
            resultObj.Should().Contain("\"moveHandleOptions\":{");
            resultObj.Should().Contain("\"themeColor\":\"#ff00ff\"");
            resultObj.Should().Contain("\"resizeHandleOptions\":{");
            resultObj.Should().Contain("\"themeColor\":\"#00ffff\"");
        }

        public static IEnumerable<object[]> TestData_Setup_Options_Preview()
        {
            yield return WrapArgs(
                null,
                GetExpectedSerializedOptions());

            yield return WrapArgs(
                ".testClass",
                "{\"preview\":\".testClass\"," + GetExpectedSerializedOptions()[1..]);

            yield return WrapArgs(
                new ElementReference("ElementReferenceId"),
                "{\"preview\":{\"id\":\"ElementReferenceId\",\"context\":null}," + GetExpectedSerializedOptions()[1..]);

            yield return WrapArgs(
                new ElementReference("ElementReferenceId", new CustomElementReferenceContext()),
                "{\"preview\":{\"id\":\"ElementReferenceId\",\"context\":{}}," + GetExpectedSerializedOptions()[1..]);

            yield return WrapArgs(
                new ElementReference[]
                {
                    new ElementReference("ElementReferenceId"),
                    new ElementReference("ElementReferenceId", new CustomElementReferenceContext())
                },
                "{\"preview\":[{\"id\":\"ElementReferenceId\",\"context\":null},{\"id\":\"ElementReferenceId\",\"context\":{}}]," + GetExpectedSerializedOptions()[1..]);

            static object[] WrapArgs(
                object? preview,
                string expectedObject)
                    => new object[]
                    {
                        preview!,
                        expectedObject
                    };
        }

        private static string GetExpectedSerializedOptions()
            => "{\"autoCrop\":true,\"autoCropArea\":0.5,\"background\":true,\"center\":true,\"checkCrossOrigin\":true,\"checkOrientation\":true,\"cropBoxMovable\":true,\"cropBoxResizable\":true,\"dragMode\":\"crop\",\"guides\":true,\"highlight\":true,\"minCanvasHeight\":0,\"minCanvasWidth\":0,\"minContainerHeight\":100,\"minContainerWidth\":200,\"minCropBoxHeight\":0,\"minCropBoxWidth\":0,\"modal\":true,\"movable\":true,\"responsive\":true,\"restore\":true,\"rotatable\":true,\"scalable\":true,\"toggleDragModeOnDblclick\":true,\"wheelZoomRatio\":0.1,\"zoomOnTouch\":true,\"zoomOnWheel\":true,\"zoomable\":false,\"handleAction\":\"select\",\"moveHandleAction\":\"move\",\"handlePlain\":false,\"correlationId\":\"Cropper.Blazor\"}";

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
