﻿using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Common.ServiceBase;
using NServiceDiscovery.Entity;
using NServiceDiscovery.Repository;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscoveryAPI.Services;
using System.Collections.Generic;

namespace NServiceDiscoveryAPI.Controllers
{
    [ApiController]
    public class ServiceBaseController : ControllerBase
    {
        [HttpGet]
        [Route("/favicon.ico")]
        public ActionResult<string> GetFavIcon()
        {
            return string.Empty;
        }

        [HttpGet]
        [Route("/")]
        public ActionResult<string> GetIndex()
        {
            return LocalRedirectPermanent("/index.html");
        }

        [HttpGet]
        [Route("/health")]
        public ActionResult<ServiceHealth> GetHealth([FromServices] IInstanceHealthService instanceHealthService)
        {
            var health = instanceHealthService.GetHealth();
            return health;
        }

        [HttpGet]
        [Route("/tenants")]
        public ActionResult<List<Tenant>> GetTenants([FromServices] IMemoryTenantsRepository tenantsRepository)
        {
            var tenants = tenantsRepository.GetAll();
            return tenants;
        }

        [HttpGet]
        [Route("/info")]
        public ActionResult<string> GetInfo()
        {
            return "INFO";
        }

        [HttpGet]
        [Route("/peers")]
        public ActionResult<List<DiscoveryPeer>> GetPeers()
        {
            return Memory.Peers;
        }

        [HttpGet]
        [Route("/clients/discovery")]
        public ActionResult<List<DiscoveryClient>> GetDiscoveryClients()
        {
            return Memory.DiscoveryClients;
        }

        [HttpGet]
        [Route("/clients/configuration")]
        public ActionResult<List<DiscoveryClient>> GetConfigurationClients()
        {
            return Memory.ConfigurationClients;
        }

        [HttpGet]
        [Route("/status")]
        public ActionResult<InstanceStatus> GetStatus([FromServices] IInstanceStatusService statusService)
        {
            return statusService.GetStatus();
        }
    }
}
