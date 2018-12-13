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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;

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

            var csredis = new CSRedis.CSRedisClient("127.0.0.1:6379,password=,defaultDatabase=2,poolsize=10,ssl=false,writeBuffer=10240,prefix=keyTest_");
            RedisHelper.Initialization(csredis);

            //注册MVC分布式缓存
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));

            //微信
            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env
            , IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
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


            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value)
                .UseSenparcGlobal(false, null);

            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);


        }
    }
}
