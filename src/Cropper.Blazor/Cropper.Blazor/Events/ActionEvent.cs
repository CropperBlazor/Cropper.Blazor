namespace Cropper.Blazor.Events
{
    /// <summary>
    /// Provides the metadata of a Action Event.
    /// </summary>
    public enum ActionEvent
    {
        /// <summary>
        /// Create a new crop box.
        /// </summary>
        Crop,

        /// <summary>
        /// Move the canvas (image wrapper).
        /// </summary>
        Move,

        /// <summary>
        /// Zoom in / out the canvas (image wrapper) by touch.
        /// </summary>
        Zoom,

        /// <summary>
        /// Resize the east side of the crop box.
        /// </summary>
        E,

        /// <summary>
        /// Resize the south side of the crop box.
        /// </summary>
        S,

        /// <summary>
        /// Resize the west side of the crop box.
        /// </summary>
        W,

        /// <summary>
        /// Resize the north side of the crop box.
        /// </summary>
        N,

        /// <summary>
        /// Resize the northeast side of the crop box.
        /// </summary>
        Ne,

        /// <summary>
        /// Resize the northwest side of the crop box.
        /// </summary>
        Nw,

        /// <summary>
        /// Resize the southeast side of the crop box.
        /// </summary>
        Se,

        /// <summary>
        /// Resize the southwest side of the crop box.
        /// </summary>
        Sw,

        /// <summary>
        /// Move the crop box (all directions).
        /// </summary>
        All,

        /// <summary>
        /// None.
        /// </summary>
        None
    }
}
