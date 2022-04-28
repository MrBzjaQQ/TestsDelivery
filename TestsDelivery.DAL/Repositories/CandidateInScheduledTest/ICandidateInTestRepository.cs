using System.Collections.Generic;

namespace TestsDelivery.DAL.Repositories.CandidateInScheduledTest
{
    public interface ICandidateInTestRepository : IBaseRepository<Models.ScheduledTest.CandidateInScheduledTest>
    {
        void Create(IEnumerable<Models.ScheduledTest.CandidateInScheduledTest> candidates);

        IEnumerable<Models.ScheduledTest.CandidateInScheduledTest> GetByTestId(long testId);
    }
}
