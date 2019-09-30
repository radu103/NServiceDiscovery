using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Entity;
using NServiceDiscoveryAPI.Messages;
using NServiceDiscovery.Repository;
using NServiceDiscovery.RuntimeInMemory;
using System.Linq;
using System;
using System.Collections.Generic;
using NServiceDiscovery.Util;
using NServiceDiscoveryAPI.Services;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class EurekaServicesController : TenantController
    {
        [HttpGet]
        [Route("/eureka/apps")]        
        public ActionResult<ServicesRuntime> GetAllApps([FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);
            return repo.GetAll();
        }

        [HttpGet]
        [Route("/eureka/apps/delta")]
        public ActionResult<ServicesRuntime> GetAllAppsDelta([FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);
            return repo.GetAll();
        }


        [HttpGet]
        [Route("/eureka/apps/{appName}")]
        public ActionResult<Application> GetApp([FromRoute] string appName, [FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);
            return repo.GetByAppName(appName);
        }

        [HttpPost]
        [Route("/eureka/apps/{appName}")]
        public ActionResult<string> AddAppInstance([FromRoute] string appName, [FromBody] ServiceInstaceRegisterRequest request, [FromServices] IPublishChangesService publishService)
        {
            request.instance.AppName = appName;

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            Instance instance = repo.Add(request.instance);

            publishService.PublishAddedOrUpdatedInstance(instance, "ADD_INSTANCE");

            this.HttpContext.Response.StatusCode = 204;

            return string.Empty;
        }

        [HttpDelete]
        [Route("/eureka/apps/{appName}/{instanceID}")]
        public ActionResult<string> DeleteInstance([FromRoute] string appName, [FromRoute] string instanceID, [FromServices] IPublishChangesService publishService)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            var instance = repo.Delete(appName, instanceID);

            if (instance != null)
            {
                publishService.PublishDeletedInstance(instance.TenantId, appName, instanceID);

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
        // "/eureka/apps/{appName}/{instanceID}/status?value={status}&lastDirtyTimestamp=1568746948343"
        public ActionResult<string> ChangeInstanceStatus([FromRoute] string appName, [FromRoute] string instanceID, [FromQuery] string value, [FromQuery] long lastDirtyTimestamp, [FromServices] IPublishChangesService publishService)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            if(lastDirtyTimestamp == 0)
            {
                lastDirtyTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
            }

            var instance = repo.ChangeStatus(appName, instanceID, value, lastDirtyTimestamp);

            if (instance != null)
            {
                publishService.PublishInstanceStatusChange(instance.TenantId, appName, instanceID, value, lastDirtyTimestamp);

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
        // "/eureka/apps/{appName}/{instanceID}?status=UP&lastDirtyTimestamp=1568804226113"
        public ActionResult<string> ReceiveInstanceHeartbeat([FromRoute] string appName, [FromRoute] string instanceID, [FromQuery] long lastDirtyTimestamp, [FromServices] IPublishChangesService publishService)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            var status = string.Empty;

            if(lastDirtyTimestamp == 0)
            {
                lastDirtyTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
            }
          
            if (Request.QueryString.Value.ToString().IndexOf("status") >= 0)
            {
                // status change request
                var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(Request.QueryString.Value.ToString());
                var items = queryDictionary.SelectMany(x => x.Value, (col, value) => new KeyValuePair<string, string>(col.Key, value)).ToList();

                var statusItem = items.SingleOrDefault(q => q.Key.CompareTo("status") == 0);
                if(statusItem.Key != null)
                {
                    status = statusItem.Value;
                }

                var dirtyItem = items.SingleOrDefault(q => q.Key.CompareTo("lastDirtyTimestamp") == 0);
                if (dirtyItem.Key != null)
                {
                    lastDirtyTimestamp = Convert.ToInt64(dirtyItem.Value);
                }
            }
            
            var instance = repo.SaveInstanceHearbeat(appName, instanceID, status, lastDirtyTimestamp);

            if (instance != null)
            {
                publishService.PublishAddedOrUpdatedInstance(instance, "UPDATE_INSTANCE");

                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return string.Empty;
        }

        [HttpGet]
        [Route("/eureka/vips/{vipAddress}")]
        public ActionResult<List<Instance>> GetInstancesForVipAddress([FromRoute] string vipAddress, [FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            List<Instance> list = repo.GetInstancesForVipAddress(vipAddress);

            if (list.Count > 0)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return list;
        }

        [HttpGet]
        [Route("/eureka/svips/{svipAddress}")]
        public ActionResult<List<Instance>> GetInstancesForSVipAddress([FromRoute] string svipAddress, [FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            List<Instance> list = repo.GetInstancesForSVipAddress(svipAddress);

            if (list.Count > 0)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return list;
        }

        [HttpGet]
        [Route("/eureka/datacenters")]
        public ActionResult<List<string>> GetDataCenters([FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            List<string> list = repo.GetDataCenters();

            return list;
        }

        [HttpGet]
        [Route("/eureka/countries")]
        public ActionResult<List<int>> GeCountries([FromServices] IMemoryDiscoveryClientRepository clientRepo)
        {
            clientRepo.Add(new DiscoveryClient(Request.HttpContext.Connection.RemoteIpAddress.ToString()));

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            List<int> list = repo.GetCountries();

            return list;
        }
    }
}
