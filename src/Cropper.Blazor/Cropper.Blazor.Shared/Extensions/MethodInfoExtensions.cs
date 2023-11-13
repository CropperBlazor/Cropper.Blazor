using System.Globalization;
using System.Reflection;
using System.Text;
using MudBlazor;

namespace Cropper.Blazor.Shared.Extensions
{
    // Adaptation from : https://stackoverflow.com/questions/1312166/print-full-signature-of-a-method-from-a-methodinfo/1312321
    public static partial class MethodInfoExtensions
    {
        /// <summary>
        /// Return the method signature as a string.
        /// </summary>
        /// <param name="method">The Method</param>
        /// <param name="callable">Return as an callable string(public void a(string b) would return a(b))</param>
        /// <returns>Method signature</returns>
        public static string GetSignature(this MethodInfo method, bool callable = false)
        {
            // Define local variables
            var firstParameter = true;
            var secondParameter = false;
            var stringBuilder = new StringBuilder();

            // Define the method access
            if (callable == false)
            {
                // Append return type
                stringBuilder.Append(method.ReturnType.TypeName().RemoveNamespace());
                stringBuilder.Append(' ');
            }

            // Add the name of the method
            stringBuilder.Append(method.Name);

            // Add generics method
            if (method.IsGenericMethod)
            {
                stringBuilder.Append('<');

                foreach (var genericArgument in method.GetGenericArguments())
                {
                    if (firstParameter)
                    {
                        firstParameter = false;
                    }
                    else
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.Append(genericArgument.TypeName());
                }

                stringBuilder.Append('>');
            }

            stringBuilder.Append('(');
            firstParameter = true;

            foreach (var parameter in method.GetParameters())
            {
                if (firstParameter)
                {
                    firstParameter = false;

                    if (method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                    {
                        if (callable)
                        {
                            secondParameter = true;
                            continue;
                        }
                        stringBuilder.Append("this ");
                    }
                }
                else if (secondParameter == true)
                {
                    secondParameter = false;
                }
                else
                {
                    stringBuilder.Append(", ");
                }

                if (parameter.ParameterType.IsByRef)
                {
                    stringBuilder.Append("ref ");
                }
                else if (parameter.IsOut)
                {
                    stringBuilder.Append("out ");
                }

                if (!callable)
                {
                    stringBuilder.Append(parameter.ParameterType.TypeName());
                    stringBuilder.Append(' ');
                }

                stringBuilder.Append(parameter.Name);

                if (parameter.DefaultValue is not DBNull)
                {
                    bool isStruct = parameter.ParameterType.IsValueType && !parameter.ParameterType.IsPrimitive;

                    if (isStruct)
                    {
                        stringBuilder.Append($" = default");
                    }
                    else
                    {
                        stringBuilder.Append($" = {PresentDefaultValue(parameter.DefaultValue)}");
                    }
                }
            }

            stringBuilder.Append(')');

            // Return final result
            return stringBuilder.ToString();
        }

        private static DefaultConverter<object> _converter = new DefaultConverter<object>()
        {
            Culture = CultureInfo.InvariantCulture
        };

        public static string PresentDefaultValue(this object value)
        {
            if (value is null)
            {
                return "null";
            }

            Type type = value.GetType();

            if (type == typeof(string))
            {
                if (value.ToString() == string.Empty)
                {
                    return "";
                }
                else
                {
                    return $"\"{value}\"";
                }
            }

            if (type.IsEnum)
            {
                return $"{type.Name}.{value}";
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                return _converter.Set(value);
            }

            if (type.IsGenericType) // for instance event callbacks
            {
                return "";
            }

            if (type.IsValueType)
            {
                return _converter.Set(value);
            }

            if (type.IsClass)
            {
                return type.Name;
            }

            return "";
        }

        public static string GetAliases(string value, Type type = null)
        {
            switch (value.ToUpperInvariant())
            {
                case "STRING": return "string";
                case "INT16": return "short";
                case "INT32": return "int";
                case "INT64": return "long";
                case "INTPTR": return "nint";
                case "UINT16": return "ushort";
                case "UINT32": return "uint";
                case "UINT64": return "ulong";
                case "UINTPTR": return "nuint";
                case "DOUBLE": return "double";
                case "DECIMAL": return "decimal";
                case "OBJECT": return "object";
                case "VOID": return "void";
                case "BOOLEAN": return "bool";
                case "SBYTE": return "sbyte";
                case "CHAR": return "char";
                case "FLOAT": return "float";
                default:
                    {
                        if (type != null)
                        {
                            return string.IsNullOrWhiteSpace(type.FullName) ? type.Name.RemoveNamespace() : type.FullName.RemoveNamespace();
                        }
                        else
                        {
                            return value.RemoveNamespace();
                        }
                    }
            }
        }

        /// <summary>
        /// Get full type name with full namespace names
        /// </summary>
        /// <param name="type">Type. May be generic or nullable</param>
        /// <returns>Full type name, fully qualified namespaces</returns>
        public static string TypeName(this Type type, Func<string, string>? GenericArgumentFormatter = null)
        {
            var first = true;
            var nullableType = Nullable.GetUnderlyingType(type);

            if (nullableType != null)
            {
                return (nullableType.Name + "?").RemoveNamespace();
            }

            if (!(type.IsGenericType && type.Name.Contains('`')))
            {
                return GetAliases(type.Name.ToUpperInvariant(), type);
            }

            var stringBuilder = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));

