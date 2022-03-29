using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTests
{
    public interface IScheduledTestsRepository
    {
        void CreateScheduledTest(ScheduledTest scheduledTest);
    }
}
