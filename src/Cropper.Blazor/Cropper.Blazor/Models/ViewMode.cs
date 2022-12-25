namespace Cropper.Blazor.Models
{
    /// <summary>
    /// Enumeration of view modes
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// No restrictions
        /// </summary>
        Vm0,

        /// <summary>
        /// Restrict the crop box not to exceed the size of the canvas
        /// </summary>
        Vm1,

        /// <summary>
        /// Restrict the minimum canvas size to fit within the container. 
        /// If the proportions of the canvas and the container differ, 
        /// the minimum canvas will be surrounded by extra space in one of the dimensions
        /// </summary>
        Vm2,

        /// <summary>
        /// Restrict the minimum canvas size to fill fit the container. 
        /// If the proportions of the canvas and the container are different, 
        /// the container will not be able to fit the whole canvas in one of the dimensions
        /// </summary>
        Vm3
    }
}
