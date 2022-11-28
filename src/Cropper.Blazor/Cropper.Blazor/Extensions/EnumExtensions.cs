using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace Cropper.Blazor.Extensions
{
    /// <summary>
    /// Extension methods for enumerated types.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Convert a enumeration to a string value.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="type">The 'this' object reference.</param>
        /// <returns>The string representation of the enumerated EnumMember attribute value.</returns>
        public static string? ToEnumString<T>([NotNull] this T type) where T : Enum
        {
            Type enumType = typeof(T);
            string name = Enum.GetName(enumType, type!)!;
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
            return enumMemberAttribute?.Value;
        }
    }
}
