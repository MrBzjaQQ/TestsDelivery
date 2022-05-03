using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.CandidateInScheduledTest
{
    public class ScheduledTestInstancesRepository : BaseRepository<TestsDeliveryContext, Models.ScheduledTest.ScheduledTestInstance>, IScheduledTestInstancesRepository
    {
        public ScheduledTestInstancesRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
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
