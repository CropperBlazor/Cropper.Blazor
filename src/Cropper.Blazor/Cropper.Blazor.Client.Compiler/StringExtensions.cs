using System.Text.RegularExpressions;

namespace Cropper.Blazor.Client.Compiler
{
    public static class StringExtensions
    {

        public static string ToLfLineEndings(this string self)
        {
            if (self == null)
                return null;
            return Regex.Replace(self, @"\r?\n", "\n");
        }
    }
}
