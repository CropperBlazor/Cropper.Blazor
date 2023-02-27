using System;
using Cropper.Blazor.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;

namespace Cropper.MVC.With.Blazor.Server.Net7
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddServerSideBlazor()
                .AddHubOptions(options =>
                {
                    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                    options.EnableDetailedErrors = true;
                    options.HandshakeTimeout = TimeSpan.FromSeconds(15);
                    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                    options.MaximumParallelInvocationsPerClient = 1;
                    options.MaximumReceiveMessageSize = 32 * 1024 * 100;
                    options.StreamBufferCapacity = 10;
                });
            services.AddCropper();
            services.AddMudServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Normally, you would have .MapBlazorHub() and /MapFallbackToPage("/_Host") here.
                //
                // The change below puts your Blazor content under /Blazor (YourSite.com/Blazor/SomePage). This serves a few purposes:
                //  - Eliminates the possibility of conflicts with existing MVC routes.
                //  - Ordinarily, Blazor takes over the default route (YourSite.com with no path) which can be problematic.
                //    Our goal is to avoid interfering with existing MVC behavior.
                // 
                // Some day, if the entire MVC app is ever completely re-worked in Blazor, you could change this
                // back to the typical settings, tweak a few other minor changes in _Host that support this, and perhaps have a party.
                endpoints.MapBlazorHub("/Blazor/_blazor");
                endpoints.MapFallbackToPage("~/Blazor/{*clientroutes:nonfile}", "/Blazor/_Host");
            });
        }
    }
}
