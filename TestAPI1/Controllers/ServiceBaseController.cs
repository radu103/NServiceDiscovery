using Microsoft.AspNetCore.Mvc;

namespace TestAPI1.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
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
        public ActionResult<string> GetInfo()
        {
            return "INFO";
        }

    }
}
