using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceDiscovery.Util;
using NServiceDiscoveryAPI.GlobalFilters;
using NServiceDiscoveryAPI.Services;
using System.Linq;
using System.Net;

namespace NServiceDiscoveryAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.Filters.Add(new CopyTenantIdFromBearerTokentoRouteDataFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IMQTTService, MQTTService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            StartupOps(app);
        }

        private void StartupOps(IApplicationBuilder app)
        {
            // set Instance endpoints
            var ips = NetworkUtil.GetIPAddresses();
            FindInstanceUrl(ips);

            // instantiate the MQTTService singleton instance
            var serviceProvider = app.ApplicationServices;
            Program.mqttService = serviceProvider.GetService<IMQTTService>();
        }

        private void FindInstanceUrl(IPAddress[] ips)
        {
            var ipv4IPs = ips.Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.ToString().CompareTo("127.0.0.1") != 0).ToList();

            foreach (var ip in ipv4IPs)
            {
                if (NetworkUtil.HasIPAddress(ip.ToString()))
                {
                    Program.InstanceConfig.HttpEndpoint = "http://" + ip.ToString() + ":8771/eureka/apps";
                    Program.InstanceConfig.SecureHttpEndpoint = "http://" + ip.ToString() + ":8443/eureka/apps";
                }
            }
        }
    }
}
