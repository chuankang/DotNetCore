using AutoMapper;
using DotNetCore.Interface;
using DotNetCore.Models;
using DotNetCore.Models.Basic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiA.Filters;

namespace WebApiA.Controllers
{
    [SwaggerTag("用户API")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[Authorize(Policy = "Admin")]//权限验证
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        [HttpPost]
        [LogFilter]
        public JsonResult AddUser([FromBody] UserViewModel request)
        {
            ResultEntity retEntity = new ResultEntity
            {
                Code = 0,
                Message = $@"用户名:{request.UserName},出生日期:{request.Birthday}。数据保存成功"
            };

            return Json(retEntity);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [Authorize(Policy = "Admin")]
        [HttpDelete]
        [LogFilter]
        public JsonResult DeleteUser(int id)
        {
            ResultEntity retEntity = new ResultEntity
            {
                Code = 0,
                Message = $@"用户ID:{id}。删除成功"
            };

            return Json(retEntity);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        [HttpPut]
        [LogFilter]
        public JsonResult UpdateUser([FromBody] UserViewModel request)
        {
            ResultEntity retEntity = new ResultEntity
            {
                Code = 0,
                Message = $@"用户ID:{request.Id}。修改成功"
            };

            return Json(retEntity);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [HttpGet]
        [LogFilter]
        public JsonResult GetUsers()
        {
            List<UserViewModel> users = new List<UserViewModel>
            {
                new UserViewModel{ Id=1,UserName="James",Birthday="1991-01-01"},
                new UserViewModel{ Id=2,UserName="Londo",Birthday="1992-07-08"},
                new UserViewModel{ Id=3,UserName="Allen",Birthday="1993-05-11"}
            };

            ResultEntity resultEntity = new ResultEntity<List<UserViewModel>>
            {
                Code = 0,
                Message = "请求成功",
                Data = users
            };

            return Json(resultEntity);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [HttpGet]
        [Obsolete]
        public async Task<JsonResult> GetUserList()
        {
            var userList = await _userService.GetUserListAsync();

            var userViewModels = Mapper.Map<List<User>, List<UserViewModel>>(userList);

            return Json(userViewModels);
        }

        [HttpGet]
        [Obsolete]
        public JsonResult GetAddressByName(string name)
        {
            string address = _userService.GetAddressByName(name);

            return Json(address);
        }
    }
}
