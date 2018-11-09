﻿using System;
using DotNetCore.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using WebApiB.Basic;

namespace WebApiB
{
    public class Startup
    {
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("WebApiB", new Info { Title = "用户API接口B", Version = "v1" });
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "WebApiB.xml");
                options.IncludeXmlComments(xmlPath);
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //依赖注入
            DependencyInjection.Initialize(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //数据库连接读取配置文件
            ServiceLocator.Instance = app.ApplicationServices;
            ServiceLocator.AppConfigurations = _appConfigurations;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc().UseSwagger(c =>
                {
                    c.RouteTemplate = "{documentName}/swagger.json";
                })
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/WebApiB/swagger.json", "WebApiB");
                });
        }
    }
}
