using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace DotNetCore.Mvc.Handler
{
    public class ErrorHandler: IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;
            
            //这里可以写一些日志记录代码

            context.Result = new ContentResult
            {
                Content = $"捕捉到未处理的异常：{ex.GetType()}\nFilter已进行错误处理。",
                //ContentType = "text/plain",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            context.ExceptionHandled = true;//设置异常已经处理
        }
    }
}
