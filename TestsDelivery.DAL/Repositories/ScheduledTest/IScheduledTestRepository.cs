namespace TestsDelivery.DAL.Repositories.ScheduledTest
{
    public interface IScheduledTestRepository
    {
        void ScheduleTest(Models.ScheduledTest.ScheduledTest test);

        Models.ScheduledTest.ScheduledTest GetTest(long id);
    }
}
