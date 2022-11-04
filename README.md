<p align="center">
  <a href="https://CropperBlazor.github.io/">
    <img src="content/cropperblazor.png?raw=true" align="center" alt="Cropper.Blazor" width="200 px">
  </a>
  <h1 align="center">
    Cropper.Blazor - Test 10
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
[![NuGet version](https://img.shields.io/nuget/v/Cropper.Blazor?color=ff5c9b&label=NuGet%20version)](https://www.nuget.org/packages/Cropper.Blazor)

## Demo
- [CropperBlazor.github.io/demo](https://CropperBlazor.github.io/demo)

## Installation

```
dotnet add package Cropper.Blazor
```

## Usage

Import Custom Element:

```razor
@using Cropper.Blazor.Components
```

Add the following to `index.html` (client-side) or `_Host.cshtml` (server-side) in the `head`
```razor
<link href="_content/Cropper.Blazor/cropper.css" rel="stylesheet" />
```

Add the following to `index.html` or `_Host.cshtml` in the `body`
```razor
<script src="_content/Cropper.Blazor/cropper.js"></script>
```

Add the following to the relevant sections of `Program.cs`
```c#
using Cropper.Blazor.Extensions;
```
```c#
builder.Services.AddCropper();
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
  Loading="lazy"
  ErrorLoadImageSrc="@ErrorLoadImageSrc"
  IsErrorLoadImage="@IsErrorLoadImage"
  OnErrorLoadImageEvent="OnErrorLoadImageEvent"
  Options="options">
</CropperComponent>
```

And then use it in [*.razor.cs file](https://github.com/CropperBlazor/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor.cs)

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b feature/<my-new-feature>`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/<my-new-feature>`
5. Submit a pull request :D
