using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Cropper.Blazor.UnitTests")]

namespace Cropper.Blazor.Extensions
{
    static internal class UriExtensions
    {
        static internal string GetHostName(this Uri baseUri)
        {
            string redundantPath = baseUri.PathAndQuery;

            return baseUri.ToString().Replace(redundantPath, string.Empty);
        }
    }
}
