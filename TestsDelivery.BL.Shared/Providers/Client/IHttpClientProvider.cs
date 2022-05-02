namespace TestsDelivery.BL.Shared.Providers.Client
{
    public interface IHttpClientProvider
    {
        HttpClient Get(IDictionary<string, string> additionalHeaders = null);
    }
}
