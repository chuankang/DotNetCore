using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ResourceManagement;

namespace CMSWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ////ResourcesBuilder.ResourceManifest，用Bootstrap中文网的CDN资源替换了内置的Google CDN资源，解决我们伟大的那堵墙所阻止的jquery库，如果不替换，进入到Orchard Core 的管理后台，你就会发现无法点开管理菜单。
            //services.AddScoped<IResourceManifestProvider, ResourcesBuilder.ResourceManifest>();
            services.AddOrchardCms();
            services.Configure<IdentityOptions>(options =>
            {

                options.Password.RequireDigit = false;

                options.Password.RequireLowercase = true;

                options.Password.RequireUppercase = true;

                options.Password.RequireNonAlphanumeric = false;

                options.Password.RequiredUniqueChars = 3;

                options.Password.RequiredLength = 6;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseOrchardCore();
        }
    }
}
