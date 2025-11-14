using System;
using Bunit;
using Microsoft.AspNetCore.Components;

#if NET8_0_OR_GREATER
using TestContext = Bunit.BunitContext;
#endif

namespace Cropper.Blazor.Testing
{
    public static class BunitContextExtensions
    {
        public static IRenderedComponent<T> GetIRenderedComponent<T>(this TestContext testContext) where T : IComponent
        {
#if NET8_0_OR_GREATER
            return testContext
                .Render<T>();
#else
            return testContext
                .RenderComponent<T>();
#endif
        }

        public static IRenderedComponent<T> GetIRenderedComponent<T>(this TestContext testContext, Action<ComponentParameterCollectionBuilder<T>> actionParameters) where T : IComponent
        {
#if NET8_0_OR_GREATER
            return testContext
                .Render<T>(actionParameters);
#else
            return testContext
                .RenderComponent<T>(actionParameters);
#endif
        }
    }
}
