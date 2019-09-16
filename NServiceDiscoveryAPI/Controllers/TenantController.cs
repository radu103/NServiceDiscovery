using Microsoft.AspNetCore.Mvc;
using NServiceDiscovery.Configuration;

namespace NServiceDiscoveryAPI.Controllers
{
    public class TenantController : ControllerBase
    {
        internal string GetTenantIdFromRouteData()
        {
            var tenantId = DefaultConfigurationData.DefaultTenantID;

            if (this.RouteData.Values.Keys.Count > 0 && this.RouteData.Values["TenantId"] != null) {
                tenantId = this.RouteData.Values["TenantId"].ToString();
            }

            return tenantId;
        }
    }
}