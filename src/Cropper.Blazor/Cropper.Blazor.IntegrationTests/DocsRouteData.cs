using System.Collections;

namespace Cropper.Blazor.IntegrationTests;

public sealed class ExamplePageRoutes : IEnumerable<object[]>
{
    private static readonly string[] Routes =
    [
        "/examples/cropperusage",
        "/examples/viewmodes",
        "/examples/preview",
        "/examples/dimensions",
        "/examples/aspectratio",
        "/examples/zooming",
        "/examples/cropping",
        "/examples/replacing",
        "/examples/rebuild"
    ];

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (string version in new[] { "/v1", "/v2" })
        {
            foreach (string route in Routes)
            {
                yield return [version + route];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class ApiPageRoutes : IEnumerable<object[]>
{
    private static readonly string[] Contracts =
    [
        "CropperComponent",
        "CroppedCanvasReceiver",
        "ImageReceiver",
        "DragMode",
        "CropperComponentType",
        "CropperData",
        "ImageData",
        "ContainerData",
        "CanvasData",
        "CropBoxData",
        "JSEventData",
        "CroppedCanvas",
        "ActionEvent",
        "ImageSmoothingQuality",
        "Options",
        "GetCroppedCanvasOptions",
        "SetCropBoxDataOptions",
        "SetCanvasDataOptions",
        "SetDataOptions",
        "CropEndEvent",
        "CropMoveEvent",
        "CropStartEvent",
        "CropReadyEvent",
        "ZoomEvent",
        "CropEvent",
        "ImageProcessingException",
        "IUrlImageInterop"
    ];

    public IEnumerator<object[]> GetEnumerator()
    {
        foreach (string version in new[] { "/v1", "/v2" })
        {
            yield return [version + "/api", "Cropper Component API"];

            foreach (string contract in Contracts)
            {
                yield return [version + "/api/" + contract, contract];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
