using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DemoBAPI.Controllers
{
    /// <summary>
    /// 产品API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // GET: api/Products
        /// <summary>
        /// 无参Get
        /// </summary>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] { "product1", "product2" };
        }

        /// <summary>
        /// 产品获得ID
        /// </summary>
        [HttpGet("{GetId}", Name = "Get")]
        public string GetId(int id)
        {
            return "张三的Id";
        }
    }
}
