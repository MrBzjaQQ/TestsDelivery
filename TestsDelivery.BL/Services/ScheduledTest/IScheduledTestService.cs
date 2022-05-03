using System.Collections.Generic;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.ScheduledTest;
using ScheduledTestDomain = TestsDelivery.Domain.ScheduledTest.ScheduledTest;

namespace TestsDelivery.BL.Services.ScheduledTest
{
    public interface IScheduledTestService
    {
        ScheduledTestDomain ScheduleTest(ScheduledTestDomain test);

        ScheduledTestDomain GetTest(long id);

        IEnumerable<ScheduledTestInListDto> GetList(ListFilter filter);
    }
}
