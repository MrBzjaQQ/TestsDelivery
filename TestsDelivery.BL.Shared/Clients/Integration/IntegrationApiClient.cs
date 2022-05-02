using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using TestsDelivery.BL.Shared.Providers.Client;

namespace TestsDelivery.BL.Shared.Clients.Integration
{
    public class IntegrationApiClient : IIntegrationApiClient
    {
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ILogger<IntegrationApiClient> _logger;

        public IntegrationApiClient(
            IHttpClientProvider httpClientProvider,
            ILogger<IntegrationApiClient> logger)
        {
            _httpClientProvider = httpClientProvider;
            _logger = logger;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url)
            where TResponse : class
        {
            _logger.LogInformation($"Sending GET request to URL: {url}");

            using var httpClient = _httpClientProvider.Get();
            using var response = await httpClient.GetAsync(url);

            return await DeserializeAsync<TResponse>(url, response);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string url,
            TRequest body,
            IDictionary<string, string> additionalHeaders = null)
            where TResponse : class
        {
            const string requestMethod = "POST";
            var content = Serialize(url, body, requestMethod);

            using var httpClient = _httpClientProvider.Get(additionalHeaders);

            using var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var exception = new IntegrationException(
                    (int)response.StatusCode,
                    requestMethod,
                    url,
                    response.ReasonPhrase);

                _logger.LogError(exception.Message);
                throw exception;
            }

            return await DeserializeAsync<TResponse>(url, response);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(
            string url,
            TRequest body,
            IDictionary<string, string> additionalHeaders = null)
            where TResponse : class
        {
            const string requestMethod = "PUT";
            var content = Serialize(url, body, requestMethod);

            using var httpClient = _httpClientProvider.Get(additionalHeaders);
            using var response = await httpClient.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var exception = new IntegrationException(
                    (int)response.StatusCode,
                    requestMethod,
                    url,
                    response.ReasonPhrase);

                _logger.LogError(exception.Message);
                throw exception;
            }

            return await DeserializeAsync<TResponse>(url, response);
        }

        private HttpContent Serialize<TRequest>(
            string url,
            TRequest body,
            string requestType)
        {
            var bodyType = body.GetType();
            var stringBody = bodyType.IsClass && bodyType != typeof(string)
                ? JsonConvert.SerializeObject(body, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
                : body.ToString();

            _logger.LogInformation($"Sending {requestType} request to {url} with body: {body}");

            return new StringContent(stringBody, Encoding.UTF8, "application/json");
        }

        private async Task<TResponse> DeserializeAsync<TResponse>(string url, HttpResponseMessage response)
            where TResponse : class
        {
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Response from URL = {url}: {content}");
            return content.Length > 0 ? JsonConvert.DeserializeObject<TResponse>(content) : default;
        }
    }
}
