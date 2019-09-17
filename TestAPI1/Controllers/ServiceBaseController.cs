using Microsoft.AspNetCore.Mvc;

namespace TestAPI1.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
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
