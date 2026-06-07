using System.Diagnostics;
using System.Globalization;
using System.Xml.Linq;
using Xunit;

namespace Cropper.Blazor.Codecov;

public sealed class CodecovVerificationTests
{
    private static readonly string[] DotNetCoverageFiles =
    [
        "Cropper.Blazor.UnitTests/coverage.net6.0.cobertura.xml",
        "Cropper.Blazor.UnitTests/coverage.net7.0.cobertura.xml",
        "Cropper.Blazor.UnitTests/coverage.net8.0.cobertura.xml",
        "Cropper.Blazor.UnitTests/coverage.net9.0.cobertura.xml",
        "Cropper.Blazor.UnitTests/coverage.net10.0.cobertura.xml",
    ];

    private static readonly string[] TypeScriptUnitCoverageRequiredFiles =
    [
        "Cropper/cropperCanvasInteropCommands.ts",
        "Cropper/cropperDataInteropCommands.ts",
        "Cropper/cropperImageInteropCommands.ts",
        "Cropper/cropperSelectionInteropCommands.ts",
        "Cropper/cropperViewerInteropCommands.ts",
        "Cropper/helpers/blob-helper.ts",
        "Cropper/helpers/cropper-url-image-helper.ts",
    ];

    [Fact]
    public void CoverageFiles_RequiredByCodecovPipeline_Exist()
    {
        string repositoryRoot = FindRepositoryRoot();
        List<string> expectedFiles = [.. DotNetCoverageFiles];

        if (IsEnabled("CODECOV_RUN_JS_TESTS"))
        {
            expectedFiles.Add("Cropper.Blazor/coverage/cobertura-coverage.xml");
        }

        List<string> missingFiles = [];

        foreach (string expectedFile in expectedFiles)
        {
            string fullPath = Path.Combine(repositoryRoot, expectedFile.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(fullPath))
            {
                missingFiles.Add(expectedFile);
            }
        }

        Assert.True(
            missingFiles.Count == 0,
            $"Coverage file(s) missing:{Environment.NewLine}{string.Join(Environment.NewLine, missingFiles.Select(file => $"- {file}"))}{Environment.NewLine}{Environment.NewLine}Generate .NET coverage with: dotnet test --no-build --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:ExcludeByAttribute='ExcludeFromCodeCoverage' /p:SkipAutoProps=true /p:Exclude=\"[*]Cropper.Blazor.Testing.*\" from Cropper.Blazor.UnitTests. Generate TypeScript coverage with npm run coverage from Cropper.Blazor.Tests when CODECOV_RUN_JS_TESTS=true.");
    }

