using Steeltoe.Common.Discovery;
using Steeltoe.Common.Http.LoadBalancer;
using Steeltoe.Common.LoadBalancer;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestAPI1.Services
{
    public class AutoTestWithLoadBalanceService : IAutoTestService, IAutoTestWithLoadBalanceService
    {
        private const string APP_NAME = "test-api-1";
        private const string APP_NAME_URL = "http://" + APP_NAME + "/info";

        private IDiscoveryClient _discoveryClient = null;
        private ILoadBalancer _loadBalancer = null;

        private LoadBalancerHttpClientHandler _handler = null;
        private HttpClient _httpClient = null;

        private Random _random = new Random();

        public AutoTestWithLoadBalanceService(IDiscoveryClient discoveryClient)
        {
            _discoveryClient = discoveryClient;

            _loadBalancer = new RandomLoadBalancer(_discoveryClient);

            _handler = new LoadBalancerHttpClientHandler(_loadBalancer);
        }

        private HttpClient GetHttpClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient(_handler, false);
            }

            return _httpClient;
        }

        public async Task<string> ServiceCallAsync()
        {
            var httpClient = new HttpClient(_handler, false);

            var response = await httpClient.GetStringAsync(APP_NAME_URL);

            return "Response from own /info enpoint : " + response;
        }
    }
}