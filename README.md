**Cropper.Blazor** is a component element that wraps around [Cropper.js](https://github.com/fengyuanchen/cropperjs)

[![Deploy to GitHub Pages](https://github.com/DurkaTechnologies/Cropper.Blazor/actions/workflows/main.yml/badge.svg?branch=dev&event=deployment_status)](https://github.com/DurkaTechnologies/Cropper.Blazor/actions/workflows/main.yml)

<img src="src/Cropper.Blazor/Client/wwwroot/crop.png" width="256" height="256" />


## Demo

https://durkatechnologies.github.io/Cropper.Blazor

## Usage

Import Custom Element:

```csharp
@using Cropper.Blazor.Components
```

And then use it in Razor file ([for example](https://github.com/DurkaTechnologies/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor)):

```csharp
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


And then use it in [Razor.cs file](https://github.com/DurkaTechnologies/Cropper.Blazor/blob/dev/src/Cropper.Blazor/Client/Pages/CropperDemo.razor.cs)

## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -m 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request :D
