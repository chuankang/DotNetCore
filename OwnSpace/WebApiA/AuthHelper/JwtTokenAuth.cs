using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiA.Models;

namespace WebApiA.AuthHelper
{
    /// <summary>
    /// 中间件
    /// </summary>
    public class JwtTokenAuth
    {
        private readonly RequestDelegate _next;

        public JwtTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            //检测是否包含'Authorization'请求头
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            string tokenHeader = httpContext.Request.Headers["Authorization"].ToString();
            //序列化token，获取授权
            TokenJwt tokenJwt = JwtHelper.SerializeJwt(tokenHeader);

            //授权 注意这个可以添加多个角色声明，请注意这是一个 list
            var claimList = new List<Claim>();
            var claim = new Claim(ClaimTypes.Role, tokenJwt.Role);
            claimList.Add(claim);
            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            return _next(httpContext);
        }
    }
}
