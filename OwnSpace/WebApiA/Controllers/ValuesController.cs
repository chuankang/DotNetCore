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
        public IEnumerable<string> GetValue()
        {
            return new string[] { "value1 from webapi A", "value2 from webapi A" };
        }

        [HttpGet("{id}")]
        public string GetId(int id)
        {
            return $"value {id} from WebApiA";
        }

        [HttpPost("{id2}")]
        public IEnumerable<string> GetName()
        {
            return new string[] { "NAME A" };
        }
    }
}
