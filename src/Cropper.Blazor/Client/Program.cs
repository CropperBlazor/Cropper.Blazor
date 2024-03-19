using Cropper.Blazor.Client;
using Cropper.Blazor.Client.Extensions;
using Cropper.Blazor.Client.Services;
using Cropper.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment);

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
{
    services
        .AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(hostEnvironment.BaseAddress)
        })
        .AddSingleton<GitHubApiClient>()
        .AddCropper()
        .TryAddDocsViewServices();
}