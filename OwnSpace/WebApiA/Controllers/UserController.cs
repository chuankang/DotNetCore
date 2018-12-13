using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.Interface;
using DotNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApiA.Controllers
{
    [SwaggerTag("用户")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(Policy = "Admin")]//权限验证
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public string GetSex(string name)
        {
            return name == "ck" ? "Man" : "默认";
        }

        [HttpGet]
        public int? GetAge(string name)
        {
            return name == "ck" ? 24 : 0;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetUserList()
        {
            var userList = await _userService.GetUserListAsync();

            var userViewModels = Mapper.Map<List<User>, List<UserViewModel>>(userList);

            return Json(userViewModels);
        }

        [HttpGet]
        public JsonResult GetAddressByName(string name)
        {
            string address = _userService.GetAddressByName(name);

            return Json(address);
        }
    }
}
