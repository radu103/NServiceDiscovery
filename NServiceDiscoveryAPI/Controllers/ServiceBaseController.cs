using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Entity;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscoveryAPI.Services;
using System.Collections.Generic;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ServiceBaseController : ControllerBase
    {
        [HttpGet]
        [Route("/favicon.ico")]
        public ActionResult<string> GetFavIcon()
        {
            return string.Empty;
        }

        [HttpGet]
        [Route("/health")]
        public ActionResult<string> GetHealth()
        {
            return "OK";
        }

        [HttpGet]
        [Route("/info")]
        public ActionResult<List<DiscoveryPeer>> GetInfo()
        {
            return Memory.Peers;
        }

        [HttpGet]
        [Route("/status")]
        public ActionResult<InstanceStatus> GetStatus([FromServices] IInstanceStatusService statusService)
        {
            return statusService.GetStatus();
        }
    }
}
