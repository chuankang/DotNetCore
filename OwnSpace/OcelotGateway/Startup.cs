using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

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
            //注入配置文件，AddOcelot要求参数是IConfigurationRoot类型，所以要作个转换

            services.AddOcelot(Configuration as ConfigurationRoot);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //添加中间件
            app.UseOcelot().Wait();
        }
    }
}