<p align="center">
  <a href="https://durkatechnologies.github.io/Cropper.Blazor">
    <img src="src/Cropper.Blazor/Client/wwwroot/cropperblazor.png?raw=true" align="center" alt="Cropper.Blazor" width="200 px">
  </a>
  <h1 align="center">Cropper.Blazor</h1>
  <p align="center">
    <b>Cropper.Blazor</b>  is a component element that wraps around <a href="https://github.com/fengyuanchen/cropperjs"><b>Cropper.js</b></a>
  </p>
</p>

[![Build and run test](https://github.com/DurkaTechnologies/Cropper.Blazor/actions/workflows/ci.yml/badge.svg?event=push)](https://github.com/DurkaTechnologies/Cropper.Blazor/actions/workflows/ci.yml)
[![Deploy to GitHub Pages](https://github.com/DurkaTechnologies/Cropper.Blazor/actions/workflows/cd.yml/badge.svg?event=push)](https://github.com/DurkaTechnologies/Cropper.Blazor/actions/workflows/cd.yml)
[![coverage](https://codecov.io/github/DurkaTechnologies/Cropper.Blazor/branch/dev/graph/badge.svg?token=39M66DO85T)](https://codecov.io/github/DurkaTechnologies/Cropper.Blazor)
[![GitHub](https://img.shields.io/github/license/DurkaTechnologies/Cropper.Blazor?color=ff5c9b)](https://github.com/DurkaTechnologies/Cropper.Blazor/blob/dev/LICENSE)
[![GitHub](https://img.shields.io/github/last-commit/DurkaTechnologies/Cropper.Blazor?color=009DEA)](https://github.com/DurkaTechnologies/Cropper.Blazor)

## Demo
- [durkatechnologies.github.io/Cropper.Blazor/demo](https://durkatechnologies.github.io/Cropper.Blazor/demo)

## Usage

Import Custom Element:

```razor
@using Cropper.Blazor.Components
```

And then use it in Razor file ([for example](https://github.com/DurkaTechnologies/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor)):

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

And then use it in [*.razor.cs file](https://github.com/DurkaTechnologies/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor.cs)

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b feature/<my-new-feature>`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin feature/<my-new-feature>`
5. Submit a pull request :D
