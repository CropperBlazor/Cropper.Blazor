using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace Cropper.Blazor.IntegrationTests;

public sealed class SeleniumTestFixture : IDisposable
{
    private const int DefaultServerStartupTimeoutSeconds = 90;
    private const int DefaultUiWaitTimeoutSeconds = 5;
    private const int DefaultUiStartupWaitTimeoutSeconds = 30;
    private const int DefaultUiWaitPollingMilliseconds = 250;
    private const int DefaultFailureInspectionSeconds = 15;
    private readonly Process? _serverProcess;
    private readonly string? _serverOutputDirectory;

    public SeleniumTestFixture()
    {
        BaseUrl = Environment.GetEnvironmentVariable("CROPPER_BLAZOR_TEST_BASE_URL") ?? $"http://localhost:{GetFreeTcpPort()}";
        ShouldManageServer = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CROPPER_BLAZOR_TEST_BASE_URL"));

        if (ShouldManageServer)
        {
            _serverProcess = StartServer(BaseUrl, out _serverOutputDirectory);
            WaitForServer();
        }

        Driver = CreateDriver();
        Driver.Manage().Window.Size = new System.Drawing.Size(1440, 1200);
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(UiWaitTimeoutSeconds))
        {
            PollingInterval = TimeSpan.FromMilliseconds(UiWaitPollingMilliseconds)
        };
        Wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
        StartupWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(UiStartupWaitTimeoutSeconds))
        {
            PollingInterval = TimeSpan.FromMilliseconds(UiWaitPollingMilliseconds)
        };
        StartupWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
    }

    public string BaseUrl { get; }

    public IWebDriver Driver { get; }

    public WebDriverWait Wait { get; }

    public WebDriverWait StartupWait { get; }

    public static int UiWaitTimeoutSeconds => GetPositiveInt32EnvironmentVariable("CROPPER_BLAZOR_TEST_UI_WAIT_SECONDS", DefaultUiWaitTimeoutSeconds);

    public static int UiStartupWaitTimeoutSeconds => GetPositiveInt32EnvironmentVariable("CROPPER_BLAZOR_TEST_UI_STARTUP_WAIT_SECONDS", DefaultUiStartupWaitTimeoutSeconds);

    public static int UiWaitPollingMilliseconds => GetPositiveInt32EnvironmentVariable("CROPPER_BLAZOR_TEST_UI_WAIT_POLLING_MS", DefaultUiWaitPollingMilliseconds);

    public static int ServerStartupTimeoutSeconds => GetPositiveInt32EnvironmentVariable("CROPPER_BLAZOR_TEST_SERVER_STARTUP_SECONDS", DefaultServerStartupTimeoutSeconds);

    public static int FailureInspectionSeconds => GetPositiveInt32EnvironmentVariable("CROPPER_BLAZOR_TEST_FAILURE_INSPECTION_SECONDS", DefaultFailureInspectionSeconds);

    public static bool IsHeaded => true;

	public static bool PauseOnFailure => IsEnabled("CROPPER_BLAZOR_TEST_PAUSE_ON_FAILURE");

    private bool ShouldManageServer { get; }

    public Uri GetUri(string relativePath)
    {
        return new Uri(new Uri(BaseUrl.TrimEnd('/') + "/"), relativePath.TrimStart('/'));
    }

    public IWebElement WaitForElement(By by)
    {
        return Wait.Until(driver =>
        {
            IWebElement element = driver.FindElement(by);
            return element.Displayed ? element : null;
        })!;
    }

	public IWebElement WaitForStartupElement(By by, int retries = 3)
	{
		Exception? lastException = null;

		for (int i = 0; i < retries; i++)
		{
			try
			{
				return StartupWait.Until(driver =>
				{
					IWebElement? element = driver
						.FindElements(by)
						.FirstOrDefault();

					return element is { Displayed: true, Enabled: true }
						? element
						: null;
				})!;
			}
			catch (WebDriverException ex)
			{
				lastException = ex;

				Thread.Sleep(250);
			}
		}

		throw new WebDriverException(
			$"Failed to find startup element: {by}",
			lastException);
	}

	public IWebElement WaitForClickable(By by)
    {
        return Wait.Until(driver =>
        {
            IWebElement element = driver.FindElement(by);
            return element.Displayed && element.Enabled ? element : null;
        })!;
    }

    public T ExecuteScript<T>(string script, params object[] args)
    {
        return (T)((IJavaScriptExecutor)Driver).ExecuteScript(script, args)!;
    }

    public T ExecuteAsyncScript<T>(string script, params object[] args)
    {
        return (T)((IJavaScriptExecutor)Driver).ExecuteAsyncScript(script, args)!;
    }

    public void Dispose()
    {
        Driver.Quit();
        Driver.Dispose();

        if (_serverProcess is not null && !_serverProcess.HasExited)
        {
            _serverProcess.Kill(entireProcessTree: true);
            _serverProcess.Dispose();
        }

        if (_serverOutputDirectory is not null)
        {
            try
            {
                Directory.Delete(_serverOutputDirectory, recursive: true);
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }

    public string CaptureDiagnostics(string testName)
    {
        string artifactsDirectory = Path.Combine(FindSolutionDirectory(), "TestResults", "Selenium", SanitizeFileName(testName));
        Directory.CreateDirectory(artifactsDirectory);

        string screenshotPath = Path.Combine(artifactsDirectory, "screenshot.png");
        string htmlPath = Path.Combine(artifactsDirectory, "page.html");
        string consolePath = Path.Combine(artifactsDirectory, "browser-console.log");
        string metadataPath = Path.Combine(artifactsDirectory, "metadata.txt");

        if (Driver is ITakesScreenshot screenshotDriver)
        {
            try
            {
                screenshotDriver.GetScreenshot().SaveAsFile(screenshotPath);
            }
            catch (UnhandledAlertException)
            {
                File.WriteAllText(screenshotPath + ".skipped.txt", "Screenshot capture skipped because an unhandled browser alert was present.");
            }
            catch (WebDriverException ex)
            {
                File.WriteAllText(screenshotPath + ".skipped.txt", $"Screenshot capture skipped: {ex.Message}");
            }
        }

        File.WriteAllText(htmlPath, Driver.PageSource);
        File.WriteAllText(metadataPath, $"Url: {Driver.Url}{Environment.NewLine}Title: {Driver.Title}{Environment.NewLine}Headed: {IsHeaded}{Environment.NewLine}");

        try
        {
            IEnumerable<LogEntry> logs = Driver.Manage().Logs.GetLog(LogType.Browser);
            File.WriteAllLines(consolePath, logs.Select(log => $"[{log.Timestamp:O}] {log.Level}: {log.Message}"));
        }
        catch (WebDriverException ex)
        {
            File.WriteAllText(consolePath, $"Browser log capture was not available: {ex.Message}");
        }

        return artifactsDirectory;
    }

    public void AssertNoBrowserConsoleErrors()
    {
        IReadOnlyCollection<LogEntry> logs;

        try
        {
            logs = Driver.Manage().Logs.GetLog(LogType.Browser);
        }
        catch (WebDriverException)
        {
            return;
        }

        Assert.DoesNotContain(logs, log => log.Level == LogLevel.Severe);
    }

    private void DismissAlertIfPresent()
    {
        try
        {
            Driver.SwitchTo().Alert().Dismiss();
        }
        catch (NoAlertPresentException)
        {
        }
        catch (WebDriverException)
        {
        }
    }

    public void PauseForInspection(string reason)
    {
        if (!PauseOnFailure || !IsHeaded)
        {
            return;
        }

        Console.WriteLine(reason);
        Console.WriteLine($"Chrome is visible for {FailureInspectionSeconds} second(s) before cleanup.");
        Thread.Sleep(TimeSpan.FromSeconds(FailureInspectionSeconds));
    }

    private static IWebDriver CreateDriver()
    {
        ChromeOptions options = new();

        if (!IsHeaded)
        {
            options.AddArgument("--headless=new");
        }
      
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--window-size=1440,1200");
        options.AddArgument("--disable-application-cache");
        options.AddArgument("--disable-cache");
        options.AddArgument($"--user-data-dir={Path.Combine(Path.GetTempPath(), "CropperBlazorSelenium", Guid.NewGuid().ToString("N"))}");
        options.AcceptInsecureCertificates = true;
        options.SetLoggingPreference(LogType.Browser, LogLevel.All);

        return new ChromeDriver(options);
    }

    public void NavigateTo(string relativePath)
    {
        Driver.Navigate().GoToUrl(GetUri(relativePath));
        ClearBrowserState();
        Driver.Navigate().Refresh();
    }

    public void ClearBrowserState()
    {
        try
        {
            ((IJavaScriptExecutor)Driver).ExecuteAsyncScript(
                "const done = arguments[arguments.length - 1];" +
                "Promise.all((navigator.serviceWorker ? navigator.serviceWorker.getRegistrations() : Promise.resolve([])).then(rs => Promise.all(rs.map(r => r.unregister()))), caches ? caches.keys().then(keys => Promise.all(keys.map(key => caches.delete(key)))) : Promise.resolve()).then(() => done(true)).catch(() => done(false));");
        }
        catch (WebDriverException)
        {
            // Best-effort cache cleanup only.
        }
    }

    private static bool IsEnabled(string environmentVariable)
    {
        string? value = Environment.GetEnvironmentVariable(environmentVariable);
        return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
    }

    private static int GetPositiveInt32EnvironmentVariable(string environmentVariable, int defaultValue)
    {
        string? value = Environment.GetEnvironmentVariable(environmentVariable);

        return int.TryParse(value, out int result) && result > 0 ? result : defaultValue;
    }

    private static string SanitizeFileName(string value)
    {
        foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        {
            value = value.Replace(invalidFileNameChar, '_');
        }

        return value;
    }

    private static Process StartServer(string baseUrl, out string? serverOutputDirectory)
    {
        string solutionDirectory = FindSolutionDirectory();
        string outputDirectory;

        if (!IsEnabled("CROPPER_BLAZOR_TEST_SKIP_BUILD"))
        {
            serverOutputDirectory = Path.Combine(Path.GetTempPath(), "CropperBlazorSeleniumServer", Guid.NewGuid().ToString("N"));
            outputDirectory = serverOutputDirectory;
            EnsureServerIsBuilt(solutionDirectory, outputDirectory);
        }
        else
        {
            serverOutputDirectory = null;
            outputDirectory = Path.Combine(solutionDirectory, "Server", "bin", "Debug", "net10.0");
        }

        string serverAssemblyPath = Path.Combine(outputDirectory, "Cropper.Blazor.Server.dll");
        if (!File.Exists(serverAssemblyPath))
        {
            throw new FileNotFoundException("Could not find the built Cropper.Blazor.Server assembly for Selenium tests.", serverAssemblyPath);
        }

        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            Arguments = $"\"{serverAssemblyPath}\" --urls {baseUrl}",
            WorkingDirectory = solutionDirectory,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        startInfo.Environment["ASPNETCORE_STATICWEBASSETS"] = Path.Combine(outputDirectory, "Cropper.Blazor.Server.staticwebassets.runtime.json");

        Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start Cropper.Blazor.Server.");
        process.OutputDataReceived += (_, _) => { };
        process.ErrorDataReceived += (_, _) => { };
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }

    private static int GetFreeTcpPort()
    {
        TcpListener listener = new(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();

        return port;
    }

    private static void EnsureServerIsBuilt(string solutionDirectory, string serverOutputDirectory)
    {
        EnsureCropperStaticAssetsAreBuilt(solutionDirectory);

        string serverProjectPath = Path.Combine(solutionDirectory, "Server", "Cropper.Blazor.Server.csproj");
        string baseOutputPath = EnsureTrailingDirectorySeparator(Path.Combine(serverOutputDirectory, "bin"));
        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            WorkingDirectory = solutionDirectory,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        startInfo.ArgumentList.Add("build");
        startInfo.ArgumentList.Add(serverProjectPath);
        startInfo.ArgumentList.Add("-f");
        startInfo.ArgumentList.Add("net10.0");
        startInfo.ArgumentList.Add("-c");
        startInfo.ArgumentList.Add("Debug");
        startInfo.ArgumentList.Add("-o");
        startInfo.ArgumentList.Add(serverOutputDirectory);
        startInfo.ArgumentList.Add("-p:UseAppHost=false");
        startInfo.ArgumentList.Add($"-p:BaseOutputPath={baseOutputPath}");
        startInfo.ArgumentList.Add("-p:DebugType=None");
        startInfo.ArgumentList.Add("-p:DebugSymbols=false");
        startInfo.ArgumentList.Add("/nodeReuse:false");

        using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not build Cropper.Blazor.Server before starting Selenium server.");
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Could not build Cropper.Blazor.Server before Selenium test run.{Environment.NewLine}{output}{Environment.NewLine}{error}");
        }

    }

    private static string EnsureTrailingDirectorySeparator(string path)
    {
        return Path.EndsInDirectorySeparator(path) ? path : path + Path.DirectorySeparatorChar;
    }

    private static void EnsureCropperStaticAssetsAreBuilt(string solutionDirectory)
    {
        string cropperProjectDirectory = Path.Combine(solutionDirectory, "Cropper.Blazor");
        string wwwrootDirectory = Path.Combine(cropperProjectDirectory, "wwwroot");
        string[] requiredFiles =
        [
            Path.Combine(wwwrootDirectory, "cropper.min.js"),
            Path.Combine(wwwrootDirectory, "cropperJsInterop.min.js"),
        ];

        if (requiredFiles.All(File.Exists))
        {
            return;
        }

        ProcessStartInfo startInfo = new()
        {
            FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "npm",
            Arguments = OperatingSystem.IsWindows() ? "/c npm run build:debug" : "run build:debug",
            WorkingDirectory = cropperProjectDirectory,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
        };

        using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not build Cropper.Blazor static assets before starting Selenium server.");
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0 || !requiredFiles.All(File.Exists))
        {
            throw new InvalidOperationException($"Could not build Cropper.Blazor static assets before Selenium test run.{Environment.NewLine}{output}{Environment.NewLine}{error}");
        }
    }

    private void WaitForServer()
    {
        using HttpClient httpClient = new();
        DateTimeOffset timeout = DateTimeOffset.UtcNow.AddSeconds(ServerStartupTimeoutSeconds);
        Exception? lastException = null;

        while (DateTimeOffset.UtcNow < timeout)
        {
            if (_serverProcess?.HasExited == true)
            {
                throw new InvalidOperationException($"Cropper.Blazor.Server exited before becoming ready with code {_serverProcess.ExitCode}.");
            }

            try
            {
                using HttpResponseMessage response = httpClient.GetAsync(BaseUrl).GetAwaiter().GetResult();

                if ((int)response.StatusCode < 500)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
            }

            Thread.Sleep(500);
        }

        throw new TimeoutException($"Cropper.Blazor.Server did not become ready at {BaseUrl}.", lastException);
    }

    private static string FindSolutionDirectory()
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
}
