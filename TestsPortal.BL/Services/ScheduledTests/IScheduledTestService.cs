using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Services.ScheduledTests
{
    public interface IScheduledTestService
    {
        IEnumerable<ScheduledTestInstance> ScheduleTest(ScheduledTest scheduledTest);

        long GetInstanceOriginalId(long testId);
    }
}
