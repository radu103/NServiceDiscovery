using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Messages;
using NServiceDiscovery.Repository;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private MemoryServicesRepository repo = new MemoryServicesRepository(Program.config.TenantID);

        [HttpGet]
        [Route("/eureka/apps")]
        public ActionResult<ServicesRuntime> GetAllInstances()
        {
            return repo.GetAll();
        }

        [HttpPost]
        [Route("/eureka/apps/{appName}")]
        public ActionResult<ServiceInstance> GetApp([FromQuery] string appName, [FromBody] ServiceInstaceRegisterRequest request)
        {
            request.instance.AppName = appName;
            request.instance.InstanceId = request.instance.HostName;

            return repo.Add(request.instance);
        }

        [HttpDelete]
        [Route("/eureka/v2/apps/{appName}/{instanceID}")]
        public ActionResult<bool> DeleteInstance([FromQuery] string appName, [FromQuery] string instanceID)
        {
            return repo.Delete(appName, instanceID);
        }

        [HttpPut]
        [Route("/eureka/v2/apps/{appName}/{instanceID}/status?value={status}")]
        public ActionResult<bool> ChangeStatus([FromQuery] string appName, [FromQuery] string instanceID, [FromQuery] string status)
        {
            return repo.ChangeStatus(appName, instanceID, status);
        }
    }
}
