using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.ScheduledTest;
using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public interface IScheduledTestService
    {
        ScheduledTestToCreate ScheduleTest(ScheduledTestDomain test);

        ScheduledTestDomain GetTest(long id);

        ScheduledTestsList GetList(ListFilter filter);
    }
}
