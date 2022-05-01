using AutoMapper;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTestInstances
{
    public class ScheduledTestInstancesRepository : BaseRepository<ScheduledTestInstance>, IScheduledTestInstancesRepository
    {
        public ScheduledTestInstancesRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public void Create(IEnumerable<ScheduledTestInstance> scheduledTests)
        {
            Context.AddRange(scheduledTests);
            Context.SaveChanges();
        }
    }
}
