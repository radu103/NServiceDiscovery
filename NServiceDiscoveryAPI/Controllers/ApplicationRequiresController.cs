﻿using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Repository;
using System.Collections.Generic;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ApplicationRequiresController : TenantController
    {
        [HttpPost]
        [Route("/dependencies/apps/{appName}/{dependency}")]
        public ActionResult<string> AddApplicationDepedency([FromRoute] string appName, [FromRoute] string dependency)
        {
            string tenantId = this.GetTenantIdFromRouteData();

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            var res = repo.AddDependencyForApplication(appName, dependency);

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

        [HttpPost]
        [Route("/dependencies/apps/{appName}")]
        public ActionResult<string> AddApplicationDepedencies([FromRoute] string appName, [FromBody] List<string> dependencies)
        {
            string tenantId = this.GetTenantIdFromRouteData();

            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            var res = repo.AddDependenciesForApplication(appName, dependencies);

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

        [HttpDelete]
        [Route("/dependencies/apps/{appName}/{dependency}")]
        public ActionResult<string> DeleteApplicationDependency([FromRoute] string appName, [FromRoute] string dependency)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            var res = repo.DeleteDependencyForApplication(appName, dependency);

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
        [Route("/dependencies/apps/{appName}")]
        public ActionResult<string> DeleteApplicationDependencies([FromRoute] string appName, [FromRoute] List<string> dependencies)
        {
            MemoryServicesRepository repo = new MemoryServicesRepository(this.GetTenantIdFromRouteData(), Program.InstanceConfig.EvictionInSecs);

            var res = repo.DeleteDependenciesForApplication(appName, dependencies);

            if (res > 0)
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
