using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Entity;
using NServiceDiscoveryAPI.Messages;
using NServiceDiscovery.Repository;
using NServiceDiscovery.RuntimeInMemory;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ServicesController : TenantController
    {
        [HttpGet]
        [Route("/eureka/apps")]        
        public ActionResult<ServicesRuntime> GetAllApps()
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData());
            return repo.GetAll();
        }

        [HttpGet]
        [Route("/eureka/apps/{appName}")]
        public ActionResult<Application> GetApp([FromRoute] string appName)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData());
            return repo.GetByAppName(appName);
        }

        [HttpPost]
        [Route("/eureka/apps/{appName}")]
        public ActionResult<string> AddAppInstance([FromRoute] string appName, [FromBody] ServiceInstaceRegisterRequest request)
        {
            request.instance.AppName = appName;
            request.instance.InstanceId = request.instance.HostName;

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData());
            Instance instance = repo.Add(request.instance);

            this.HttpContext.Response.StatusCode = 204;

            return string.Empty;
        }

        [HttpDelete]
        [Route("/eureka/apps/{appName}/{instanceID}")]
        public ActionResult<string> DeleteInstance([FromRoute] string appName, [FromRoute] string instanceID)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData());

            var result = repo.Delete(appName, instanceID);

            if (result)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 500;
            }

            return string.Empty;
        }

        [HttpPut]
        [Route("/eureka/apps/{appName}/{instanceID}/status")]
        // "/eureka/apps/{appName}/{instanceID}/status?value={status}"
        public ActionResult<string> ChangeInstanceStatus([FromRoute] string appName, [FromRoute] string instanceID, [FromQuery] string value)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData());

            var result = repo.ChangeStatus(appName, instanceID, value);

            if (result)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 500;
            }

            return string.Empty;
        }

        [HttpPut]
        [Route("/eureka/apps/{appName}/{instanceID}")]
        public ActionResult<string> ReceiveInstanceHeartbeat([FromRoute] string appName, [FromRoute] string instanceID)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData());

            var result = repo.SaveInstanceHearbeat(appName, instanceID);

            if (result)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return string.Empty;
        }
    }
}
