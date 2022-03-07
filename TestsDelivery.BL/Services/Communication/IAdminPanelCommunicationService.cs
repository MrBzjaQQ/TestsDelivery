using TestsDelivery.UserModels.ScheduledTest;

namespace TestsDelivery.BL.Services.Communication
{
    public interface IAdminPanelCommunicationService
    {
        void ScheduleTest(ScheduledTestDetailedModel test);
    }
}
