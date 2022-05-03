using AutoMapper;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTests
{
    public class ScheduledTestsRepository : BaseRepository<TestsPortalContext, ScheduledTest>, IScheduledTestsRepository
    {
        public ScheduledTestsRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
