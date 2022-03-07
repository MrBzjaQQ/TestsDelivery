using TestsDelivery.BL.Clients.Integration;
using TestsDelivery.BL.Factories;
using TestsDelivery.BL.Services.TestPortalInstances;

namespace TestsDelivery.BL.Providers.Communication
{
    public class CommunicationServiceProvider : ICommunicationServiceProvider
    {
        private readonly ITestPortalInstancesService _instancesService;
        private readonly ICommunicationServiceFactory _serviceFactory;
        private readonly IIntegrationApiClient _apiClient;

        public CommunicationServiceProvider(
            IIntegrationApiClient apiClient,
            ITestPortalInstancesService instancesService,
            ICommunicationServiceFactory serviceFactory)
        {
            _apiClient = apiClient;
            _instancesService = instancesService;
            _serviceFactory = serviceFactory;
        }

        public TInterface Get<TInterface>(string instanceName)
        {
            var url = _instancesService.GetInstanceUrl(instanceName);
            return _serviceFactory.Create<TInterface>(_apiClient, url);
        }
    }
}
