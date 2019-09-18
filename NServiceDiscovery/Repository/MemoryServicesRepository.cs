using System.Collections.Generic;
using NServiceDiscovery.Entity;
using System.Linq;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscovery.Configuration;
using System;

namespace NServiceDiscovery.Repository
{
    public class MemoryServicesRepository
    {
        private string repoTenantId = DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;

        public MemoryServicesRepository(string tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                repoTenantId = tenantId;
            }
        }

        public ServicesRuntime GetAll()
        {
            return Memory.Runtime;
        }

        public Instance GetByInstanceId(string instanceId)
        {
            Instance instance = null;

            foreach (var app in ServicesRuntime.Applications)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);
                if(instance != null) { break; }
            }

            return instance;
        }

        public Application GetByAppName(string appName)
        {
            var list = new List<Instance>();

            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            return app;
        }

        public bool ChangeStatus(string appName, string instanceId, string status, long lastDirtyTimestamp)
        {
            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                for(var i = 0; i < app.Instances.Count; i++)
                {
                    if(app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        app.Instances[i].Status = status;
                        app.Instances[i].LeaseInfo.LastRenewalTimestamp = DateTime.Now.Ticks;
                        app.Instances[i].LastDirtyTimestamp = lastDirtyTimestamp;
                        app.Instances[i].LastUpdatedTimestamp = DateTime.Now.Ticks;

                        if (status.CompareTo("UP") == 0)
                        {
                            app.Instances[i].LeaseInfo.ServiceUpTimestamp = DateTime.Now.Ticks;
                        }

                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public bool Delete(string appName, string instanceId)
        {
            Instance instance = null;

            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if(app != null)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);

                if (instance != null) {
                    app.Instances.Remove(instance);
                    return true;
                }
            }

            return false;
        }

        public Instance Add(Instance instance)
        {
            var appId = instance.AppName;

            var appFound = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appId) == 0);
            if(appFound == null)
            {
                appFound = new Application()
                {
                    Name = appId,
                    Protocol = ApplicationProtocol.HTTP,
                    Instances = new List<Instance>(),
                    TenantId = repoTenantId
                };

                ServicesRuntime.Applications.Add(appFound);
            }

            instance.TenantId = repoTenantId;

            instance.LastDirtyTimestamp = instance.LastUpdatedTimestamp = DateTime.Now.Ticks;

            instance.LeaseInfo.RegistrationTimestamp = DateTime.Now.Ticks;
            instance.LeaseInfo.LastRenewalTimestamp = instance.LeaseInfo.RegistrationTimestamp;
            instance.LeaseInfo.EvictionTimestamp = instance.LeaseInfo.LastRenewalTimestamp + DefaultConfigurationData.DefaultEvictionInSecs * DefaultConfigurationData.TicksPerSecond;
            
            appFound.Instances.Add(instance);

            return instance;
        }

        public bool SaveInstanceHearbeat(string appName, string instanceId, string status, long lastDirtyTimestamp)
        {
            var app = ServicesRuntime.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                for (var i = 0; i < app.Instances.Count; i++)
                {
                    if (app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        app.Instances[i].Status = status;
                        app.Instances[i].LastDirtyTimestamp = app.Instances[i].LastUpdatedTimestamp = lastDirtyTimestamp;
                        app.Instances[i].LeaseInfo.LastRenewalTimestamp = DateTime.Now.Ticks;
                        app.Instances[i].LeaseInfo.EvictionTimestamp = app.Instances[i].LeaseInfo.LastRenewalTimestamp + DefaultConfigurationData.DefaultEvictionInSecs * DefaultConfigurationData.TicksPerSecond;
                        break;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
