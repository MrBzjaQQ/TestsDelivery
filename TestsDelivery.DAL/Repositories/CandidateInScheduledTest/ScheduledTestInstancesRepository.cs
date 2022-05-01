using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;

namespace TestsDelivery.DAL.Repositories.CandidateInScheduledTest
{
    public class ScheduledTestInstancesRepository : BaseRepository<Models.ScheduledTest.ScheduledTestInstance>, IScheduledTestInstancesRepository
    {
        public ScheduledTestInstancesRepository(TestsDeliveryContext context)
            : base(context)
        {
        }

        public void Create(IEnumerable<Models.ScheduledTest.ScheduledTestInstance> instances)
        {
            Context.AddRange(instances);
            Context.SaveChanges();
        }

        public IEnumerable<Models.ScheduledTest.ScheduledTestInstance> GetByTestId(long testId)
        {
            return Context
                .ScheduledTestInstances
                .Include(x => x.Candidate)
                .Where(x => x.ScheduledTestId == testId)
                .ToList();
        }
    }
}
