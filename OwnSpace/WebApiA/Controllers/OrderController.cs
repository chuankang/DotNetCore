using DotNetCore.Models;
using DotNetCore.Models.Basic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiA.Filters;

namespace WebApiA.Controllers
{
    [SwaggerTag("订单API")]
    [Route("api/[controller]/[action]")]
    public class OrderController : Controller
    {
        [HttpGet]
        [LogFilter]
        public JsonResult GetOrderInfo(string orderId = "123")
        {
            if (orderId != "123")
            {
                return Json(new ResultEntity { Code = 1, Message = "无此订单信息" });
            }

            OrderViewModel users = new OrderViewModel
            {
                OrderId = orderId,
                Status = "配送中...",
                PayType = "1",
                OrderSource = "京东",
                TotalMoney = "￥100.00"
            };
            ResultEntity resultEntity = new ResultEntity<OrderViewModel>
            {
                Code = 0,
                Message = "请求成功",
                Data = users
            };

            return Json(resultEntity);
        }
    }
}
