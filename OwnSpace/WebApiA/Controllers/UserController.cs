using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.Interface;
using DotNetCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
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
            if (name == "ck")
                return "Man";
            return "默认";
        }

        [HttpGet]
        public int? GetAge(string name)
        {
            if (name == "ck")
                return 24;
            return 0;
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
            var address = _userService.GetAddressByName(name);

            return Json(address);
        }
    }
}
