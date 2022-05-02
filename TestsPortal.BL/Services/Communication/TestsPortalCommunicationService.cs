using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.UserModels.AnsweredTests;

namespace TestsPortal.BL.Services.Communication
{
    public class TestsPortalCommunicationService : ITestsPortalCommunicationService
    {
        private readonly IIntegrationApiClient _apiClient;

        public TestsPortalCommunicationService(IIntegrationApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task FinishTest(AnsweredTestCreateModel test, string instanceUrl)
        {
            await _apiClient.PostAsync<AnsweredTestCreateModel, AnsweredTestReadModel>(instanceUrl, test);
        }
    }
}
