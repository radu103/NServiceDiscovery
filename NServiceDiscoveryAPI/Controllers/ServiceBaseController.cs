using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Common.ServiceBase;
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
        [Route("/")]
        public ActionResult<string> GetIndex()
        {
            return LocalRedirectPermanent("/eureka/apps");
        }

        [HttpGet]
        [Route("/health")]
        public ActionResult<ServiceHealth> GetHealth([FromServices] IInstanceHealthService instanceHealthService)
        {
            instanceHealthService.GetHealth();
            return instanceHealthService.GetHealth();
        }

        [HttpGet]
        [Route("/info")]
        public ActionResult<string> GetInfo()
        {
            return "INFO";
        }

        [HttpGet]
        [Route("/peers")]
        public ActionResult<List<DiscoveryPeer>> GetPeers()
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
