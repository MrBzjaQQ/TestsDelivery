using TestsDelivery.BL.Clients.Integration;

namespace TestsDelivery.BL.Factories
{
    public interface ICommunicationServiceFactory
    {
        TInterface Create<TInterface>(IIntegrationApiClient apiClient, string instanceUrl);
    }
}
