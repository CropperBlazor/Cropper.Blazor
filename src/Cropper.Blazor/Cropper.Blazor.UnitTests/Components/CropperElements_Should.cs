using System;
using Bunit;
using Cropper.Blazor.Components;
using Cropper.Blazor.Testing;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Xunit;

#if NET8_0_OR_GREATER
using TestContext = Bunit.BunitContext;
#endif

namespace Cropper.Blazor.UnitTests.Components
{
    public class CropperElements_Should : IDisposable
    {
        private readonly TestContext _testContext;

        public CropperElements_Should()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public void Render_CropperCanvas_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperCanvas>("cropper-canvas");
        }

        [Fact]
        public void Render_CropperImage_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperImage>("cropper-image");
        }

        [Fact]
        public void Render_CropperSelection_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperSelection>("cropper-selection");
        }

        [Fact]
        public void Render_CropperShade_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperShade>("cropper-shade");
        }

        [Fact]
        public void Render_CropperHandle_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperHandle>("cropper-handle");
        }

        [Fact]
        public void Render_CropperGrid_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperGrid>("cropper-grid");
        }

        [Fact]
        public void Render_CropperCrosshair_With_Class_And_Attributes()
        {
            AssertCustomElement<CropperCrosshair>("cropper-crosshair");
        }

        [Fact]
        public void Render_Nested_Cropper_Elements()
        {
            // act
            IRenderedComponent<CropperCanvas> component = _testContext.GetIRenderedComponent<CropperCanvas>(parameters => parameters
                .AddChildContent<CropperSelection>(selectionParameters => selectionParameters
                    .AddChildContent<CropperGrid>()));

            // assert
            component.MarkupMatches("<cropper-canvas><cropper-selection><cropper-grid></cropper-grid></cropper-selection></cropper-canvas>");
        }

        public void Dispose()
        {
            _testContext.DisposeTestContext();
        }

        private void AssertCustomElement<TComponent>(string elementName)
            where TComponent : CropperElementBase, IComponent
        {
            // arrange
            const string expectedClass = "cropper-element";

            // act
            IRenderedComponent<TComponent> component = _testContext.GetIRenderedComponent<TComponent>(parameters => parameters
                .Add(p => p.Class, expectedClass)
                .AddUnmatched("data-testid", elementName)
                .AddUnmatched("movable", true));

            // assert
            component.MarkupMatches($"<{elementName} class=\"{expectedClass}\" data-testid=\"{elementName}\" movable></{elementName}>");
        }
    }
}
