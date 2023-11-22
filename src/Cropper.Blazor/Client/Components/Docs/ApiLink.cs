using Cropper.Blazor.Components;
using Cropper.Blazor.Models;
using Cropper.Blazor.Shared.Extensions;

namespace Cropper.Blazor.Client.Components.Docs
{
    public static class ApiLink
    {
        private static Dictionary<Type, string> SpecialCaseComponents =
           new()
           {
               [typeof(CropperComponent)] = "cropper-component"
           };

        public static string GetComponentLinkFor(Type type)
        {
            return $"components/{GetComponentName(type)}";
        }

        private static string GetComponentName(Type type)
        {
            if (!SpecialCaseComponents.TryGetValue(type, out var component))
            {
                component = new string(type.ToString().Replace("Cropper.Blazor", "").TakeWhile(c => c != '`').ToArray())
                    .ToLowerInvariant();
            }

            return component;
        }

        public static Type? GetTypeFromComponentLink(string component)
        {
            if (component.Contains('#') == true)
            {
                component = component.Substring(0, component.IndexOf('#'));
            }

            if (string.IsNullOrEmpty(component))
            {
                return null;
            }

            var assembly = typeof(CropperComponent).Assembly;
            var types = assembly.GetTypes();

            foreach (var x in types)
            {
                if (new string(x.Name.ToLowerInvariant().TakeWhile(c => c != '`').ToArray()) == $"{component}".ToLowerInvariant())
                {
                    if (x.Name.Contains('`'))
                    {
                        return x;
                    }
                    else if (x.Name.ToLowerInvariant() == $"{component}".ToLowerInvariant())
                    {
                        return x;
                    }
                }
            }

            return null;
        }

        public static string GetContextType(this Type type)
        {
            string value = string.Empty;

            if (type == typeof(Options))
            {
                value = nameof(Options).CreateLink();
            }
            else if (type == typeof(SetDataOptions))
            {
                value = nameof(SetDataOptions).CreateLink();
            }

            return value;
        }
    }
}
