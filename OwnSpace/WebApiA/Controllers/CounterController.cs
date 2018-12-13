using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize(Policy = "Client")]//权限验证
    public class CounterController : Controller
    {
        private static int _count;

        [HttpGet]
        public string Count()
        {
            return $"Count {++_count} from WebapiA";
        }
    }
}