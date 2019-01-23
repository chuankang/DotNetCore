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
using DotNetCore.Common;
using DotNetCore.Models.Basic;
using DotNetCore.Models.Team;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

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
        public JsonResult RedisTest()
        {
            //普通模式
            //参数名           默认值         说明
            //password          <空>        密码
            //defaultDatabase   0           默认数据库
            //poolsize          50          连接池大小
            //preheat           true        预热连接
            //ssl               false       是否开启加密传输
            //writeBuffer       10240       异步方法写入缓冲区大小(字节)
            //tryit             0           执行命令出错，尝试重试的次数
            //name              <空>         连接名称，可以使用 Client List 命令查看
            //prefix            <空>         key前辍，所有方法都会附带此前辍，csredis.Set(prefix + "key", 111);
            //var csredis = new CSRedisClient(@"127.0.0.1:6380,defaultDatabase=0,password=,
            //        poolsize=50,ssl=false,writeBuffer=10240,prefix=fund");

            //////初始化RedisHelper
            //RedisHelper.Initialization(csredis);

            //设置值，默认永不过期
            //设置过期时间，redis-server 2.8+ 才支持
            //RedisHelper.Set("name", name,60);
            //异步操作
            //RedisHelper.SetAsync("async name", "ck async");

            //如果确定一定以及肯定非要有切换数据库的需求
            //var connectionString = "127.0.0.1:6379,password=,poolsize=10,ssl=false,writeBuffer=10240,prefix=key前辍";
            //var redis = new CSRedisClient[10];
            //for (int i = 0; i < redis.Length; i++)
            //{
            //    redis[i] = new CSRedisClient(connectionString+ ",defualtDatabase=" + i);
            //}

            //var cacheName = RedisHelper.Get("name");



            ////发布
            //RedisHelper.Publish("chan1", "1");
            ////发布
            //RedisHelper.Publish("chan1", "2");
            ////发布
            //RedisHelper.Publish("chan1", "3");
            //普通订阅
            var sub = RedisHelper.Subscribe(("chan1", msg => GetName(msg.Body)));
         

            ////使用管道模式，打包多条命令一起执行，从而提高性能。
            //var ret1 = RedisHelper.StartPipe().Set("a", "1").Get("a").EndPipe();
            //var ret2 = RedisHelper.StartPipe(p => p.Set("a", "1").Get("a"));
            //var ret3 = RedisHelper.StartPipe().Get("b").Get("a").Get("a").EndPipe();
            //var get = RedisHelper.Get<CaptchaResult>("name");

            return Json("");
        }

        /// <summary>
        /// csredis测试发布订阅
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult RedisSubAndPub()
        {
            //发布
            RedisHelper.Publish("chan1", "4");
            //发布
            RedisHelper.Publish("chan1", "5");
            //发布
            RedisHelper.Publish("chan1", "6");
            return Json(null);
        }

        private static void GetName(string message)
        {
            var s = message;
        }
    }
}
