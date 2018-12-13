using System;
using System.Collections.Generic;
using DotNetCore.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebApiA.AuthHelper;
using WebApiA.Basic;

namespace WebApiA
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
            #region swagger
            services.AddSwaggerGen(c =>
            {
                //启用注释nuget包
                c.EnableAnnotations();
                c.SwaggerDoc("WebApiA", new Info { Title = "用户API接口A", Version = "v1" });
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                string basePath = AppContext.BaseDirectory;//Linux路径区分大小写，这里用appcontext
                string xmlPath = Path.Combine(basePath, "WebApiA.xml");
                //如果有xml注释文档就读取，需在项目属性生成xml
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Blog.Core", new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition("Blog.Core", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入{token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });
            #endregion

            #region Token服务注册
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin,Client").Build());
            });
            #endregion


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

            #region swagger
            // 启用Swagger.
            app.UseSwagger();

            // 启用中间件以提供用户界面（HTML、js、CSS等），特别是指定JSON端点。
            app.UseSwaggerUI(c =>
            {
                //文档终结点
                c.SwaggerEndpoint("/swagger/WebApiA/swagger.json", "测试接口 V1");
                //页面头名称
                c.DocumentTitle = "平台API";
                //页面API文档格式 Full=全部展开， List=只展开列表, None=都不展开
                c.DocExpansion(DocExpansion.List);
            });

            #endregion

            //将TokenAuth注册中间件
            app.UseMiddleware<JwtTokenAuth>();

            app.UseMvc();
            app.UseStaticFiles();
            app.UseCookiePolicy();
        }
    }
}
