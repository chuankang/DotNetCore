using System;
using AutoMapper;
using DotNetCore.Interface;
using DotNetCore.Models;
using DotNetCore.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CSRedis;
using DotNetCore.Models.Basic;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.Mvc.Controllers
{
    public class RedisCoverController : Controller
    {
        private readonly IUserService _userService;

        public RedisCoverController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult RedisTest(string name)
        {
            ////普通模式
            //var csredis = new CSRedis.CSRedisClient
            //    ("127.0.0.1:6379,defaultDatabase=0,password=,poolsize=50,ssl=false,writeBuffer=10240,prefix=keyTest_");
            
            ////初始化RedisHelper
            //RedisHelper.Initialization(csredis);

            var result = new CaptchaResult
            {
                CaptchaCode = "123",
                CaptchaByteData = new byte[2]
            };
            //设置值，默认永不过期
            RedisHelper.Set("result", result);
            //异步操作
            //RedisHelper.SetAsync("async name", "ck async");

            //如果确定一定以及肯定非要有切换数据库的需求
            //var connectionString = "127.0.0.1:6379,password=,poolsize=10,ssl=false,writeBuffer=10240,prefix=key前辍";
            //var redis = new CSRedisClient[10];
            //for (int i = 0; i < redis.Length; i++)
            //{
            //    redis[i] = new CSRedisClient(connectionString+ ",defualtDatabase=" + i);
            //}

            //普通订阅
            RedisHelper.Subscribe(
                ("chan1", msg => Console.WriteLine(msg.Body)),
                ("chan2", msg => Console.WriteLine(msg.Body)));


            //发布
            RedisHelper.Publish("chan1", "123123123");

            //使用管道模式，打包多条命令一起执行，从而提高性能。
            var ret1 = RedisHelper.StartPipe().Set("a", "1").Get("a").EndPipe();
            var ret2 = RedisHelper.StartPipe(p => p.Set("a", "1").Get("a"));
            var ret3 = RedisHelper.StartPipe().Get("b").Get("a").Get("a").EndPipe();
            var get = RedisHelper.Get<CaptchaResult>("name");

            return null;
        }
    }
}
