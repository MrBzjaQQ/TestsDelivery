using System.Collections.Generic;
using System.Net.Http;

namespace TestsDelivery.BL.Providers.Client
{
    public class TestsPotralClientProvider : IHttpClientProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestsPotralClientProvider(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Get(IDictionary<string, string> additionalHeaders = null)
        {
            var httpClient = _httpClientFactory.CreateClient();

            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return httpClient;
        }
    }
}
