using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceDiscovery.Configuration;
using NServiceDiscovery.MQTT;
using NServiceDiscovery.Persistency;
using NServiceDiscovery.Repository;
using NServiceDiscovery.Util;
using NServiceDiscoveryAPI.GlobalFilters;
using NServiceDiscoveryAPI.Services;
using System;
using System.IO.Compression;
using System.Linq;

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

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddSingleton<IMQTTSettings, MQTTSettings>();
            services.AddSingleton<IMQTTService, MQTTService>();
            services.AddSingleton<IMQTTProcessingService, MQTTProcessingService>();

            services.AddSingleton<IPublishChangesService, PublishChangesService>();
            services.AddSingleton<IPublishClientsService, PublishClientsService>();

            services.AddSingleton<IEvictionService, EvictionService>();
            services.AddSingleton<IMemoryTenantsRepository, MemoryTenantsRepository>();
            services.AddSingleton<IPersistencyService, PersistencyService>();

            services.AddSingleton<IMemoryDiscoveryPeerRepository, MemoryDiscoveryPeerRepository>(); 
            services.AddSingleton<IMemoryDiscoveryClientRepository, MemoryDiscoveryClientRepository>();
            services.AddSingleton<IMemoryGeneralConfigurationClientRepository, MemoryGeneralConfigurationClientRepository>();

            services.AddSingleton<IInstanceStatusService, InstanceStatusService>();
            services.AddSingleton<IInstanceHealthService, InstanceHealthService>();

            // persistency sync service
            services.AddSingleton<IPersistencyService, PersistencyService>();

            // persistency repositories
            services.AddSingleton<IMongoDBSettings, MongoDBSettings>();

            services.AddSingleton<IPersistencyDefaultApplicationsRepository, PersistencyDefaultApplicationsRepository>();
            services.AddSingleton<IPersistencyTenantRepository, PersistencyTenantRepository>();

            services.AddSingleton<IPersistencyAppConfigurationsRepository, PersistencyAppConfigurationsRepository>();
            services.AddSingleton<IPersistencyGeneralConfigurationsRepository, PersistencyGeneralConfigurationsRepository>();
            services.AddSingleton<IPersistencyApplicationsRepository, PersistencyApplicationsRepository>();
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

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseMvc();
            
            app.UseStaticFiles(new StaticFileOptions()
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            StartupConfigurationLoadFromEnvironment(app);
        }

        private void StartupConfigurationLoadFromEnvironment(IApplicationBuilder app)
        {
            // instantiate the MQTTService singleton instance
            Program.serviceProvider = app.ApplicationServices;

            Program.persistencyService = Program.serviceProvider.GetService<IPersistencyService>();

            // get mongo db configuration from VCAP_SERVICES
            #region get_mongodb_configuration

                Program.mongoDbSettings = Program.serviceProvider.GetService<IMongoDBSettings>();

                if (Program.cloudFoundryVcapServices != null && Program.cloudFoundryVcapServices.MongoDBs.Count > 0)
                {
                    // get mongo db configuration from CF environment VCAP_SERVICES
                    var mongoService = Program.cloudFoundryVcapServices.MongoDBs.FirstOrDefault();

                    Program.mongoDbSettings.MongoDbUrl = mongoService.Credentials.URI;
                    Program.mongoDbSettings.User = mongoService.Credentials.Username;
                    Program.mongoDbSettings.Password = mongoService.Credentials.Password;
                    Program.mongoDbSettings.HostName = mongoService.InstanceName;
                    Program.mongoDbSettings.Port = Convert.ToInt32(mongoService.Credentials.Port);
                    Program.mongoDbSettings.DbName = mongoService.Credentials.DbName;
                }
                else
                {
                    // default devel mongo settings
                    Program.mongoDbSettings.MongoDbUrl = "mongodb://admin:admin2019@ds235711.mlab.com:35711/nservicediscovery";
                    Program.mongoDbSettings.User = "admin";
                    Program.mongoDbSettings.Password = "admin2019";
                    Program.mongoDbSettings.HostName = "ds235711.mlab.com";
                    Program.mongoDbSettings.Port = 35711;
                    Program.mongoDbSettings.DbName = "nservicediscovery";
                }

            #endregion

            // get mqtt configuration from VCAP_SERVICES
            #region get_mqtt_configuration

                Program.mqttSettings = Program.serviceProvider.GetService<IMQTTSettings>();

                var topicTemplate = CloudFoundryEnvironmentUtil.GetMQTTTopicTemplateFromEnv();

                if (!string.IsNullOrEmpty(topicTemplate))
                {
                    Program.mqttSettings.TopicTemplate = topicTemplate;
                }
                else
                {
                    Program.mqttSettings.TopicTemplate = DefaultConfigurationData.DefaultMQTTTopicTemplate;
                }

                var interval = CloudFoundryEnvironmentUtil.GetMQTTReconnectIntervalInSecondsFromEnv();

                if (interval > 0)
                {
                    Program.mqttSettings.ReconnectSeconds = interval;
                }
                else
                {
                    Program.mqttSettings.ReconnectSeconds = DefaultConfigurationData.DefaultMQTTReconnectSeconds;
                }

                if (Program.cloudFoundryVcapServices != null && Program.cloudFoundryVcapServices.MQTTBrokers.Count > 0)
                {
                    var mqttService = Program.cloudFoundryVcapServices.MQTTBrokers.FirstOrDefault();

                    Program.mqttSettings.Host = mqttService.Credentials.URI;
                    Program.mqttSettings.User = mqttService.Credentials.Username;
                    Program.mqttSettings.Password = mqttService.Credentials.Password;
                    Program.mqttSettings.Port = Convert.ToInt32(mqttService.Credentials.Port);
                }
                else
                {
                    Program.mqttSettings.Host = DefaultConfigurationData.DefaultMQTTHost;
                    Program.mqttSettings.User = DefaultConfigurationData.DefaultMQTTUsername;
                    Program.mqttSettings.Password = DefaultConfigurationData.DefaultMQTTPassword;
                    Program.mqttSettings.Port = DefaultConfigurationData.DefaultMQTTPort;
                }

            #endregion

            // load tenants from persistency

            #region single_tenant_or_not

                var SINGLE_TENANT_ID = CloudFoundryEnvironmentUtil.GetTenantIdFromEnv();
                var SINGLE_TENANT_TYPE = CloudFoundryEnvironmentUtil.GetTenantTypeFromEnv();

                if (string.IsNullOrEmpty(SINGLE_TENANT_ID) && string.IsNullOrEmpty(SINGLE_TENANT_TYPE))
                {
                    var tenantsRepo = Program.serviceProvider.GetService<IMemoryTenantsRepository>();
                    var persistencyTenantsRepo = Program.serviceProvider.GetService<IPersistencyTenantRepository>();
                
                    var persistencyTenants = persistencyTenantsRepo.LoadPersistedTenants();

                    foreach(var t in persistencyTenants)
                    {
                        tenantsRepo.Add(new NServiceDiscovery.Entity.Tenant()
                        {
                            TenantId = t.TenantId,
                            TenantToken = t.TenantToken
                        });
                    }

                    Program.Tenants = tenantsRepo.GetAll();
                }

            #endregion

            // start persistency sync timer with random interval
            var random = new Random();
            Program.persistencyService.SetPersistencyTimerInterval(random.Next(Program.InstanceConfig.PersistencySyncMinRandomSeconds, Program.InstanceConfig.PersistencySyncMaxRandomSeconds));

            Program.mqttService = Program.serviceProvider.GetService<IMQTTService>();

            Program.evictionService = Program.serviceProvider.GetService<IEvictionService>();
        }
    }
}
