namespace TestsDelivery.BL.Shared.Providers.Client
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientProvider(IHttpClientFactory httpClientFactory)
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
