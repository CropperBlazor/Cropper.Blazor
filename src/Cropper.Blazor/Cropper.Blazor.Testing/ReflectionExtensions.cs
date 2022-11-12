using System;
using System.Reflection;

namespace Cropper.Blazor.Testing
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Uses reflection to get the field value from an object.
        /// </summary>
        ///
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        ///
        /// <returns>The field value from the object.</returns>
        public static object GetInstanceField(this object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;

            Type type = instance.GetType();
            FieldInfo? field = type.GetField(fieldName, bindFlags);

            return field!.GetValue(instance)!;
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this MethodInfo methodInfo)
            where TAttribute : Attribute
        {
            ArgumentNullException.ThrowIfNull(methodInfo);

            object? attribute = methodInfo.GetCustomAttribute(typeof(TAttribute), false);

            ArgumentNullException.ThrowIfNull(attribute);

            return (TAttribute)attribute;
        }
    }
}
