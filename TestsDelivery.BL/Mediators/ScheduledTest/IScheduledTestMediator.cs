using TestsDelivery.UserModels.ScheduledTest;

namespace TestsDelivery.BL.Mediators.ScheduledTest
{
    public interface IScheduledTestMediator
    {
        ScheduledTestReadModel ScheduleTest(ScheduledTestCreateModel model);

        ScheduledTestReadModel GetTest(long id);
    }
}
