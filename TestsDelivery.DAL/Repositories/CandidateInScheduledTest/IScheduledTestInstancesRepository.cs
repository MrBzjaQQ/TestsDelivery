using System.Collections.Generic;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.CandidateInScheduledTest
{
    public interface IScheduledTestInstancesRepository : IBaseRepository<Models.ScheduledTest.ScheduledTestInstance>
    {
        void Create(IEnumerable<Models.ScheduledTest.ScheduledTestInstance> instances);

        IEnumerable<Models.ScheduledTest.ScheduledTestInstance> GetByTestId(long testId);
    }
}
