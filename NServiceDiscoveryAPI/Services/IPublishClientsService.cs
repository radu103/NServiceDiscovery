namespace NServiceDiscoveryAPI.Services
{
    public interface IPublishClientsService
    {
        void PublishClientConfigurationActivity(string tenantId, string clientHostname);
        void PublishClientDiscoveryActivity(string tenantId, string clientHostname);
    }
}