using Steeltoe.Common.Discovery;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestAPI1.Services
{
    public class AutoTestService : IAutoTestService
    {
        private const string APP_NAME = "test-api-1";
        private const string APP_NAME_URL = "http://" + APP_NAME + "/info";

        private IDiscoveryClient _discoveryClient = null;
        private DiscoveryHttpClientHandler _handler = null;
        private HttpClient _httpClient = null;

        private Random _random = new Random();

        public AutoTestService(IDiscoveryClient discoveryClient)
        {
            _discoveryClient = discoveryClient;
            _handler = new DiscoveryHttpClientHandler(_discoveryClient);
        }

        private HttpClient GetHttpClient()
        {
            if(_httpClient == null)
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