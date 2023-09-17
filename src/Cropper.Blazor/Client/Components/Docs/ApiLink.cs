using Cropper.Blazor.Components;
using Cropper.Blazor.Models;

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

        public static Type GetTypeFromComponentLink(string component)
        {
            if (component.Contains('#') == true)
            {
                component = component.Substring(0, component.IndexOf('#'));
            }

            if (string.IsNullOrEmpty(component))
            {
                throw new ArgumentException(nameof(component));
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

            throw new ArgumentNullException(nameof(component));
        }

        public static string GetContextType(this Type type)
        {
            string value = string.Empty;

            if (type == typeof(Options))
            {
                value = CreateLink(nameof(Options));
            }
            else if (type == typeof(SetDataOptions))
            {
                value = CreateLink(nameof(SetDataOptions));
            }

            return value;
        }

        private static string CreateLink(string name)
        {
            return $"<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"contract/{name}\">{name}</a>";
        }
    }
}
