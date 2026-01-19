using System.Reflection;
using Cropper.Blazor.Client.Models;
using Cropper.Blazor.Components;
using Cropper.Blazor.Events;
using Cropper.Blazor.Exceptions;
using Cropper.Blazor.Models;
using Cropper.Blazor.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Cropper.Blazor.Client.Components.Docs
{
    public partial class DocsApi
    {
        [Parameter] public Type Type { get; set; }
        [Parameter] public bool IsContract { get; set; } = false;
        [Parameter] public bool? IsComponentContract { get; set; } = null;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        public DocsPage DocsPage { get; set; }

        // used for default value getting
        private object CompInstance;
        private readonly List<string> _hiddenMethods =
        [
            "ToString",
            "GetType",
            "GetHashCode",
            "Equals",
            "SetParametersAsync",
            "ReferenceEquals"
        ];

        protected override async Task OnParametersSetAsync()
        {
            CompInstance = !Type.IsAssignableTo(typeof(IComponent)) ? null : Activator.CreateInstance(Type);

            await base.OnParametersSetAsync();
        }

        private string? GetHrefPage()
        {
            if (Type == typeof(CropperComponent))
            {
                return "examples/cropperusage";
            }
            else if (Type == typeof(CroppedCanvasReceiver))
            {
                return "examples/cropping#crop-a-polygon-image-in-background";
            }
            else if (Type == typeof(ImageReceiver))
            {
                return "examples/cropping#crop-a-round-image-in-background";
            }

            return null;
        }

        private IEnumerable<ApiProperty> GetEventCallbacks()
        {
            if (Type == null)
            {
                yield break;
            }

            string saveTypename = DocStrings.GetSaveTypename(Type);

            if (IsContract)
            {
                yield break;
            }
            else
            {
                IEnumerable<PropertyInfo>? propertyInfos = IsComponentContract == true
                    ? Type.GetPropertyInfos()
                    : Type.GetPropertyInfosWithAttribute<ParameterAttribute>();
                foreach (var info in propertyInfos.OrderBy(x => x.Name))
                {
                    if (IsEventCallback(info))
                    {
                        yield return new ApiProperty
                        {
                            Name = info.Name,
                            PropertyInfo = info,
                            Default = string.Empty,
                            Description = DocStrings.GetMemberDescription(saveTypename, info, IsContract, IsComponentContract),
                            IsTwoWay = CheckIsTwoWayEventCallback(info),
                            Type = info.PropertyType,
                        };
                    }
                }
            }
        }

        private string GetClassDescription()
        {
            if (Type.IsClass)
            {
                string saveTypename = DocStrings.GetSaveTypename(Type);

                return DocStrings.GetClassDescription(saveTypename);
            }
            else if (Type.IsEnum)
            {
                return DocStrings.GetEnumDescription(Type.Name);
            }

            return string.Empty;
        }

        private IEnumerable<ApiMethod> GetMethods()
        {
            if (Type == null)
            {
                yield break;
            }

            string saveTypename = DocStrings.GetSaveTypename(Type);

            if (IsContract)
            {
                yield break;
            }
            else
            {
                foreach (var info in Type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static).OrderBy(x => x.Name))
                {
                    if (!_hiddenMethods.Any(x => x.Contains(info.Name)) && !info.Name.StartsWith("get_") && !info.Name.StartsWith("set_"))
                    {
                        bool hasNoJsInvokableAttribute = info.GetCustomAttributes(typeof(JSInvokableAttribute), true).Length == 0;

                        Attribute? attribute = info
                            .GetCustomAttribute(typeof(ObsoleteAttribute), true);
                        string? warningSignatureMessage = null;

                        if (attribute != null)
                        {
                            ObsoleteAttribute obsoleteAttr = (ObsoleteAttribute)attribute;

                            warningSignatureMessage = obsoleteAttr.Message;
                        }

                        yield return new ApiMethod()
                        {
                            MethodInfo = info,
                            IsJsInvokable = !hasNoJsInvokableAttribute,
                            WarningSignatureMessage = warningSignatureMessage,
                            Return = info.ReturnParameter,
                            Signature = info.GetSignature(),
                            Parameters = info.GetParameters(),
                            Documentation = DocStrings.GetMemberDescription(saveTypename, info, IsContract, IsComponentContract)
                        };
                    }
                }
            }
        }

        private static bool IsEventCallback(PropertyInfo? propertyInfo)
        {
            return (propertyInfo!.PropertyType.Name.Contains("EventCallback") && (propertyInfo!.PropertyType.FullName ?? "").Contains(typeof(EventCallback).Namespace))
                || (propertyInfo!.PropertyType.Name.Contains("Action") && (propertyInfo!.PropertyType.FullName ?? "").Contains(typeof(Action).Namespace))
                || (propertyInfo!.PropertyType.Name.Contains("Func") && (propertyInfo!.PropertyType.FullName ?? "").Contains(typeof(Func<>).Namespace));
        }

        private IEnumerable<ApiProperty> GetProperties()
        {
            if (Type == null)
            {
                yield break;
            }

            string saveTypename = DocStrings.GetSaveTypename(Type);
            IEnumerable<PropertyInfo> types = null!;

            if (IsContract || IsComponentContract == true)
            {
                types = Type
                    .GetPropertyInfos();
            }
            else
            {
                types = Type
                    .GetPropertyInfosWithAttribute<ParameterAttribute>();
            }

            if (Type.IsEnum)
            {
                foreach (var info in Enum.GetValues(Type))
                {
                    string? enumDisplayStatus = Convert.ChangeType(info, Type).ToString();
                    yield return ToApiProperty(Type, enumDisplayStatus, ((int)info).ToString());
                }
            }
            else
            {

                foreach (var info in types.OrderBy(x => x.Name))
                {
                    if (!IsEventCallback(info))
                    {
                        yield return ToApiProperty(info, saveTypename);
                    }
                }
            }
        }

        private ApiProperty ToApiProperty(PropertyInfo info, string saveTypename)
        {
            object defaultValue = GetDefaultValue(info);

            return new ApiProperty
            {
                Name = info.Name,
                PropertyInfo = info,
                Default = defaultValue,
                IsTwoWay = CheckIsTwoWayProperty(info),
                Description = DocStrings.GetMemberDescription(saveTypename, info, IsContract, IsComponentContract),
                Type = info.PropertyType
            };
        }

        private static ApiProperty ToApiProperty(Type type, string? enumDisplayStatus, string value)
        {
            return new ApiProperty
            {
                Name = enumDisplayStatus,
                PropertyInfo = null,
                Default = value,
                Description = DocStrings.GetEnumValueDescription(type.Name, enumDisplayStatus),
                Type = type
            };
        }

        private static string AnalyseMethodDocumentation(string documentation, string occurrence, string parameter = "")
        {
            try
            {
                // Define local variable
                string doublequotes = @"""";

                // Define the start tag and the end tag
                string endTag = $"</{occurrence}>";
                string startTag = $"<{occurrence}{(parameter == string.Empty ? "" : " name=" + doublequotes + parameter + doublequotes)}>";

                // Check if the documentation is valid and contains the start tag
                if (documentation != null && documentation.Contains(startTag))
                {
                    // Remove the beginning of the documentation until the start tag
                    documentation = documentation.Substring(documentation.IndexOf(startTag), documentation.Length - documentation.IndexOf(startTag));

                    // Check if the documentation contains the end tag
                    if (documentation.Contains(endTag))
                    {
                        // Return the extracted information
                        // If the information is not for summary, ' : ' is only added if there is a non-empty information to be returned
                        return ((occurrence != "summary" && documentation.Substring(startTag.Length, documentation.IndexOf(endTag) - startTag.Length).Trim() != "" ? "" : "") +
                            documentation.Substring(startTag.Length, documentation.IndexOf(endTag) - startTag.Length).Trim())
                            .Replace("&gt;", ">")
                            .Replace("&lt;", "<");
                    }
                }
            }
            catch
            {
                // ignored
            }

            return string.Empty;
        }

        private static bool CheckIsTwoWayEventCallback(PropertyInfo propertyInfo) => propertyInfo.Name.EndsWith("Changed");

        private bool CheckIsTwoWayProperty(PropertyInfo propertyInfo)
        {
            PropertyInfo? eventCallbackInfo = Type.GetProperty(propertyInfo.Name + "Changed");

            return eventCallbackInfo != null &&
                eventCallbackInfo.PropertyType.Name.Contains("EventCallback") &&
                eventCallbackInfo.GetCustomAttribute<ParameterAttribute>() != null &&
                eventCallbackInfo.GetCustomAttribute<ObsoleteAttribute>() == null;
        }

        private async Task OnPageChanged(int newPage)
        {
            await DocsPage.ContentNavigation.ScrollToSection(new Uri(NavigationManager.BaseUri + "/api#methods"));
        }

        private object GetDefaultValue(PropertyInfo info)
        {
            if (CompInstance == null)
            {
                var constructors = Type.GetConstructors();

                if (!constructors.Any())
                {
                    return info.GetValue(Activator.CreateInstance(Type), null);
                }

                ParameterInfo[] parameters = constructors.First().GetParameters();

                if (!parameters.Any())
                {
                    if (Type == typeof(JSEventData<>))
                    {
                        return new JSEventData<object>();
                    }
                    else
                    {
                        return info.GetValue(Activator.CreateInstance(Type), null);
                    }
                }

                if (Type == typeof(CroppedCanvas))
                {
                    return info.GetValue(new CroppedCanvas(default));
                }
                else if (Type == typeof(CroppedCanvasReceiver))
                {
                    return info.GetValue(new CroppedCanvasReceiver(default, default));
                }
                else if (Type == typeof(ImageProcessingException))
                {
                    return info.GetValue(new ImageProcessingException(default));
                }
                else
                {
                    throw new InvalidOperationException("Unsupported type");
                }
            }

            return info.GetValue(CompInstance);
        }

        #region Grouping properties

        private enum Grouping { Categories, Inheritance, None }

        private readonly Grouping _propertiesGrouping = Grouping.None;

        private TableGroupDefinition<ApiProperty> PropertiesGroupDefinition => _propertiesGrouping switch
        {
            Grouping.Categories => new() { Selector = (p) => p.PropertyInfo.GetCustomAttribute<CategoryAttribute>()?.Name ?? "Misc" },
            Grouping.Inheritance => new() { Selector = (p) => BaseDefinitionClass(p.PropertyInfo) },
            _ => null
        };

        // -- Grouping properties by inheritance ------------------------------------------------------------------------------------------

        private static Type BaseDefinitionClass(MethodInfo m) => m.GetBaseDefinition().DeclaringType;

        private static Type BaseDefinitionClass(PropertyInfo p) => BaseDefinitionClass(p.GetMethod ?? p.SetMethod);  // used for grouping properties

        private static bool IsOverridden(MethodInfo m) => m.GetBaseDefinition().DeclaringType != m.DeclaringType;

        private static bool IsOverridden(PropertyInfo p) => IsOverridden(p.GetMethod ?? p.SetMethod);                // used for the "overridden" chip

        #endregion
    }
}
