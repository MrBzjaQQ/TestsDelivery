using TestsDelivery.UserModels.ScheduledTest;

namespace TestsPortal.BL.Mediators.ScheduledTest
{
    public interface IScheduledTestMediator
    {
        public ScheduledTestReadModel ScheduleTest(ScheduledTestDetailedModel model);
    }
}
