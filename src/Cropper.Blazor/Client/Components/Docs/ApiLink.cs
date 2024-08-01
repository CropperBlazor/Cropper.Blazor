using System.Reflection;
using Cropper.Blazor.Components;
using Cropper.Blazor.Models;
using Cropper.Blazor.Shared.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cropper.Blazor.Client.Components.Docs
{
    public static class ApiLink
    {
        private static readonly Dictionary<Type, string> _specialCaseComponents =
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
            if (!_specialCaseComponents.TryGetValue(type, out string? component))
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
                component = component[..component.IndexOf('#')];
            }

            if (string.IsNullOrEmpty(component))
            {
                return null;
            }

            Assembly assembly = typeof(CropperComponent).Assembly;
            Type[] types = assembly.GetTypes();

            foreach (Type x in types)
            {
                if (new string(x.Name.TakeWhile(c => c != '`').ToArray()).Equals($"{component}", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (x.Name.Contains('`'))
                    {
                        return x;
                    }
                    else if (x.Name.Equals($"{component}", StringComparison.InvariantCultureIgnoreCase))
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
            else if (type == typeof(CropperComponentType))
            {
                value = nameof(CropperComponentType).CreateLink();
            }
            else if (type == typeof(RenderFragment))
            {
                value = nameof(RenderFragment).CreateLink();
            }
            else if (type == typeof(IJSObjectReference))
            {
                value = nameof(IJSObjectReference).CreateLink();
            }

            return value;
        }
    }
}
