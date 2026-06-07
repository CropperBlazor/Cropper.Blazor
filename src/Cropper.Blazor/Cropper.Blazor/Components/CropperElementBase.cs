using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Components
{
    /// <summary>
    /// Provides shared parameters for Cropper.js custom element wrappers.
    /// </summary>
    public abstract class CropperElementBase : ComponentBase
    {
        /// <summary>
        /// User class names, separated by space.
        /// </summary>
        [Parameter]
        public string? Class { get; set; }

        /// <summary>
        /// Gets custom element attributes forwarded to the rendered Cropper.js element.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, object>? InputAttributes { get; set; }

        /// <summary>
        /// Captures all additional attributes passed to the component that do not match declared parameters.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Content rendered inside the Cropper.js custom element.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets additional attributes to render on the Cropper.js custom element.
        /// </summary>
        protected IReadOnlyDictionary<string, object>? Attributes => GetAttributes();

        private IReadOnlyDictionary<string, object>? GetAttributes()
        {
            if ((InputAttributes is null || InputAttributes.Count == 0)
                && (AdditionalAttributes is null || AdditionalAttributes.Count == 0))
            {
                return null;
            }

            if (InputAttributes is null || InputAttributes.Count == 0)
            {
                return AdditionalAttributes;
            }

            if (AdditionalAttributes is null || AdditionalAttributes.Count == 0)
            {
                return InputAttributes;
            }

            Dictionary<string, object> mergedAttributes = AdditionalAttributes
                .ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

            foreach (KeyValuePair<string, object> attribute in InputAttributes)
            {
                mergedAttributes[attribute.Key] = attribute.Value;
            }

            return mergedAttributes;
        }
    }
}
