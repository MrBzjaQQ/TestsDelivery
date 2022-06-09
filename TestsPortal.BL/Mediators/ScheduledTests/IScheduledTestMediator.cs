using TestsDelivery.UserModels.ScheduledTest;

namespace TestsPortal.BL.Mediators.ScheduledTests
{
    public interface IScheduledTestMediator
    {
        IEnumerable<ScheduledTestInstanceReadModel> ScheduleTest(ScheduledTestDetailedModel model, string host);
    }
}
