using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Services.ScheduledTests
{
    public interface IScheduledTestService
    {
        ScheduledTest ScheduleTest(ScheduledTest scheduledTest);
    }
}
