using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DemoAAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        /// <summary>
        /// 无参Get请求
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new [] { "api-a", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// 有参Get请求
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "张三";
        }
    }
}
