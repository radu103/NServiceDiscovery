using System.Collections.Generic;
using NServiceDiscovery.Entity;
using System.Linq;
using NServiceDiscovery.RuntimeInMemory;
using NServiceDiscovery.Configuration;
using System;
using NServiceDiscovery.Util;
using Newtonsoft.Json;
using System.Text;

namespace NServiceDiscovery.Repository
{
    public class MemoryServicesRepository
    {
        private string repoTenantId = string.Empty;

        public MemoryServicesRepository()
        {
            repoTenantId = DefaultConfigurationData.DefaultTenantID + "-" + DefaultConfigurationData.DefaultTenantType;
        }

        public MemoryServicesRepository(string tenantId)
        {
            if (!string.IsNullOrEmpty(tenantId))
            {
                repoTenantId = tenantId;
            }
        }

        private Tenant GetTenant()
        {
            var tenant = new Tenant();

            var aux = repoTenantId.Split(new char[] { '-' });

            tenant.TenantId = aux[0];
            tenant.TenantType = aux[1];

            return tenant;
        }

        public AppsSyncInfo GetTenantSyncInfo()
        {
            var info = new AppsSyncInfo();

            Tenant tenant = GetTenant();

            info.TenantId = tenant.TenantId;
            info.TenantType = tenant.TenantType;

            try
            {
                var tenantApps = ServicesRuntime.AllApplications.Applications.FindAll(app => app.TenantId.CompareTo(repoTenantId) == 0).ToList();
                tenantApps = tenantApps.OrderBy(app => app.Name).ToList();

                var AllTenantApps = new AllApplications()
                {
                    Applications = tenantApps,
                    VersionsDelta = ServicesRuntime.AllApplications.VersionsDelta,
                    AppsHashCode = string.Empty
                };

                var appsJson = JsonConvert.SerializeObject(AllTenantApps);
                info.Apps = JsonConvert.DeserializeObject<AllApplications>(appsJson);

                info.Apps.Applications = info.Apps.Applications;

                StringBuilder signature = new StringBuilder();

                foreach (var app in info.Apps.Applications)
                {
                    signature.Append(app.Name + "_");

                    var instances = app.Instances.OrderBy(i => i.InstanceId).ToList();
                    foreach (var inst in instances)
                    {
                        signature.Append(inst.InstanceId + "_" + inst.Status + "_" + inst.OverriddenStatus + "_");
                    }
                }

                var sigStr = signature.ToString();

                if (!string.IsNullOrEmpty(sigStr))
                {
                    info.MD5Hash = sigStr.GetMd5Hash();
                }
                else
                {
                    info.MD5Hash = string.Empty;
                }

                return info;
            }
            catch (Exception err)
            {
                Console.WriteLine("GetTenantSyncInfo ERROR - ", err.Message);
            }

            return null;
        }

        private void UpdateAppsHashCode()
        {
            var hash = string.Empty;

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            var up_instances_count = 0;

            foreach (var app in apps)
            {
                var up_instances = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.Status.CompareTo("UP") == 0).ToList();
                up_instances_count += up_instances.Count;

                // TO DO : update string for "apps__hashcode" for STARTING, DOWN, ERROR
            }

            hash += "UP_" + up_instances_count.ToString() + "_";

            ServicesRuntime.AllApplications.AppsHashCode = hash;
        }

        private void IncreaseVersion()
        {
            ServicesRuntime.AllApplications.VersionsDelta += 1;
            UpdateAppsHashCode();
        }

        public ServicesRuntime GetAll()
        {
            return Memory.Runtime;
        }

