using System.IO;
using System.Linq;

namespace Cropper.Blazor.Client.Compiler;

public static class Paths
{
    private const string DocsDirectory = "Client";
    private const string DocStringsFile = "DocStrings.generated.cs";
    private const string SnippetsFile = "Snippets.generated.cs";
    private const string NewFilesToBuild = "NewFilesToBuild.txt";

    public const string ExampleDiscriminator = "Example"; // example components must contain this string

    public static string SrcDirPath
    {
        get
        {
            var workingPath = Path.GetFullPath(".");
            do
            {
                workingPath = Path.GetDirectoryName(workingPath);
            }
            while (Path.GetFileName(workingPath) != "Cropper.Blazor" && !string.IsNullOrWhiteSpace(workingPath));

            return workingPath;
        }
    }

    public static string DocsDirPath => Directory.EnumerateDirectories(SrcDirPath, DocsDirectory).FirstOrDefault();

    public static string DocsStringSnippetsDirPath => Path.Join(DocsDirPath, "Models");

    public static string SnippetsFilePath => Path.Join(DocsStringSnippetsDirPath, SnippetsFile);

    public static string DocStringsFilePath => Path.Join(DocsStringSnippetsDirPath, DocStringsFile);

    public static string NewFilesToBuildPath => Path.Join(DocsDirPath, NewFilesToBuild);
}
