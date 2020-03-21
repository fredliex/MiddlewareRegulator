using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiddlewareRegulator(appBuilder =>
            {
                var staticFileMiddleware = appBuilder.Find<StaticFileMiddleware>();
                if (staticFileMiddleware == null) 
                {
                    appBuilder.UseStaticFiles();
                    staticFileMiddleware = appBuilder.Find<StaticFileMiddleware>();
                }
                appBuilder.Remove(staticFileMiddleware);
                appBuilder.Insert(0, staticFileMiddleware);
            });

            services.AddMiddlewareRegulator(appBuilder =>
            {
                var developerExceptionPageMiddleware = appBuilder.Find<DeveloperExceptionPageMiddleware>();
                if (developerExceptionPageMiddleware != null) appBuilder.Remove(developerExceptionPageMiddleware);
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
