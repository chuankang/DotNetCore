using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OcelotGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();

            //注入WebHostBuilder
          var x = builder.ConfigureServices(service =>
                {
                    service.AddSingleton(builder);
                })

                //加载configuration配置文件
                .ConfigureAppConfiguration(conbuilder =>
                {
                    conbuilder.AddJsonFile("configuration.json");
                })
                .UseKestrel()
                .UseUrls("http://*:5000")
                .UseStartup<Startup>()
                .Build();
            return x;
        }
    }
}