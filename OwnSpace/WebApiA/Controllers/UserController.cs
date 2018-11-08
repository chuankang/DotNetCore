using Microsoft.AspNetCore.Mvc;

namespace WebApiA.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpGet]
        public string GetSex(string name)
        {
            if (name == "ck")
                return "Man";
            return "默认";
        }

        [HttpGet]
        public int? GetAge(string name)
        {
            if (name == "ck")
                return 24;
            return 0;
        }
    }
}
