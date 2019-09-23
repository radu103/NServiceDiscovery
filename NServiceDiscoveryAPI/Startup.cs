using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NServiceDiscovery.Entity;
using NServiceDiscoveryAPI.GlobalFilters;
using NServiceDiscoveryAPI.Services;
using System;

namespace NServiceDiscoveryAPI
{
    public class Startup
    {
        private void StartUpEvironmentVars(IConfiguration configuration)
        {
            // Kestrel
            try
            {
                Program.InstanceConfig.Urls = configuration.GetValue<string>("ASPNETCORE_URLS");
            }
            catch(Exception err)
            {
                Program.InstanceConfig.Urls = string.Empty;
                Console.WriteLine("ASPNETCORE_URLS ERROR : " + err.Message);
            }

            // cloudd foundry
            try
            {
                var VCAP_APPLICATION = configuration.GetValue<string>("VCAP_APPLICATION");
                if (VCAP_APPLICATION != null)
                {
                    var vcapApp = JsonConvert.DeserializeObject<CloudFoundryVcapApplication>(VCAP_APPLICATION);
                    Program.InstanceConfig.Urls = vcapApp.ApplicationUrls[0];
                }
            }
            catch (Exception err)
            {
                Program.InstanceConfig.Urls = string.Empty;
                Console.WriteLine("VCAP_APPLICATION" + err.Message);
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StartUpEvironmentVars(configuration);
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
            services.AddSingleton<IInstanceStatusService, InstanceStatusService>();
            services.AddSingleton<IEvictionService, EvictionService>();
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
            // instantiate the MQTTService singleton instance
            var serviceProvider = app.ApplicationServices;
            Program.mqttService = serviceProvider.GetService<IMQTTService>();
            Program.evictionService = serviceProvider.GetService<IEvictionService>();
        }
    }
}
