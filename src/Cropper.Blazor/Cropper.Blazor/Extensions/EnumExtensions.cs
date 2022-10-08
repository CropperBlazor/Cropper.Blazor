using System.Runtime.Serialization;

namespace Cropper.Blazor.Extensions
{
    public static class EnumExtensions
    {
        public static string ToEnumString<T>(this T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).SingleOrDefault();
            return enumMemberAttribute?.Value;
        }
    }
}
