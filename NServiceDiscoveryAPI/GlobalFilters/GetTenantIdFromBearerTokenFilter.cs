using Microsoft.AspNetCore.Mvc.Filters;
using NServiceDiscovery.Configuration;
using System.Linq;

namespace NServiceDiscoveryAPI.GlobalFilters
{
    public class CopyTenantIdFromBearerTokentoRouteDataFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            var bearerToken = headers.SingleOrDefault(h => h.Key.CompareTo("Authorization") == 0 && h.Value.ToString().IndexOf("Bearer ") >= 0);

            if (!string.IsNullOrEmpty(bearerToken.Key) && !string.IsNullOrEmpty(bearerToken.Value))
            {
                var token = bearerToken.Value.ToString().Replace("Bearer ", string.Empty);
                context.RouteData.Values.Add("TenantId", token);
            }
            else
            {
                context.RouteData.Values.Add("TenantId", DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType);

                //context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                //context.Result = new JsonResult("Forbidden");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
