using System.Collections.Generic;

namespace TestsDelivery.DAL.Repositories.CandidateInScheduledTest
{
    public interface IScheduledTestInstancesRepository : IBaseRepository<Models.ScheduledTest.ScheduledTestInstance>
    {
        void Create(IEnumerable<Models.ScheduledTest.ScheduledTestInstance> instances);

        IEnumerable<Models.ScheduledTest.ScheduledTestInstance> GetByTestId(long testId);
    }
}
