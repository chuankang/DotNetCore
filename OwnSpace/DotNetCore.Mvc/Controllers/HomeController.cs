using System;
using AutoMapper;
using DotNetCore.Interface;
using DotNetCore.Models;
using DotNetCore.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DotNetCore.Models.Basic;

namespace DotNetCore.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<JsonResult> GetUserList()
        {
            var userList = await _userService.GetUserListAsync();

            var userViewModels = Mapper.Map<List<User>, List<UserViewModel>>(userList);

            return Json(userViewModels);
        }

        [HttpGet]
        public JsonResult GetTest(int id)
        {
            var list = new List<UserViewModel>
            {
                new UserViewModel {UserName = "张三", Birthday = "2018-10-10"},
                new UserViewModel {UserName = "李四", Birthday = "2018-10-11"}
            };

            var ret = new ResultEntity<List<UserViewModel>>
            {
                Code = 0,
                Data = list
            };

            return Json(ret);
        }
    }
}
