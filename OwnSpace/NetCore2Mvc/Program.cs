using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NetCore2Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseUrls("http://*:5000")//.Net Core 默认创建的项目部署完成以后，只能在本机内访问，外部通过IP是打不开的，可以通过配置Nginx实现。也可以通过修改Program.cs
                .UseStartup<Startup>()
                .Build();
    }
}
