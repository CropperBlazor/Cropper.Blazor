using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace Cropper.Blazor.Shared.Extensions
{
    public static class XmlDocumentationExtension
    {
        public static IEnumerable<PropertyInfo> GetPropertyInfosWithAttribute<AttributeType>(this Type type)
            where AttributeType : Attribute
        {
            foreach (var propertyInfo in GetPropertyInfos(type))
            {
                if (propertyInfo.GetCustomAttributes(typeof(AttributeType), true).Length > 0)
                {
                    yield return propertyInfo;
                }
            }
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfos(this Type type) => type.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic);

        /// <summary>Gets the file path of an assembly.</summary>
        /// <param name="assembly">The assembly to get the file path of.</param>
        /// <returns>The file path of the assembly.</returns>
        public static string GetDirectoryPath(this Assembly assembly)
        {
            var codeBase = "file://" + assembly.Location;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        #region System.Type.ConvertToCSharpSource

        /// <summary>Converts a <see cref="Type"/> into a <see cref="string"/> as it would appear in C# source code.</summary>
        /// <param name="type">The <see cref="Type"/> to convert to a <see cref="string"/>.</param>
        /// <param name="showGenericParameters">If the generic parameters are the generic types, whether they should be shown or not.</param>
        /// <returns>The <see cref="string"/> as the <see cref="Type"/> would appear in C# source code.</returns>
        public static string ConvertToCSharpSource(this Type type, bool showGenericParameters = false)
        {
            var genericParameters = new Queue<Type>();
            foreach (var x in type.GetGenericArguments())
                genericParameters.Enqueue(x);
            return ConvertToCsharpSource(type);

            string ConvertToCsharpSource(Type type)
            {
                _ = type ?? throw new ArgumentNullException(nameof(type));
                var result = type.IsNested
                    ? ConvertToCsharpSource(type.DeclaringType) + "."
                    : ""; //: type.Namespace + ".";
                result += Regex.Replace(type.Name, "`.*", string.Empty);
                if (type.IsGenericType)
                {
                    result += "<";
                    var firstIteration = true;
                    foreach (var generic in type.GetGenericArguments())
                    {
                        if (genericParameters.Count <= 0)
                        {
                            break;
                        }
                        var correctGeneric = genericParameters.Dequeue();
                        result += (firstIteration ? string.Empty : ",") +
                                  (correctGeneric.IsGenericParameter
                                      ? showGenericParameters ? (firstIteration ? string.Empty : " ") + correctGeneric.Name : string.Empty
                                      : (firstIteration ? string.Empty : " ") + correctGeneric.ConvertToCSharpSource());
                        firstIteration = false;
                    }
                    result += ">";
                }
                return result;
            }
        }

        #endregion

        #region XML Code Documentation

        public static HashSet<Assembly> LoadedAssemblies = new();
        public static Dictionary<string, string> LoadedXmlDocumentation = new();

        public static void LoadXmlDocumentation(Assembly assembly)
        {
            string xmlFilePath;

            if (LoadedAssemblies.Contains(assembly))
            {
                return;
            }

            var directoryPath = assembly.GetDirectoryPath();
            if (!string.IsNullOrEmpty(directoryPath))
            {
                xmlFilePath = Path.Combine(directoryPath, assembly.GetName().Name + ".xml");
            }
            else
            {
                xmlFilePath = assembly.GetName().Name + ".xml";
            }

            if (File.Exists(xmlFilePath))
            {
                using var streamReader = new StreamReader(xmlFilePath);
                LoadXmlDocumentation(streamReader);
            }
            // currently marking assembly as loaded even if the XML file was not found
            // may want to adjust in future, but I think this is good for now
            LoadedAssemblies.Add(assembly);
        }

        /// <summary>Loads the XML code documentation into memory so it can be accessed by extension methods on reflection types.</summary>
        /// <param name="xmlDocumentation">The content of the XML code documentation.</param>
        public static void LoadXmlDocumentation(string xmlDocumentation)
        {
            using var stringReader = new StringReader(xmlDocumentation);
            LoadXmlDocumentation(stringReader);
        }

        /// <summary>Loads the XML code documentation into memory so it can be accessed by extension methods on reflection types.</summary>
        /// <param name="textReader">The text reader to process in an XmlReader.</param>
        public static void LoadXmlDocumentation(TextReader textReader)
        {
            using var xmlReader = XmlReader.Create(textReader);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "member")
                {
                    var raw_name = xmlReader["name"];
                    LoadedXmlDocumentation[raw_name] = xmlReader.ReadInnerXml();
                }
            }
        }

        /// <summary>Clears the currently loaded XML documentation.</summary>
        public static void ClearXmlDocumentation()
        {
            LoadedAssemblies.Clear();
            LoadedXmlDocumentation.Clear();
        }

        /// <summary>Gets the XML documentation on a type.</summary>
        /// <param name="type">The type to get the XML documentation of.</param>
        /// <returns>The XML documentation on the type.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this Type type)
        {
            LoadXmlDocumentation(type.Assembly);
            var key = "T:" + XmlDocumentationKeyHelper(type.FullName, null);
            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        /// <summary>Gets the XML documentation on a method.</summary>
        /// <param name="methodInfo">The method to get the XML documentation of.</param>
        /// <returns>The XML documentation on the method.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this MethodInfo methodInfo, bool isProperty = false)
        {
            LoadXmlDocumentation(methodInfo.DeclaringType.Assembly);

            var typeGenericMap = new Dictionary<string, int>();
            var tempTypeGeneric = 0;
            Array.ForEach(methodInfo.DeclaringType.GetGenericArguments(), x => typeGenericMap[x.Name] = tempTypeGeneric++);

            var methodGenericMap = new Dictionary<string, int>();
            var tempMethodGeneric = 0;
            Array.ForEach(methodInfo.GetGenericArguments(), x => methodGenericMap.Add(x.Name, tempMethodGeneric++));

            var parameterInfos = methodInfo.GetParameters();

            var memberTypePrefix = isProperty ? "P:" : "M:";
            var declarationTypeString = GetXmlDocumentationFormattedString(methodInfo.DeclaringType, false, typeGenericMap, methodGenericMap);
            var memberNameString = methodInfo.Name;

            if (memberNameString.StartsWith("get_"))
            {
                memberNameString = memberNameString.Replace("get_", string.Empty);
            }

            var methodGenericArgumentsString =
                methodGenericMap.Count > 0 ?
                "``" + methodGenericMap.Count :
                string.Empty;
            var parametersString =
                parameterInfos.Length > 0 ?
                "(" + string.Join(",", methodInfo.GetParameters().Select(x => GetXmlDocumentationFormattedString(x.ParameterType, true, typeGenericMap, methodGenericMap))).Replace("MudBlazor.Docs.Models.T", "`0") + ")" :
                string.Empty;

            var key =
                memberTypePrefix +
                declarationTypeString +
                "." +
                memberNameString +
                methodGenericArgumentsString +
                parametersString;

            if (methodInfo.Name is "op_Implicit" or "op_Explicit")
            {
                key += "~" + GetXmlDocumentationFormattedString(methodInfo.ReturnType, true, typeGenericMap, methodGenericMap);
            }

            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        /// <summary>Gets the XML documentation on a constructor.</summary>
        /// <param name="constructorInfo">The constructor to get the XML documentation of.</param>
        /// <returns>The XML documentation on the constructor.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this ConstructorInfo constructorInfo)
        {
            LoadXmlDocumentation(constructorInfo.DeclaringType.Assembly);

            var typeGenericMap = new Dictionary<string, int>();
            var tempTypeGeneric = 0;
            Array.ForEach(constructorInfo.DeclaringType.GetGenericArguments(), x => typeGenericMap[x.Name] = tempTypeGeneric++);

            // constructors don't support generic types so this will always be empty
            var methodGenericMap = new Dictionary<string, int>();

            var parameterInfos = constructorInfo.GetParameters();

            var memberTypePrefix = "M:";
            var declarationTypeString = GetXmlDocumentationFormattedString(constructorInfo.DeclaringType, false, typeGenericMap, methodGenericMap);
            var memberNameString = "#ctor";
            var parametersString =
                parameterInfos.Length > 0 ?
                "(" + string.Join(",", constructorInfo.GetParameters().Select(x => GetXmlDocumentationFormattedString(x.ParameterType, true, typeGenericMap, methodGenericMap))) + ")" :
                string.Empty;

            var key =
                memberTypePrefix +
                declarationTypeString +
                "." +
                memberNameString +
                parametersString;

            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        public static string GetXmlDocumentationFormattedString(
            Type type,
            bool isMethodParameter,
            Dictionary<string, int> typeGenericMap,
            Dictionary<string, int> methodGenericMap)
        {
            if (type.IsGenericParameter)
            {
                return methodGenericMap.TryGetValue(type.Name, out var methodIndex)
                    ? "``" + methodIndex
                    : "`" + typeGenericMap[type.Name];
            }
            else if (type.HasElementType)
            {
                var elementTypeString = GetXmlDocumentationFormattedString(
                    type.GetElementType(),
                    isMethodParameter,
                    typeGenericMap,
                    methodGenericMap);

                if (type.IsPointer)
                {
                    return elementTypeString + "*";
                }
                else if (type.IsArray)
                {
                    var rank = type.GetArrayRank();
                    var arrayDimensionsString = rank > 1
                        ? "[" + string.Join(",", Enumerable.Repeat("0:", rank)) + "]"
                        : "[]";
                    return elementTypeString + arrayDimensionsString;
                }
                else if (type.IsByRef)
                {
                    return elementTypeString + "@";
                }
                else
                {
                    // Hopefully this will never hit. At the time of writing
                    // this code, type.HasElementType is only true if the type
                    // is a pointer, array, or by reference.
                    throw new Exception(nameof(GetXmlDocumentationFormattedString) +
                        " encountered an unhandled element type. " +
                        "Please submit this issue to the Towel GitHub repository. " +
                        "https://github.com/ZacharyPatten/Towel/issues/new/choose");
                }
            }
            else
            {
                var prefaceString = type.IsNested
                    ? GetXmlDocumentationFormattedString(
                        type.DeclaringType,
                        isMethodParameter,
                        typeGenericMap,
                        methodGenericMap) + "."
                    : type.Namespace + ".";

                string typeNameString = isMethodParameter
                    ? typeNameString = Regex.Replace(type.Name, @"`\d+", string.Empty)
                    : typeNameString = type.Name;

                var genericArgumentsString = type.IsGenericType && isMethodParameter
                    ? "{" + string.Join(",",
                        type.GetGenericArguments().Select(argument =>
                            GetXmlDocumentationFormattedString(
                                argument,
                                isMethodParameter,
                                typeGenericMap,
                                methodGenericMap))
                        ) + "}"
                    : string.Empty;

                return prefaceString + typeNameString + genericArgumentsString;
            }
        }

        /// <summary>Gets the XML documentation on a property.</summary>
        /// <param name="propertyInfo">The property to get the XML documentation of.</param>
        /// <returns>The XML documentation on the property.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this PropertyInfo propertyInfo)
        {
            LoadXmlDocumentation(propertyInfo.DeclaringType.Assembly);
            var key = "P:" + XmlDocumentationKeyHelper(propertyInfo.DeclaringType.FullName, propertyInfo.Name);
            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        /// <summary>Gets the XML documentation on a enum.</summary>
        /// <param name="fieldInfo">The enum to get the XML documentation of.</param>
        /// <returns>The XML documentation on the enum.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this Enum fieldInfo)
        {
            Type type = fieldInfo.GetType();
            LoadXmlDocumentation(type.Assembly);
            var key = "F:" + XmlDocumentationKeyHelper(type.FullName, fieldInfo.ToString());
            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        /// <summary>Gets the XML documentation on a field.</summary>
        /// <param name="fieldInfo">The field to get the XML documentation of.</param>
        /// <returns>The XML documentation on the field.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this FieldInfo fieldInfo)
        {
            LoadXmlDocumentation(fieldInfo.DeclaringType.Assembly);
            var key = "F:" + XmlDocumentationKeyHelper(fieldInfo.DeclaringType.FullName, fieldInfo.Name);
            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        /// <summary>Gets the XML documentation on an event.</summary>
        /// <param name="eventInfo">The event to get the XML documentation of.</param>
        /// <returns>The XML documentation on the event.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this EventInfo eventInfo)
        {
            LoadXmlDocumentation(eventInfo.DeclaringType.Assembly);
            var key = "E:" + XmlDocumentationKeyHelper(eventInfo.DeclaringType.FullName, eventInfo.Name);
            LoadedXmlDocumentation.TryGetValue(key, out var documentation);
            return documentation;
        }

        public static string XmlDocumentationKeyHelper(string typeFullNameString, string memberNameString)
        {
            var key = Regex.Replace(typeFullNameString, @"\[.*\]", string.Empty).Replace('+', '.');
            if (!(memberNameString is null))
            {
                key += "." + memberNameString;
            }
            return key;
        }

        /// <summary>Gets the XML documentation on a member.</summary>
        /// <param name="memberInfo">The member to get the XML documentation of.</param>
        /// <returns>The XML documentation on the member.</returns>
        /// <remarks>The XML documentation must be loaded into memory for this function to work.</remarks>
        public static string GetDocumentation(this MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo)
            {
                return fieldInfo.GetDocumentation();
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.GetDocumentation();
            }
            else if (memberInfo is EventInfo eventInfo)
            {
                return eventInfo.GetDocumentation();
            }
            else if (memberInfo is ConstructorInfo constructorInfo)
            {
                return constructorInfo.GetDocumentation();
            }
            else if (memberInfo is MethodInfo methodInfo)
            {
                return methodInfo.GetDocumentation();
            }
            else if (memberInfo is Type type) // + TypeInfo
            {
                return type.GetDocumentation();
            }
            else if (memberInfo.MemberType.HasFlag(MemberTypes.Custom))
            {
                // This represents a custom type that is not part of
                // the standard .NET languages as far as I'm aware.
                // This will never be supported so return null.
                return null;
            }
            else
            {
                // Hopefully this will never hit. At the time of writing
                // this code, I am only aware of the following Member types:
                // FieldInfo, PropertyInfo, EventInfo, ConstructorInfo,
                // MethodInfo, and Type.
                throw new Exception(nameof(GetDocumentation) +
                    " encountered an unhandled type [" + memberInfo.GetType().FullName + "]. " +
                    "Please submit this issue to the Towel GitHub repository. " +
                    "https://github.com/ZacharyPatten/Towel/issues/new/choose");
            }
        }

        /// <summary>Gets the XML documentation for a parameter.</summary>
        /// <param name="parameterInfo">The parameter to get the XML documentation for.</param>
        /// <returns>The XML documentation of the parameter.</returns>
        public static string GetDocumentation(this ParameterInfo parameterInfo)
        {
            var memberDocumentation = parameterInfo.Member.GetDocumentation();
            if (!(memberDocumentation is null))
            {
                var regexPattern =
                    Regex.Escape(@"<param name=" + "\"" + parameterInfo.Name + "\"" + @">") +
                    ".*?" +
                    Regex.Escape(@"</param>");

                var match = Regex.Match(memberDocumentation, regexPattern);
                if (match.Success)
                {
                    return match.Value;
                }
            }
            return null;
        }

        #endregion
    }
}
