﻿using System.IO;
using System.Linq;

namespace Cropper.Blazor.Client.Compiler;

public class Paths
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

    public string DocsDirPath
    {
        get
        {
            return Directory.EnumerateDirectories(SrcDirPath, DocsDirectory).FirstOrDefault();
        }
    }

    public string DocsStringSnippetsDirPath
    {
        get
        {
            return Path.Join(DocsDirPath, "Models");
        }
    }

    public string SnippetsFilePath
    {
        get
        {
            return Path.Join(DocsStringSnippetsDirPath, SnippetsFile);
        }
    }

    public string DocStringsFilePath
    {
        get
        {
            return Path.Join(DocsStringSnippetsDirPath, DocStringsFile);
        }
    }

    public string NewFilesToBuildPath
    {
        get
        {
            return Path.Join(DocsDirPath, NewFilesToBuild);
        }
    }
}
