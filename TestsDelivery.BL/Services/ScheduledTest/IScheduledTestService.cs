using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public interface IScheduledTestService
    {
        ScheduledTestDomain ScheduleTest(ScheduledTestDomain test);

        ScheduledTestDomain GetTest(long id);
    }
}
