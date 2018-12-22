using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApiA.AuthHelper;
using WebApiA.Models;

namespace WebApiA.Controllers
{
    [SwaggerTag("登录授权API")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        /// <summary>
        /// 获取JWT
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="role">角色</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token2")]
        public JsonResult GetJwtStr(long id = 1, string role = "")
        {
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            var tokenModel = new TokenJwt
            {
                Uid = id,
                Role = role
            };

            string jwtStr = JwtHelper.IssueJwt(tokenModel);

            return Json(jwtStr);
        }
    }
}