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
    public partial class DocStrings
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

                Assembly assembly = typeof(CropperComponent).Assembly;
                IOrderedEnumerable<Type> types = assembly.GetTypes().OrderBy(t => GetSaveTypename(t));

                foreach (var type in types)
                {
                    foreach (var property in type.GetPropertyInfosWithAttribute<ParameterAttribute>())
                    {
                        string doc = property.GetDocumentation() ?? "";
                        doc = NormalizeWord(doc);
                        doc = ConvertCrefToHTML(doc);
                        doc = ConvertMarkdownToHTML(doc);

                        //doc = Regex.Replace(doc, @"</?.+?>", "");  // remove all other XML tags
                        cb.AddLine($"public const string {GetSaveTypename(type)}_{property.Name} = @\"{EscapeDescription(doc).Trim()}\";\n");
                    }

                    // TableContext was causing conflicts due to the imperfect mapping from the name of class to the name of field in DocStrings
                    if (type.IsSubclassOf(typeof(Attribute)) || GetSaveTypename(type) == "TypeInference"
                            || GetSaveTypename(type).StartsWith("EventUtil_"))
                        continue;

                    foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
                    {
                        if (!hiddenMethods.Any(x => x.Contains(method.Name)) && !method.Name.StartsWith("set_"))
                        {
                            // omit methods defined in System.Enum
                            if (GetBaseDefinitionClass(method) == typeof(Enum))
                                continue;

                            bool isProperty = method.Name.StartsWith("get_");

                            string doc = method.GetDocumentation(isProperty) ?? "";
                            string formattedReturnSignature = method.GetFormattedReturnSignature();

                            doc = ConvertSeeTagsForMethod(doc, formattedReturnSignature);
                            doc = NormalizeWord(doc);
                            doc = ConvertCrefToHTML(doc);
                            doc = ConvertMarkdownToHTML(doc);

                            if (isProperty)
                            {
                                cb.AddLine($"public const string {GetSaveTypename(type)}_property_{method.Name.Replace("get_", string.Empty)} = @\"{EscapeDescription(doc).Trim()}\";\n");
                            }
                            else
                            {
                                cb.AddLine($"public const string {GetSaveTypename(type)}_method_{GetSaveMethodIdentifier(method)} = @\"{EscapeDescription(doc)}\";\n");
                            }
                        }
                    }

                    if (type.IsEnum)
                    {
                        string[] enumNames = type.GetEnumNames();

                        foreach (string enumName in enumNames)
                        {
                            Enum enumValue = (Enum)Enum.Parse(type, enumName);
                            string doc = enumValue.GetDocumentation();
                            doc = NormalizeWord(doc);
                            doc = ConvertCrefToHTML(doc);
                            doc = ConvertMarkdownToHTML(doc);

                            string description = EscapeDescription(doc);

                            cb.AddLine($"public const string {GetSaveTypename(type)}_enum_{enumName} = @\"{description}\";\n");
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

        private static string ConvertMarkdownToHTML(string markdownText)
        {
            // Define a regular expression pattern to match Markdown elements with URLs and text
            string pattern = "<(\\w+) href=\"(.*?)\">(.*?)</\\1>";

            // Replace Markdown elements with HTML links
            string htmlText = Regex.Replace(markdownText, pattern, "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"$2\">$3</a>");

            return htmlText;
        }

        private static string ConvertCrefToHTML(string markdownText)
        {
            // Define a regular expression pattern to match Markdown elements with URLs and text
            string pattern = $"<(\\w+) cref=\"([^\"]+)\" />";

            // Replace Markdown elements with HTML links
            string htmlText = Regex.Replace(markdownText, pattern, match =>
            {
                string result = match.Groups[2].Value;
                string value = result.RemoveNamespace();

                if (result.EndsWith("Microsoft.AspNetCore.Components.Web.ErrorEventArgs"))
                {
                    return $"<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.web.erroreventargs\">{value}</a>";
                }

                return $"<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"contract/{value}\">{value}</a>";
            });

            return htmlText;
        }

        private static string ConvertSeeTagsForMethod(string doc, string formattedReturnSignature)
        {
            string result = doc
                .Replace("<br />", "")
                .Replace("<paramref name=\"scaleX\" />", "scaleX")
                .Replace("<see cref=\"T:Microsoft.AspNetCore.Components.ElementReference\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.elementreference\">ElementReference</a>")
                .Replace("<see cref=\"T:Microsoft.AspNetCore.Components.Forms.IBrowserFile\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.forms.ibrowserfile\">IBrowserFile</a>")
                .Replace("<see cref=\"T:Microsoft.JSInterop.DotNetStreamReference\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/microsoft.jsinterop.dotnetstreamreference\">DotNetStreamReference</a>")
                .Replace("<see cref=\"T:System.Threading.Tasks.ValueTask\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask\">ValueTask</a>")
                .Replace("<see cref=\"T:System.Threading.Tasks.ValueTask`1\" />", $"<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.valuetask\">{formattedReturnSignature}</a>")
                .Replace("<see cref=\"T:Cropper.Blazor.Events.JSEventData`1\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"contract/JSEventData\">JSEventData<></a>")
                .Replace("<see cref=\"T:System.Threading.CancellationToken\" />", "<a target=\"_blank\" style=\"color: var(--mud-palette-primary); \" href=\"https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource\">CancellationToken</a>");

            return result;
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

        [GeneratedRegex("<see cref=\"[TFPME]:(Cropper\\.)?([^>]+)\" */>")]
        private static partial Regex ConvertSeeTagsRegex();
    }
}
