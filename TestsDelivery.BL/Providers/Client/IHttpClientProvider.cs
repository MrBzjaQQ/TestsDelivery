using System.Collections.Generic;
using System.Net.Http;

namespace TestsDelivery.BL.Providers.Client
{
    public interface IHttpClientProvider
    {
        HttpClient Get(IDictionary<string, string> additionalHeaders = null);
    }
}
