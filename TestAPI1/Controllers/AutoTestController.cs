using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestAPI1.Services;

namespace TestAPI1.Controllers
{
    [ApiController]
    public class AutoTestController : ControllerBase
    {
        [HttpGet]
        [Route("/test")]
        public async Task<ActionResult<string>> AutoTestViaHttp([FromServices] IAutoTestService autoTestService)
        {
            var result = await autoTestService.ServiceCallAsync();
            return result;
        }
    }
}