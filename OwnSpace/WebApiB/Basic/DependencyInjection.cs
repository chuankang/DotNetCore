using DotNetCore.Interface;
using DotNetCore.Repositories;
using DotNetCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiB.Basic
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    public class DependencyInjection
    {
        public static void Initialize(IServiceCollection services)
        {
            //Repositories
            services.AddSingleton<IUserRepository, UserRepository>();

            //Business
            services.AddSingleton<IUserService, UserService>();
        }
    }
}
