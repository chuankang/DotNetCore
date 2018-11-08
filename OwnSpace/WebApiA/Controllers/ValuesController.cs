using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1 from webapi A", "value2 from webapi A" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"value {id} from WebApiA";
        }
    }
}
