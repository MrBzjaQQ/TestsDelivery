using System;

namespace TestsDelivery.BL.Clients.Integration
{
    public class IntegrationException : Exception
    {
        public IntegrationException(int statusCode,
            string requestMethod,
            string url,
            string description)
            : base($"Status code: {statusCode}\nRequestMethod: {requestMethod}\nURL: {url}\nDescription: {description}")
        {
        }
    }
}
