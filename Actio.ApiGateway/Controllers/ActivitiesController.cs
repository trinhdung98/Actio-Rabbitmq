using System.Threading.Tasks;
using Actio.ApiGateway.Messages.Commands.Activities;
using Microsoft.AspNetCore.Mvc;

namespace Actio.ApiGateway.Controllers
{
    [Route("[controller]")]
    public class ActivitiesController : Controller
    {
        public ActivitiesController()
        {
        }

        public IActionResult Post([FromBody] CreateActivity command)
        {
            System.Console.WriteLine($"Post activity: {command.Name}");
            return Ok();
        }
    }
}