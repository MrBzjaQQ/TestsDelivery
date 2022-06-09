using TestsDelivery.BL.Shared.Clients.Integration;
using TestsDelivery.UserModels.AnsweredTests;
using TestsDelivery.UserModels.Communication;

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
            await _apiClient.PostAsync<AnsweredTestCreateModel, object>($"{instanceUrl}api/ScheduledTests/FinishTest", test);
        }

        public async Task StartTest(long testId, string instanceUrl)
        {
            await _apiClient.PostAsync<IdModel, object>($"{instanceUrl}api/ScheduledTests/StartTest", new IdModel { Id = testId });
        }
    }
}
