using System.Diagnostics;
using Xunit;

namespace Cropper.Blazor.SuperLinter;

public sealed class SuperLinterTests
{
    [Fact]
    public void SuperLinter_Checkov_PassesLocally()
    {
        RunSuperLinter("CHECKOV", "/github/workspace/.*");
    }

    [Fact]
    public void SuperLinter_CSharp_PassesLocally()
    {
        RunSuperLinter("CSHARP", "/github/workspace/src/Cropper.Blazor/.*");
    }

    [Fact]
    public void SuperLinter_GitHubActions_PassesLocally()
    {
        RunSuperLinter("GITHUB_ACTIONS", "/github/workspace/.github/.*");
    }

    [Fact]
    public void SuperLinter_GitHubActionsZizmor_PassesLocally()
    {
        RunSuperLinter("GITHUB_ACTIONS_ZIZMOR", "/github/workspace/.github/.*");
    }

    [Fact]
    public void SuperLinter_Html_PassesLocally()
    {
        RunSuperLinter("HTML", "/github/workspace/src/Cropper.Blazor/Client/.*|/github/workspace/src/Cropper.Blazor/Client.V1/.*");
    }

    [Fact]
    public void SuperLinter_Css_PassesLocally()
    {
        RunSuperLinter("CSS", "/github/workspace/src/Cropper.Blazor/Client/.*|/github/workspace/src/Cropper.Blazor/Cropper.Blazor/.*");
    }

    [Fact]
    public void SuperLinter_DotNetSolutionFormatWhitespace_PassesLocally()
    {
        RunSuperLinter("DOTNET_SLN_FORMAT_WHITESPACE", "/github/workspace/src/Cropper.Blazor/.*");
    }

    [Fact]
    public void SuperLinter_EditorConfig_PassesLocally()
    {
        RunSuperLinter("EDITORCONFIG", "/github/workspace/.*");
    }

    [Fact]
    public void SuperLinter_GitMergeConflictMarkers_PassesLocally()
    {
        RunSuperLinter("GIT_MERGE_CONFLICT_MARKERS", "/github/workspace/.*");
    }

    [Fact]
    public void SuperLinter_Gitleaks_PassesLocally()
    {
        RunSuperLinter("GITLEAKS", "/github/workspace/.*");
    }

    [Fact]
    public void SuperLinter_Jscpd_PassesLocally()
    {
        RunSuperLinter("JSCPD", "/github/workspace/src/Cropper.Blazor/Cropper.Blazor/.*");
    }

    [Fact]
    public void SuperLinter_JavaScriptEs_PassesLocally()
    {
        RunSuperLinter("JAVASCRIPT_ES", "/github/workspace/src/Cropper.Blazor/Cropper.Blazor/.*");
    }

    [Fact]
    public void SuperLinter_Json_PassesLocally()
    {
        RunSuperLinter("JSON", "/github/workspace/.*[.]json$");
    }

    [Fact]
    public void SuperLinter_PreCommit_PassesLocally()
    {
        RunSuperLinter("PRE_COMMIT", "/github/workspace/.*", validateAllCodebase: true);
    }

    [Fact]
    public void SuperLinter_Yaml_PassesLocally()
    {
        RunSuperLinter("YAML", "/github/workspace/.github/.*");
    }

    [Fact]
    public void SuperLinter_DotNetFormatStyle_PassesLocally()
    {
        RunSuperLinter("DOTNET_SLN_FORMAT_STYLE", "/github/workspace/src/Cropper.Blazor/.*");
    }

    [Fact]
    public void SuperLinter_DotNetFormatAnalyzers_PassesLocally()
    {
        RunSuperLinter("DOTNET_SLN_FORMAT_ANALYZERS", "/github/workspace/src/Cropper.Blazor/.*");
    }

    private static void RunSuperLinter(string linterName, string includeRegex, bool validateAllCodebase = false)
    {
        string solutionRoot = FindSolutionRoot();
        string gitRepositoryRoot = FindGitRepositoryRoot(solutionRoot);
        string dockerArguments = string.Join(' ',
        [
            "run --rm",
            "-e RUN_LOCAL=true",
            "-e GITHUB_WORKSPACE=/github/workspace",
            "-e GITHUB_TOKEN=dummy",
            "-e DEFAULT_BRANCH=dev",
            "-e FROM_REF=HEAD~1",
            "-e TO_REF=HEAD",
            $"-e OUTPUT_FOLDER=Reports/{linterName}",
            "-e OUTPUT_DETAILS=detailed",
            "-e LOG_LEVEL=WARN",
            $"-e VALIDATE_ALL_CODEBASE={validateAllCodebase.ToString().ToLowerInvariant()}",
            $"-e VALIDATE_{linterName}=true",
            "-e FILTER_REGEX_EXCLUDE=\"(\\W|^)(obj/|bin/|node_modules/|webpack\\.config\\.js)|(\\W|^).*([.]min[.]css)($)|(\\W|^).*([.]min[.]js)($)\"",
            $"-e FILTER_REGEX_INCLUDE=\"{includeRegex}\"",
            "-e LINTER_RULES_PATH=.github/linters",
            "-e JSCPD_CONFIG_FILE=\".jscpd.json\"",
            "-e HTML_FILE_NAME=\".htmlhintrc\"",
            "-e CSS_FILE_NAME=\".stylelintrc.json\"",
            "-e GITHUB_ACTIONS_ZIZMOR_CONFIG_FILE=\".zizmor.yaml\"",
            "-e YAML_CONFIG_FILE=\".yaml-lint.yml\"",
            $"-v \"{gitRepositoryRoot}:/github/workspace\"",
            "ghcr.io/super-linter/super-linter:v8.2.1"
        ]);

        CommandResult dockerVersion = Run("docker", "--version", solutionRoot);
        Assert.True(dockerVersion.ExitCode == 0, $"Docker is required for the Super-Linter test. Output:{Environment.NewLine}{dockerVersion.Output}");

        CommandResult result = Run("docker", dockerArguments, solutionRoot);
        Assert.True(result.ExitCode == 0, $"Super-Linter {linterName} failed or Docker Desktop is not running. Command: docker {dockerArguments}{Environment.NewLine}{result.Output}");
    }

    private static CommandResult Run(string fileName, string arguments, string workingDirectory)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = fileName,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException($"Could not start {fileName}.");
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return new CommandResult(process.ExitCode, output + error);
    }

    private static string FindSolutionRoot()
    {
        DirectoryInfo directory = new(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Cropper.Blazor.sln")))
            {
                return directory.FullName;
            }

            directory = directory.Parent!;
        }

        throw new DirectoryNotFoundException("Could not find Cropper.Blazor.sln from the test output directory.");
    }

    private static string FindGitRepositoryRoot(string startDirectory)
    {
        DirectoryInfo directory = new(startDirectory);

        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, ".git")))
            {
                return directory.FullName;
            }

            directory = directory.Parent!;
        }

        throw new DirectoryNotFoundException("Could not find .git from the solution directory.");
    }

    private sealed record CommandResult(int ExitCode, string Output);
}
