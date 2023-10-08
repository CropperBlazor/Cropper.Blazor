<p align="center">
  <a href="https://CropperBlazor.github.io/">
    <img src="content/cropperblazor.png?raw=true" align="center" alt="Cropper.Blazor" width="200 px">
  </a>
  <h1 align="center">
    Cropper.Blazor
  </h1>
  <p align="center">
    <b>Cropper.Blazor</b> is a component that wraps around <a href="https://github.com/fengyuanchen/cropperjs"><b>Cropper.js</b></a> version 1.6.1
  </p>
</p>

[![Build and run test](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/ci.yml/badge.svg?event=push)](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/ci.yml)
[![Deploy to GitHub Pages](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/cd.yml/badge.svg?event=push)](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/cd.yml)
[![Deploy to NuGet](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/release.yml/badge.svg?event=push)](https://github.com/CropperBlazor/Cropper.Blazor/actions/workflows/release.yml)
[![coverage](https://codecov.io/github/CropperBlazor/Cropper.Blazor/branch/dev/graph/badge.svg?token=39M66DO85T)](https://codecov.io/github/CropperBlazor/Cropper.Blazor)
[![GitHub](https://img.shields.io/github/license/CropperBlazor/Cropper.Blazor?color=ff5c9b)](https://github.com/CropperBlazor/Cropper.Blazor/blob/dev/LICENSE)
[![GitHub](https://img.shields.io/github/last-commit/CropperBlazor/Cropper.Blazor?color=009DEA)](https://github.com/CropperBlazor/Cropper.Blazor)
[![NuGet Badge](https://buildstats.info/nuget/Cropper.Blazor)](https://www.nuget.org/packages/Cropper.Blazor/)

Most powerful tool for enabling image cropping when creating an 
application in Blazor WebAssembly / Server, Hybrid with MAUI, MVC and
other frameworks.

Cropper.Blazor is an essential component for building interactive image cropping and manipulation features in Blazor web applications. This versatile Blazor library empowers developers to integrate intuitive image cropping functionality directly into their Blazor projects, offering users a seamless and responsive image editing experience.

## Demo
- [CropperBlazor.github.io/demo](https://CropperBlazor.github.io/demo)

## API
- [https://cropperblazor.github.io/api](https://cropperblazor.github.io/api)

## Prerequisites
- Supported .NET 7.0, .NET 6.0 versions for these web platforms:
  - Blazor WebAssembly
  - Blazor Server
  - Blazor Server Hybrid with MVC
  - MAUI Blazor Hybrid
  
  Note: if you have problem with MAUI project dependencies: 
  - `dotnet workload update` + rebuilt the project. If that doesn't help, try the step below about override package
  - override package, for example: `<PackageReference Include="Microsoft.Extensions.Logging.Abstractions" VersionOverride="7.0.1" />`

  ## Key features and aspects of the Cropper.Blazor package include:

- Blazor Integration: Cropper.Blazor is specifically designed for Blazor applications, allowing developers to effortlessly incorporate image cropping capabilities into their Blazor components and pages.
- Drag-and-Resize Interaction: Users can easily drag and resize the cropping area to precisely select the desired portion of an image. This intuitive interaction method ensures accurate and efficient cropping.
- Rotation Support: Cropper.Blazor includes the ability to rotate the selected image area, giving users full control over the orientation of their cropped content.
- Aspect Ratio Control: Developers can define custom aspect ratios for cropping, ensuring that the resulting image maintains specific proportions. This is particularly valuable for applications that require standardized image dimensions.
- Zoom Functionality: Cropper.Blazor allows users to zoom in and out of the image to fine-tune their cropping selection, guaranteeing precise results.
- Data Retrieval: The library provides methods to retrieve detailed information about the cropped area, such as coordinates and dimensions. This data can be easily captured and utilized for further processing or image uploads.
- Customization: Cropper.Blazor offers a wide range of configuration options, enabling developers to tailor the cropping experience to match the visual style and user interface of their Blazor applications.
- Cross-Browser Compatibility: The package is compatible with various modern web browsers, ensuring consistent functionality and user experience across different platforms.
- Documentation: Cropper.Blazor is accompanied by comprehensive documentation and practical examples, simplifying the integration process and helping developers make the most of its features.
- Open Source: Cropper.Blazor is open-source software, available for free use in both personal and commercial Blazor projects.

Cropper.Blazor streamlines the implementation of image cropping and editing within Blazor applications, enhancing user engagement and enabling dynamic image manipulation. Whether you are developing a Blazor-based image editor, profile picture uploader, or any other application that requires image cropping, Cropper.Blazor is a valuable package that simplifies the development process and enriches your application's capabilities.

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
