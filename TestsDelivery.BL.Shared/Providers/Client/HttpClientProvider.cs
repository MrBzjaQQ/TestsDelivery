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
            // TODO: SECURITY RISK. TEMPORARY SOLUTION FOR DEVELOPMENT PURPOSES
            // var httpClient = _httpClientFactory.CreateClient();
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(handler);

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
