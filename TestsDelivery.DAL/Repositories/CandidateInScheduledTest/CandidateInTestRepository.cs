using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;

namespace TestsDelivery.DAL.Repositories.CandidateInScheduledTest
{
    public class CandidateInTestRepository : BaseRepository<Models.ScheduledTest.CandidateInScheduledTest>, ICandidateInTestRepository
    {
        public CandidateInTestRepository(TestsDeliveryContext context)
            : base(context)
        {
        }

        public void Create(IEnumerable<Models.ScheduledTest.CandidateInScheduledTest> candidates)
        {
            Context.AddRange(candidates);
            Context.SaveChanges();
        }

        public IEnumerable<Models.ScheduledTest.CandidateInScheduledTest> GetByTestId(long testId)
        {
            return Context
                .CandidatesInScheduledTest
                .Include(x => x.Candidate)
                .Where(x => x.ScheduledTestId == testId)
                .ToList();
        }
    }
}
