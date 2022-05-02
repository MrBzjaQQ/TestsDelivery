namespace TestsDelivery.BL.Shared.Clients.Integration
{
    public interface IIntegrationApiClient
    {
        Task<TResponse> GetAsync<TResponse>(string url)
            where TResponse : class;

        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest body, IDictionary<string, string> additionalHeaders = null)
            where TResponse : class;

        Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest body, IDictionary<string, string> additionalHeaders = null)
            where TResponse : class;
    }
}
