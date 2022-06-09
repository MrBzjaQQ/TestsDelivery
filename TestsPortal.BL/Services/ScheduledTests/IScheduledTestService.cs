using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Services.ScheduledTests
{
    public interface IScheduledTestService
    {
        IEnumerable<ScheduledTestInstance> ScheduleTest(ScheduledTestToCreate scheduledTest);

        long GetInstanceOriginalId(long testId);
    }
}
