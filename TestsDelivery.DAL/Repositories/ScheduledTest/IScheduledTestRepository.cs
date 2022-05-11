using System.Collections.Generic;
using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Shared;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.ScheduledTest
{
    public interface IScheduledTestRepository : IBaseRepository<Models.ScheduledTest.ScheduledTest>
    {
        IEnumerable<ScheduledTestInList> GetList(GenericFilter<ScheduledTestInList> filter);

        int GetScheduledTestsCount(GenericFilter<ScheduledTestInList> filter);
    }
}
