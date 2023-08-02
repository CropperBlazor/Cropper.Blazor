using Cropper.Blazor.Client.Extensions;
using Cropper.Blazor.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
})
.AddCropper()
.TryAddDocsViewServices();

await builder.Build().RunAsync();
