using System.Reflection;

namespace Cropper.Blazor.Client.Models
{
    // this is needed for the copy-to-clipboard feature
    public static partial class Snippets
    {
        public static string GetCode(string component)
        {
            FieldInfo? field = typeof(Snippets).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField)
                .FirstOrDefault(f => f.Name == component);

            if (field == null)
            {
                return $"Snippet for component '{component}' not found!";
            }

            return (string?)field.GetValue(null) ?? string.Empty;
        }

        public const string InstallScriptManual = @"<script src=""_content/Cropper.Blazor/cropper.min.js""></script>";

        public const string InstallServicesManual = @"
            using Cropper.Blazor.Extensions;

            builder.Services.AddCropper();
            ";

        public const string InstallServicesOverrideGlobal = @"
            using Cropper.Blazor.Extensions;

            builder.Services.AddCropper(new CropperJsInteropOptions()
            {
                DefaultInternalPathToCropperModule = ""{YourPath}/_content/Cropper.Blazor/cropperJsInterop.min.js""
            });
            ";

        public const string InstallServicesOverrideInternal = @"
            using Cropper.Blazor.Extensions;

            builder.Services.AddCropper(new CropperJsInteropOptions()
            {
                IsActiveGlobalPath = true,
                GlobalPathToCropperModule = ""{StartUrlWithPath}/_content/Cropper.Blazor/cropperJsInterop.min.js""
            });
            ";

        public const string MinMaxZoomRatio_Script = @"
            await JSRuntime.InvokeVoidAsync(""cropper.setZoomLimits"", cropperComponentId, minZoomRatio, maxZoomRatio);
            ";
    }
}
