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

        [HttpGet]
        [Route("/test/load/balancer")]
        public async Task<ActionResult<string>> AutoTestWithLoadBalancerViaHttp([FromServices] IAutoTestWithLoadBalanceService autoTestLoadBalancedService)
        {
            var result = await autoTestLoadBalancedService.ServiceCallAsync();
            return result;
        }
    }
}