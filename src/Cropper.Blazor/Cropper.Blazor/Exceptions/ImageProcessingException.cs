using System;

namespace Cropper.Blazor.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageProcessingException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ImageProcessingException(string message) : base(message)
        {

        }
    }
}
