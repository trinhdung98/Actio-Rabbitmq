using Microsoft.AspNetCore.Mvc;

namespace Actio.ApiGateway.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Api Gateway");
        }
    }
}