using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Entity;
using NServiceDiscoveryAPI.Messages;
using NServiceDiscovery.Repository;
using System.Collections.Generic;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ApplicationKeyValueStoreController : TenantController
    {
        [HttpPost]
        [Route("/configuration/store/apps/{appName}/{key}/{value}")]
        public ActionResult<string> AddApplicationKeyValue([FromRoute] string appName, [FromRoute] string key, [FromRoute] string value)
        {
            string tenantId = this.GetTenantIdFromRouteData();
            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);

            var keyValue = new StoreKeyValue()
            {
                TenantId = tenantId,
                AppName = appName,
                Key = key,
                Value = value
            };

            var res = repo.AddForApplication(appName, keyValue);

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

        [HttpPut]
        [Route("/configuration/store/apps/{appName}/{key}/{value}")]
        public ActionResult<string> UpdateApplicationKeyValue([FromRoute] string appName, [FromRoute] string key, [FromRoute] string value)
        {
            string tenantId = this.GetTenantIdFromRouteData();

            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(tenantId);

            var keyValue = new StoreKeyValue()
            {
                TenantId = tenantId,
                AppName = appName,
                Key = key,
                Value = value
            };

            var res = repo.UpdateForApplication(appName, keyValue);

            if (res)
            {
                this.HttpContext.Response.StatusCode = 200;
            }
            else
            {
                this.HttpContext.Response.StatusCode = 500;
            }

            return string.Empty;
        }

        [HttpDelete]
        [Route("/configuration/store/apps/{appName}/{key}")]
        public ActionResult<string> DeleteApplicationKeyValue([FromRoute] string appName, [FromRoute] string key)
        {
            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(this.GetTenantIdFromRouteData());

            var res = repo.DeleteForApplication(appName, key);

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
        [Route("/configuration/store/apps/{appName}/{key}")]
        public ActionResult<StoreKeyValue> GetApplicationKeyValue([FromRoute] string appName, [FromRoute] string key)
        {
            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(this.GetTenantIdFromRouteData());

            var res = repo.GetForApplication(appName, key);

            if (res == null)
            {
                this.HttpContext.Response.StatusCode = 404;
            }
            
            return res;
        }

        [HttpGet]
        [Route("/configuration/store/apps/{appName}")]
        public ActionResult<List<StoreKeyValue>> GetApplicationKeys([FromRoute] string appName)
        {
            MemoryConfigurationStoreRepository repo = new MemoryConfigurationStoreRepository(this.GetTenantIdFromRouteData());
            return repo.GetAllForApplication(appName);
        }
    }
}
