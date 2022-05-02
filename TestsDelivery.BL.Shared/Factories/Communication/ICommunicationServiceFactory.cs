using TestsDelivery.BL.Shared.Clients.Integration;

namespace TestsDelivery.BL.Shared.Factories
{
    public interface ICommunicationServiceFactory
    {
        TInterface Create<TInterface>(IIntegrationApiClient apiClient, string instanceUrl);
    }
}
