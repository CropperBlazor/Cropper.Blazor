using System.Reflection;

namespace Cropper.Blazor.UnitTests
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Uses reflection to get the field value from an object.
        /// </summary>
        ///
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        ///
        /// <returns>The field value from the object.</returns>
        internal static object GetInstanceField(this object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}
