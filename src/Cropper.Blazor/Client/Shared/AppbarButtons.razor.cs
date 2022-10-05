using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Shared;

public partial class AppbarButtons
{
    [Inject] private LayoutService LayoutService { get; set; }
}