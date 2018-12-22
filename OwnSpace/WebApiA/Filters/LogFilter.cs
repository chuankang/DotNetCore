using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebApiA.Filters
{
    public class LogFilter: ActionFilterAttribute
    {
        private ILog _log = LogManager.GetLogger(Startup.repository.Name, typeof(LogFilter));

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var x = context.HttpContext.Connection.RemoteIpAddress.ToString();
          
            _log.Info($"请求IP地址：{x}");
        }
    }
}
