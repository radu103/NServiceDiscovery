using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Common.ServiceBase;

namespace TestAPI1.Controllers
{
    [ApiController]
    public class ServiceBaseController : ControllerBase
    {
        private ServiceHealth _health = new ServiceHealth();

        [HttpGet]
        [Route("/favicon.ico")]
        public ActionResult<string> GetFavIcon()
        {
            return string.Empty;
        }

        [HttpGet]
        [Route("/health")]
        public ActionResult<ServiceHealth> GetHealth()
        {
            return _health;
        }

        [HttpGet]
        [Route("/info")]
        public ActionResult<string> GetInfo()
        {
            return "INFO";
        }

        [HttpGet]
        [Route("/status")]
        public ActionResult<string> GetStatus()
        {
            return "STATUS";
        }
    }
}