    [Fact]
    public void DotNetCoverageFiles_AreValidCoberturaReports()
    {
        string repositoryRoot = FindRepositoryRoot();
        List<string> invalidFiles = [];

        foreach (string coverageFile in DotNetCoverageFiles)
        {
            string fullPath = Path.Combine(repositoryRoot, coverageFile.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(fullPath))
            {
                invalidFiles.Add($"- {coverageFile}: file is missing");

                continue;
            }

            try
            {
                XDocument document = XDocument.Load(fullPath);
                XElement coverage = document.Root ?? throw new InvalidOperationException("The Cobertura coverage file has no root element.");
                decimal lineRate = GetRequiredRate(coverage, "line-rate");
                decimal branchRate = GetRequiredRate(coverage, "branch-rate");
                int linesValid = GetRequiredCount(coverage, "lines-valid");
                int branchesValid = GetRequiredCount(coverage, "branches-valid");

                if (lineRate < 0m || lineRate > 1m || branchRate < 0m || branchRate > 1m || linesValid <= 0 || branchesValid <= 0)
                {
                    invalidFiles.Add($"- {coverageFile}: invalid rates/counts: lines {lineRate:P2} ({linesValid}), branches {branchRate:P2} ({branchesValid})");
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException or FormatException or IOException or System.Xml.XmlException)
            {
                invalidFiles.Add($"- {coverageFile}: {exception.Message}");
            }
        }

        Assert.True(invalidFiles.Count == 0, $".NET coverage file(s) are invalid:{Environment.NewLine}{string.Join(Environment.NewLine, invalidFiles)}");
    }

    [Fact]
    public void TypeScriptCoverage_IsComplete_WhenEnabled()
    {
        if (!IsEnabled("CODECOV_RUN_JS_TESTS"))
        {
            return;
        }

        string repositoryRoot = FindRepositoryRoot();
        string typeScriptCoverage = Path.Combine(repositoryRoot, "Cropper.Blazor", "coverage", "cobertura-coverage.xml");

        Assert.True(File.Exists(typeScriptCoverage), $"TypeScript coverage file is missing: {typeScriptCoverage}");

        XDocument document = XDocument.Load(typeScriptCoverage);
        XElement coverage = document.Root ?? throw new InvalidOperationException("The TypeScript Cobertura coverage file has no root element.");
        int linesValid = GetRequiredCount(coverage, "lines-valid");
        int branchesValid = GetRequiredCount(coverage, "branches-valid");

        Assert.True(linesValid > 0, "TypeScript coverage report must include at least one valid line.");
        Assert.True(branchesValid > 0, "TypeScript coverage report must include at least one valid branch.");

        List<string> failedFiles = [];

        foreach (string file in TypeScriptUnitCoverageRequiredFiles)
        {
            XElement classElement = coverage.Descendants("class")
                .FirstOrDefault(element => IsCoverageFile(element, file))
                ?? throw new InvalidOperationException($"The TypeScript coverage report is missing '{file}'.");
            decimal lineRate = GetRequiredRate(classElement, "line-rate");
            decimal branchRate = GetRequiredRate(classElement, "branch-rate");

            if (lineRate != 1m || branchRate != 1m)
            {
                failedFiles.Add($"- {file}: lines {lineRate:P2}, branches {branchRate:P2}");
            }
        }

        Assert.True(failedFiles.Count == 0, $"Unit-coverable TypeScript files must be 100% covered:{Environment.NewLine}{string.Join(Environment.NewLine, failedFiles)}");
    }

    [Fact]
    public void TypeScriptCoverage_IncludesAllRuntimeTypeScriptFiles_WhenEnabled()
    {
        if (!IsEnabled("CODECOV_RUN_JS_TESTS"))
        {
            return;
        }

        string repositoryRoot = FindRepositoryRoot();
        string sourceRoot = Path.Combine(repositoryRoot, "Cropper.Blazor", "Cropper");
        string typeScriptCoverage = Path.Combine(repositoryRoot, "Cropper.Blazor", "coverage", "cobertura-coverage.xml");

        Assert.True(File.Exists(typeScriptCoverage), $"TypeScript coverage file is missing: {typeScriptCoverage}");

        HashSet<string> reportedFiles = XDocument.Load(typeScriptCoverage)
            .Descendants("class")
            .Select(element => NormalizeCoverageFileName(element.Attribute("filename")?.Value))
            .Where(static file => !string.IsNullOrWhiteSpace(file))
            .Select(static file => file!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        string[] expectedFiles = Directory.EnumerateFiles(sourceRoot, "*.ts", SearchOption.AllDirectories)
            .Where(static file => !file.EndsWith(".d.ts", StringComparison.OrdinalIgnoreCase))
            .Where(static file => !file.EndsWith(".test.ts", StringComparison.OrdinalIgnoreCase))
            .Where(static file => !file.EndsWith(".coverage.test.ts", StringComparison.OrdinalIgnoreCase))
            .Where(static file => !file.EndsWith(".spec.ts", StringComparison.OrdinalIgnoreCase))
            .Where(static file => !file.EndsWith("cropperJsInterop.ts", StringComparison.OrdinalIgnoreCase))
            .Where(static file => !file.EndsWith("cropperJsInterop.types.ts", StringComparison.OrdinalIgnoreCase))
            .Where(static file => !file.Contains($"{Path.DirectorySeparatorChar}types{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Select(file => Path.GetRelativePath(Path.Combine(repositoryRoot, "Cropper.Blazor"), file).Replace('\\', '/'))
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        string[] missingFiles = expectedFiles
            .Where(file => !reportedFiles.Contains(file))
            .ToArray();

        Assert.True(missingFiles.Length == 0, $"TypeScript coverage report is missing runtime file(s):{Environment.NewLine}{string.Join(Environment.NewLine, missingFiles.Select(file => $"- {file}"))}");
    }

    [Fact]
    public void CodecovUploadCommand_CanBeValidatedLocally_WhenEnabled()
    {
        if (!IsEnabled("CODECOV_VERIFY_UPLOAD"))
        {
            return;
        }

        string? token = Environment.GetEnvironmentVariable("CODECOV_TOKEN");
        Assert.False(string.IsNullOrWhiteSpace(token), "CODECOV_TOKEN must be set when CODECOV_VERIFY_UPLOAD=true.");

        string repositoryRoot = FindRepositoryRoot();
        string dotNetFiles = string.Join(',', DotNetCoverageFiles.Select(file => Path.Combine(repositoryRoot, file.Replace('/', Path.DirectorySeparatorChar))));
        CommandResult dotNetUpload = Run("codecov", $"upload-process -t {token} -f \"{dotNetFiles}\" -F DotNet --disable-search --verbose", repositoryRoot);

        Assert.True(dotNetUpload.ExitCode == 0, $"Codecov DotNet upload validation failed:{Environment.NewLine}{dotNetUpload.Output}");

        if (IsEnabled("CODECOV_RUN_JS_TESTS"))
        {
            string typeScriptCoverage = Path.Combine(repositoryRoot, "Cropper.Blazor", "coverage", "cobertura-coverage.xml");
            CommandResult typeScriptUpload = Run("codecov", $"upload-process -t {token} -f \"{typeScriptCoverage}\" -F TypeScript --disable-search --verbose", repositoryRoot);

            Assert.True(typeScriptUpload.ExitCode == 0, $"Codecov TypeScript upload validation failed:{Environment.NewLine}{typeScriptUpload.Output}");
        }
    }

    private static bool IsEnabled(string environmentVariable)
    {
        string? value = Environment.GetEnvironmentVariable(environmentVariable);
        return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
    }

    private static decimal GetRequiredRate(XElement element, string attributeName)
    {
        string value = element.Attribute(attributeName)?.Value
            ?? throw new InvalidOperationException($"The coverage report is missing the '{attributeName}' attribute.");

        return decimal.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    private static int GetRequiredCount(XElement element, string attributeName)
    {
        string value = element.Attribute(attributeName)?.Value
            ?? throw new InvalidOperationException($"The coverage report is missing the '{attributeName}' attribute.");

        return int.Parse(value, CultureInfo.InvariantCulture);
    }

    private static bool IsCoverageFile(XElement element, string expectedFile)
    {
        string? filename = NormalizeCoverageFileName(element.Attribute("filename")?.Value);

        return filename is not null
            && (string.Equals(filename, expectedFile, StringComparison.OrdinalIgnoreCase)
                || filename.EndsWith($"/{expectedFile}", StringComparison.OrdinalIgnoreCase));
    }

    private static string? NormalizeCoverageFileName(string? filename)
    {
        return string.IsNullOrWhiteSpace(filename)
            ? null
            : filename.Replace('\\', '/');
    }

    private static string GetCoverageDetails(XElement coverage)
    {
        IEnumerable<string> files = coverage
            .Descendants("class")
            .Select(element => new
            {
                FileName = element.Attribute("filename")?.Value ?? element.Attribute("name")?.Value ?? "<unknown>",
                LineRate = GetRequiredRate(element, "line-rate"),
                BranchRate = GetRequiredRate(element, "branch-rate")
            })
            .Where(file => file.LineRate < 1m || file.BranchRate < 1m)
            .OrderBy(file => file.FileName, StringComparer.OrdinalIgnoreCase)
            .Select(file => $"- {file.FileName}: lines {file.LineRate:P2}, branches {file.BranchRate:P2}");

        return "Files below 100%:" + Environment.NewLine + string.Join(Environment.NewLine, files);
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

    private static string FindRepositoryRoot()
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

    private sealed record CommandResult(int ExitCode, string Output);
}
