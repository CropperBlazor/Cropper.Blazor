using ColorCode.Common;
using ColorCode.Styling;

namespace Cropper.Blazor.Client.Compiler
{
    public class StyleDictionaryKebabCase
    {
        /// <summary>
        /// A theme with reference names in kebab-case style.
        /// </summary>
        public static StyleDictionary KebabCase
        {
            get
            {
                return new StyleDictionary
                {
                    new Style(ScopeName.String)
                    {
                        ReferenceName = "string"
                    },
                    new Style(ScopeName.Keyword)
                    {
                        ReferenceName = "keyword"
                    },
                    new Style(ScopeName.HtmlElementName)
                    {
                        ReferenceName = "html-element-name"
                    },
                    new Style(ScopeName.HtmlAttributeName)
                    {
                        ReferenceName = "html-attribute-name"
                    },
                    new Style(ScopeName.HtmlAttributeValue)
                    {
                        ReferenceName = "html-attribute-value"
                    },
                    new Style(ScopeName.HtmlOperator)
                    {
                        ReferenceName = "html-operator"
                    },
                    new Style(ScopeName.Comment)
                    {
                        ReferenceName = "comment"
                    },
                    new Style(ScopeName.HtmlTagDelimiter)
                    {
                        ReferenceName = "html-tag-delimiter"
                    }
                };
            }
        }
    }
}
