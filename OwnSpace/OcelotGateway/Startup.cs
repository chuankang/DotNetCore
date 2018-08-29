using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Swashbuckle.AspNetCore.Swagger;

namespace OcelotGateway
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
            ////如果我们需要对下游API进行认证以及鉴权服务的，则首先Ocelot 网关这里需要添加认证服务。这和我们给一个单独的API或者ASP.NET Core Mvc添加认证服务没有什么区别。
            //var authenticationProviderKey = "TestKey";
            //services.AddAuthentication()
            //    .AddJwtBearer(authenticationProviderKey, x =>
            //    {
            //    });
            //注入配置文件，AddOcelot要求参数是IConfigurationRoot类型，所以要作个转换

            services.AddOcelot(Configuration as ConfigurationRoot);


            //swagger
            services.AddMvc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ApiGateway", new Info { Title = "网关服务", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //swagger
            var apis = new List<string> { "DemoAApi", "DemoBApi" };
            app.UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    apis.ForEach(m =>
                    {
                        options.SwaggerEndpoint($"/{m}/swagger.json", m);
                    });
                });

            //添加中间件
            app.UseOcelot().Wait();
        }
    }
}