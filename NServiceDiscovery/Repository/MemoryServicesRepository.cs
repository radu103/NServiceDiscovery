﻿using System.Collections.Generic;
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

        private void UpdateAppsHashCode()
        {
            var hash = string.Empty;

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            var up_instances_count = 0;

            foreach(var app in apps)
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
            ServicesRuntime.AllApplications._VersionsDelta += 1;
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
                if(instance != null) { break; }
            }

            return instance;
        }

        public Application GetByAppName(string appName)
        {
            var list = new List<Instance>();

            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            return app;
        }

        public bool ChangeStatus(string appName, string instanceId, string status, long lastDirtyTimestamp)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                for(var i = 0; i < app.Instances.Count; i++)
                {
                    if(app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        app.Instances[i].ActionType = "STATUS";
                        app.Instances[i].Status = status;
                        
                        app.Instances[i].LastDirtyTimestamp = lastDirtyTimestamp.ToString();
                        app.Instances[i].LastUpdatedTimestamp = (DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH).ToString();

                        app.Instances[i].LeaseInfo.LastRenewalTimestamp = DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH;

                        if (status.CompareTo("UP") == 0)
                        {
                            app.Instances[i].LeaseInfo.ServiceUpTimestamp = DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH;
                        }

                        break;
                    }
                }

                IncreaseVersion();

                return true;
            }

            return false;
        }

        public bool Delete(string appName, string instanceId)
        {
            Instance instance = null;

            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if(app != null)
            {
                instance = app.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instanceId) == 0);

                if (instance != null) {

                    app.Instances.Remove(instance);

                    IncreaseVersion();

                    return true;
                }
            }

            return false;
        }

        public Instance Add(Instance instance)
        {
            var appId = instance.AppName;

            var appFound = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appId) == 0);

            if (appFound == null)
            {
                appFound = new Application()
                {
                    Name = appId,
                    Protocol = ApplicationProtocol.HTTP,
                    Instances = new List<Instance>(),
                    TenantId = repoTenantId
                };

                ServicesRuntime.AllApplications.Applications.Add(appFound);
            }

            instance.TenantId = repoTenantId;

            instance.ActionType = "ADDED";

            instance.LastDirtyTimestamp = instance.LastUpdatedTimestamp = (DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH).ToString();

            instance.LeaseInfo.RegistrationTimestamp = DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH;
            instance.LeaseInfo.LastRenewalTimestamp = instance.LeaseInfo.RegistrationTimestamp - Memory.TICKS_AT_EPOCH;
            instance.LeaseInfo.EvictionTimestamp = instance.LeaseInfo.LastRenewalTimestamp + DefaultConfigurationData.DefaultEvictionInSecs * DefaultConfigurationData.TicksPerSecond;

            var existingInstance = appFound.Instances.SingleOrDefault(i => i.TenantId.CompareTo(repoTenantId) == 0 && i.InstanceId.CompareTo(instance.InstanceId) == 0);

            if(existingInstance != null)
            {
                Delete(existingInstance.AppName, existingInstance.InstanceId);
            }

            appFound.Instances.Add(instance);

            IncreaseVersion();

            return instance;
        }

        public bool SaveInstanceHearbeat(string appName, string instanceId, string status, long lastDirtyTimestamp)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                for (var i = 0; i < app.Instances.Count; i++)
                {
                    if (app.Instances[i].TenantId.CompareTo(repoTenantId) == 0 && app.Instances[i].InstanceId.CompareTo(instanceId) == 0)
                    {
                        app.Instances[i].ActionType = "HEARTBEAT";
                        app.Instances[i].Status = status;

                        app.Instances[i].LastDirtyTimestamp = app.Instances[i].LastUpdatedTimestamp = lastDirtyTimestamp.ToString();

                        app.Instances[i].LeaseInfo.LastRenewalTimestamp = DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH;
                        app.Instances[i].LeaseInfo.EvictionTimestamp = app.Instances[i].LeaseInfo.LastRenewalTimestamp + DefaultConfigurationData.DefaultEvictionInSecs * DefaultConfigurationData.TicksPerSecond;

                        if (status.CompareTo("UP") == 0)
                        {
                            app.Instances[i].LeaseInfo.ServiceUpTimestamp = DateTime.Now.Ticks - Memory.TICKS_AT_EPOCH;
                        }

                        break;
                    }
                }

                IncreaseVersion();

                return true;
            }

            return false;
        }

        public List<Instance> GetInstancesForVipAddress(string vipAddress)
        {
            var list = new List<Instance>();

            var apps = ServicesRuntime.AllApplications.Applications.FindAll(a => a.TenantId.CompareTo(repoTenantId) == 0).ToList();

            foreach(var app in apps)
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

            foreach(var app in apps)
            {
                var instances = app.Instances.FindAll(i => i.TenantId.CompareTo(repoTenantId) == 0).ToList();

                foreach(var instance in instances)
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
                var dep = app.RequiresApps.SingleOrDefault(r => r.CompareTo(dependency) == 0);

                if (dep == null)
                {
                    app.RequiresApps.Add(dependency);
                    return true;
                }
            }

            return false;
        }

        public bool DeleteDependencyForApplication(string appName, string dependency)
        {
            var app = ServicesRuntime.AllApplications.Applications.SingleOrDefault(a => a.TenantId.CompareTo(repoTenantId) == 0 && a.Name.CompareTo(appName) == 0);

            if (app != null)
            {
                var dep = app.RequiresApps.SingleOrDefault(r => r.CompareTo(dependency) == 0);
                if (dep != null)
                {
                    app.RequiresApps.Remove(dep);
                    return true;
                }
            }

            return false;
        }
    }
}