        public Instance GetByInstanceId(string instanceId)
        {
            Instance instance = null;

            foreach (var app in ServicesRuntime.AllApplications.Applications)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);
                if (instance != null) { break; }
            }

            return instance;
        }

        public Application GetByAppName(string appName)
        {
            var list = new List<Instance>();

            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            return app;
        }

        public Instance ChangeStatus(string appName, string instanceId, string status, long lastDirtyTimestamp)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            var idx = -1;

            if (app != null)
            {
                for (var i = 0; i < app.Instances.Count; i++)
                {
                    if (app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        idx = i;

                        app.Instances[i].ActionType = "MODIFIED";
                        app.Instances[i].Status = status;

                        app.Instances[i].LastDirtyTimestamp = lastDirtyTimestamp;
                        app.Instances[i].LastUpdatedTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

                        app.Instances[i].LeaseInfo.LastRenewalTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

                        if (status.CompareTo("UP") == 0)
                        {
                            app.Instances[i].LeaseInfo.ServiceUpTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
                        }

                        break;
                    }
                }

                IncreaseVersion();

                return app.Instances[idx];
            }

            return null;
        }

        public Instance Delete(string appName, string instanceId)
        {
            Instance instance = null;

            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);

                if (instance != null)
                {

                    app.Instances.Remove(instance);

                    IncreaseVersion();

                    return instance;
                }
            }

            return null;
        }

        public Instance Add(Instance instance)
        {
            var appId = instance.AppName;

            var appFound = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appId) == 0);

            if (appFound == null)
            {
                appFound = new Application()
                {
                    TenantId = repoTenantId,
                    Name = appId,
                    Instances = new List<Instance>()
                };

                ServicesRuntime.AllApplications.Applications.Add(appFound);
            }

            instance.TenantId = repoTenantId;

            instance.Status = "STARTING";
            instance.ActionType = "ADDED";

            if (string.IsNullOrEmpty(instance.InstanceId))
            {
                instance.InstanceId = instance.HostName;
            }

            instance.LastDirtyTimestamp = instance.LastUpdatedTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

            instance.LeaseInfo.RegistrationTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
            instance.LeaseInfo.LastRenewalTimestamp = instance.LeaseInfo.RegistrationTimestamp;
            instance.LeaseInfo.EvictionTimestamp = instance.LeaseInfo.LastRenewalTimestamp + DefaultConfigurationData.DefaultEvictionInSecs * DateTimeConversions.TicksPerSecond;
            instance.LeaseInfo.ServiceUpTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);

            var existingInstance = appFound.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instance.InstanceId) == 0);

            if (existingInstance != null)
            {
                Delete(existingInstance.AppName, existingInstance.InstanceId);
            }

            appFound.Instances.Add(instance);

            IncreaseVersion();

            return instance;
        }

        public Instance SaveInstanceHearbeat(string appName, string instanceId, string status, long lastDirtyTimestamp)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            int idx = -1;

            if (app != null)
            {
                for (var i = 0; i < app.Instances.Count; i++)
                {
                    if (app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        idx = i;

                        app.Instances[i].ActionType = "MODIFIED";
                        app.Instances[i].Status = status;

                        app.Instances[i].LastDirtyTimestamp = app.Instances[i].LastUpdatedTimestamp = lastDirtyTimestamp;

                        app.Instances[i].LeaseInfo.LastRenewalTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
                        app.Instances[i].LeaseInfo.EvictionTimestamp = app.Instances[i].LeaseInfo.LastRenewalTimestamp + DefaultConfigurationData.DefaultEvictionInSecs * DateTimeConversions.TicksPerSecond;

                        if (status.CompareTo("UP") == 0)
                        {
                            app.Instances[i].LeaseInfo.ServiceUpTimestamp = DateTimeConversions.ToJavaMillis(DateTime.UtcNow);
                        }

                        break;
                    }
                }

                IncreaseVersion();

                return app.Instances[idx];
            }

            return null;
        }

        public List<Instance> GetInstancesForVipAddress(string vipAddress)
        {
            var list = new List<Instance>();

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            foreach (var app in apps)
            {
                var instances = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.VipAddress.CompareTo(vipAddress) == 0).ToList();
                list.AddRange(instances);
            }

            return list;
        }

        public List<string> GetDataCenters()
        {
            List<string> list = new List<string>();

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            foreach (var app in apps)
            {
                var instances = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0).ToList();

                foreach (var instance in instances)
                {
                    list.Add(instance.DataCenterInfo.Name);
                }
            }

            var distinctDataCenters = list.Distinct().ToList();

            return distinctDataCenters;
        }

        public List<int> GetCountries()
        {
            List<int> list = new List<int>();

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            foreach (var app in apps)
            {
                var instances = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0).ToList();

                foreach (var instance in instances)
                {
                    list.Add(instance.CountryId);
                }
            }

            var distinctCountries = list.Distinct().ToList();

            return distinctCountries;
        }

        public List<Instance> GetInstancesForSVipAddress(string svipAddress)
        {
            var list = new List<Instance>();

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            foreach (var app in apps)
            {
                var instances = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.SecureVipAddress.CompareTo(svipAddress) == 0).ToList();
                list.AddRange(instances);
            }

            return list;
        }

        public bool AddDependencyForApplication(string appName, string dependency)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                var dep = app.Dependencies.SingleOrDefault(r => r.CompareTo(dependency) == 0);

                if (dep == null)
                {
                    app.Dependencies.Add(dependency);
                    return true;
                }
            }

            return false;
        }

        public bool AddDependenciesForApplication(string appName, List<string> dependencies)
        {
            var ok = true;

            foreach (var dependency in dependencies)
            {
                var dOk = AddDependencyForApplication(appName, dependency);
                if (!dOk)
                {
                    ok = false;
                    break;
                }
            }

            return ok;
        }

        public bool DeleteDependencyForApplication(string appName, string dependency)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                var dep = app.Dependencies.SingleOrDefault(r => r.CompareTo(dependency) == 0);
                if (dep != null)
                {
                    app.Dependencies.Remove(dep);
                    return true;
                }
            }

            return false;
        }
    }
}
