using System;

namespace Cropper.Blazor.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during image processing.
    /// </summary>
    public class ImageProcessingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the exception with a specified error message.
        /// </summary>
        /// <param name="message">The error message that describes the error.</param>
        public ImageProcessingException(string message) : base(message)
        {

        }
    }
}
