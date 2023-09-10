using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Cropper.Blazor.Components;
using Cropper.Blazor.Shared.Extensions;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Compiler
{
    public class DocStrings
    {
        private static string[] hiddenMethods = { "ToString", "GetType", "GetHashCode", "Equals", "SetParametersAsync", "ReferenceEquals" };

        public bool Execute()
        {
            var paths = new Paths();
            var success = true;
            try
            {
                var currentCode = string.Empty;
                if (File.Exists(paths.DocStringsFilePath))
                {
                    currentCode = File.ReadAllText(paths.DocStringsFilePath);
                }

                var cb = new CodeBuilder();
                cb.AddHeader();
                cb.AddLine("namespace Cropper.Blazor.Client.Models");
                cb.AddLine("{");
                cb.IndentLevel++;
                cb.AddLine("public static partial class DocStrings");
                cb.AddLine("{");
                cb.IndentLevel++;

                var assembly = typeof(CropperComponent).Assembly;
                foreach (var type in assembly.GetTypes().OrderBy(t => GetSaveTypename(t)))
                {
                    foreach (var property in type.GetPropertyInfosWithAttribute<ParameterAttribute>())
                    {
                        var doc = property.GetDocumentation() ?? "";
                        doc = ConvertSeeTags(doc);
                        doc = Regex.Replace(doc, @"</?.+?>", "");  // remove all other XML tags
                        cb.AddLine($"public const string {GetSaveTypename(type)}_{property.Name} = @\"{EscapeDescription(doc).Trim()}\";\n");
                    }

                    // TableContext was causing conflicts due to the imperfect mapping from the name of class to the name of field in DocStrings
                    if (type.IsSubclassOf(typeof(Attribute)) || GetSaveTypename(type) == "TypeInference"
                            || GetSaveTypename(type).StartsWith("EventUtil_"))
                        continue;

                    foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
                    {
                        if (!hiddenMethods.Any(x => x.Contains(method.Name)) && !method.Name.StartsWith("get_") && !method.Name.StartsWith("set_"))
                        {
                            // omit methods defined in System.Enum
                            if (GetBaseDefinitionClass(method) == typeof(Enum))
                                continue;

                            var doc = method.GetDocumentation() ?? "";
                            doc = ConvertSeeTagsFormethod(doc);
                            doc = NormalizeWord(doc);
                            cb.AddLine($"public const string {GetSaveTypename(type)}_method_{GetSaveMethodIdentifier(method)} = @\"{EscapeDescription(doc)}\";\n");
                        }
                    }
                }

                cb.IndentLevel--;
                cb.AddLine("}");
                cb.IndentLevel--;
                cb.AddLine("}");

                if (currentCode != cb.ToString())
                {
                    File.WriteAllText(paths.DocStringsFilePath, cb.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error generating {paths.DocStringsFilePath} : {e.Message}");
                success = false;
            }

            return success;
        }

        private static string GetSaveTypename(Type t) => Regex.Replace(t.ConvertToCSharpSource(), @"[\.,<>]", "_").TrimEnd('_');

        /* Methods can be overloaded so the method name doesn't identify it uniquely. Instead of method name we need the method signature.
         * Currently the return type of a method is also used, but probably it can be removed.
         *
         * Alternatively we could use the format similar to this used in XML documentation - it will be even better because I think it is
         * less likely to be changed in the future. See XmlDocumentation.cs for a method computing identifiers.
         */
        private static string GetSaveMethodIdentifier(MethodInfo method) => Regex.Replace(method.ToString(), "[^A-Za-z0-9_]", "_");

        private static Type GetBaseDefinitionClass(MethodInfo m) => m.GetBaseDefinition().DeclaringType;

        /* Replace <see cref="TYPE_OR_MEMBER_QUALIFIED_NAME"/> tags by TYPE_OR_MEMBER_QUALIFIED_NAME without "Cropper.Blazor." at the beginning.
         * It is a quick fix. It should be rather represented by <a href="...">...</a> but it is more difficult.
         */
        private static string ConvertSeeTags(string doc)
        {
            return Regex.Replace(doc, "<see cref=\"[TFPME]:(Cropper\\.)?([^>]+)\" */>", match =>
            {
                string result = match.Groups[2].Value;     // get the name of Type or type member (Field, Property, Method, or Event)
                result = Regex.Replace(result, "`1", "");  // remove `1 from generic type name
                return result;
            });
        }

        private static string ConvertSeeTagsFormethod(string doc)
        {
            var result = doc
                .Replace("<br />", "")
                .Replace("<paramref name=\"scaleX\" />", "scaleX")
                .Replace("<see cref=\"T:Microsoft.AspNetCore.Components.Forms.IBrowserFile\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.ibrowserfile\">IBrowserFile</a>")
                .Replace("<see cref=\"T:Microsoft.JSInterop.DotNetStreamReference\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/microsoft.jsinterop.dotnetstreamreference\">DotNetStreamReference</a>")
                .Replace("<see cref=\"T:System.Threading.Tasks.ValueTask\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask\">ValueTask</a>")
                .Replace("<see cref=\"T:System.Threading.Tasks.ValueTask`1\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask\">ValueTask<></a>")
                .Replace("<see cref=\"T:Cropper.Blazor.Events.JSEventData`1\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); pointer-events: none; cursor: default; opacity: .6; \" href=\"#\">JSEventData<></a>")
                .Replace("<see cref=\"T:System.Threading.CancellationToken\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource\">CancellationToken</a>");

            return ConvertSeeTags(result);
        }

        private static string NormalizeWord(string doc)
        {
            //string.Join(" ", doc.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            return Regex.Replace(doc, @"\s+", " ");
        }

        private static string EscapeDescription(string doc)
        {
            return doc.Replace("\"", "\"\"");
        }
    }
}
