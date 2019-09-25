using Microsoft.AspNetCore.Mvc.Filters;
using NServiceDiscovery.Configuration;
using System.Linq;

namespace NServiceDiscoveryAPI.GlobalFilters
{
    public class CopyTenantIdFromBearerTokentoRouteDataFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var fullTenantId = string.Empty;

            var headers = context.HttpContext.Request.Headers;
            var bearerToken = headers.SingleOrDefault(h => h.Key.CompareTo("Authorization") == 0 && h.Value.ToString().IndexOf("Bearer ") >= 0);

            if (!string.IsNullOrEmpty(bearerToken.Key) && !string.IsNullOrEmpty(bearerToken.Value))
            {
                var token = bearerToken.Value.ToString().Replace("Bearer ", string.Empty);
                fullTenantId = token.Split(":")[0];
            }
            else
            {
                fullTenantId = Program.Tenants[0].TenantId + "-" + Program.Tenants[0].TenantType;
            }

            context.RouteData.Values.Add("TenantId", fullTenantId);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
