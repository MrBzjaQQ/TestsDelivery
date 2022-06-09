using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTestInstances
{
    public class ScheduledTestInstancesRepository : BaseRepository<TestsPortalContext, ScheduledTestInstance>, IScheduledTestInstancesRepository
    {
        public ScheduledTestInstancesRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public long GetInstanceOriginalId(long testId)
        {
            return DbSet.Where(x => x.Id == testId).Select(x => x.OriginalId).Single();
        }

        public string GetAdminInstanceForTest(long testId)
        {
            return DbSet
                .Include(x => x.ScheduledTest)
                .Where(x => x.Id == testId)
                .Select(x => x.ScheduledTest.AdminPanelInstance)
                .Single();
        }
    }
}
