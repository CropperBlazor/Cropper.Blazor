using Cropper.Blazor.Components;

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
    }
}
