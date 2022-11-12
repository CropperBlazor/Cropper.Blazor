using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Cropper.Blazor.Extensions
{
    public static class EnumExtensions
    {
        public static string ToEnumString<T>(this T type) where T : Enum
        {
            Type enumType = typeof(T);
            string name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
            return enumMemberAttribute?.Value;
        }
    }
}
