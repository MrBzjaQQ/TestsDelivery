using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTests
{
    public class ScheduledTestsRepository : BaseRepository<ScheduledTest>, IScheduledTestsRepository
    {
        public ScheduledTestsRepository(TestsPortalContext context)
            : base(context)
        {
        }
    }
}
