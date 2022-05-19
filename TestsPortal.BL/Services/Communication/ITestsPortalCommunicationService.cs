using TestsDelivery.UserModels.AnsweredTests;

namespace TestsPortal.BL.Services.Communication
{
    public interface ITestsPortalCommunicationService
    {
        Task FinishTest(AnsweredTestCreateModel test, string instanceUrl);

        Task StartTest(long testId, string instanceUrl);
    }
}
