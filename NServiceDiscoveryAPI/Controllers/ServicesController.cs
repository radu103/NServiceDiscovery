using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Messages;
using NServiceDiscovery.Repository;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private ServiceRepository repo = new ServiceRepository();

        [HttpGet]
        [Route("/eureka/apps")]
        public ActionResult<ServicesRuntime> Get()
        {
            return repo.GetAll();
        }

        [HttpPost]
        [Route("/eureka/apps/{appName}")]
        public ActionResult<ServiceInstance> Get([FromQuery] string appName, [FromBody] ServiceInstaceRegisterRequest request)
        {
            request.instance.AppId = appName;
            request.instance.InstanceId = request.instance.HostName;

            return repo.Add(request.instance);
        }
    }
}
