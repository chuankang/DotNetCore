using System;
using AutoMapper;
using DotNetCore.Interface;
using DotNetCore.Models;
using DotNetCore.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetCore.Common;
using DotNetCore.Models.Basic;
using Microsoft.AspNetCore.Http;
using DotNetCore.Models.Team;

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

        /// <summary>
        /// 返回验证码图片
        /// </summary>
        /// <returns></returns>
        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;

            int height = 36;

            var captchaCode = Captcha.GenerateCaptchaCode();

            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);

            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);

            Stream s = new MemoryStream(result.CaptchaByteData);

            return new FileStreamResult(s, "image/png");
        }

        /// <summary>
        /// 验证输入的验证码是否和session中的一致
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult Validate(CaptchaResult request)
        {
            if (ModelState.IsValid)
            {
                // Validate Captcha Code
                if (!Captcha.ValidateCaptchaCode(request.CaptchaCode, HttpContext))
                {
                    // return error
                }

                // continue business logic
            }

            return null;
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        [HttpGet]
        public JsonResult BulkInsert()
        {
            var teamList = new List<Team>();

            for (var i = 0; i < 5; i++)
            {
                var team = new Team
                {
                    Address = "shanghai" + i,
                    CreatedTime = "2018-11-21".ToDateTime(),
                    DeleteState = 1,
                    Name = "james" + i
                };
                teamList.Add(team);
            }
            _userService.InsertTeam(teamList);

            return Json(null);
        }

        [HttpGet]
        public async Task<JsonResult> Test()
        {
            var url = "https://www.baidu.com";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(url));
                string result = response.Content.ReadAsStringAsync().Result;
            }

            return null;
        }
    }
}
