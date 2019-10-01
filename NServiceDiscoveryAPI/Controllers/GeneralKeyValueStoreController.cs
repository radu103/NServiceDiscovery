using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Repository;
using NServiceDiscoveryAPI.Services;
using System.Collections.Generic;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class GeneralKeyValueStoreController : TenantController
    {
        [HttpPost]
        [Route("/configuration/store/{key}/{value}")]
        public ActionResult<string> AddGeneralKeyValue([FromRoute] string key, [FromRoute] string value, [FromServices] IMemoryGeneralConfigurationClientRepository clientRepo, [FromServices] IPublishClientsService clientPublishService)
        {
            var tenantId = this.GetTenantIdFromRouteData();
            var clientHostname = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            clientRepo.Add(new DiscoveryClient(clientHostname));
            clientPublishService.PublishClientDiscoveryActivity(tenantId, clientHostname);

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);

            var keyValue = new StoreKeyValue()
            {
                TenantId = tenantId,
                Key = key,
                Value = value
            };

            var res = repo.Add(keyValue);

            if (res)
            {
                this.HttpContext.Response.StatusCode = 204;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return string.Empty;
        }

        [HttpPut]
        [Route("/configuration/store/{key}/{value}")]
        public ActionResult<string> UpdateGeneralKeyValue([FromRoute] string key, [FromRoute] string value, [FromServices] IMemoryGeneralConfigurationClientRepository clientRepo, [FromServices] IPublishClientsService clientPublishService)
        {
            var tenantId = this.GetTenantIdFromRouteData();
            var clientHostname = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            clientRepo.Add(new DiscoveryClient(clientHostname));
            clientPublishService.PublishClientDiscoveryActivity(tenantId, clientHostname);

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);

            var keyValue = new StoreKeyValue()
            {
                TenantId = tenantId,
                Key = key,
                Value = value
            };

            var res = repo.Update(keyValue);

            if (res)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return string.Empty;
        }

        [HttpDelete]
        [Route("/configuration/store/{key}")]
        public ActionResult<string> DeleteGeneralKeyValue([FromRoute] string key, [FromServices] IMemoryGeneralConfigurationClientRepository clientRepo, [FromServices] IPublishClientsService clientPublishService)
        {
            var tenantId = this.GetTenantIdFromRouteData();
            var clientHostname = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            clientRepo.Add(new DiscoveryClient(clientHostname));
            clientPublishService.PublishClientDiscoveryActivity(tenantId, clientHostname);

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);
            var res = repo.Delete(key);

            if (res)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 404;
            }

            return string.Empty;
        }

        [HttpGet]
        [Route("/configuration/store/{key}")]
        public ActionResult<StoreKeyValue> GetGeneralKeyValue([FromRoute] string key, [FromServices] IMemoryGeneralConfigurationClientRepository clientRepo, [FromServices] IPublishClientsService clientPublishService)
        {
            var tenantId = this.GetTenantIdFromRouteData();
            var clientHostname = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            clientRepo.Add(new DiscoveryClient(clientHostname));
            clientPublishService.PublishClientDiscoveryActivity(tenantId, clientHostname);

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);
            var res = repo.Get(key);

            if (res == null)
            {
                this.HttpContext.Response.StatusCode = 404;
            }
            
            return res;
        }

        [HttpPost]
        [Route("/configuration/store")]
        public ActionResult<string> AddGeneralKeyValues([FromBody] List<StoreKeyValue> keyValues, [FromServices] IMemoryGeneralConfigurationClientRepository clientRepo, [FromServices] IPublishClientsService clientPublishService)
        {
            var tenantId = this.GetTenantIdFromRouteData();
            var clientHostname = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            clientRepo.Add(new DiscoveryClient(clientHostname));
            clientPublishService.PublishClientDiscoveryActivity(tenantId, clientHostname);

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);

            var res = repo.AddKeys(keyValues);

            if (res)
            {
                this.HttpContext.Response.StatusCode = 204;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 500;
            }

            return string.Empty;
        }

        [HttpGet]
        [Route("/configuration/store")]
        public ActionResult<List<StoreKeyValue>> GetGeneralKeys([FromServices] IMemoryGeneralConfigurationClientRepository clientRepo, [FromServices] IPublishClientsService clientPublishService)
        {
            var tenantId = this.GetTenantIdFromRouteData();
            var clientHostname = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            clientRepo.Add(new DiscoveryClient(clientHostname));
            clientPublishService.PublishClientDiscoveryActivity(tenantId, clientHostname);

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);
            return repo.GetAll();
        }
    }
}
