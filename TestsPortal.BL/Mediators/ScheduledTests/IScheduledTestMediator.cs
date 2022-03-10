using TestsDelivery.UserModels.ScheduledTest;

namespace TestsPortal.BL.Mediators.ScheduledTests
{
    public interface IScheduledTestMediator
    {
        public ScheduledTestReadModel ScheduleTest(ScheduledTestDetailedModel model);
    }
}
