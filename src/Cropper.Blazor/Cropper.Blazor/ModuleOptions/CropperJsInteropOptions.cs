namespace Cropper.Blazor.ModuleOptions
{
    /// <summary>
    /// Contains cropper JavaScript interop options.
    /// </summary>
    public class CropperJsInteropOptions : ICropperJsInteropOptions
    {
        /// <summary>
        /// Represents an internal (default) path to cropper js interop module.
        /// </summary>
        public string DefaultInternalPathToCropperModule { get; set; } = "_content/Cropper.Blazor/cropperJsInterop.min.js";

        /// <summary>
        /// Represents state regarding using global path to cropper js interop module instead of internal (default).
        /// </summary>
        public bool IsActiveGlobalPath { get; set; } = false;

        /// <summary>
        /// Represents a global (conclusive) path to cropper js interop module.
        /// </summary>
        public string GlobalPathToCropperModule { get; set; } = string.Empty;
    }
}
