<p align="center">
  <a href="https://CropperBlazor.github.io/">
    <img src="content/cropperblazor.png?raw=true" align="center" alt="Cropper.Blazor" width="200 px">
  </a>
  <h1 align="center">
    Cropper.Blazor
  </h1>
  <p align="center">
    <b>Cropper.Blazor</b>  is a component element that wraps around <a href="https://github.com/fengyuanchen/cropperjs"><b>Cropper.js</b></a>
  </p>
</p>

[![Build and run test](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/ci.yml/badge.svg?event=push)](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/ci.yml)
[![Deploy to GitHub Pages](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/cd.yml/badge.svg?event=push)](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/cd.yml)
[![coverage](https://codecov.io/github/CropperBlazor/Cropper.Blazor/branch/dev/graph/badge.svg?token=39M66DO85T)](https://codecov.io/github/CropperBlazor/Cropper.Blazor)
[![GitHub](https://img.shields.io/github/license/CropperBlazor/Cropper.Blazor?color=ff5c9b)](https://github.com/CropperBlazor/Cropper.Blazor/blob/dev/LICENSE)
[![GitHub](https://img.shields.io/github/last-commit/CropperBlazor/Cropper.Blazor?color=009DEA)](https://github.com/CropperBlazor/Cropper.Blazor)
[![NuGet Badge](https://buildstats.info/nuget/Cropper.Blazor)](https://www.nuget.org/packages/Cropper.Blazor/)

## Demo
- [CropperBlazor.github.io/demo](https://CropperBlazor.github.io/demo)

## Prerequisites
- Supported .NET 7.0, .NET 6.0 versions for these web platforms:
  - Blazor WebAssembly
  - Blazor Server
  - Blazor Server Hybrid with MVC
  - MAUI Blazor Hybrid

## Installation

```
dotnet add package Cropper.Blazor
```

## Usage

Import Custom Element:

```razor
@using Cropper.Blazor.Components
```


Add the following to `index.html` (client-side — Blazor Webassembly, Blazor MAUI) or `_Host.cshtml` (server-side — Blazor Server, MVC with Blazor Server) in the `head`
```razor
<link href="_content/Cropper.Blazor/cropper.min.css" rel="stylesheet" />
```

Add the following to `index.html` or `_Host.cshtml` in the `body`
```razor
<script src="_content/Cropper.Blazor/cropper.min.js"></script>
```


Add the following to the relevant sections of `Program.cs`
```c#
using Cropper.Blazor.Extensions;
```
```c#
builder.Services.AddCropper();
```

In addition, you can change the path to the CropperJSInterop.min.js module if for some reason it is located outside the server root folder as follows:
- Override internal path to CropperJSInterop.min.js module:
```c#
builder.Services.AddCropper(new CropperJsInteropOptions()
{
    DefaultInternalPathToCropperModule = "<YourPath>/_content/Cropper.Blazor/cropperJsInterop.min.js"
})
```
- Override full global path to CropperJSInterop.min.js module:
```c#
builder.Services.AddCropper(new CropperJsInteropOptions()
{
    IsActiveGlobalPath = true,
    GlobalPathToCropperModule = "<StartUrlWithPath>/_content/Cropper.Blazor/cropperJsInterop.min.js"
})
```

Also for server-side (Blazor Server or MVC with Blazor Server) you need add configuration SignalR, increase MaximumReceiveMessageSize of a single incoming hub message (default is 32KB) and map SignalR to your path. [For example](https://github.com/CropperBlazor/Cropper.Blazor/blob/dev/examples/Cropper.Blazor.Server.Net7/Program.cs):
```c#
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        options.MaximumReceiveMessageSize = 32 * 1024 * 100;
    });
```
```c#
app.MapBlazorHub();
```


And then use it in Razor file ([for example](https://github.com/CropperBlazor/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor)):

```razor
<CropperComponent
  Class="cropper-demo"
  ErrorLoadImageClass="cropper-error-load"
  @ref="cropperComponent"
  OnCropStartEvent="OnCropStartEvent"
  OnCropEndEvent="OnCropEndEvent"
  OnCropEvent="OnCropEvent"
  OnZoomEvent="OnZoomEvent"
  OnCropMoveEvent="OnCropMoveEvent"
  OnReadyEvent="OnCropReadyEvent"
  OnLoadImageEvent="OnLoadImageEvent"
  Src="@Src"
  InputAttributes="@InputAttributes"
  ErrorLoadImageSrc="@ErrorLoadImageSrc"
  IsErrorLoadImage="@IsErrorLoadImage"
  OnErrorLoadImageEvent="OnErrorLoadImageEvent"
  Options="options">
</CropperComponent>
```

And then use it in [*.razor.cs file](https://github.com/CropperBlazor/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor.cs)

You may override Cropper JavaScript module with execution script which can replace 6 event handlers (onReady, onCropStart, onCropMove, onCropEnd, onCrop, onZoom), such as overriding the onZoom callback in JavaScript:
```js
window.overrideCropperJsInteropModule = (minZoomRatio, maxZoomRatio) => {
    window.cropper.onZoom = function (imageObject, event, correlationId) {
        const jSEventData = this.getJSEventData(event, correlationId);
        const isApplyPreventZoomRatio = minZoomRatio != null || maxZoomRatio != null;

        if (isApplyPreventZoomRatio && (event.detail.ratio < minZoomRatio || event.detail.ratio > maxZoomRatio)) {
            event.preventDefault();
        }
        else {
            imageObject.invokeMethodAsync('CropperIsZoomed', jSEventData);
        }
    };
};
```

Analysis of the signature onReady, onCropStart, onCropMove, onCropEnd, onCrop, onZoom event handlers:
### imageObject

- Type: `Object`

Reference to base cropper component.

### event

- Type: `CustomEvent`

Represent Cropper Event.

### correlationId

- Type: `String`
- Default: `Cropper.Blazor`

A Correlation ID is a unique identifier that is added to the very first interaction(incoming request)
to identify the context and is passed to all components that are involved in the transaction flow.

Definitely need to tell these rules in Blazor:
```c#
await JSRuntime!.InvokeVoidAsync("window.overrideCropperJsInteropModule", MinZoomRatio, MaxZoomRatio);
```

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b feature/<my-new-feature>`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/<my-new-feature>`
5. Submit a pull request :D
