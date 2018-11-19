using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Common;
using DotNetCore.Mvc.AutoMapper;
using DotNetCore.Repositories.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DotNetCore.Mvc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly AppConfigurations _appConfigurations;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

			//AutoMapper静态方法初始化
			AutoMapperConfig.CreateMappings();

			//读取数据库连接配置文件1
			var config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var appConfigurations = new ServiceCollection().AddOptions().Configure<AppConfigurations>
                (config.GetSection("ConnectionStrings")).BuildServiceProvider();
            _appConfigurations = appConfigurations.GetService<IOptions<AppConfigurations>>().Value;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //依赖注入
            DependencyInjection.Initialize(services);

            // Entity Framework Contexts
            var ownDbCon = _appConfigurations.DefaultConnection;
            services.AddDbContext<TestDbcontext>(options => options.UseSqlServer(ownDbCon));

            //add session support 验证码
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //数据库连接读取配置文件2
            ServiceLocator.Instance = app.ApplicationServices;
            ServiceLocator.AppConfigurations = _appConfigurations;

            //Middleware1 为HTTP Request 提供存取网站的文件 简单理解就是使得网站上的静态文件可访问
            app.UseStaticFiles();

            //必须在usemvc之前调用。Session依赖Cookie才能工作，所以请确保用户首先接受GDPR cookie策略，这是ASP.NET Core 2.1默认模板里添加的
            app.UseSession();

            //Middlerwate2  MVC routing机制。有了这两个middleware，我们的的网站就有了MVC routing和读取静态文件的功能
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
           
        }
    }
}
