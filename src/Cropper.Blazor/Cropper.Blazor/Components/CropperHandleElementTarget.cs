namespace Cropper.Blazor.Components
{
    /// <summary>
    /// Selects which Cropper.js <c>cropper-handle</c> element is configured by <see cref="CropperHandleElement" />.
    /// </summary>
    public enum CropperHandleElementTarget
    {
        /// <summary>
        /// Configure the plain canvas handle, usually used for creating selections.
        /// </summary>
        Plain,

        /// <summary>
        /// Configure the selection move handle.
        /// </summary>
        Move
    }
}
