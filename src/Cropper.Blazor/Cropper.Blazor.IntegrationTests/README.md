# Cropper.Blazor Selenium integration tests

These tests run the Blazor demo in Chrome and verify Cropper.js demo behavior.

## Headless run

```powershell
dotnet test Cropper.Blazor.IntegrationTests\Cropper.Blazor.IntegrationTests.csproj
```

## Visible Chrome run

Use this when you want to see the browser while the tests run:

```powershell
$env:CROPPER_BLAZOR_TEST_HEADED = "true"
dotnet test Cropper.Blazor.IntegrationTests\Cropper.Blazor.IntegrationTests.csproj --filter FullyQualifiedName~V2Demo_LoadsCropperElements
```

## Pause on failure

This keeps Chrome open after a failure so the page can be inspected manually. Press Enter in the test console to let cleanup continue.

```powershell
$env:CROPPER_BLAZOR_TEST_HEADED = "true"
$env:CROPPER_BLAZOR_TEST_PAUSE_ON_FAILURE = "true"
dotnet test Cropper.Blazor.IntegrationTests\Cropper.Blazor.IntegrationTests.csproj --filter FullyQualifiedName~V2Demo_LoadsCropperElements
```

## Test an already running app

```powershell
$env:CROPPER_BLAZOR_TEST_BASE_URL = "http://localhost:5025"
$env:CROPPER_BLAZOR_TEST_HEADED = "true"
dotnet test Cropper.Blazor.IntegrationTests\Cropper.Blazor.IntegrationTests.csproj
```

If `CROPPER_BLAZOR_TEST_BASE_URL` is not set, the fixture starts `Server/Cropper.Blazor.Server.csproj` automatically at `http://localhost:5025`.

## Failure artifacts

On failure, diagnostics are written under:

```text
TestResults/Selenium/<test-name>/
```

Artifacts include:

- `screenshot.png`
- `page.html`
- `metadata.txt`
- `browser-console.log`