            if (GenericArgumentFormatter is not null)
            {
                stringBuilder.Append("<a target=\"_blank\"><<a/ >");
            }
            else
            {
                stringBuilder.Append('<');
            }

            foreach (var t in type.GetGenericArguments())
            {
                if (!first)
                {
                    stringBuilder.Append(',');
                }

                string typeName = t.TypeName();

                if (GenericArgumentFormatter is not null)
                {
                    typeName = GenericArgumentFormatter(typeName);
                }

                stringBuilder.Append(typeName);

                first = false;
            }

            stringBuilder.Append('>');

            // Return result
            return stringBuilder.ToString().RemoveNamespace();
        }

        public static string RemoveNamespace(this string value)
        {
            return value.Split('.')[value.Split('.').Length - 1];
        }

        public static string GetFormattedReturnSignature(this MethodInfo method, bool callable = false)
        {
            // Return final result
            return method.ReturnType.GetFormattedReturnSignature(callable);
        }

        public static string GetFormattedReturnSignature(this Type type, bool callable = false)
        {
            // Define local variables
            var stringBuilder = new StringBuilder();

            // Define the method access
            if (callable == false)
            {
                // Append return type
                stringBuilder.Append(type.TypeName(CreateLink).RemoveNamespace());
                stringBuilder.Append(' ');
            }

            // Return final result
            return stringBuilder.ToString();
        }

        public static string CreateLink(this string name)
        {
            if (name == "string")
            {
                return $"<a target=\"_blank\">{name}</a>";
            }
            else if (name == "Action")
            {
                return $"<a target=\"_blank\" rel=\"noopener\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.action-1\">{name}</a>";
            }
            else if (name == "ErrorEventArgs")
            {
                return $"<a target=\"_blank\">{name}</a>";
            }
            else
            {
                // Split the input string by angle brackets '<' and '>'
                string[] parts = name.Split('<', '>');

                // Get the first word inside the angle brackets
                if (parts.Length > 1)
                {
                    string? firstWord = parts[1].Trim();

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(CreateLink(parts.First()));
                    stringBuilder.Append("<");
                    stringBuilder.Append(CreateLink(firstWord));
                    stringBuilder.Append(">");

                    return stringBuilder.ToString();
                }
            }

            return $"<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"contract/{name}\">{name}</a>";
        }
    }
}
